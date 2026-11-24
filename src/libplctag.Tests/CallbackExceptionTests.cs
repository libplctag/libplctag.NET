// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Concurrent;
using Moq;
using Xunit;
using libplctag.NativeImport;

namespace libplctag.Tests
{
    // https://github.com/libplctag/libplctag.NET/issues/511
    public class CallbackExceptionTests
    {
        [Fact]
        public void Exception_thrown_by_event_handler_does_not_escape_the_native_callback()
        {
            const int tagId = 11;
            plctag.callback_func_ex callback = null;
            var native = new Mock<INative>();
            native
                .Setup(m => m.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()))
                .Callback<string, plctag.callback_func_ex, IntPtr, int>((_, cb, __, ___) => callback = cb)
                .Returns(tagId);

            var observed = new ConcurrentQueue<Exception>();
            var thrown = new InvalidOperationException();
            var tag = new Tag(native.Object) { onCallbackException = observed.Enqueue };
            tag.Aborted += (s, e) => throw thrown;
            tag.Initialize();

            var ex = Record.Exception(() => callback(tagId, (int)EVENT.PLCTAG_EVENT_ABORTED, (int)STATUS.PLCTAG_STATUS_OK, IntPtr.Zero));

            Assert.Null(ex);
            Assert.Equal(new Exception[] { thrown }, observed);
        }
    }
}
