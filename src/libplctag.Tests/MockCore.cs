// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using static libplctag.NativeImport.plctag;

namespace libplctag.Tests
{
    /// <summary>
    /// A core made of <see cref="CoreModel"/>s: create_ex makes one per handle (1, 2, ...) and every call goes to it.
    /// The test plays the core's side on <see cref="Model"/>, the latest tag. It can still override a setup on
    /// <see cref="Native"/> for an outcome the model does not choose (a read served from the cache, ...).
    /// </summary>
    sealed class MockCore
    {
        public const int Handle = 1;   // the first tag created
        public readonly Mock<INative> Native = new Mock<INative>();
        public readonly CoreChoices Choose = new CoreChoices();

        readonly ConcurrentDictionary<int, CoreModel> _tags = new ConcurrentDictionary<int, CoreModel>();
        int _created;

        public MockCore()
        {
            Native.Setup(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()))
                  .Returns<string, callback_func_ex, IntPtr, int>((_, callback, __, timeout) =>
                  {
                      var handle = Interlocked.Increment(ref _created);
                      var tag = new CoreModel(handle, callback, Choose);
                      var result = tag.Create(timeout);
                      if (result != Status.Ok) return (int)result;   // create_ex.1: no handle
                      _tags[handle] = tag;
                      return handle;
                  });
            Native.Setup(n => n.plc_tag_status(It.IsAny<int>())).Returns<int>(h => (int)On(h, t => t.GetStatus()));
            Native.Setup(n => n.plc_tag_read(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((h, timeout) => (int)On(h, t => t.Read(timeout)));
            Native.Setup(n => n.plc_tag_write(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((h, timeout) => (int)On(h, t => t.Write(timeout)));
            Native.Setup(n => n.plc_tag_abort(It.IsAny<int>())).Returns<int>(h => (int)On(h, t => t.Abort()));
            Native.Setup(n => n.plc_tag_unregister_callback(It.IsAny<int>())).Returns<int>(h => (int)On(h, t => t.UnregisterCallback()));
            Native.Setup(n => n.plc_tag_destroy(It.IsAny<int>())).Returns<int>(h => (int)On(h, t => t.Destroy()));
        }

        /// <summary>A handle create_ex never returned is not found, like a destroyed one.</summary>
        Status On(int handle, Func<CoreModel, Status> call) => _tags.TryGetValue(handle, out var t) ? call(t) : Status.ErrorNotFound;

        /// <summary>The latest native tag, or null before create_ex has returned one.</summary>
        public CoreModel Model => _tags.TryGetValue(Volatile.Read(ref _created), out var t) ? t : null;

        /// <summary>
        /// The tickler, on a task of its own: it finishes the next <paramref name="count"/> creations or operations
        /// with <paramref name="s"/> as each starts, and stops early once <paramref name="call"/> has ended. It then
        /// stops for good, so a call left waiting is a deadlock that Coyote reports.
        /// </summary>
        public Task Tickler(Task call, int count = 1, Status s = Status.Ok) => Task.Run(async () =>
        {
            for (var i = 0; i < count; i++)
            {
                while (Model?.GetStatus() != Status.Pending)
                {
                    if (call.IsCompleted) return;
                    await Task.Yield();
                }
                Model.Finish(s);
            }
        });

        /// <summary>A new tag on this core. Its Timeout is long, so that a test that hangs fails by its xUnit Timeout instead.</summary>
        public Tag NewTag() => new Tag(Native.Object) { Timeout = TimeSpan.FromDays(1) };
    }
}
