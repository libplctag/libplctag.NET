using libplctag;
using libplctag.DataTypes;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class LoggingExample
    {
        public static void Run()
        {
            LibPlcTag.LogEvent += LibPlcTag_LogEvent;
            LibPlcTag.DebugLevel = DebugLevel.Detail;

            //Instantiate the tag with the proper mapper and datatype
            var myTag = new Tag<DintPlcMapper, int>()
            {
                Name = "MyTag[0]",
                Gateway = "127.0.0.1",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5),
            };

            //Initialize the tag to set up structures and prepare for read/write
            //This is optional as an optimization before using the tag
            //If omitted, the tag will initialize on the first Read() or Write()
            myTag.Initialize();

            //The value is held locally and only synchronized on Read() or Write()
            myTag.Value = 3737;

            //Transfer Value to PLC
            myTag.Write();

            //Transfer from PLC to Value
            myTag.Read();

            //Write to console
            int myDint = myTag.Value;
            Console.WriteLine(myDint);
        }

        private static void LibPlcTag_LogEvent(object sender, LogEventArgs e)
        {
            Console.WriteLine($"{e.DebugLevel}\t\t{e.Message}");
        }
    }
}
