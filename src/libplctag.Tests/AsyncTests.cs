using System;
using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag.Tests
{
    public class AsyncTests
    {

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
            cts.CancelAfter(500);

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
                Timeout = TimeSpan.FromMilliseconds(500)
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
            var nativeTag = new Mock<INativeTag>();

            nativeTag                                                       // The initial creation of the tag object returns a status, so we return pending
                .Setup(m => m.plc_tag_create(It.IsAny<string>(), 0))
                .Returns((int)Status.Pending);

            nativeTag
                .SetupSequence(m => m.plc_tag_status(It.IsAny<int>()))
                .Returns((int)Status.Pending)
                .Returns((int)Status.Pending)
                .Returns((int)Status.Pending)
                .Returns((int)Status.Pending)
                .Returns((int)Status.Pending)
                .Returns((int)Status.Pending)
                .Returns((int)Status.Pending)
                .Returns((int)Status.Ok);

            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                Timeout = TimeSpan.FromMilliseconds(500)
            };

            // Act
            await tag.InitializeAsync();

            // Assert
            Assert.Equal(Status.Ok, tag.GetStatus());
        }

    }
}
