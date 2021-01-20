using System;
using Xunit;
using Moq;

namespace libplctag.Tests
{
    public class OtherTests
    {

        [Fact]
        public void Status_ok_when_first_created()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object);

            // Act

            // Assert
            var status = tag.GetStatus();
            Assert.Equal(Status.Ok, status);
        }

        [Fact]
        public void AttributeStringFormatted()
        {
            // Arrange
            var nativeTag = new Mock<INativeTag>();
            var tag = new NativeTagWrapper(nativeTag.Object)
            {
                ElementSize = 4,
                ElementCount = 10,
                PlcType = PlcType.Slc500,
                Name = "TagName",
            };


            // Act
            tag.Initialize();


            // Assert
            nativeTag.Verify(m => m.plc_tag_create("plc=slc500&elem_size=4&elem_count=10&name=TagName", It.IsAny<int>()), Times.Once);

        }
    }
}
