using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("libplctag.Tests")]

namespace libplctag
{
    class TestTag : Tag
    {
        public TestTag(INativeMethods nativeMethods)
            :base(nativeMethods)
        {

        }
    }
}
