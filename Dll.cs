using System;
using System.Runtime.InteropServices;

namespace libplctag
{
    class Dll
    {

        [DllImport("plctag.dll", EntryPoint = "plc_tag_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_destroy(Int32 tag);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_status", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_status(Int32 tag);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_decode_error", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr plc_tag_decode_error(int error);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_read(Int32 tag, int timeout);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_write", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_write(Int32 tag, int timeout);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_get_size(Int32 tag);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_abort", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_abort(Int32 tag);

        /* 64-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint64", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 plc_tag_get_uint64(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 plc_tag_get_int64(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_int64(Int32 tag, int offset, Int64 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_float64", CallingConvention = CallingConvention.Cdecl)]
        public static extern double plc_tag_get_float64(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_float64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_float64(Int32 tag, int offset, double val);

        /* 32-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 plc_tag_get_uint32(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 plc_tag_get_int32(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_int32(Int32 tag, int offset, Int32 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_float32", CallingConvention = CallingConvention.Cdecl)]
        public static extern float plc_tag_get_float32(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_float32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_float32(Int32 tag, int offset, float val);

        /* 16-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint16", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 plc_tag_get_uint16(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int16", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 plc_tag_get_int16(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint16", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int16", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_int16(Int32 tag, int offset, Int16 val);

        /* 8-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint8", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte plc_tag_get_uint8(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int8", CallingConvention = CallingConvention.Cdecl)]
        public static extern sbyte plc_tag_get_int8(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint8", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_uint8(Int32 tag, int offset, byte val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int8", CallingConvention = CallingConvention.Cdecl)]
        public static extern int plc_tag_set_int8(Int32 tag, int offset, sbyte val);

    }
}
