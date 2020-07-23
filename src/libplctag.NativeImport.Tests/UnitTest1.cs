using System;
using Xunit;

namespace libplctag.plc_tag_NativeImport.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Dll_Load()
        {
            // The simplest way to check the Dll is working is use the decode error API which returns a string
            var output = plctag.plc_tag_decode_error(0);
            Assert.Equal("PLCTAG_STATUS_OK", output);
        }
    }
}
