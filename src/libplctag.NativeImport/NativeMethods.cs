using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libplctag.NativeImport
{
    static class NativeMethods
    {

        const string DLL_NAME = "plctag";



        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_check_lib_version), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_create), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern Int32 plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_destroy), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_destroy(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_shutdown), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_shutdown();


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_register_callback), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_register_callback(Int32 tag_id, plctag.callback_func func);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_unregister_callback), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_unregister_callback(Int32 tag_id);


        


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_register_logger), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_register_logger(plctag.log_callback_func func);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_unregister_logger), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_unregister_logger(Int32 tag_id);


        


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_lock), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_lock(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_unlock), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_unlock(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_status), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_status(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_decode_error), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr plc_tag_decode_error(int err);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_read), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_read(Int32 tag, int timeout);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_write), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_write(Int32 tag, int timeout);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_size), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_size(Int32 tag);

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_size), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_size(Int32 tag, int new_size);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_abort), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_abort(Int32 tag);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_int_attribute), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern int plc_tag_get_int_attribute(Int32 tag, [MarshalAs(UnmanagedType.LPStr)] string attrib_name, int default_value);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_int_attribute), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
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



        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_string), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern int plc_tag_get_string(Int32 tag_id, int string_start_offset, StringBuilder buffer, int buffer_length);

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_string), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
        public static extern int plc_tag_set_string(Int32 tag_id, int string_start_offset, [MarshalAs(UnmanagedType.LPStr)] string string_val);

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_string_length), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_string_length(Int32 tag_id, int string_start_offset);

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_string_capacity), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_string_capacity(Int32 tag_id, int string_start_offset);

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_string_total_length), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_string_total_length(Int32 tag_id, int string_start_offset);


        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_get_raw_bytes), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_get_raw_bytes(Int32 tag_id, int start_offset, [Out] byte[] buffer, int buffer_length);

        [DllImport(DLL_NAME, EntryPoint = nameof(plc_tag_set_raw_bytes), CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int plc_tag_set_raw_bytes(Int32 tag_id, int start_offset, [In] byte[] buffer, int buffer_length);
    }
}
