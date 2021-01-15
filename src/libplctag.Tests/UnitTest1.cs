using System;
using Xunit;

namespace libplctag.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void Status_ok_when_first_created()
        {
            var tag = new TestTag(new MockNativeMethods());

            Assert.Equal(Status.Ok, tag.GetStatus());
        }

        [Fact]
        public void Can_not_use_if_already_disposed()
        {
            var tag = new TestTag(new MockNativeMethods());

            tag.Dispose();

            Assert.Throws<ObjectDisposedException>(() => tag.GetStatus());
        }

        [Fact]
        public void AttributeStringFormatted()
        {

            var native = new MockNativeMethods();

            var tag = new TestTag(native)
            {
                ElementSize = 4,
                ElementCount = 10,
                PlcType = PlcType.Slc500,
                Name = "TagName"
            };

            tag.Initialize();

            Assert.Equal("protocol=&plc=slc500&elem_size=4&elem_count=10&name=TagName", native.AttributeString);

        }
    }
}
