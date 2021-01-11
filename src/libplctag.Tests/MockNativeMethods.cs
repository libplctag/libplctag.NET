using libplctag.NativeImport;
using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Tests
{
    class MockNativeMethods : INativeMethods
    {
        public bool ForceExtractLibrary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int plc_tag_abort(int tag)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_create(string lpString, int timeout)
        {
            throw new NotImplementedException();
        }

        public string plc_tag_decode_error(int err)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_destroy(int tag)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_get_bit(int tag, int offset_bit)
        {
            throw new NotImplementedException();
        }

        public float plc_tag_get_float32(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public double plc_tag_get_float64(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public short plc_tag_get_int16(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_get_int32(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public long plc_tag_get_int64(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public sbyte plc_tag_get_int8(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_get_int_attribute(int tag, string attrib_name, int default_value)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_get_size(int tag)
        {
            throw new NotImplementedException();
        }

        public ushort plc_tag_get_uint16(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public uint plc_tag_get_uint32(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public ulong plc_tag_get_uint64(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public byte plc_tag_get_uint8(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_lock(int tag)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_read(int tag, int timeout)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_register_callback(int tag_id, plctag.callback_func func)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_register_logger(plctag.log_callback_func func)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_bit(int tag, int offset_bit, int val)
        {
            throw new NotImplementedException();
        }

        public void plc_tag_set_debug_level(int debug_level)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_float32(int tag, int offset, float val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_float64(int tag, int offset, double val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_int16(int tag, int offset, short val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_int32(int tag, int offset, int val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_int64(int tag, int offset, long val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_int8(int tag, int offset, sbyte val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_int_attribute(int tag, string attrib_name, int new_value)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_uint16(int tag, int offset, ushort val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_uint32(int tag, int offset, uint val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_uint64(int tag, int offset, ulong val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_set_uint8(int tag, int offset, byte val)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_shutdown()
        {
            throw new NotImplementedException();
        }

        public int plc_tag_status(int tag)
        {
            return (int)Status.Ok;
        }

        public int plc_tag_unlock(int tag)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_unregister_callback(int tag_id)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_unregister_logger(int tag_id)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_write(int tag, int timeout)
        {
            throw new NotImplementedException();
        }
    }
}
