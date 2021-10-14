using libplctag.NativeImport;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("libplctag.Tests")]

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace libplctag
{
    interface INativeTag
    {
        int plc_tag_abort(int tag);
        int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch);
        int plc_tag_create(string lpString, int timeout);
        string plc_tag_decode_error(int err);
        int plc_tag_destroy(int tag);
        int plc_tag_get_bit(int tag, int offset_bit);
        float plc_tag_get_float32(int tag, int offset);
        double plc_tag_get_float64(int tag, int offset);
        short plc_tag_get_int16(int tag, int offset);
        int plc_tag_get_int32(int tag, int offset);
        long plc_tag_get_int64(int tag, int offset);
        sbyte plc_tag_get_int8(int tag, int offset);
        int plc_tag_get_int_attribute(int tag, string attrib_name, int default_value);
        int plc_tag_get_size(int tag);
        int plc_tag_set_size(int tag, int new_size);
        ushort plc_tag_get_uint16(int tag, int offset);
        uint plc_tag_get_uint32(int tag, int offset);
        ulong plc_tag_get_uint64(int tag, int offset);
        byte plc_tag_get_uint8(int tag, int offset);
        int plc_tag_lock(int tag);
        int plc_tag_read(int tag, int timeout);
        int plc_tag_register_callback(int tag_id, plctag.callback_func func);
        int plc_tag_register_logger(plctag.log_callback_func func);
        int plc_tag_set_bit(int tag, int offset_bit, int val);
        void plc_tag_set_debug_level(int debug_level);
        int plc_tag_set_float32(int tag, int offset, float val);
        int plc_tag_set_float64(int tag, int offset, double val);
        int plc_tag_set_int16(int tag, int offset, short val);
        int plc_tag_set_int32(int tag, int offset, int val);
        int plc_tag_set_int64(int tag, int offset, long val);
        int plc_tag_set_int8(int tag, int offset, sbyte val);
        int plc_tag_set_int_attribute(int tag, string attrib_name, int new_value);
        int plc_tag_set_uint16(int tag, int offset, ushort val);
        int plc_tag_set_uint32(int tag, int offset, uint val);
        int plc_tag_set_uint64(int tag, int offset, ulong val);
        int plc_tag_set_uint8(int tag, int offset, byte val);
        int plc_tag_shutdown();
        int plc_tag_status(int tag);
        int plc_tag_unlock(int tag);
        int plc_tag_unregister_callback(int tag_id);
        int plc_tag_unregister_logger(int tag_id);
        int plc_tag_write(int tag, int timeout);
        int plc_tag_get_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length);
        int plc_tag_set_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length);
        int plc_tag_get_string_length(int tag, int string_start_offset);
        int plc_tag_get_string(int tag, int string_start_offset, StringBuilder buffer, int buffer_length);
        int plc_tag_get_string_total_length(int tag, int string_start_offset);
        int plc_tag_get_string_capacity(int tag, int string_start_offset);
        int plc_tag_set_string(int tag, int string_start_offset, string string_val);
    }
}