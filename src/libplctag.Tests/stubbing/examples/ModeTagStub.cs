using libplctag.NativeImport;
using System;
using System.Text;

namespace libplctag.Tests.stubbing.examples
{
    public class ModeTagStub(int tagIdentifier, Tag tag) : TagStub(tagIdentifier, tag)
    {
        public static readonly byte[] ProgramMode = [0x01, 0x00, 0x00, 0x00];
        public static readonly byte[] RunMode = [0x02, 0x00, 0x00, 0x00];
        public static readonly int ModeTagHandle = 1;
        private int _counter = 0;
        
        
        public static Tag CreateModeTag(INative native)
        {
            Tag mode = new(native)
            {
                Gateway = MyControlLogixDevice.Gateway,
                Path = MyControlLogixDevice.Path, 
                Protocol = MyControlLogixDevice.Protocol,
                PlcType = MyControlLogixDevice.PlcType,
                Name = "@Mode",
                ElementSize = 4,
                Timeout = MyControlLogixDevice.Timeout
            };
            return mode;
        }


        public override int plc_tag_abort(int tag)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_create(string lpString, int timeout)
        {
            return ModeTagHandle;
        }

        public override int plc_tag_create_ex(string lpString, plctag.callback_func_ex func, IntPtr userdata, int timeout)
        {
            return ModeTagHandle;
        }

        public override string plc_tag_decode_error(int err)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_destroy(int tag)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_bit(int tag, int offset_bit)
        {
            throw new NotImplementedException();
        }

        public override float plc_tag_get_float32(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override double plc_tag_get_float64(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override short plc_tag_get_int16(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_int32(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override long plc_tag_get_int64(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override sbyte plc_tag_get_int8(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_int_attribute(int tag, string attrib_name, int default_value)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_int_attribute(int tag, string attrib_name, int new_value)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_byte_array_attribute(int tag, string attrib_name, byte[] buffer, int buffer_length)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_size(int tag)
        {
            return 4;
        }

        public override int plc_tag_set_size(int tag, int new_size)
        {
            throw new NotImplementedException();
        }

        public override ushort plc_tag_get_uint16(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override uint plc_tag_get_uint32(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override ulong plc_tag_get_uint64(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override byte plc_tag_get_uint8(int tag, int offset)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_lock(int tag)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_read(int tag, int timeout)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_register_callback(int tag_id, plctag.callback_func func)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_register_logger(plctag.log_callback_func func)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_bit(int tag, int offset_bit, int val)
        {
            throw new NotImplementedException();
        }

        public override void plc_tag_set_debug_level(int debug_level)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_float32(int tag, int offset, float val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_float64(int tag, int offset, double val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_int16(int tag, int offset, short val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_int32(int tag, int offset, int val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_int64(int tag, int offset, long val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_int8(int tag, int offset, sbyte val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_uint16(int tag, int offset, ushort val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_uint32(int tag, int offset, uint val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_uint64(int tag, int offset, ulong val)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_uint8(int tag, int offset, byte val)
        {
            throw new NotImplementedException();
        }

        public override void plc_tag_shutdown()
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_status(int tag)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_unlock(int tag)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_unregister_callback(int tag_id)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_unregister_logger(int tag_id)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_write(int tag, int timeout)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length)
        {
            
            switch (_counter)
            {
                case 0: Array.Copy(ProgramMode, buffer, ProgramMode.Length); break;
                case 1: Array.Copy(RunMode, buffer, RunMode.Length); break;
                case 2: throw new TimeoutException();
            }

            // Move to the next method, wrap around
            _counter = (_counter + 1) % 3;
            

            return (int)Status.Ok;
        }

        public override int plc_tag_set_raw_bytes(int tag, int start_offset, byte[] buffer, int buffer_length)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_string_length(int tag, int string_start_offset)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_string(int tag, int string_start_offset, StringBuilder buffer, int buffer_length)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_string_total_length(int tag, int string_start_offset)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_get_string_capacity(int tag, int string_start_offset)
        {
            throw new NotImplementedException();
        }

        public override int plc_tag_set_string(int tag, int string_start_offset, string string_val)
        {
            throw new NotImplementedException();
        }

        
    }
}