// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libplctag.NativeImport
{
    /// <summary>
    /// This class provides low-level (raw) access to the native libplctag library (which is written in C).
    /// The purpose of this package is to expose the API for this native library, and handle platform and configuration issues.
    /// 
    /// <para>See <see href="https://github.com/libplctag/libplctag/wiki/API"/> for documentation.</para>
    /// </summary>
    public static class plctag
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callback_func(Int32 tag_id, Int32 event_id, Int32 status);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callback_func_ex(Int32 tag_id, Int32 event_id, Int32 status, IntPtr userdata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void log_callback_func(Int32 tag_id, int debug_level, [MarshalAs(UnmanagedType.LPStr)] string message);

        public static int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)
        {
            return NativeMethods.plc_tag_check_lib_version(req_major, req_minor, req_patch);
        }

        public static Int32 plc_tag_create(string lpString, int timeout)
        {
            return NativeMethods.plc_tag_create(lpString, timeout);
        }

        public static Int32 plc_tag_create_ex(string lpString, callback_func_ex func, IntPtr userdata, int timeout)
        {
            return NativeMethods.plc_tag_create_ex(lpString, func, userdata, timeout);
        }

        public static int plc_tag_destroy(Int32 tag)
        {
            return NativeMethods.plc_tag_destroy(tag);
        }

        public static void plc_tag_shutdown()
        {
            NativeMethods.plc_tag_shutdown();
        }

        public static int plc_tag_register_callback(Int32 tag_id, callback_func func)
        {
            return NativeMethods.plc_tag_register_callback(tag_id, func);
        }

        public static int plc_tag_register_callback_ex(Int32 tag_id, callback_func_ex func, IntPtr userdata)
        {
            return NativeMethods.plc_tag_register_callback_ex(tag_id, func, userdata);
        }

        public static int plc_tag_unregister_callback(Int32 tag_id)
        {
            return NativeMethods.plc_tag_unregister_callback(tag_id);
        }

        public static int plc_tag_register_logger(log_callback_func func)
        {
            return NativeMethods.plc_tag_register_logger(func);
        }
        public static int plc_tag_unregister_logger()
        {
            return NativeMethods.plc_tag_unregister_logger();
        }

        public static int plc_tag_lock(Int32 tag)
        {
            return NativeMethods.plc_tag_lock(tag);
        }

        public static int plc_tag_unlock(Int32 tag)
        {
            return NativeMethods.plc_tag_unlock(tag);
        }
        public static int plc_tag_status(Int32 tag)
        {
            return NativeMethods.plc_tag_status(tag);
        }

        public static string plc_tag_decode_error(int err)
        {
            return Marshal.PtrToStringAnsi(NativeMethods.plc_tag_decode_error(err));
        }

        public static int plc_tag_read(Int32 tag, int timeout)
        {
            return NativeMethods.plc_tag_read(tag, timeout);
        }

        public static int plc_tag_write(Int32 tag, int timeout)
        {
            return NativeMethods.plc_tag_write(tag, timeout);
        }

        public static int plc_tag_get_size(Int32 tag)
        {
            return NativeMethods.plc_tag_get_size(tag);
        }

        public static int plc_tag_set_size(Int32 tag, int new_size)
        {
            return NativeMethods.plc_tag_set_size(tag, new_size);
        }

        public static int plc_tag_abort(Int32 tag)
        {
            return NativeMethods.plc_tag_abort(tag);
        }

        public static int plc_tag_get_int_attribute(Int32 tag, string attrib_name, int default_value)
        {
            return NativeMethods.plc_tag_get_int_attribute(tag, attrib_name, default_value);
        }

        public static int plc_tag_set_int_attribute(Int32 tag, string attrib_name, int new_value)
        {
            return NativeMethods.plc_tag_set_int_attribute(tag, attrib_name, new_value);
        }

        public static int plc_tag_get_byte_array_attribute(Int32 tag, string attrib_name, byte[] buffer, int buffer_length)
        {
            return NativeMethods.plc_tag_get_byte_array_attribute(tag, attrib_name, buffer, buffer_length);
        }

        public static UInt64 plc_tag_get_uint64(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_uint64(tag, offset);
        }

        public static Int64 plc_tag_get_int64(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_int64(tag, offset);
        }

        public static int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val)
        {
            return NativeMethods.plc_tag_set_uint64(tag, offset, val);
        }

        public static int plc_tag_set_int64(Int32 tag, int offset, Int64 val)
        {
            return NativeMethods.plc_tag_set_int64(tag, offset, val);
        }

        public static double plc_tag_get_float64(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_float64(tag, offset);
        }

        public static int plc_tag_set_float64(Int32 tag, int offset, double val)
        {
            return NativeMethods.plc_tag_set_float64(tag, offset, val);
        }
        public static UInt32 plc_tag_get_uint32(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_uint32(tag, offset);
        }

        public static Int32 plc_tag_get_int32(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_int32(tag, offset);
        }

        public static int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val)
        {
            return NativeMethods.plc_tag_set_uint32(tag, offset, val);
        }

        public static int plc_tag_set_int32(Int32 tag, int offset, Int32 val)
        {
            return NativeMethods.plc_tag_set_int32(tag, offset, val);
        }

        public static float plc_tag_get_float32(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_float32(tag, offset);
        }

        public static int plc_tag_set_float32(Int32 tag, int offset, float val)
        {
            return NativeMethods.plc_tag_set_float32(tag, offset, val);
        }

        public static UInt16 plc_tag_get_uint16(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_uint16(tag, offset);
        }

        public static Int16 plc_tag_get_int16(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_int16(tag, offset);
        }

        public static int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val)
        {
            return NativeMethods.plc_tag_set_uint16(tag, offset, val);
        }

        public static int plc_tag_set_int16(Int32 tag, int offset, Int16 val)
        {
            return NativeMethods.plc_tag_set_int16(tag, offset, val);
        }

        public static byte plc_tag_get_uint8(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_uint8(tag, offset);
        }

        public static sbyte plc_tag_get_int8(Int32 tag, int offset)
        {
            return NativeMethods.plc_tag_get_int8(tag, offset);
        }

        public static int plc_tag_set_uint8(Int32 tag, int offset, byte val)
        {
            return NativeMethods.plc_tag_set_uint8(tag, offset, val);
        }

        public static int plc_tag_set_int8(Int32 tag, int offset, sbyte val)
        {
            return NativeMethods.plc_tag_set_int8(tag, offset, val);
        }

        public static int plc_tag_get_bit(Int32 tag, int offset_bit)
        {
            return NativeMethods.plc_tag_get_bit(tag, offset_bit);
        }

        public static int plc_tag_set_bit(Int32 tag, int offset_bit, int val)
        {
            return NativeMethods.plc_tag_set_bit(tag, offset_bit, val);
        }
        public static void plc_tag_set_debug_level(int debug_level)
        {
            NativeMethods.plc_tag_set_debug_level(debug_level);
        }



        public static int plc_tag_get_string(Int32 tag_id, int string_start_offset, StringBuilder buffer, int buffer_length)
        {
            return NativeMethods.plc_tag_get_string(tag_id, string_start_offset, buffer, buffer_length);
        }

        public static int plc_tag_set_string(Int32 tag_id, int string_start_offset, string string_val)
        {
            return NativeMethods.plc_tag_set_string(tag_id, string_start_offset, string_val);
        }

        public static int plc_tag_get_string_length(Int32 tag_id, int string_start_offset)
        {
            return NativeMethods.plc_tag_get_string_length(tag_id, string_start_offset);
        }

        public static int plc_tag_get_string_capacity(Int32 tag_id, int string_start_offset)
        {
            return NativeMethods.plc_tag_get_string_capacity(tag_id, string_start_offset);
        }

        public static int plc_tag_get_string_total_length(Int32 tag_id, int string_start_offset)
        {
            return NativeMethods.plc_tag_get_string_total_length(tag_id, string_start_offset);
        }


        public static int plc_tag_get_raw_bytes(Int32 tag_id, int start_offset, byte[] buffer, int buffer_length)
        {
            return NativeMethods.plc_tag_get_raw_bytes(tag_id, start_offset, buffer, buffer_length);
        }

        public static int plc_tag_set_raw_bytes(Int32 tag_id, int start_offset, byte[] buffer, int buffer_length)
        {
            return NativeMethods.plc_tag_set_raw_bytes(tag_id, start_offset, buffer, buffer_length);
        }


    }
}