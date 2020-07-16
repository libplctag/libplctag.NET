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
            var myTag = new Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, DataType.DINT, "PROGRAM:SomeProgram.SomeDINT", 5000);

            while (myTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.SetInt32(0, 3737);
            myTag.Write(0);

            while (myTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.Read(0);

            while (myTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);
        }
    }
}
