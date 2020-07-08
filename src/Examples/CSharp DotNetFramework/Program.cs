using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {

            var myTag = new Tag(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, DataType.DINT, "MY_DINT");

            while (myTag.GetStatus() == StatusCode.StatusPending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.SetInt32(0, 3737);
            myTag.Write(TimeSpan.Zero);

            while (myTag.GetStatus() == StatusCode.StatusPending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.Read(TimeSpan.Zero);
            while (myTag.GetStatus() == StatusCode.StatusPending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);

        }
    }
}
