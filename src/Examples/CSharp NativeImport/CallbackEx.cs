using System;
using System.Runtime.InteropServices;
using System.Threading;
using libplctag.NativeImport;
using libplctag.NativeImport.Common;

namespace NativeImport_Examples
{
    public struct MyUserDataClass
    {
        public int Value;
    }

    class CallbackEx
    {
        
        public static void Run()
        {

            // Create the tag handle
            var tagHandle = plctag.plc_tag_create("protocol=ab_eip&gateway=127.0.0.1&path=1,0&plc=LGX&elem_size=4&elem_count=1&name=MyTag[0]", 1000);
            var statusBeforeRead = plctag.plc_tag_status(tagHandle);
            if (statusBeforeRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusBeforeRead}");
            }
            while (plctag.plc_tag_status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }


            // Create some user data and pin it in memory
            var myUserData = new MyUserDataClass();
            var myUserDataPtr = GCHandle.Alloc(myUserData, GCHandleType.Pinned);


            // ... so that we can pass it to our callback, 
            var myCallback = new Delegates.callback_func_ex(MyCallback);
            var statusAfterRegistration = plctag.plc_tag_register_callback_ex(tagHandle, myCallback, (IntPtr)myUserDataPtr);
            if (statusAfterRegistration != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRegistration}");
            }

            plctag.plc_tag_read(tagHandle, 1000);
            while (plctag.plc_tag_status(tagHandle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusAfterRead = plctag.plc_tag_status(tagHandle);
            if (statusAfterRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRead}");
            }

            plctag.plc_tag_destroy(tagHandle);
            
            myUserDataPtr.Free();

        }

        public static void MyCallback(int tag_id, int event_id, int status, IntPtr userDataPtr)
        {
            // ... and retrieve it later
            var gcHandle = GCHandle.FromIntPtr(userDataPtr);
            var userData = (MyUserDataClass)gcHandle.Target;

            Console.WriteLine($"Tag Id: {tag_id}    Event Id: {event_id}    Status: {status}     User Data: {userData.Value}");
        }

    }
}
