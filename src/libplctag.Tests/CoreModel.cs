// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Coyote.Random;
using static libplctag.NativeImport.plctag;
using static libplctag.Tests.State;

namespace libplctag.Tests
{
    enum State { NoTag, Creating, Idle, Reading, Writing, Failed, Destroyed }

    enum Transition { Create, StartRead, StartWrite, Finish, Abort, Destroy }

    /// <summary>The outcomes the contract leaves open, chosen by the test.</summary>
    sealed class CoreChoices
    {
        /// <summary>create_ex returns this error and no handle (create_ex.1), instead of a handle.</summary>
        public Status? CreateFails;

        /// <summary>
        /// What the tickler does while a blocking read or write waits: it finishes the operation with this status
        /// (F3, F4), or, if null, nothing, so that the call's timeout expires and the call aborts it (read.5, write.5).
        /// </summary>
        public Status? BlockedCallEndsWith = Status.Ok;
    }

    /// <summary>
    /// One native tag, as the core's contract describes it (docs/core-contract), in two parts:
    /// <list type="number">
    /// <item>its <see cref="Transitions"/>: how the tag changes, whoever makes it change (a caller, the tickler, a
    /// timeout). Each happens atomically, under the tag's mutex. The diagram is these.</item>
    /// <item>the core's calls: each a short script of transitions, plus what the call returns. A blocking call
    /// releases the mutex while it waits, so other threads' transitions can happen in between.</item>
    /// </list>
    /// The assumptions behind both (docs/core-contract): transitions are atomic, in any interleaving; callbacks are
    /// hints, with no promise of sequence: a transition only raises events, and each call ends by delivering some of them
    /// as callbacks (see <see cref="Deliver"/>).
    /// Deliberately not realistic: no threads of its own, no timers. Nothing happens unless the wrapper calls in, or
    /// the test plays the tickler. A transition the contract does not allow throws, which fails the test.
    /// </summary>
    sealed class CoreModel
    {
        sealed record Row(string Id, State From, Transition Transition, Condition If, State To, Action<CoreModel> Does, string Note);

        sealed record Condition(string Label, Func<CoreModel, bool> Holds);

        static readonly Condition Always = new("", _ => true);
        static readonly Condition FinishesOk = new("OK", m => m._finishWith == Status.Ok);
        static readonly Condition FinishesWithError = new("error", m => m._finishWith != Status.Ok);
        static readonly Condition CreationEnds = new("creation ends", _ => AnyOf(true, false));
        static readonly Condition CreationGoesOn = new("creation goes on", _ => true);

        static readonly Row[] Transitions =
        {
            //  id    from       transition              if                 to          does                                                  note
            new("C1", NoTag,     Transition.Create,      Always,            Creating,   m => m._stored = Status.Pending,                      "the creation starts"),

            new("R1", Idle,      Transition.StartRead,   Always,            Reading,    m => m.Started(Event.ReadStarted),                    "raises READ_STARTED(OK)"),
            new("W1", Idle,      Transition.StartWrite,  Always,            Writing,    m => m.Started(Event.WriteStarted),                   "raises WRITE_STARTED(OK)"),

            new("F1", Creating,  Transition.Finish,      FinishesOk,        Idle,       m => m.Ended(Event.Created),                          "raises CREATED(OK)"),
            new("F2", Creating,  Transition.Finish,      FinishesWithError, Failed,     m => m.Ended(Event.Created),                          "raises CREATED(e)"),
            new("F3", Reading,   Transition.Finish,      Always,            Idle,       m => m.Ended(Event.ReadCompleted),                    "raises READ_COMPLETED(s)"),
            new("F4", Writing,   Transition.Finish,      Always,            Idle,       m => m.Ended(Event.WriteCompleted),                   "raises WRITE_COMPLETED(s)"),

            new("A1", Creating,  Transition.Abort,       CreationEnds,      Failed,     m => m.Aborted(Event.Created, Status.Ok),             "raises CREATED(OK), ABORTED(OK)"),
            new("A6", Creating,  Transition.Abort,       CreationGoesOn,    Creating,   m => m.Raised(Event.Aborted, Status.Ok),              "raises ABORTED(OK), the tickler may still finish it"),
            new("A2", Reading,   Transition.Abort,       Always,            Idle,       m => m.Aborted(Event.ReadCompleted, Status.ErrorAbort), "raises READ_COMPLETED(ERR_ABORT), ABORTED(OK)"),
            new("A3", Writing,   Transition.Abort,       Always,            Idle,       m => m.Aborted(Event.WriteCompleted, Status.ErrorAbort), "raises WRITE_COMPLETED(ERR_ABORT), ABORTED(OK)"),
            new("A4", Idle,      Transition.Abort,       Always,            Idle,       m => m.Aborted(null, Status.Ok),                      "raises ABORTED(OK)"),
            new("A5", Failed,    Transition.Abort,       Always,            Failed,     m => m.Raised(Event.Aborted, Status.Ok),             "raises ABORTED(OK), the status stays"),

            // the handle goes first, so no other call finds the tag; then what was in flight is aborted
            new("D1", Idle,      Transition.Destroy,     Always,            Destroyed,  m => m.Raised(Event.Destroyed, Status.Ok),             "raises DESTROYED"),
            new("D2", Failed,    Transition.Destroy,     Always,            Destroyed,  m => m.Raised(Event.Destroyed, Status.Ok),             "raises DESTROYED"),
            new("D3", Creating,  Transition.Destroy,     Always,            Destroyed,  m => m.AbortedThenDestroyed(Event.Created, Status.Ok), "raises CREATED(OK), ABORTED(OK), DESTROYED"),
            new("D4", Reading,   Transition.Destroy,     Always,            Destroyed,  m => m.AbortedThenDestroyed(Event.ReadCompleted, Status.ErrorAbort), "raises READ_COMPLETED(ERR_ABORT), ABORTED(OK), DESTROYED"),
            new("D5", Writing,   Transition.Destroy,     Always,            Destroyed,  m => m.AbortedThenDestroyed(Event.WriteCompleted, Status.ErrorAbort), "raises WRITE_COMPLETED(ERR_ABORT), ABORTED(OK), DESTROYED"),
        };

