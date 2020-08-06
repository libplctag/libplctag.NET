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

            var myTag = new Tag()
            {
                Name = "PROGRAM:SomeProgram.SomeDINT",
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                ElementSize = 4,
                Timeout = TimeSpan.FromSeconds(5)
            };

            myTag.Initialize();

            myTag.SetInt32(0, 3737);

            myTag.Write();

            myTag.Read();

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);
        }
    }
}
