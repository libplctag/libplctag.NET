using System;

namespace CSharpDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDatatypes.Run();
            ExampleAsync.SyncAsyncMultipleTagComparison();
            ExampleAsync.AsyncParallelCancellation();
            ExampleArray.Run();
            ExampleListTags.Run();
            ExampleArray.Run();
            NativeImportExample.Run();
            NativeImportExample.RunCallbackExample();
            NativeImportExample.RunLoggerExample();
            Console.ReadKey();
        }
    }
}