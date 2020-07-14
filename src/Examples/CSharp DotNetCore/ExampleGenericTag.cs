using libplctag;
using libplctag.Generic;
using System;
using System.Net;
using System.Threading;

namespace ExampleConsoleApp
{
    class ExampleGenericTag
    {
        public static void Run()
        {
            var myTag = new GenericTag<PlcTypeDINT, int>(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, "PROGRAM:SomeProgram.SomeDINT");
            while (myTag.GetStatus() == StatusCode.StatusPending)
            {
                Thread.Sleep(100);
            }
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.Value = 3737;
            myTag.Write(TimeSpan.Zero);
            while (myTag.GetStatus() == StatusCode.StatusPending)
            {
                Thread.Sleep(100);
            }
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());


            myTag.Read(TimeSpan.Zero);
            while (myTag.GetStatus() == StatusCode.StatusPending)
            {
                Thread.Sleep(100);
            }
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            int myDint = myTag.Value;

            Console.WriteLine(myDint);
        }
    }
}
