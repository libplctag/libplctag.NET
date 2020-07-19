using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class Example
    {
        public static void Run()
        {

            const int TIMEOUT = 5000;

            var myTag = new Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, DataType.DINT, "PROGRAM:SomeProgram.SomeDINT", TIMEOUT);

            myTag.SetInt32(0, 3737);

            myTag.Write(TIMEOUT);

            myTag.Read(TIMEOUT);

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);
        }
    }
}