        readonly object _gate = new object();   // the tag's mutex
        readonly int _handle;
        readonly CoreChoices _choose;
        readonly List<(Event Event, Status Status)> _raised = new List<(Event, Status)>();   // raised, not yet delivered
        callback_func_ex _callback;
        State _state = NoTag;
        Status _stored;                         // the tag's one status; any call can overwrite it
        Status _finishWith;                     // the finish's status

        public CoreModel(int handle, callback_func_ex callback, CoreChoices choose)
        {
            _handle = handle;
            _callback = callback;
            _choose = choose;
        }

        bool InFlight => _state == Creating || _state == Reading || _state == Writing;

        void Apply(Transition transition, Status finishWith = Status.Ok)
        {
            _finishWith = finishWith;
            var row = Transitions.FirstOrDefault(r => r.From == _state && r.Transition == transition && r.If.Holds(this))
                      ?? throw new InvalidOperationException($"The core's contract has no {transition} from {_state}");
            _state = row.To;
            row.Does(this);
        }

        /// <summary>A call: <paramref name="body"/> under the tag's mutex, then a delivery of raised events.</summary>
        T Call<T>(Func<T> body)
        {
            lock (_gate)
            {
                try { return body(); }
                finally { Deliver(); }
            }
        }

        // ---- the core's calls: each a script of transitions, its steps numbered (call.step) ------------------

        public Status Create(int timeout) => Call(() =>
        {
            if (_choose.CreateFails is Status error)
                return error;                                      // create_ex.1  fails: returns the error, no handle
            Apply(Transition.Create);                              // create_ex.2  C1
            if (timeout == 0)
                return Status.Ok;                                  //              timeout 0: returns the handle, the creation goes on
            Apply(Transition.Finish, Status.Ok);                   // create_ex.3  blocks until the tickler has created it (F1),
            return Status.Ok;                                      //              returns the handle; no other thread has it until then
        });

        public Status GetStatus() => Call(() =>
        {
            if (_state == Destroyed)
                return Status.ErrorNotFound;                                       // status.1  ERR_NOT_FOUND
            return InFlight && _stored == Status.Ok ? Status.Pending : _stored;    // status.2  the stored status, PENDING while in flight
        });

        public Status Read(int timeout) => Start(Transition.StartRead, timeout);
        public Status Write(int timeout) => Start(Transition.StartWrite, timeout);

        /// <summary>read() and write(): the same steps, read.N and write.N.</summary>
        Status Start(Transition start, int timeout)
        {
            var returned = Call<Status?>(() =>
            {
                if (_state == Destroyed)
                    return Status.ErrorNotFound;                   // .1  ERR_NOT_FOUND
                if (InFlight)
                    return Status.ErrorBusy;                       // .2  something in flight: ERR_BUSY
                Apply(start);                                      // .3  R1, W1 (from Failed: not in the contract)
                if (timeout == 0)
                    return Status.Pending;                         //     timeout 0: returns PENDING
                return null;
            });
            if (returned is Status status)
                return status;

            Thread.Yield();                                        // .4  blocks: waits with the mutex released, so other
                                                                   //     threads' calls and the tickler can move the tag
            return Call(() =>
            {
                if (InFlight)
                {
                    if (_choose.BlockedCallEndsWith is Status s)
                        Apply(Transition.Finish, s);               //     (the tickler finishes it during the wait: F3, F4)
                    else
                    {
                        Apply(Transition.Abort);                   // .5  the timeout expires: aborts it (A2, A3), ERR_TIMEOUT
                        return Status.ErrorTimeout;
                    }
                }
                return _stored;                                    // .6  ended, by whoever (the tickler, an abort, a destroy:
            });                                                    //     the call holds the tag, not its handle): returns the status
        }

