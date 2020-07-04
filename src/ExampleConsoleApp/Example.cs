using libplctag;
using System;
using System.Net;
using System.Threading;

namespace ExampleConsoleApp
{
    class Example
    {
        public static void Run()
        {
            var myTag = new Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuTypes.LGX, DataTypes.DINT, "PROGRAM:SomeProgram.SomeDINT");
            while (myTag.GetStatus() == StatusCode.PLCTAG_STATUS_PENDING)
            {
                Thread.Sleep(100);
            }
            myTag.ThrowIfError();

            myTag.SetInt32(0, 3737);
            myTag.Write(TimeSpan.Zero);
            while (myTag.GetStatus() == StatusCode.PLCTAG_STATUS_PENDING)
            {
                Thread.Sleep(100);
            }
            myTag.ThrowIfError();


            myTag.Read(TimeSpan.Zero);
            while (myTag.GetStatus() == StatusCode.PLCTAG_STATUS_PENDING)
            {
                Thread.Sleep(100);
            }
            myTag.ThrowIfError();

            int myDint = myTag.GetInt32(0);


            Console.WriteLine(myDint);
        }
    }
}
