using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;


namespace libplctag.Tests
{
    class MockTag : NativeTagWrapper
    {

        public MockTag()
            :base(new MockNativeTag())
        {

        }

        public MockTag(INativeTag nativeTag)
            : base(nativeTag)
        {

        }

    }
}
