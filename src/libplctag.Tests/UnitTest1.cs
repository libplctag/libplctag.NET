using System;
using Xunit;

namespace libplctag.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            var mockNativeMethods = new MockNativeMethods();
            var x = new TestTag(mockNativeMethods);

        }
    }
}
