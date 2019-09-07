using System;
using System.Runtime.InteropServices;

namespace libplctag.NativeImport
{
    public class plctag
    {

        const string DLL_NAME = "plctag";

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_create", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_destroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern int destroy(Int32 tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_status", CallingConvention = CallingConvention.Cdecl)]
        public static extern int status(Int32 tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_decode_error", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr plc_tag_decode_error(int error);

        public string decode_error(int error) => Marshal.PtrToStringAnsi(plc_tag_decode_error(error));

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_read", CallingConvention = CallingConvention.Cdecl)]
        public static extern int read(Int32 tag, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_write", CallingConvention = CallingConvention.Cdecl)]
        public static extern int write(Int32 tag, int timeout);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_size", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_size(Int32 tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_abort", CallingConvention = CallingConvention.Cdecl)]
        public static extern int abort(Int32 tag);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint64", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt64 get_uint64(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 get_int64(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_uint64(Int32 tag, int offset, UInt64 val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_int64(Int32 tag, int offset, Int64 val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_float64", CallingConvention = CallingConvention.Cdecl)]
        public static extern double get_float64(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_float64", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_float64(Int32 tag, int offset, double val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 get_uint32(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 get_int32(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_uint32(Int32 tag, int offset, UInt32 val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_int32(Int32 tag, int offset, Int32 val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_float32", CallingConvention = CallingConvention.Cdecl)]
        public static extern float get_float32(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_float32", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_float32(Int32 tag, int offset, float val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint16", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt16 get_uint16(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int16", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 get_int16(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint16", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_uint16(Int32 tag, int offset, UInt16 val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int16", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_int16(Int32 tag, int offset, Int16 val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_uint8", CallingConvention = CallingConvention.Cdecl)]
        public static extern byte get_uint8(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_get_int8", CallingConvention = CallingConvention.Cdecl)]
        public static extern sbyte get_int8(Int32 tag, int offset);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_uint8", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_uint8(Int32 tag, int offset, byte val);

        [DllImport(DLL_NAME, EntryPoint = "plc_tag_set_int8", CallingConvention = CallingConvention.Cdecl)]
        public static extern int set_int8(Int32 tag, int offset, sbyte val);

    }
}
