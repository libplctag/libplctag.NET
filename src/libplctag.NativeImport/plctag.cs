using System;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport
{
    public static class plctag
    {

        const string DLL_NAME = "plctag";

        static plctag()
        {
            LibraryExtractor.Init();
        }


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_check_lib_version), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_create), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Int32 plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_destroy), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_destroy(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_shutdown), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_shutdown();


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_register_callback), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_register_callback(Int32 tag_id, callback_func func);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_unregister_callback), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_unregister_callback(Int32 tag_id);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callback_func(Int32 tag_id, Int32 event_id, Int32 status);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_register_logger), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_register_logger(log_callback_func func);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_unregister_logger), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_unregister_logger(Int32 tag_id);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void log_callback_func(Int32 tag_id, int debug_level, [MarshalAs(UnmanagedType.LPStr)] string message);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_lock), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_lock(Int32 tag);
        

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_unlock), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_unlock(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_status), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_status(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_decode_error), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        static extern IntPtr plc_tag_decode_error_raw(int error);
        public static string plc_tag_decode_error(int error) => Marshal.PtrToStringAnsi(plc_tag_decode_error_raw(error));

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_read), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_read(Int32 tag, int timeout);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_write), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_write(Int32 tag, int timeout);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_size), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_size(Int32 tag);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_abort), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_abort(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_int_attribute), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_int_attribute(Int32 tag, [MarshalAs(UnmanagedType.LPStr)] string attrib_name, int default_value);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_int_attribute), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_int_attribute(Int32 tag, [MarshalAs(UnmanagedType.LPStr)] string attrib_name, int new_value);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_uint64), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern UInt64 plc_tag_get_uint64(Int32 tag, int offset);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_int64), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Int64 plc_tag_get_int64(Int32 tag, int offset);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_uint64), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_int64), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_int64(Int32 tag, int offset, Int64 val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_float64), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern double plc_tag_get_float64(Int32 tag, int offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_float64), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_float64(Int32 tag, int offset, double val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_uint32), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern UInt32 plc_tag_get_uint32(Int32 tag, int offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_int32), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Int32 plc_tag_get_int32(Int32 tag, int offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_uint32), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_int32), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_int32(Int32 tag, int offset, Int32 val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_float32), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern float plc_tag_get_float32(Int32 tag, int offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_float32), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_float32(Int32 tag, int offset, float val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_uint16), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern UInt16 plc_tag_get_uint16(Int32 tag, int offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_int16), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern Int16 plc_tag_get_int16(Int32 tag, int offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_uint16), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_int16), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_int16(Int32 tag, int offset, Int16 val);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_uint8), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern byte plc_tag_get_uint8(Int32 tag, int offset);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_int8), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern sbyte plc_tag_get_int8(Int32 tag, int offset);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_uint8), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_uint8(Int32 tag, int offset, byte val);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_int8), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_int8(Int32 tag, int offset, sbyte val);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_bit), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_bit(Int32 tag, int offset_bit);

        
        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_bit), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_bit(Int32 tag, int offset_bit, int val);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_debug_level), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern void plc_tag_set_debug_level(int debug_level);


    }
}