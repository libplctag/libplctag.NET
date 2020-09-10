using libplctag;
using libplctag.DataTypes;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class SimpleExample
    {
        public static void Run()
        {
            //This is the absolute most simplified example code
            //Please see the other examples for more features/optimizations

            //Instantiate the tag with the proper mapper and datatype
            var myTag = new Tag<DintPlcMapper, int>()
            {
                Name = "PROGRAM:SomeProgram.SomeDINT",
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };

            //Write value to PLC
            //This will call Initialize internally since it's the first use of this tag
            //myTag.Value will be set to 3737 before being transferred to PLC
            myTag.Write(3737);

            //Read value from PLC
            //Value will also be accessible at myTag.Value
            int myDint = myTag.Read();

            //Write to console
            Console.WriteLine(myDint);
        }
    }
}
