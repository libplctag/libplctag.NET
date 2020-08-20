using libplctag;
using libplctag.DataTypes;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleArray
    {
        public static void Run()
        {
            Console.WriteLine($"\r\n*** ExampleArray ***");

            //DINT Test Read/Write
            const int ARRAY_LENGTH = 5;
            const int TIMEOUT = 1000;
            const string gateway = "10.10.10.10";
            const string path = "1,0";

            //Generics version
            var dintTag = new Tag<DintMarshaller, int[]>()
            {
                Name = "TestDINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                ArrayLength = 5,
                Timeout=TimeSpan.FromSeconds(3),
            };
            dintTag.Initialize();
            dintTag.Read();
            //Read back value from local memory
            for (int i = 0; i < ARRAY_LENGTH; i++)
            {
                Console.WriteLine($"Value[{i}]: {dintTag.Value[i]}");
            }

            var myArrayTag = new Tag()
            {
                Name = "TestArray",
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                ElementSize = 4,
                ElementCount = ARRAY_LENGTH,
                Timeout = TimeSpan.FromMilliseconds(TIMEOUT),
            };

            myArrayTag.Initialize();

            //Read tag value - This pulls the value from the PLC into the local Tag value
            Console.WriteLine($"Starting tag read");
            myArrayTag.Read();

            //Read back value from local memory
            for (int i = 0; i < ARRAY_LENGTH; i++)
            {
                int arrayDint = myArrayTag.GetInt32(i * 4);
                Console.WriteLine($"Value[{i}]: {arrayDint}");
            }

        }

    }
}
