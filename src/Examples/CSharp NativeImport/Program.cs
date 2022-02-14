using System;

namespace NativeImport_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //Multithread.main();
            //NativeImportExample.Run();
            //NativeImportExample.RunCallbackExample();
            CallbackEx.Run();
            Console.ReadKey();
        }
    }
}
