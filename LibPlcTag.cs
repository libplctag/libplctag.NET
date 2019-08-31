using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace libplctag
{
    public class Libplctag : IDisposable
    {
        private readonly Dictionary<string, Int32> _tags;

        public Libplctag()
        {
            _tags = new Dictionary<string, Int32>();
        }

        public void AddTag(Tag tag)
        {
            /* use timeout of 0 for legacy support */
            var ptr = plc_tag_create(tag.UniqueKey, 0);
            _tags.Add(tag.UniqueKey, ptr);
        }

        public void AddTag(Tag tag, int timeout)
        {
            var ptr = plc_tag_create(tag.UniqueKey, timeout);
            _tags.Add(tag.UniqueKey, ptr);
        }

        public int GetStatus(Tag tag)
        {
            var status = plc_tag_status(_tags[tag.UniqueKey]);
            return status;
        }

        public string DecodeError(int error)
        {
            var ptr = plc_tag_decode_error(error);
            return Marshal.PtrToStringAnsi(ptr);
        }

        public void RemoveTag(Tag tag)
        {
            plc_tag_destroy(_tags[tag.UniqueKey]);
            _tags.Remove(tag.UniqueKey);
        }

        public int ReadTag(Tag tag, int timeout)
        {
#if true
            var result = plc_tag_read(_tags[tag.UniqueKey], timeout);
            return result;
#else
            if (_tags.Count != 0)
            {
                return plc_tag_read(_tags[tag.UniqueKey], timeout);
            }
            return Libplctag.PLCTAG_ERR_NOT_FOUND;
#endif
        }

        public int WriteTag(Tag tag, int timeout)
        {
            var result = plc_tag_write(_tags[tag.UniqueKey], timeout);
            return result;
        }

        /* 64-bit types */

        public UInt64 GetUint64Value(Tag tag, int offset)
        {
            return plc_tag_get_uint64(_tags[tag.UniqueKey], offset);
        }

        public void SetUint64Value(Tag tag, int offset, UInt64 value)
        {
            plc_tag_set_uint64(_tags[tag.UniqueKey], offset, value);
        }

        public Int64 GetInt64Value(Tag tag, int offset)
        {
            return plc_tag_get_int64(_tags[tag.UniqueKey], offset);
        }

        public void SetInt64Value(Tag tag, int offset, Int64 value)
        {
            plc_tag_set_int64(_tags[tag.UniqueKey], offset, value);
        }

        public double GetFloat64Value(Tag tag, int offset)
        {
            return plc_tag_get_float64(_tags[tag.UniqueKey], offset);
        }

        public void SetFloat64Value(Tag tag, int offset, double value)
        {
            plc_tag_set_float64(_tags[tag.UniqueKey], offset, value);
        }

        /* 32-bit types */

        public UInt32 GetUint32Value(Tag tag, int offset)
        {
            return plc_tag_get_uint32(_tags[tag.UniqueKey], offset);
        }

        public void SetUint32Value(Tag tag, int offset, UInt32 value)
        {
            plc_tag_set_uint32(_tags[tag.UniqueKey], offset, value);
        }

        public Int32 GetInt32Value(Tag tag, int offset)
        {
            return plc_tag_get_int32(_tags[tag.UniqueKey], offset);
        }

        public void SetInt32Value(Tag tag, int offset, Int32 value)
        {
            plc_tag_set_int32(_tags[tag.UniqueKey], offset, value);
        }

        public float GetFloat32Value(Tag tag, int offset)
        {
            return plc_tag_get_float32(_tags[tag.UniqueKey], offset);
        }

        public void SetFloat32Value(Tag tag, int offset, float value)
        {
            plc_tag_set_float32(_tags[tag.UniqueKey], offset, value);
        }

        /* 16-bit types */

        public UInt16 GetUint16Value(Tag tag, int offset)
        {
            return plc_tag_get_uint16(_tags[tag.UniqueKey], offset);
        }

        public void SetUint16Value(Tag tag, int offset, UInt16 value)
        {
            plc_tag_set_uint16(_tags[tag.UniqueKey], offset, value);
        }

        public Int16 GetInt16Value(Tag tag, int offset)
        {
            return plc_tag_get_int16(_tags[tag.UniqueKey], offset);
        }

        public void SetInt16Value(Tag tag, int offset, Int16 value)
        {
            plc_tag_set_int16(_tags[tag.UniqueKey], offset, value);
        }

        /* 8-bit types */

        public byte GetUint8Value(Tag tag, int offset)
        {
#if true
            return plc_tag_get_uint8(_tags[tag.UniqueKey], offset);
#else
            if (_tags.Count != 0)
            {
                return plc_tag_get_uint8(_tags[tag.UniqueKey], offset);
            }
            return 0;
#endif
        }

        public void SetUint8Value(Tag tag, int offset, byte value)
        {
            plc_tag_set_uint8(_tags[tag.UniqueKey], offset, value);
        }

        public sbyte GetInt8Value(Tag tag, int offset)
        {
            return plc_tag_get_int8(_tags[tag.UniqueKey], offset);
        }

        public void SetInt8Value(Tag tag, int offset, sbyte value)
        {
            plc_tag_set_int8(_tags[tag.UniqueKey], offset, value);
        }

        /* bits */

        /// <summary>
        /// <returns></returns>
        /// To read a tag that aliases a single bit, set 'index' to value < 0
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="index"></param>
        /// <param name="timeout"></param>
        public bool ReadBool(Tag tag, int index, int timeout)
        {
            var readResult = ReadTag(tag, timeout);
            if (readResult != StatusCode.PLCTAG_STATUS_OK)
            {
                return false;
            }

            // workaround for tags aliasing a single bit
            if (index < 0)
            {
                return Convert.ToBoolean(GetUint8Value(tag, 0));
            }

            if (tag.ElementSize * 8 <= index)
            {
                return false;
            }

            switch (tag.ElementSize)
            {
                case DataType.Int64:  // aka LINT
                    return Convert.ToBoolean((GetUint64Value(tag, 0) >> index) & 1UL);

                case DataType.Int32:  // aka DINT
                    return Convert.ToBoolean((GetUint32Value(tag, 0) >> index) & 1U);

                case DataType.Int16:  // aka INT
                    return Convert.ToBoolean((GetUint16Value(tag, 0) >> index) & 1U);

                case DataType.Int8:  // aka SINT
                    return Convert.ToBoolean((GetUint8Value(tag, 0) >> index) & 1U);

                default:
                    return false;
            }
        }

        /// <summary>
        /// Convenience wrapper for ReadBool()
        /// To read a tag that aliases a single bit, set 'bit' to value < 0
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="bit"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool GetBitValue(Tag tag, int bit, int timeout)
        {
            return ReadBool(tag, bit, timeout);
        }

        /// <summary>
        /// <returns></returns>
        /// To write a tag that aliases a single bit, set 'index' to value < 0
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        public int WriteBool(Tag tag, int index, bool value, int timeout)
        {
            var readResult = ReadTag(tag, timeout);
            if (readResult != StatusCode.PLCTAG_STATUS_OK)
            {
                return readResult;
            }

            // workaround for tags aliasing a single bit
            if (index < 0)
            {
                byte dataByte = GetUint8Value(tag, 0);
                bool dataBit = Convert.ToBoolean(dataByte);
                dataBit = value;
                dataByte = Convert.ToByte(dataBit);
                SetUint8Value(tag, 0, dataByte);
                return WriteTag(tag, timeout);
            }

            if (tag.ElementSize * 8 <= index)
            {
                return StatusCode.PLCTAG_ERR_OUT_OF_BOUNDS;
            }
            switch (tag.ElementSize)
            {
                case DataType.Int64:  // aka LINT
                    UInt64 data64 = GetUint64Value(tag, 0);
                    if (value)
                    {
                        data64 |= (UInt64)1 << index;
                    }
                    else
                    {
                        data64 &= ~((UInt64)1 << index);
                    }
                    SetUint64Value(tag, 0, data64);
                    break;

                case DataType.Int32:  // aka DINT
                    UInt32 data32 = GetUint32Value(tag, 0);
                    if (value)
                    {
                        data32 |= (UInt32)1 << index;
                    }
                    else
                    {
                        data32 &= ~((UInt32)1 << index);
                    }
                    SetUint32Value(tag, 0, data32);
                    break;

                case DataType.Int16:  // aka INT
                    UInt16 data16 = GetUint16Value(tag, 0);
                    if (value)
                    {
                        data16 |= (UInt16)(1 << index);
                    }
                    else
                    {
                        data16 &= (UInt16)~(1 << index);
                    }
                    SetUint16Value(tag, 0, data16);
                    break;

                case DataType.Int8:  // aka SINT
                    byte data8 = GetUint8Value(tag, 0);
                    if (value)
                    {
                        data8 |= (byte)(1 << index);
                    }
                    else
                    {
                        data8 &= (byte)~(1 << index);
                    }
                    SetUint8Value(tag, 0, data8);
                    break;

                default:
                    return StatusCode.PLCTAG_ERR_NOT_ALLOWED;
            }
            return WriteTag(tag, timeout);
        }

        /// <summary>
        /// Convenience wrapper for WriteBool()
        /// To write a tag that aliases a single bit, set 'bit' to value < 0
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="bit"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int SetBitValue(Tag tag, int bit, bool value, int timeout)
        {
            return WriteBool(tag, bit, value, timeout);
        }

        public void Dispose()
        {
            foreach (var tag in _tags)
            {
                plc_tag_destroy(tag.Value);
            }
            _tags.Clear();
        }

        [DllImport("plctag.dll", EntryPoint = "plc_tag_create", CallingConvention = CallingConvention.Cdecl)]
        static extern Int32 plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_destroy", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_destroy(Int32 tag);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_status", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_status(Int32 tag);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_decode_error", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr plc_tag_decode_error(int error);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_read", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_read(Int32 tag, int timeout);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_write", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_write(Int32 tag, int timeout);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_size", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_get_size(Int32 tag);

        /* 64-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint64", CallingConvention = CallingConvention.Cdecl)]
        static extern UInt64 plc_tag_get_uint64(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int64", CallingConvention = CallingConvention.Cdecl)]
        static extern Int64 plc_tag_get_int64(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint64", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int64", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_int64(Int32 tag, int offset, Int64 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_float64", CallingConvention = CallingConvention.Cdecl)]
        static extern double plc_tag_get_float64(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_float64", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_float64(Int32 tag, int offset, double val);

        /* 32-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint32", CallingConvention = CallingConvention.Cdecl)]
        static extern UInt32 plc_tag_get_uint32(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int32", CallingConvention = CallingConvention.Cdecl)]
        static extern Int32 plc_tag_get_int32(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint32", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int32", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_int32(Int32 tag, int offset, Int32 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_float32", CallingConvention = CallingConvention.Cdecl)]
        static extern float plc_tag_get_float32(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_float32", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_float32(Int32 tag, int offset, float val);

        /* 16-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint16", CallingConvention = CallingConvention.Cdecl)]
        static extern UInt16 plc_tag_get_uint16(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int16", CallingConvention = CallingConvention.Cdecl)]
        static extern Int16 plc_tag_get_int16(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint16", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int16", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_int16(Int32 tag, int offset, Int16 val);

        /* 8-bit types */

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_uint8", CallingConvention = CallingConvention.Cdecl)]
        static extern byte plc_tag_get_uint8(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_get_int8", CallingConvention = CallingConvention.Cdecl)]
        static extern sbyte plc_tag_get_int8(Int32 tag, int offset);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_uint8", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_uint8(Int32 tag, int offset, byte val);

        [DllImport("plctag.dll", EntryPoint = "plc_tag_set_int8", CallingConvention = CallingConvention.Cdecl)]
        static extern int plc_tag_set_int8(Int32 tag, int offset, sbyte val);

    }
}