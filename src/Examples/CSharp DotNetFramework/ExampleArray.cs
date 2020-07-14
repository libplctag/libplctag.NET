using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetFramework
{
    class ExampleArray
    {
        public static void Run()
        {
            Console.WriteLine($"\r\n*** ExampleArray ***");

            //DINT Test Read/Write
            const int ARRAY_LENGTH = 5;
            var myArrayTag = new Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, DataType.DINT, "TestArray", ARRAY_LENGTH);

            //Check that tag gets created properly
            while (myArrayTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myArrayTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myArrayTag.GetStatus());
            Console.WriteLine($"Tag created and verified on PLC");


            //Read tag value - This pulls the value from the PLC into the local Tag value
            Console.WriteLine($"Starting tag read");
            myArrayTag.Read(0);

            //Wait for Read to complete
            while (myArrayTag.GetStatus() == Status.Pending)
                Thread.Sleep(100);
            if (myArrayTag.GetStatus() != Status.Ok)
                throw new LibPlcTagException(myArrayTag.GetStatus());
            Console.WriteLine($"Tag read complete");

            //Read back value from local memory
            for (int i = 0; i < ARRAY_LENGTH; i++)
            {
                int arrayDint = myArrayTag.GetInt32(i* DataType.DINT);
                Console.WriteLine($"Value[{i}]: {arrayDint}");
            }

        }
    }
}