        public Status Abort() => Call(() =>
        {
            if (_state == Destroyed)
                return Status.ErrorNotFound;                       // abort.1  ERR_NOT_FOUND
            Apply(Transition.Abort);                               // abort.2  A1-A5, returns OK
            return Status.Ok;
        });

        public Status UnregisterCallback() => Call(() =>
        {
            if (_state == Destroyed || _callback == null)
                return Status.ErrorNotFound;                       // unregister_callback.1  ERR_NOT_FOUND
            _callback = null;                                      // unregister_callback.2  drops the callback and
            _raised.Clear();                                       //                        undelivered events, returns OK
            return Status.Ok;
        });

        public Status Destroy() => Call(() =>
        {
            if (_state == Destroyed)
                return Status.ErrorNotFound;                       // destroy.1  ERR_NOT_FOUND
            Apply(Transition.Destroy);                             // destroy.2  D1-D5, returns OK
            return Status.Ok;
        });

        // ---- the tickler, played by the test ----------------------------------------------------------------

        /// <summary>The tickler finishes the creation or operation in flight with <paramref name="s"/> (F1-F4). With nothing in flight it does nothing.</summary>
        public void Finish(Status s) => Call(() =>
        {
            if (InFlight) Apply(Transition.Finish, s);
            return 0;
        });

        /// <summary>The test delivers an event now, of its choosing.</summary>
        public void Raise(Event e, Status s)
        {
            lock (_gate) _callback?.Invoke(_handle, (int)e, (int)s, IntPtr.Zero);
        }

        // ---- events -----------------------------------------------------------------------------------------

        void Raised(Event e, Status s) => _raised.Add((e, s));

        /// <summary>
        /// Delivers raised events, as assumption 3 allows: some of them (at least one), each type once with its last
        /// status, in any order; the rest stay raised for a later delivery. Which ones and in what order are random:
        /// Coyote's choices under Coyote, so that it explores late, merged and reordered deliveries.
        /// Every call ends with one, so after any transition at least one event is delivered (assumption 3).
        /// </summary>
        void Deliver()
        {
            if (_raised.Count == 0)
                return;

            var choose = Generator.Create();
            var now = Enumerable.Range(0, _raised.Count).Where(_ => choose.NextBoolean()).ToList();
            if (now.Count == 0)
                now.Add(choose.NextInteger(_raised.Count));
            for (var i = now.Count - 1; i > 0; i--)
            {
                var j = choose.NextInteger(i + 1);
                (now[i], now[j]) = (now[j], now[i]);
            }

            var delivered = now.Select(i => _raised[i]).GroupBy(e => e.Event).Select(g => g.Last()).ToList();
            foreach (var i in now.OrderByDescending(i => i))
                _raised.RemoveAt(i);
            foreach (var (e, s) in delivered)
                _callback?.Invoke(_handle, (int)e, (int)s, IntPtr.Zero);
        }

        // ---- what the transitions do ------------------------------------------------------------------------

        void Started(Event e)
        {
            _stored = Status.Pending;
            Raised(e, Status.Ok);
        }

        void Ended(Event e)
        {
            _stored = _finishWith;
            Raised(e, _finishWith);
        }

        void AbortedThenDestroyed(Event completed, Status completedWith)
        {
            Aborted(completed, completedWith);
            Raised(Event.Destroyed, Status.Ok);
        }

        void Aborted(Event? completed, Status completedWith)
        {
            if (completed is Event e) Raised(e, completedWith);
            Raised(Event.Aborted, Status.Ok);
            _stored = AnyOf(Status.Ok, Status.ErrorAbort);         // the status an abort leaves is not documented: any final one
        }

        /// <summary>One of <paramref name="possibilities"/>, at random: where the documentation does not say, the model allows every possibility, so that nothing comes to rely on one (Coyote's choice under Coyote).</summary>
        static T AnyOf<T>(params T[] possibilities) => possibilities[Generator.Create().NextInteger(possibilities.Length)];

        // ---- the diagram ------------------------------------------------------------------------------------

        static string Name(Transition t) => t switch
        {
            Transition.StartRead => "start read",
            Transition.StartWrite => "start write",
            _ => t.ToString().ToLowerInvariant(),
        };

        public static string ToMermaid()
        {
            var text = new StringBuilder("stateDiagram-v2\n    [*] --> NoTag\n");
            foreach (var row in Transitions)
            {
                var condition = row.If == Always ? "" : $" [{row.If.Label}]";
                text.Append($"    {row.From} --> {row.To} : {Name(row.Transition)}{condition} / {row.Id} {row.Note}\n");
            }
            return text.ToString();
        }
    }
}
