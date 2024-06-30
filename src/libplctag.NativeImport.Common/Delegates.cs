using System;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport.Common
{
    public static class Delegates
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callback_func(Int32 tag_id, Int32 event_id, Int32 status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callback_func_ex(Int32 tag_id, Int32 event_id, Int32 status, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void log_callback_func(Int32 tag_id, int debug_level, [MarshalAs(UnmanagedType.LPStr)] string message);
    }
}
