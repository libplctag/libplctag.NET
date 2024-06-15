// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag.Tests
{
    public class AsyncTests
    {

        readonly TimeSpan REALISTIC_LATENCY_FOR_CREATE = TimeSpan.FromMilliseconds(50);
        readonly TimeSpan REALISTIC_LATENCY_FOR_READ = TimeSpan.FromMilliseconds(50);
        readonly TimeSpan REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS = TimeSpan.FromMilliseconds(1000);

        [Fact]
        public async Task Cancelled_cancellation_token_throws_a_TaskCanceledException()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();

            nativeTag                                                       // The initial creation of the tag object returns a status, so we return pending
                .Setup(m => m.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), 0))
                .Returns((int)Status.Pending);

            nativeTag                                                       // Subsequent calls to determine the tag status should still return pending
                .Setup(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns((int)Status.Pending);

            var tag = new NativeTagWrapper(nativeTag.Object);
            var cts = new CancellationTokenSource();

            // Act, Assert
            cts.CancelAfter(REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS);
            await Assert.ThrowsAsync<TaskCanceledException>(async () => {
                await tag.InitializeAsync(cts.Token);
            });
        }


        [Fact]
        public async Task Timeout_throws_a_LibPlcTagException()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();

            nativeTag                                                       // The initial creation of the tag object returns a status, so we return pending
                .Setup(m => m.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), 0))
                .Returns((int)Status.Pending);

            nativeTag                                                       // Subsequent calls to determine the tag status should still return pending
                .Setup(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns((int)Status.Pending);

            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                Timeout = REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS
            };

            // Act
            var ex  = await Assert.ThrowsAsync<LibPlcTagException>(async () => {
                await tag.InitializeAsync();
            });

            // Assert
            Assert.Equal(Status.ErrorTimeout.ToString(), ex.Message);
        }

        [Fact]
        public async Task TryInitialize_returns_an_ErrorTimeout()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();

            nativeTag                                                       // The initial creation of the tag object returns a status, so we return pending
                .Setup(m => m.plc_tag_create(It.IsAny<string>(), 0))
                .Returns((int)Status.Pending);

            nativeTag                                                       // Subsequent calls to determine the tag status should still return pending
                .Setup(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns((int)Status.Pending);

            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                Timeout = REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS
            };

            // Act
            var result = await tag.TryInitializeAsync();

            // Assert
            Assert.Equal(Status.ErrorTimeout, result);
        }

        [Fact]
        public async Task TryInitialize_Cancelled_cancellation_token_throws_a_TaskCanceledException()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();

            nativeTag                                                       // The initial creation of the tag object returns a status, so we return pending
                .Setup(m => m.plc_tag_create(It.IsAny<string>(), 0))
                .Returns((int)Status.Pending);

            nativeTag                                                       // Subsequent calls to determine the tag status should still return pending
                .Setup(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns((int)Status.Pending);

            var tag = new NativeTagWrapper(nativeTag.Object);
            var cts = new CancellationTokenSource();

            // Act
            cts.CancelAfter(REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS);

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(async () => {
                await tag.TryInitializeAsync(cts.Token);
            });
        }

        [Fact]
        public async Task Timeout_returns_pending_but_eventually_ok()
        {
            // Arrange
            var nativeTag = GetMock();

            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                Timeout = REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS
            };

            // Act
            var task = tag.InitializeAsync();
            var statusWhileWaiting = tag.GetStatus();
            await task;
            var statusAfterAwaited = tag.GetStatus();

            // Assert
            Assert.Equal(Status.Pending, statusWhileWaiting);
            Assert.Equal(Status.Ok, statusAfterAwaited);
        }

        [Fact]
        public async Task AsyncRead_completes_within_timeout_period()
        {
            // Arrange
            var nativeTag = GetMock();

            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                Timeout = REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS
            };

            // Act
            await tag.ReadAsync();

            // Assert
            Assert.Equal(Status.Ok, tag.GetStatus());
        }


        Mock<INativeTag> GetMock()
        {
            const int tagId = 11;

            NativeImport.plctag.callback_func_ex callback = null;
            Status? status = null;

            var nativeTag = new Mock<INativeTag>();

            // The NativeTagWrapper should provide the native tag with a callback.
            // We will store this locally when a create call occurs, and fire it shortly after ...
            nativeTag
                .Setup(m => m.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<NativeImport.plctag.callback_func_ex>(), It.IsAny<IntPtr>(), 0))
                .Callback<string, NativeImport.plctag.callback_func_ex, IntPtr, int>(async (attributeString, callbackFunc, userData, timeout) =>
                {
                    status = Status.Pending;
                    callback = callbackFunc;
                    await Task.Delay(REALISTIC_LATENCY_FOR_CREATE);
                    status = Status.Ok;
                    callback?.Invoke(tagId, (int)NativeImport.EVENT_CODES.PLCTAG_EVENT_CREATED, (int)NativeImport.STATUS_CODES.PLCTAG_STATUS_OK, IntPtr.Zero);
                });

            // ... as well as when a read call occurs
            nativeTag
                .Setup(m => m.plc_tag_read(It.IsAny<int>(), 0))
                .Callback<int, int>(async (tagId, timeout) =>
                {
                    status = Status.Pending;
                    callback?.Invoke(tagId, (int)NativeImport.EVENT_CODES.PLCTAG_EVENT_READ_STARTED, (int)NativeImport.STATUS_CODES.PLCTAG_STATUS_OK, IntPtr.Zero);
                    await Task.Delay(REALISTIC_LATENCY_FOR_READ);
                    status = Status.Ok;
                    callback?.Invoke(tagId, (int)NativeImport.EVENT_CODES.PLCTAG_EVENT_READ_COMPLETED, (int)NativeImport.STATUS_CODES.PLCTAG_STATUS_OK, IntPtr.Zero);
                });

            // the status was being tracked, so return it if asked
            nativeTag
                .Setup(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns(() => (int)status.Value);

            return nativeTag;
        }

    }
}
