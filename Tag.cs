using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace libplctag
{

    public class Tag : IDisposable
    {

        public string Protocol { get; }
        public IPAddress Gateway { get; }
        public string Path { get; }
        public CpuType CPU { get; }
        public int ElementSize { get; }
        public int ElementCount { get; }
        public string Name { get; }
        public int DebugLevel { get; }
        public TimeSpan DefaultTimeout { get; }

        private string UniqueKey => GetUniqueKey(Protocol, Gateway, Path, CPU, ElementSize, ElementCount, Name, DebugLevel);

        private int _pointer;

        
        private Tag (string protocol, IPAddress gateway, string path, CpuType cpuType, int elementSize, int elementCount, string name, int debugLevel, TimeSpan defaultTimeout, int pointer)
        {

            if (cpuType == CpuType.LGX && string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("PortType and Slot must be specified for Controllogix / Compactlogix processors");
            }

            Protocol = protocol;
            Gateway = gateway;
            Path = path;
            CPU = cpuType;
            ElementSize = elementSize;
            ElementCount = elementCount;
            Name = name;
            DebugLevel = debugLevel;
            DefaultTimeout = defaultTimeout;
            _pointer = pointer;
            
        }

        static HashSet<string> registeredTagKeys = new HashSet<string>();


        /// <summary>
        /// Factory method to create a tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="gateway">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <param name="cpuType">AB CPU models</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="elementSize">The size of an element in bytes. The tag is assumed to be composed of elements of the same size. For structure tags, use the total size of the structure.</param>
        /// <param name="elementCount">elements count: 1- single, n-array.</param>
        /// <param name="debugLevel"></param>
        /// <param name="defaultTimeout"></param>
        public static Tag Create(IPAddress gateway, string path, CpuType cpuType, int elementSize, int elementCount, string name, int debugLevel, TimeSpan defaultTimeout)
        {

            var protocol = "ab_eip";

            var key = GetUniqueKey(protocol, gateway, path, cpuType, elementSize, elementCount, name, debugLevel);

            if (registeredTagKeys.Contains(key))
            {
                throw new Exception("Duplicate tag created");
            }

            var tagPointer = plc_tag_create(key, (int)defaultTimeout.TotalMilliseconds);
            var newTag = new Tag(protocol, gateway, path, cpuType, elementSize, elementCount, name, debugLevel, defaultTimeout, tagPointer);
            registeredTagKeys.Add(newTag.UniqueKey);

            return newTag;
            
        }

        ~Tag()
        {
            Dispose();
        }

        private static string GetUniqueKey(string protocol, IPAddress gateway, string path, CpuType CPU, int elementSize, int elementCount, string name, int debugLevel)
        {

            var sb = new StringBuilder();

            sb.Append($"protocol={protocol}");
            sb.Append($"gateway={gateway}");

            if (!string.IsNullOrEmpty(path))
            {
                sb.Append($"&path={path}");
            }

            sb.Append($"&cpu={CPU}");
            sb.Append($"&elem_size={elementSize}");
            sb.Append($"&elem_count={elementCount}");
            sb.Append($"&name={name}");

            if (debugLevel > 0)
            {
                sb.Append($"&debug={debugLevel}");
            }

            return sb.ToString();

        }

        public void Dispose()
        {
            var result = (StatusCode)plc_tag_destroy(_pointer);
            //TODO handle result
            registeredTagKeys.Remove(UniqueKey);
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Read(TimeSpan timeout)
        {
            var result = (StatusCode)plc_tag_read(_pointer, (int)timeout.TotalMilliseconds);
            if(result == StatusCode.PLCTAG_ERR_TIMEOUT)
            {
                throw new TimeoutException();
            }
            if(result != StatusCode.PLCTAG_STATUS_OK)
            {
                throw new Exception("Read Failed");
                //TODO make custom exception type for this
            }
        }

        public void Write(TimeSpan timeout)
        {
            var result = (StatusCode)plc_tag_write(_pointer, (int)timeout.TotalMilliseconds);
            if (result == StatusCode.PLCTAG_ERR_TIMEOUT)
            {
                throw new TimeoutException();
            }
            if (result != StatusCode.PLCTAG_STATUS_OK)
            {
                throw new Exception("Write Failed");
                //TODO make custom exception type for this
            }
        }

        public int GetSize() => plc_tag_get_size(_pointer);

        public StatusCode GetStatus() => (StatusCode)plc_tag_status(_pointer);

        public ulong GetUInt64(int offset) => plc_tag_get_uint64(_pointer, offset);
        public void SetUInt64(int offset, ulong value) => plc_tag_set_uint64(_pointer, offset, value);

        public long GetInt64(int offset) => plc_tag_get_int64(_pointer, offset);
        public void SetInt64(int offset, long value) => plc_tag_set_int64(_pointer, offset, value);

        public uint GetUInt32(int offset) => plc_tag_get_uint32(_pointer, offset);
        public void SetUInt32(int offset, uint value) => plc_tag_set_uint32(_pointer, offset, value);

        public int GetInt32(int offset) => plc_tag_get_int32(_pointer, offset);
        public void SetInt32(int offset, int value) => plc_tag_set_int32(_pointer, offset, value);

        public ushort GetUInt16(int offset) => plc_tag_get_uint16(_pointer, offset);
        public void SetUInt16(int offset, ushort value) => plc_tag_set_uint16(_pointer, offset, value);

        public short GetInt16(int offset) => plc_tag_get_int16(_pointer, offset);
        public void SetInt16(int offset, short value) => plc_tag_set_int16(_pointer, offset, value);

        public byte GetUInt8(int offset) => plc_tag_get_uint8(_pointer, offset);
        public void SetUInt8(int offset, byte value) => plc_tag_set_uint8(_pointer, offset, value);

        public sbyte GetInt8(int offset) => plc_tag_get_int8(_pointer, offset);
        public void SetInt8(int offset, sbyte value) => plc_tag_set_int8(_pointer, offset, value);

        public double GetFloat64(int offset) => plc_tag_get_float64(_pointer, offset);
        public void SetFloat64(int offset, double value) => plc_tag_set_float64(_pointer, offset, value);

        public float GetFloat32(int offset) => plc_tag_get_float32(_pointer, offset);
        public void SetFloat32(int offset, float value) => plc_tag_set_float32(_pointer, offset, value);










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