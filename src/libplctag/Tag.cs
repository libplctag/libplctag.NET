using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using libplctag.NativeImport;

namespace libplctag
{

    public sealed class Tag : IDisposable
    {

        public string Protocol { get; }
        public IPAddress Gateway { get; }
        public string Path { get; }
        public CpuTypes CPU { get; }
        public int ElementSize { get; }
        public int ElementCount { get; }
        public string Name { get; }
        public int DebugLevel
        {
            get => plctag.get_int_attribute(pointer, "debug", int.MinValue);
            set => plctag.set_int_attribute(pointer, "debug", value);
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
        /// <param name="timeout"></param>
        /// <param name="debugLevel"></param>
        /// <param name="protocol">Currently only ab_eip supported.</param>
        public Tag(IPAddress gateway, string path, CpuTypes cpuType, int elementSize, string name, int elementCount = 1, TimeSpan timeout = default, int debugLevel = 0, string protocol = "ab_eip")
        {

            Protocol = protocol;
            Gateway = gateway;
            Path = path;
            CPU = cpuType;
            ElementSize = elementSize;
            ElementCount = elementCount;
            Name = name;
            DebugLevel = debugLevel;

            var attributeString = GetAttributeString(protocol, gateway, path, cpuType, elementSize, elementCount, name, debugLevel);

            pointer = plctag.create(attributeString, (int)timeout.TotalMilliseconds);

        }

        ~Tag()
        {
            Dispose();
        }

        private static string GetAttributeString(string protocol, IPAddress gateway, string path, CpuTypes CPU, int elementSize, int elementCount, string name, int debugLevel)
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

        public void ThrowIfError()
        {
            var status = GetStatus();

            switch (status)
            {
                case StatusCode.PLCTAG_STATUS_PENDING:      break;
                case StatusCode.PLCTAG_STATUS_OK:           break;
                case StatusCode.PLCTAG_ERR_ABORT:           throw new libplctag.AbortException();
                case StatusCode.PLCTAG_ERR_BAD_CONFIG:      throw new libplctag.BadConfigException();
                case StatusCode.PLCTAG_ERR_BAD_CONNECTION:  throw new libplctag.BadConnectionException();
                case StatusCode.PLCTAG_ERR_BAD_DATA:        throw new libplctag.BadDataException();
                case StatusCode.PLCTAG_ERR_BAD_DEVICE:      throw new libplctag.BadDeviceException();
                case StatusCode.PLCTAG_ERR_BAD_GATEWAY:     throw new libplctag.BadGatewayException();
                case StatusCode.PLCTAG_ERR_BAD_PARAM:       throw new libplctag.BadParameterException();
                case StatusCode.PLCTAG_ERR_BAD_REPLY:       throw new libplctag.BadReplyException();
                case StatusCode.PLCTAG_ERR_BAD_STATUS:      throw new libplctag.BadStatusException();
                case StatusCode.PLCTAG_ERR_CLOSE:           throw new libplctag.CloseException();
                case StatusCode.PLCTAG_ERR_CREATE:          throw new libplctag.CreateException();
                case StatusCode.PLCTAG_ERR_DUPLICATE:       throw new libplctag.DuplicateException();
                case StatusCode.PLCTAG_ERR_ENCODE:          throw new libplctag.EncodeException();
                case StatusCode.PLCTAG_ERR_MUTEX_DESTROY:   throw new libplctag.MutexDestroyException();
                case StatusCode.PLCTAG_ERR_MUTEX_INIT:      throw new libplctag.MutexInitException();
                case StatusCode.PLCTAG_ERR_MUTEX_LOCK:      throw new libplctag.MutexLockException();
                case StatusCode.PLCTAG_ERR_MUTEX_UNLOCK:    throw new libplctag.MutexUnlockException();
                case StatusCode.PLCTAG_ERR_NOT_ALLOWED:     throw new libplctag.NotAllowedException();
                case StatusCode.PLCTAG_ERR_NOT_FOUND:       throw new libplctag.NotFoundException();
                case StatusCode.PLCTAG_ERR_NOT_IMPLEMENTED: throw new libplctag.NotImplementedException();
                case StatusCode.PLCTAG_ERR_NO_DATA:         throw new libplctag.NoDataException();
                case StatusCode.PLCTAG_ERR_NO_MATCH:        throw new libplctag.NoMatchException();
                case StatusCode.PLCTAG_ERR_NO_MEM:          throw new libplctag.NoMemoryException();
                case StatusCode.PLCTAG_ERR_NO_RESOURCES:    throw new libplctag.NoResourcesException();
                case StatusCode.PLCTAG_ERR_NULL_PTR:        throw new libplctag.NullPointerException();
                case StatusCode.PLCTAG_ERR_OPEN:            throw new libplctag.OpenException();
                case StatusCode.PLCTAG_ERR_OUT_OF_BOUNDS:   throw new libplctag.OutOfBoundsException();
                case StatusCode.PLCTAG_ERR_READ:            throw new libplctag.ReadException();
                case StatusCode.PLCTAG_ERR_REMOTE_ERR:      throw new libplctag.RemoteErrorException();
                case StatusCode.PLCTAG_ERR_THREAD_CREATE:   throw new libplctag.ThreadCreateException();
                case StatusCode.PLCTAG_ERR_THREAD_JOIN:     throw new libplctag.ThreadJoinException();
                case StatusCode.PLCTAG_ERR_TIMEOUT:         throw new libplctag.TimeoutException();
                case StatusCode.PLCTAG_ERR_TOO_LARGE:       throw new libplctag.TooLargeException();
                case StatusCode.PLCTAG_ERR_TOO_SMALL:       throw new libplctag.TooSmallException();
                case StatusCode.PLCTAG_ERR_UNSUPPORTED:     throw new libplctag.UnsupportedException();
                case StatusCode.PLCTAG_ERR_WINSOCK:         throw new libplctag.WinsockException();
                case StatusCode.PLCTAG_ERR_WRITE:           throw new libplctag.WriteException();
                case StatusCode.PLCTAG_ERR_PARTIAL:         throw new libplctag.PartialException();
                case StatusCode.PLCTAG_ERR_BUSY:            throw new libplctag.BusyException();
                default:                                    throw new System.NotImplementedException();
            }
        }

        public void Dispose() => plctag.destroy(pointer);

        public void Abort() => plctag.abort(pointer);

        public void Read(TimeSpan timeout) => plctag.read(pointer, (int)timeout.TotalMilliseconds);

        public void Write(TimeSpan timeout) => plctag.write(pointer, (int)timeout.TotalMilliseconds);

        public int GetSize() => plctag.get_size(pointer);

        public StatusCode GetStatus() => (StatusCode)plctag.status(pointer);

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