using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CSharpDotNetCore
{
    class ExampleGenericTag
    {
        public static void Run()
        {

            var timeout = 1000;

            //Bool - Test both cases
            //Random value would look correct 50% of the time
            var boolTag = new GenericTag<PlcTypeBOOL, bool>(gateway, Path, CpuType.Logix, "TestBOOL", timeout);
            TestTag(boolTag, true);
            TestTag(boolTag, false);

            //Signed Numbers
            TestTag(new GenericTag<PlcTypeSINT, sbyte>(gateway, Path, CpuType.Logix, "TestSINT", timeout));
            TestTag(new GenericTag<PlcTypeINT, short>(gateway, Path, CpuType.Logix, "TestINT", timeout));
            TestTag(new GenericTag<PlcTypeDINT, int>(gateway, Path, CpuType.Logix, "TestDINT", timeout));
            TestTag(new GenericTag<PlcTypeLINT, long>(gateway, Path, CpuType.Logix, "TestLINT", timeout));

            //Foating Points
            TestTag(new GenericTag<PlcTypeREAL, float>(gateway, Path, CpuType.Logix, "TestREAL", timeout));
            //TestTag(new GenericTag<PlcTypeLREAL, double>(gateway, Path, CpuType.Logix, "TestLREAL", timeout));

        }
    }

    
}
