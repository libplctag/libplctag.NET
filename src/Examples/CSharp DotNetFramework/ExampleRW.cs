using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetFramework
{
    class ExampleRW
    {
        public static void Run()
        {
            Console.WriteLine($"\r\n*** ExampleRW ***");

            //DINT Test Read/Write
            var myTag = new Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, DataType.DINT, "PROGRAM:SomeProgram.SomeDINT", 5000);

            //Check that tag gets created properly
            while (myTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());
            Console.WriteLine($"Tag created and verified on PLC");


            //Read tag value - This pulls the value from the PLC into the local Tag value
            Console.WriteLine($"Starting tag read");
            myTag.Read(0);

            //Wait for Read to complete
            while (myTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());
            Console.WriteLine($"Tag read complete");

            //Read back value from local memory
            int myDint = myTag.GetInt32(0);
            Console.WriteLine($"Initial Value: {myDint}");

            //Set Tag Value
            myDint++;
            myTag.SetInt32(0, myDint);
            myTag.Write(0);
            Console.WriteLine($"Starting tag write ({myDint})");

            //Wait for Write to complete
            while (myTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());
            Console.WriteLine($"Tag write complete");

            //Read tag value - This pulls the value from the PLC into the local Tag value
            Console.WriteLine($"Starting synchronous tag read");
            myTag.Read(1000);

            //Check read success
            if (myTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myTag.GetStatus());
            Console.WriteLine($"Synchronous tag read complete");

            //Read back value from local memory
            var myDintReadBack = myTag.GetInt32(0);
            Console.WriteLine($"Final Value: {myDintReadBack}");

        }
    }
}
