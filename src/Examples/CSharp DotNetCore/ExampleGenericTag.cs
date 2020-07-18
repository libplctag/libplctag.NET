using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CSharpDotNetCore
{
    class ExampleGenericTag
    {
        public static void Run()
        {
            var MyDintArray = new Tag<DINTIntMarshaller, int>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "MY_DINT_ARRAY_1000[0]", 1000, 1000);

            for (int ii = 0; ii < 1000; ii++)
                MyDintArray[ii] = ii;

            MyDintArray.Write(1000);

            var MyStringArray = new Tag<STRINGStringMarshaller, string>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "MY_STRING_ARRAY_10[0]", 1000, 10);

            for (int ii = 0; ii < 10; ii++)
                MyStringArray[ii] = (ii*1111).ToString();

            MyStringArray.Write(1000);

        }
    }

    
}
