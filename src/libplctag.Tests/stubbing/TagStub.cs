using libplctag.NativeImport;
using System;
using System.Text;

namespace libplctag.Tests.stubbing
{
    public abstract class TagStub(int tagIdentifier, Tag tag) : INative
    {
        public readonly int TagIdentifier = tagIdentifier;
        public readonly Tag Tag = tag; // We actually do not require the real tag, just its configuration parameters to match the Attribute-string, but there is no model-object for that
        public readonly string AttributeString = tag.GetAttributeString();

        public virtual bool IsResponsibleForLpString(string lpString)
        {
            return AttributeString == lpString;
        }
        
        public virtual bool IsResponsibleForTag(int tagIdentifier)
        {
            return TagIdentifier == tagIdentifier;
        }

        public abstract int plc_tag_abort(int tag);
        public abstract int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch);
        public abstract int plc_tag_create(string lpString, int timeout);
        public abstract int plc_tag_create_ex(string lpString, plctag.callback_func_ex func, IntPtr userdata, int timeout);
        public abstract string plc_tag_decode_error(int err);


        public abstract int plc_tag_destroy(int tag);
        public abstract int plc_tag_get_bit(int tag, int offset_bit);
        public abstract float plc_tag_get_float32(int tag, int offset);
        public abstract double plc_tag_get_float64(int tag, int offset);
        public abstract short plc_tag_get_int16(int tag, int offset);
        public abstract int plc_tag_get_int32(int tag, int offset);
        public abstract long plc_tag_get_int64(int tag, int offset);
        public abstract sbyte plc_tag_get_int8(int tag, int offset);
        public abstract int plc_tag_get_int_attribute(int tag, string attrib_name, int default_value);
        public abstract int plc_tag_set_int_attribute(int tag, string attrib_name, int new_value);
        public abstract int plc_tag_get_byte_array_attribute(int tag, string attrib_name, byte[] buffer, int buffer_length);
        public abstract int plc_tag_get_size(int tag);
        public abstract int plc_tag_set_size(int tag, int new_size);
        public abstract ushort plc_tag_get_uint16(int tag, int offset);
        public abstract uint plc_tag_get_uint32(int tag, int offset);
        public abstract ulong plc_tag_get_uint64(int tag, int offset);
        public abstract byte plc_tag_get_uint8(int tag, int offset);
        public abstract int plc_tag_lock(int tag);
        public abstract int plc_tag_read(int tag, int timeout);
        public abstract int plc_tag_register_callback(int tag_id, plctag.callback_func func);
        public abstract int plc_tag_register_logger(plctag.log_callback_func func);
        public abstract int plc_tag_set_bit(int tag, int offset_bit, int val);
        public abstract void plc_tag_set_debug_level(int debug_level);
        public abstract int plc_tag_set_float32(int tag, int offset, float val);
        public abstract int plc_tag_set_float64(int tag, int offset, double val);
        public abstract int plc_tag_set_int16(int tag, int offset, short val);
        public abstract int plc_tag_set_int32(int tag, int offset, int val);
        public abstract int plc_tag_set_int64(int tag, int offset, long val);
        public abstract int plc_tag_set_int8(int tag, int offset, sbyte val);
        public abstract int plc_tag_set_uint16(int tag, int offset, ushort val);
        public abstract int plc_tag_set_uint32(int tag, int offset, uint val);
        public abstract int plc_tag_set_uint64(int tag, int offset, ulong val);
        public abstract int plc_tag_set_uint8(int tag, int offset, byte val);
        public abstract void plc_tag_shutdown();
        public abstract int plc_tag_status(int tag);
        public abstract int plc_tag_unlock(int tag);
        public abstract int plc_tag_unregister_callback(int tag_id);
        public abstract int plc_tag_unregister_logger(int tag_id);
        public abstract int plc_tag_write(int tag, int timeout);
        public abstract int plc_tag_get_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length);
        public abstract int plc_tag_set_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length);
        public abstract int plc_tag_get_string_length(int tag, int string_start_offset);
        public abstract int plc_tag_get_string(int tag, int string_start_offset, StringBuilder buffer, int buffer_length);
        public abstract int plc_tag_get_string_total_length(int tag, int string_start_offset);
        public abstract int plc_tag_get_string_capacity(int tag, int string_start_offset);
        public abstract int plc_tag_set_string(int tag, int string_start_offset, string string_val);
    }
    
    
     
}