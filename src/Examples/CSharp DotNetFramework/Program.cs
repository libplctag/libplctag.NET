using libplctag;
using System;

namespace CSharpDotNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            LibPlcTag.DebugLevel = DebugLevel.Info;
            ExampleRW.Run();
            ExampleArray.Run();

            Console.ReadKey();
        }
    }
} 