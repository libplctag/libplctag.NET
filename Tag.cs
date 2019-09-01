using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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

        private int pointer;

        /// <summary>
        /// Factory method to create a tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="protocol">Currently only ab_eip supported.</param>
        /// <param name="gateway">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <param name="cpuType">Allen-Bradley CPU model</param>
        /// <param name="elementSize">The size of an element in bytes. The tag is assumed to be composed of elements of the same size. For structure tags, use the total size of the structure.</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="elementCount">elements count: 1- single, n-array.</param>
        /// <param name="debugLevel"></param>
        /// <param name="defaultTimeout"></param>
        public Tag(string protocol, IPAddress gateway, string path, CpuType cpuType, int elementSize,  string name, int elementCount = 1, int debugLevel = 0, TimeSpan defaultTimeout = default)
        {

            Protocol = protocol;
            Gateway = gateway;
            Path = path;
            CPU = cpuType;
            ElementSize = elementSize;
            ElementCount = elementCount;
            Name = name;
            DebugLevel = debugLevel;
            DefaultTimeout = defaultTimeout;

            var attributeString = GetAttributeString(protocol, gateway, path, cpuType, elementSize, elementCount, name, debugLevel);

            pointer = Dll.plc_tag_create(attributeString, (int)defaultTimeout.TotalMilliseconds); ;

        }

        ~Tag()
        {
            Dispose();
        }

        private static string GetAttributeString(string protocol, IPAddress gateway, string path, CpuType CPU, int elementSize, int elementCount, string name, int debugLevel)
        {

            var attributes = new Dictionary<string, string>();

            attributes.Add("protocol", protocol);
            attributes.Add("gateway", gateway.ToString());

            if (!string.IsNullOrEmpty(path))
                attributes.Add("path", path);

            attributes.Add("cpu", CPU.ToString());
            attributes.Add("elem_size", elementSize.ToString());
            attributes.Add("elem_count", elementCount.ToString());
            attributes.Add("name", name);

            if (debugLevel > 0)
                attributes.Add("debug", debugLevel.ToString());

            string separator = "&";

            return string.Join(separator, attributes.Select(attr => $"{attr.Key}={attr.Value}"));

        }

        public void Dispose() => Dll.plc_tag_destroy(pointer);

        public void Abort() => Dll.plc_tag_abort(pointer);

        public void Read(TimeSpan timeout) => Dll.plc_tag_read(pointer, (int)timeout.TotalMilliseconds);

        public void Write(TimeSpan timeout) => Dll.plc_tag_write(pointer, (int)timeout.TotalMilliseconds);

        public int GetSize() => Dll.plc_tag_get_size(pointer);

        public StatusCode GetStatus() => (StatusCode)Dll.plc_tag_status(pointer);

        public ulong GetUInt64(int offset) => Dll.plc_tag_get_uint64(pointer, offset);
        public void SetUInt64(int offset, ulong value) => Dll.plc_tag_set_uint64(pointer, offset, value);

        public long GetInt64(int offset) => Dll.plc_tag_get_int64(pointer, offset);
        public void SetInt64(int offset, long value) => Dll.plc_tag_set_int64(pointer, offset, value);

        public uint GetUInt32(int offset) => Dll.plc_tag_get_uint32(pointer, offset);
        public void SetUInt32(int offset, uint value) => Dll.plc_tag_set_uint32(pointer, offset, value);

        public int GetInt32(int offset) => Dll.plc_tag_get_int32(pointer, offset);
        public void SetInt32(int offset, int value) => Dll.plc_tag_set_int32(pointer, offset, value);

        public ushort GetUInt16(int offset) => Dll.plc_tag_get_uint16(pointer, offset);
        public void SetUInt16(int offset, ushort value) => Dll.plc_tag_set_uint16(pointer, offset, value);

        public short GetInt16(int offset) => Dll.plc_tag_get_int16(pointer, offset);
        public void SetInt16(int offset, short value) => Dll.plc_tag_set_int16(pointer, offset, value);

        public byte GetUInt8(int offset) => Dll.plc_tag_get_uint8(pointer, offset);
        public void SetUInt8(int offset, byte value) => Dll.plc_tag_set_uint8(pointer, offset, value);

        public sbyte GetInt8(int offset) => Dll.plc_tag_get_int8(pointer, offset);
        public void SetInt8(int offset, sbyte value) => Dll.plc_tag_set_int8(pointer, offset, value);

        public double GetFloat64(int offset) => Dll.plc_tag_get_float64(pointer, offset);
        public void SetFloat64(int offset, double value) => Dll.plc_tag_set_float64(pointer, offset, value);

        public float GetFloat32(int offset) => Dll.plc_tag_get_float32(pointer, offset);
        public void SetFloat32(int offset, float value) => Dll.plc_tag_set_float32(pointer, offset, value);

    }

}
