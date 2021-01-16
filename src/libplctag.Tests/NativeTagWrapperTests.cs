using System;
using Xunit;

namespace libplctag.Tests
{
    public class NativeTagWrapperTests
    {

        [Fact]
        public void Status_ok_when_first_created()
        {
            var tag = new NativeTagWrapper(new MockNativeTag());

            var status = tag.GetStatus();

            Assert.Equal(Status.Ok, status);
        }

        [Fact]
        public void Can_not_use_if_already_disposed()
        {
            var tag = new NativeTagWrapper(new MockNativeTag());

            tag.Dispose();

            Assert.Throws<ObjectDisposedException>(() => tag.GetStatus());
        }

        [Fact]
        public void AttributeStringFormatted()
        {

            var nativeTag = new MockNativeTag();

            var tag = new NativeTagWrapper(nativeTag)
            {
                ElementSize = 4,
                ElementCount = 10,
                PlcType = PlcType.Slc500,
                Name = "TagName",
                Protocol = Protocol.ab_eip
            };

            tag.Initialize();

            Assert.Equal("protocol=ab_eip&plc=slc500&elem_size=4&elem_count=10&name=TagName", nativeTag.AttributeString);

        }
    }
}
