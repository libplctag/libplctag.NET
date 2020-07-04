using System;
using System.Threading;
using libplctag.NativeImport;

namespace ExampleConsoleApp
{
    class NativeImportExample
    {
        public static void Run()
        {

            var tagHandle = plctag.create("protocol=ab_eip&gateway=192.168.0.10&path=1,0&cpu=LGX&elem_size=4&elem_count=1&name=MY_DINT", 1000);

            while (plctag.status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusBeforeRead = plctag.status(tagHandle);
            if (statusBeforeRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusBeforeRead}");
            }

            plctag.read(tagHandle, 1000);
            while (plctag.status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusAfterRead = plctag.status(tagHandle);
            if (statusAfterRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRead}");
            }

            var theValue = plctag.get_uint32(tagHandle, 0);

            plctag.destroy(tagHandle);

            Console.WriteLine(theValue);
        }

        public static void RunCallbackExample()
        {

            var tagHandle = plctag.create("protocol=ab_eip&gateway=192.168.0.10&path=1,0&cpu=LGX&elem_size=4&elem_count=1&name=MY_DINT", 1000);

            while (plctag.status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusBeforeRead = plctag.status(tagHandle);
            if (statusBeforeRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusBeforeRead}");
            }

            var myCallback = new plctag.callback_func(MyCallback);
            var statusAfterRegistration = plctag.register_callback(tagHandle, myCallback);
            if (statusAfterRegistration != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRegistration}");
            }

            plctag.read(tagHandle, 1000);
            while (plctag.status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusAfterRead = plctag.status(tagHandle);
            if (statusAfterRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRead}");
            }

            var theValue = plctag.get_uint32(tagHandle, 0);

            plctag.destroy(tagHandle);

            Console.WriteLine(theValue);
        }

        public static void MyCallback(int tag_id, int event_id, int status)
        {
            Console.WriteLine($"Tag Id: {tag_id}    Event Id: {event_id}    Status: {status}");
        }

        public static void RunLoggerExample()
        {
            var myLogger = new plctag.log_callback_func(MyLogger);
            var statusAfterRegistration = plctag.register_logger(myLogger);
            if (statusAfterRegistration != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRegistration}");
            }

            var tagHandle = plctag.create("protocol=ab_eip&gateway=192.168.0.10&path=1,0&cpu=LGX&elem_size=4&elem_count=1&name=MY_DINT&debug=4", 1000);

            while (plctag.status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusBeforeRead = plctag.status(tagHandle);
            if (statusBeforeRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusBeforeRead}");
            }

            plctag.read(tagHandle, 1000);
            while (plctag.status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusAfterRead = plctag.status(tagHandle);
            if (statusAfterRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRead}");
            }

            var theValue = plctag.get_uint32(tagHandle, 0);

            plctag.destroy(tagHandle);

            Console.WriteLine(theValue);

        }

        public static void MyLogger(int tag_id, int debug_level, string message)
        {
            Console.WriteLine($"Tag Id: {tag_id}    Debug Level: {debug_level}    Message: {message}");
        }
    }
}
