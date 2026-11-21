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

    sealed class CoreChoices
    {
        public Status? CreateFails;

        public Status? BlockedCallEndsWith = Status.Ok;
    }

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

            new("D1", Idle,      Transition.Destroy,     Always,            Destroyed,  m => m.Raised(Event.Destroyed, Status.Ok),             "raises DESTROYED"),
            new("D2", Failed,    Transition.Destroy,     Always,            Destroyed,  m => m.Raised(Event.Destroyed, Status.Ok),             "raises DESTROYED"),
            new("D3", Creating,  Transition.Destroy,     Always,            Destroyed,  m => m.AbortedThenDestroyed(Event.Created, Status.Ok), "raises CREATED(OK), ABORTED(OK), DESTROYED"),
            new("D4", Reading,   Transition.Destroy,     Always,            Destroyed,  m => m.AbortedThenDestroyed(Event.ReadCompleted, Status.ErrorAbort), "raises READ_COMPLETED(ERR_ABORT), ABORTED(OK), DESTROYED"),
            new("D5", Writing,   Transition.Destroy,     Always,            Destroyed,  m => m.AbortedThenDestroyed(Event.WriteCompleted, Status.ErrorAbort), "raises WRITE_COMPLETED(ERR_ABORT), ABORTED(OK), DESTROYED"),
        };

        readonly object _gate = new object();
        readonly int _handle;
        readonly CoreChoices _choose;
        readonly List<(Event Event, Status Status)> _raised = new List<(Event, Status)>();
        callback_func_ex _callback;
        State _state = NoTag;
        Status _stored;
        Status _finishWith;

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

        T Call<T>(Func<T> body)
        {
            lock (_gate)
            {
                try { return body(); }
                finally { Deliver(); }
            }
        }

        public Status Create(int timeout) => Call(() =>
        {
            if (_choose.CreateFails is Status error)
                return error;   // create_ex.1
            Apply(Transition.Create);   // create_ex.2 C1
            if (timeout == 0)
                return Status.Ok;
            Apply(Transition.Finish, Status.Ok);   // create_ex.3 F1
            return Status.Ok;
        });

        public Status GetStatus() => Call(() =>
        {
            if (_state == Destroyed)
                return Status.ErrorNotFound;   // status.1
            return InFlight && _stored == Status.Ok ? Status.Pending : _stored;   // status.2
        });

        public Status Read(int timeout) => Start(Transition.StartRead, timeout);
        public Status Write(int timeout) => Start(Transition.StartWrite, timeout);

        Status Start(Transition start, int timeout)
        {
            var returned = Call<Status?>(() =>
            {
                if (_state == Destroyed)
                    return Status.ErrorNotFound;   // read.1 write.1
                if (InFlight)
                    return Status.ErrorBusy;   // read.2 write.2
                Apply(start);   // read.3 write.3 R1 W1
                if (timeout == 0)
                    return Status.Pending;
                return null;
            });
            if (returned is Status status)
                return status;

            Thread.Yield();   // read.4 write.4
            return Call(() =>
            {
                if (InFlight)
                {
                    if (_choose.BlockedCallEndsWith is Status s)
                        Apply(Transition.Finish, s);   // F3 F4
                    else
                    {
                        Apply(Transition.Abort);   // read.5 write.5 A2 A3
                        return Status.ErrorTimeout;
                    }
                }
                return _stored;   // read.6 write.6
            });
        }

        public Status Abort() => Call(() =>
        {
            if (_state == Destroyed)
                return Status.ErrorNotFound;   // abort.1
            Apply(Transition.Abort);   // abort.2 A1-A5
            return Status.Ok;
        });

        public Status UnregisterCallback() => Call(() =>
        {
            if (_state == Destroyed || _callback == null)
                return Status.ErrorNotFound;   // unregister_callback.1
            _callback = null;   // unregister_callback.2
            _raised.Clear();
            return Status.Ok;
        });

        public Status Destroy() => Call(() =>
        {
            if (_state == Destroyed)
                return Status.ErrorNotFound;   // destroy.1
            Apply(Transition.Destroy);   // destroy.2 D1-D5
            return Status.Ok;
        });

        public void Finish(Status s) => Call(() =>
        {
            if (InFlight) Apply(Transition.Finish, s);
            return 0;
        });

        public void Raise(Event e, Status s)
        {
            lock (_gate) _callback?.Invoke(_handle, (int)e, (int)s, IntPtr.Zero);
        }

        void Raised(Event e, Status s) => _raised.Add((e, s));

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
            _stored = AnyOf(Status.Ok, Status.ErrorAbort);
        }

        static T AnyOf<T>(params T[] possibilities) => possibilities[Generator.Create().NextInteger(possibilities.Length)];

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
