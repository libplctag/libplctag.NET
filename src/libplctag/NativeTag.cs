using libplctag.NativeImport;
using System;
using System.Text;
using static libplctag.NativeImport.plctag;

namespace libplctag
{
    class NativeTag : INativeTag
    {

        public int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)       => plctag.plc_tag_check_lib_version(req_major, req_minor, req_patch);
        public Int32 plc_tag_create(string lpString, int timeout)                               => plctag.plc_tag_create(lpString, timeout);
        public int plc_tag_destroy(Int32 tag)                                                   => plctag.plc_tag_destroy(tag);
        public int plc_tag_shutdown()                                                           => plctag.plc_tag_shutdown();
        public int plc_tag_register_callback(Int32 tag_id, callback_func func)                  => plctag.plc_tag_register_callback(tag_id, func);
        public int plc_tag_unregister_callback(Int32 tag_id)                                    => plctag.plc_tag_unregister_callback(tag_id);
        public int plc_tag_register_logger(log_callback_func func)                              => plctag.plc_tag_register_logger(func);
        public int plc_tag_unregister_logger(Int32 tag_id)                                      => plctag.plc_tag_unregister_logger(tag_id);
        public int plc_tag_lock(Int32 tag)                                                      => plctag.plc_tag_lock(tag);
        public int plc_tag_unlock(Int32 tag)                                                    => plctag.plc_tag_unlock(tag);
        public int plc_tag_status(Int32 tag)                                                    => plctag.plc_tag_status(tag);
        public string plc_tag_decode_error(int err)                                             => plctag.plc_tag_decode_error(err);
        public int plc_tag_read(Int32 tag, int timeout)                                         => plctag.plc_tag_read(tag, timeout);
        public int plc_tag_write(Int32 tag, int timeout)                                        => plctag.plc_tag_write(tag, timeout);
        public int plc_tag_get_size(Int32 tag)                                                  => plctag.plc_tag_get_size(tag);
        public int plc_tag_set_size(Int32 tag, int new_size)                                    => plctag.plc_tag_set_size(tag, new_size);
        public int plc_tag_abort(Int32 tag)                                                     => plctag.plc_tag_abort(tag);
        public int plc_tag_get_int_attribute(Int32 tag, string attrib_name, int default_value)  => plctag.plc_tag_get_int_attribute(tag, attrib_name, default_value);
        public int plc_tag_set_int_attribute(Int32 tag, string attrib_name, int new_value)      => plctag.plc_tag_set_int_attribute(tag, attrib_name, new_value);
        public UInt64 plc_tag_get_uint64(Int32 tag, int offset)                                 => plctag.plc_tag_get_uint64(tag, offset);
        public Int64 plc_tag_get_int64(Int32 tag, int offset)                                   => plctag.plc_tag_get_int64(tag, offset);
        public int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val)                        => plctag.plc_tag_set_uint64(tag, offset, val);
        public int plc_tag_set_int64(Int32 tag, int offset, Int64 val)                          => plctag.plc_tag_set_int64(tag, offset, val);
        public double plc_tag_get_float64(Int32 tag, int offset)                                => plctag.plc_tag_get_float64(tag, offset);
        public int plc_tag_set_float64(Int32 tag, int offset, double val)                       => plctag.plc_tag_set_float64(tag, offset, val);
        public UInt32 plc_tag_get_uint32(Int32 tag, int offset)                                 => plctag.plc_tag_get_uint32(tag, offset);
        public Int32 plc_tag_get_int32(Int32 tag, int offset)                                   => plctag.plc_tag_get_int32(tag, offset);
        public int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val)                        => plctag.plc_tag_set_uint32(tag, offset, val);
        public int plc_tag_set_int32(Int32 tag, int offset, Int32 val)                          => plctag.plc_tag_set_int32(tag, offset, val);
        public float plc_tag_get_float32(Int32 tag, int offset)                                 => plctag.plc_tag_get_float32(tag, offset);
        public int plc_tag_set_float32(Int32 tag, int offset, float val)                        => plctag.plc_tag_set_float32(tag, offset, val);
        public UInt16 plc_tag_get_uint16(Int32 tag, int offset)                                 => plctag.plc_tag_get_uint16(tag, offset);
        public Int16 plc_tag_get_int16(Int32 tag, int offset)                                   => plctag.plc_tag_get_int16(tag, offset);
        public int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val)                        => plctag.plc_tag_set_uint16(tag, offset, val);
        public int plc_tag_set_int16(Int32 tag, int offset, Int16 val)                          => plctag.plc_tag_set_int16(tag, offset, val);
        public byte plc_tag_get_uint8(Int32 tag, int offset)                                    => plctag.plc_tag_get_uint8(tag, offset);
        public sbyte plc_tag_get_int8(Int32 tag, int offset)                                    => plctag.plc_tag_get_int8(tag, offset);
        public int plc_tag_set_uint8(Int32 tag, int offset, byte val)                           => plctag.plc_tag_set_uint8(tag, offset, val);
        public int plc_tag_set_int8(Int32 tag, int offset, sbyte val)                           => plctag.plc_tag_set_int8(tag, offset, val);
        public int plc_tag_get_bit(Int32 tag, int offset_bit)                                   => plctag.plc_tag_get_bit(tag, offset_bit);
        public int plc_tag_set_bit(Int32 tag, int offset_bit, int val)                          => plctag.plc_tag_set_bit(tag, offset_bit, val);
        public void plc_tag_set_debug_level(int debug_level)                                    => plctag.plc_tag_set_debug_level(debug_level);
        public int plc_tag_get_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length)               => plctag.plc_tag_get_raw_bytes(tag, start_offset, buffer, buffer_length);
        public int plc_tag_set_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length)               => plctag.plc_tag_set_raw_bytes(tag, start_offset, buffer, buffer_length);
        public int plc_tag_get_string_length(int tag, int string_start_offset)                                      => plctag.plc_tag_get_string_length(tag, string_start_offset);
        public int plc_tag_get_string(int tag, int string_start_offset, StringBuilder buffer, int buffer_length)    => plctag.plc_tag_get_string(tag, string_start_offset, buffer, buffer_length);
        public int plc_tag_get_string_total_length(int tag, int string_start_offset)                                => plctag.plc_tag_get_string_total_length(tag, string_start_offset);
        public int plc_tag_get_string_capacity(int tag, int string_start_offset)                                    => plctag.plc_tag_get_string_capacity(tag, string_start_offset);
        public int plc_tag_set_string(int tag, int string_start_offset, string string_val)                          => plctag.plc_tag_set_string(tag, string_start_offset, string_val);
    }
}
