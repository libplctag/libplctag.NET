using System;
using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag.Tests
{
    public class AsyncTests
    {

        readonly TimeSpan REALISTIC_LATENCY_FOR_READ = TimeSpan.FromMilliseconds(50);
        readonly TimeSpan REALISTIC_TIMEOUT_FOR_ALL_OPERATIONS = TimeSpan.FromMilliseconds(1000);

        [Fact]
        public async Task Cancelled_cancellation_token_throws_a_TaskCanceledException()
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
                await tag.InitializeAsync(cts.Token);
            });
        }


        [Fact]
        public async Task Timeout_throws_a_LibPlcTagException()
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


            // Assert
            var ex  = await Assert.ThrowsAsync<LibPlcTagException>(async () => {
                await tag.InitializeAsync();
            });

            Assert.Equal(Status.ErrorTimeout.ToString(), ex.Message);
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
            await tag.InitializeAsync();

            // Assert
            Assert.Equal(Status.Ok, tag.GetStatus());
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

            NativeImport.plctag.callback_func onReadCompleteCallback = null;

            var nativeTag = new Mock<INativeTag>();

            // The NativeTagWrapper should provide the native tag with a callback.
            // We will store this locally and call it ...
            nativeTag
                .Setup(m => m.plc_tag_register_callback(It.IsAny<int>(), It.IsAny<NativeImport.plctag.callback_func>()))
                .Callback<int, NativeImport.plctag.callback_func>((tagId, callbackFunc) => onReadCompleteCallback = callbackFunc);

            // ... when a create call occurs, and ..
            nativeTag
                .Setup(m => m.plc_tag_create(It.IsAny<string>(), 0))
                .Callback<string, int>(async (attributeString, timeout) =>
                {
                    await Task.Delay(REALISTIC_LATENCY_FOR_READ);
                    onReadCompleteCallback?.Invoke(tagId, (int)NativeImport.EVENT_CODES.PLCTAG_EVENT_READ_COMPLETED, (int)NativeImport.STATUS_CODES.PLCTAG_STATUS_OK);
                });

            // ... when a read call occurs
            nativeTag
                .Setup(m => m.plc_tag_read(It.IsAny<int>(), 0))
                .Callback<int, int>(async (tagId, timeout) =>
                {
                    await Task.Delay(REALISTIC_LATENCY_FOR_READ);
                    onReadCompleteCallback?.Invoke(tagId, (int)NativeImport.EVENT_CODES.PLCTAG_EVENT_READ_COMPLETED, (int)NativeImport.STATUS_CODES.PLCTAG_STATUS_OK);
                });

            return nativeTag;
        }

    }
}
