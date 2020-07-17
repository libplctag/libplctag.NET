using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using libplctag.NativeImport;

namespace libplctag
{

    public sealed class Tag : IDisposable, ITag
    {

        private const int asyncStatusPollInterval = 2;

        public Protocol Protocol { get; }
        public IPAddress Gateway { get; }
        public string Path { get; }
        public CpuType CPU { get; }
        public int ElementSize { get; }
        public int ElementCount { get; }
        public string Name { get; }
        public bool UseConnectedMessaging { get; }
        public DebugLevel DebugLevel
        {
            get => (DebugLevel)plctag.get_int_attribute(0, "debug_level", int.MinValue);
            set => plctag.set_debug_level((int)value);
        }
        public int ReadCacheMillisecondDuration
        {
            get => plctag.get_int_attribute(pointer, "read_cache_ms", int.MinValue);
            set => plctag.set_int_attribute(pointer, "read_cache_ms", value);
        }

        private readonly int pointer;

        /// <summary>
        /// Provides a new tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="gateway">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <param name="cpuType">Allen-Bradley CPU model</param>
        /// <param name="elementSize">The size of an element in bytes. The tag is assumed to be composed of elements of the same size. For structure tags, use the total size of the structure.</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="elementCount">elements count: 1- single, n-array.</param>
        /// <param name="millisecondTimeout"></param>
        /// <param name="debugLevel"></param>
        /// <param name="protocol">Currently only ab_eip supported.</param>
        /// <param name="readCacheMillisecondDuration">Set the amount of time to cache read results</param>
        /// <param name="useConnectedMessaging">Control whether to use connected or unconnected messaging.</param>
        public Tag(IPAddress gateway,
                   string path,
                   CpuType cpuType,
                   int elementSize,
                   string name,
                   int millisecondTimeout,
                   int elementCount = 1,
                   DebugLevel debugLevel = DebugLevel.None,
                   Protocol protocol = Protocol.ab_eip,
                   int readCacheMillisecondDuration = default,
                   bool useConnectedMessaging = true)
        {

            Protocol = protocol;
            Gateway = gateway;
            Path = path;
            CPU = cpuType;
            ElementSize = elementSize;
            ElementCount = elementCount;
            Name = name;
            UseConnectedMessaging = useConnectedMessaging;

            var attributeString = GetAttributeString(protocol, gateway, path, cpuType, elementSize, elementCount, name, debugLevel, readCacheMillisecondDuration, useConnectedMessaging);

            pointer = plctag.create(attributeString, millisecondTimeout);

        }

        ~Tag()
        {
            Dispose();
        }

        private static string GetAttributeString(Protocol protocol, IPAddress gateway, string path, CpuType CPU, int elementSize, int elementCount, string name, DebugLevel debugLevel, int readCacheMillisecondDuration, bool useConnectedMessaging)
        {

            var attributes = new Dictionary<string, string>();

            attributes.Add("protocol", protocol.ToString());
            attributes.Add("gateway", gateway.ToString());

            if (!string.IsNullOrEmpty(path))
                attributes.Add("path", path);

            attributes.Add("cpu", CPU.ToString().ToLower());
            attributes.Add("elem_size", elementSize.ToString());
            attributes.Add("elem_count", elementCount.ToString());
            attributes.Add("name", name);

            if (debugLevel > DebugLevel.None)
                attributes.Add("debug", ((int)debugLevel).ToString());

            if (readCacheMillisecondDuration > 0)
                attributes.Add("read_cache_ms", readCacheMillisecondDuration.ToString());

            attributes.Add("use_connected_msg", useConnectedMessaging ? "1" : "0");

            string separator = "&";
            return string.Join(separator, attributes.Select(attr => $"{attr.Key}={attr.Value}"));

        }

        public void Dispose() => plctag.destroy(pointer);

        public void Abort() => plctag.abort(pointer);

        public void Read(int millisecondTimeout)
        {

            if (millisecondTimeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondTimeout), "Must be greater than 0 for a synchronous read");

            var status = (Status)plctag.read(pointer, millisecondTimeout);
            if (status == Status.Ok)
                return;
            else
                throw new LibPlcTagException(status);

        }

        public async Task ReadAsync(CancellationToken token = default)
        {

            var status = (Status)plctag.read(pointer, 0);

            if (status == Status.Ok)
                return;
            else if (status < 0)
                throw new LibPlcTagException(status);

            do
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(asyncStatusPollInterval);
                status = GetStatus();
            }
            while (status == Status.Pending);

            if (status == Status.Ok)
                return;
            else
                throw new LibPlcTagException(status);

        }

        public void Write(int millisecondTimeout)
        {

            if (millisecondTimeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondTimeout), "Must be greater than 0 for a synchronous write");

            var status = (Status)plctag.write(pointer, millisecondTimeout);
            if (status == Status.Ok)
                return;
            else
                throw new LibPlcTagException(status);

        }

        public async Task WriteAsync(CancellationToken token = default)
        {

            var status = (Status)plctag.write(pointer, 0);

            if (status == Status.Ok)
                return;
            else if (status < 0)
                throw new LibPlcTagException(status);

            do
            {
                token.ThrowIfCancellationRequested();
                status = GetStatus();
                await Task.Delay(asyncStatusPollInterval);
            }
            while (status == Status.Pending);

            if (status == Status.Ok)
                return;
            else
                throw new LibPlcTagException(status);

        }

        public int GetSize() => plctag.get_size(pointer);

        public Status GetStatus() => (Status)plctag.status(pointer);

        public int GetBit(int offset) => plctag.get_bit(pointer, offset);
        public void SetBit(int offset, int value) => plctag.set_bit(pointer, offset, value);

        public ulong GetUInt64(int offset) => plctag.get_uint64(pointer, offset);
        public void SetUInt64(int offset, ulong value) => plctag.set_uint64(pointer, offset, value);

        public long GetInt64(int offset) => plctag.get_int64(pointer, offset);
        public void SetInt64(int offset, long value) => plctag.set_int64(pointer, offset, value);

        public uint GetUInt32(int offset) => plctag.get_uint32(pointer, offset);
        public void SetUInt32(int offset, uint value) => plctag.set_uint32(pointer, offset, value);

        public int GetInt32(int offset) => plctag.get_int32(pointer, offset);
        public void SetInt32(int offset, int value) => plctag.set_int32(pointer, offset, value);

        public ushort GetUInt16(int offset) => plctag.get_uint16(pointer, offset);
        public void SetUInt16(int offset, ushort value) => plctag.set_uint16(pointer, offset, value);

        public short GetInt16(int offset) => plctag.get_int16(pointer, offset);
        public void SetInt16(int offset, short value) => plctag.set_int16(pointer, offset, value);

        public byte GetUInt8(int offset) => plctag.get_uint8(pointer, offset);
        public void SetUInt8(int offset, byte value) => plctag.set_uint8(pointer, offset, value);

        public sbyte GetInt8(int offset) => plctag.get_int8(pointer, offset);
        public void SetInt8(int offset, sbyte value) => plctag.set_int8(pointer, offset, value);

        public double GetFloat64(int offset) => plctag.get_float64(pointer, offset);
        public void SetFloat64(int offset, double value) => plctag.set_float64(pointer, offset, value);

        public float GetFloat32(int offset) => plctag.get_float32(pointer, offset);
        public void SetFloat32(int offset, float value) => plctag.set_float32(pointer, offset, value);

    }

}