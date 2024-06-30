using System;
using System.Text;
using static libplctag.NativeImport.Common.Delegates;

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
        public static int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_check_lib_version(req_major, req_minor, req_patch);
#elif NET47_OR_GREATERREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_check_lib_version(req_major, req_minor, req_patch);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static Int32 plc_tag_create(string lpString, int timeout)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_create(lpString, timeout);
#elif NET47_OR_GREATERREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_create(lpString, timeout);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static Int32 plc_tag_create_ex(string lpString, callback_func_ex func, IntPtr userdata, int timeout)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_create_ex(lpString, func, userdata, timeout);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_create_ex(lpString, func, userdata, timeout);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_destroy(Int32 tag)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_destroy(tag);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_destroy(tag);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_shutdown()
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_shutdown();
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_shutdown();
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_register_callback(Int32 tag_id, callback_func func)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_register_callback(tag_id, func);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_register_callback(tag_id, func);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_register_callback_ex(Int32 tag_id, callback_func_ex func, IntPtr userdata)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_register_callback_ex(tag_id, func, userdata);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_register_callback_ex(tag_id, func, userdata);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_unregister_callback(Int32 tag_id)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_unregister_callback(tag_id);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_unregister_callback(tag_id);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_register_logger(log_callback_func func)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_register_logger(func);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_register_logger(func);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        public static int plc_tag_unregister_logger(Int32 tag_id)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_unregister_logger(tag_id);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_unregister_logger(tag_id);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_lock(Int32 tag)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_lock(tag);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_lock(tag);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_unlock(Int32 tag)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_unlock(tag);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_unlock(tag);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        public static int plc_tag_status(Int32 tag)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_status(tag);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_status(tag);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static string plc_tag_decode_error(int err)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_decode_error(err);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_decode_error(err);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_read(Int32 tag, int timeout)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_read(tag, timeout);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_read(tag, timeout);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_write(Int32 tag, int timeout)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_write(tag, timeout);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_write(tag, timeout);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_get_size(Int32 tag)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_size(tag);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_size(tag);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_size(Int32 tag, int new_size)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_size(tag, new_size);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_size(tag, new_size);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_abort(Int32 tag)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_abort(tag);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_abort(tag);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_get_int_attribute(Int32 tag, string attrib_name, int default_value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_int_attribute(tag, attrib_name, default_value);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_int_attribute(tag, attrib_name, default_value);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_int_attribute(Int32 tag, string attrib_name, int new_value)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_int_attribute(tag, attrib_name, new_value);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_int_attribute(tag, attrib_name, new_value);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static UInt64 plc_tag_get_uint64(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_uint64(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_uint64(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static Int64 plc_tag_get_int64(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_int64(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_int64(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_uint64(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_uint64(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_int64(Int32 tag, int offset, Int64 val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_int64(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_int64(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static double plc_tag_get_float64(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_float64(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_float64(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_float64(Int32 tag, int offset, double val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_float64(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_float64(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        public static UInt32 plc_tag_get_uint32(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_uint32(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_uint32(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static Int32 plc_tag_get_int32(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_int32(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_int32(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_uint32(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_uint32(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_int32(Int32 tag, int offset, Int32 val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_int32(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_int32(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static float plc_tag_get_float32(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_float32(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_float32(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_float32(Int32 tag, int offset, float val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_float32(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_float32(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static UInt16 plc_tag_get_uint16(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_uint16(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_uint16(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static Int16 plc_tag_get_int16(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_int16(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_int16(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_uint16(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_uint16(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_int16(Int32 tag, int offset, Int16 val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_int16(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_int16(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static byte plc_tag_get_uint8(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_uint8(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_uint8(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static sbyte plc_tag_get_int8(Int32 tag, int offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_int8(tag, offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_int8(tag, offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_uint8(Int32 tag, int offset, byte val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_uint8(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_uint8(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_int8(Int32 tag, int offset, sbyte val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_int8(tag, offset, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_int8(tag, offset, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_get_bit(Int32 tag, int offset_bit)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_bit(tag, offset_bit);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_bit(tag, offset_bit);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_bit(Int32 tag, int offset_bit, int val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_bit(tag, offset_bit, val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_bit(tag, offset_bit, val);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        public static void plc_tag_set_debug_level(int debug_level)
        {
#if NETCOREAPP3_0_OR_GREATER
            libplctag.NativeImport.NetCore.plctag.plc_tag_set_debug_level(debug_level);
#elif NET47_OR_GREATER
            libplctag.NativeImport.NetFramework.plctag.plc_tag_set_debug_level(debug_level);
#else
            throw new PlatformNotSupportedException();
#endif
        }



        public static int plc_tag_get_string(Int32 tag_id, int string_start_offset, StringBuilder buffer, int buffer_length)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_string(tag_id, string_start_offset, buffer, buffer_length);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_string(tag_id, string_start_offset, buffer, buffer_length);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_string(Int32 tag_id, int string_start_offset, string string_val)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_string(tag_id, string_start_offset, string_val);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_string(tag_id, string_start_offset, string_val);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_get_string_length(Int32 tag_id, int string_start_offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_string_length(tag_id, string_start_offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_string_length(tag_id, string_start_offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_get_string_capacity(Int32 tag_id, int string_start_offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_string_capacity(tag_id, string_start_offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_string_capacity(tag_id, string_start_offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_get_string_total_length(Int32 tag_id, int string_start_offset)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_string_total_length(tag_id, string_start_offset);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_string_total_length(tag_id, string_start_offset);
#else
            throw new PlatformNotSupportedException();
#endif
        }


        public static int plc_tag_get_raw_bytes(Int32 tag_id, int start_offset, byte[] buffer, int buffer_length)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_get_raw_bytes(tag_id, start_offset, buffer, buffer_length);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_get_raw_bytes(tag_id, start_offset, buffer, buffer_length);
#else
            throw new PlatformNotSupportedException();
#endif
        }

        public static int plc_tag_set_raw_bytes(Int32 tag_id, int start_offset, byte[] buffer, int buffer_length)
        {
#if NETCOREAPP3_0_OR_GREATER
            return libplctag.NativeImport.NetCore.plctag.plc_tag_set_raw_bytes(tag_id, start_offset, buffer, buffer_length);
#elif NET47_OR_GREATER
            return libplctag.NativeImport.NetFramework.plctag.plc_tag_set_raw_bytes(tag_id, start_offset, buffer, buffer_length);
#else
            throw new PlatformNotSupportedException();
#endif
        }

    }
}