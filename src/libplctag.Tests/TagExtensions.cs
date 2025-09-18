using System;
using System.Runtime.InteropServices;

namespace libplctag.Tests
{
    public static class TagExtensions
    {
        public static T GetValue<T>(this Tag tag) where T : struct
        {
            byte[] buffer = tag.GetBuffer();
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            try
            {
                //ToInt64 is used because it's assumed the code will compile and run on 64-bit platform.
                //If it's going to run on a 32-platform, ToInt32 should be used.
                T retVal = Marshal.PtrToStructure<T>(new IntPtr(handle.AddrOfPinnedObject().ToInt64()))!;

                return (retVal);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}