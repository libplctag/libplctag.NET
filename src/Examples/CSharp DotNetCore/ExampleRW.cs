using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleRW
    {
        public static void Run()
        {
            Console.WriteLine($"\r\n*** ExampleRW ***");

            const int TIMEOUT = 5000;

            //DINT Test Read/Write
            var myTag = new Tag()
            {
                Name = "PROGRAM:SomeProgram.SomeDINT",
                ElementSize = 4,
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };
            myTag.Initialize(TIMEOUT);

            //Read tag value - This pulls the value from the PLC into the local Tag value
            Console.WriteLine($"Starting tag read");
            myTag.Read(TIMEOUT);

            //Read back value from local memory
            int myDint = myTag.GetInt32(0);
            Console.WriteLine($"Initial Value: {myDint}");

            //Set Tag Value
            myDint++;
            myTag.SetInt32(0, myDint);

            Console.WriteLine($"Starting tag write ({myDint})");
            myTag.Write(TIMEOUT);

            //Read tag value - This pulls the value from the PLC into the local Tag value
            Console.WriteLine($"Starting synchronous tag read");
            myTag.Read(TIMEOUT);

            //Read back value from local memory
            var myDintReadBack = myTag.GetInt32(0);
            Console.WriteLine($"Final Value: {myDintReadBack}");

        }
    }
}
