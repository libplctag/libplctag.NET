using libplctag;
using libplctag.Generic;
using libplctag.Generic.DataTypes;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleGenericTag
    {
        public static void Run()
        {
            var myTag = new GenericTag<PlcTypeDINT, int>(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, "PROGRAM:SomeProgram.SomeDINT", 1000);
            while (myTag.GetStatus() == Status.Pending)
            {
                Thread.Sleep(100);
            }
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.Value = 3737;
            myTag.Write(0);
            while (myTag.GetStatus() == Status.Pending)
            {
                Thread.Sleep(100);
            }
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());


            myTag.Read(0);
            while (myTag.GetStatus() == Status.Pending)
            {
                Thread.Sleep(100);
            }
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());

            int myDint = myTag.Value;

            Console.WriteLine(myDint);
        }
    }
}
