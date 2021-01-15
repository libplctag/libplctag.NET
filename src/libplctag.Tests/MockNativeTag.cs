using libplctag.NativeImport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace libplctag.Tests
{
    class MockNativeTag : INativeTag
    {

        Status status;
        readonly Dictionary<int, object> Value = new Dictionary<int, object>();


        public string AttributeString { get; internal set; }

        public bool ForceExtractLibrary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int plc_tag_abort(int tag)
        {
            status = Status.ErrorAbort;
            return plc_tag_status(tag);
        }

        public int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_create(string lpString, int timeout)
        {
            AttributeString = lpString;
            return plc_tag_status(0);
        }

        public string plc_tag_decode_error(int err)
        {
            return ((Status)err).ToString();
        }

        public int plc_tag_destroy(int tag)
        {
            status = Status.Ok;
            return plc_tag_status(tag);
        }

        public int plc_tag_get_bit(int tag, int offset_bit)     => (int)Value[offset_bit];
        public float plc_tag_get_float32(int tag, int offset)   => (float)Value[offset];
        public double plc_tag_get_float64(int tag, int offset)  => (double)Value[offset];
        public short plc_tag_get_int16(int tag, int offset)     => (short)Value[offset];
        public int plc_tag_get_int32(int tag, int offset)       => (int)Value[offset];
        public long plc_tag_get_int64(int tag, int offset)      => (long)Value[offset];
        public sbyte plc_tag_get_int8(int tag, int offset)      => (sbyte)Value[offset];
        public ushort plc_tag_get_uint16(int tag, int offset)   => (ushort)Value[offset];
        public uint plc_tag_get_uint32(int tag, int offset)     => (uint)Value[offset];
        public ulong plc_tag_get_uint64(int tag, int offset)    => (ulong)Value[offset];
        public byte plc_tag_get_uint8(int tag, int offset)      => (byte)Value[offset];


        public int plc_tag_get_int_attribute(int tag, string attrib_name, int default_value)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_get_size(int tag)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_lock(int tag)
        {
            throw new NotImplementedException();
        }

        public int plc_tag_read(int tag, int timeout) => f(timeout);

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
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_float64(int tag, int offset, double val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_int16(int tag, int offset, short val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_int32(int tag, int offset, int val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_int64(int tag, int offset, long val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_int8(int tag, int offset, sbyte val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_uint16(int tag, int offset, ushort val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_uint32(int tag, int offset, uint val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_uint64(int tag, int offset, ulong val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_set_uint8(int tag, int offset, byte val)
        {
            Value[offset] = val;
            return plc_tag_status(tag);
        }

        public int plc_tag_shutdown()
        {
            throw new NotImplementedException();
        }

        public int plc_tag_status(int tag) => (int)status;

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

        public int plc_tag_write(int tag, int timeout) => f(timeout);

        public int plc_tag_set_int_attribute(int tag, string attrib_name, int new_value)
        {
            throw new NotImplementedException();
        }


        int f(int timeout)
        {
            // non-blocking (async) mode
            if (timeout == 0)
            {
                status = Status.Pending;
                Task.Run(async () =>
                {
                    await Task.Delay(5);
                    status = Status.Ok;
                });
            }

            // blocking (sync) mode
            else
            {
                Task.Delay(timeout).GetAwaiter().GetResult();
                status = Status.Ok;
            }

            return plc_tag_status(0);
        }
    }
}
