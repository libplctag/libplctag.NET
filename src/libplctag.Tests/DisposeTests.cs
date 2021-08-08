using System;
using Xunit;
using Moq;

namespace libplctag.Tests
{
    public class DisposeTests
    {

        [Fact]
        public void Destroy_is_called_if_initialized_and_disposed()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object);

            // Act
            tag.Initialize();
            tag.Dispose();

            // Assert
            nativeTag.Verify(m => m.plc_tag_destroy(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Can_not_use_if_already_disposed()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object);

            // Act
            tag.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => tag.GetStatus());
        }

        [Fact(Skip = "The finalizer is no longer called because the Mock is holding a reference to a callback defined on the Tag. This would not happen outside of unit tests.")]
        public void Finalizer_calls_destroy()
        {

            // See https://www.inversionofcontrol.co.uk/unit-testing-finalizers-in-csharp/


            // Arrange
            var nativeTag = new Mock<INativeTag>();
            Action dispose = () =>
            {
                // This will go out of scope after dispose() is executed, so the garbage collector will be able to call the finalizer
                var tag = new NativeTagWrapper(nativeTag.Object);
                tag.Initialize();
            };

            // Act
            dispose();
            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            // Assert
            nativeTag.Verify(m => m.plc_tag_destroy(It.IsAny<int>()), Times.Once);
        }

    }
}
