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
        public DebugLevels DebugLevel { get; }

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
        public Tag(IPAddress gateway, string path, CpuTypes cpuType, int elementSize, string name, int elementCount = 1, TimeSpan timeout = default, DebugLevels debugLevel = DebugLevels.None, string protocol = "ab_eip")
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

        private static string GetAttributeString(string protocol, IPAddress gateway, string path, CpuTypes CPU, int elementSize, int elementCount, string name, DebugLevels debugLevel)
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
                case StatusCodes.STATUS_PENDING:      break;
                case StatusCodes.STATUS_OK:           break;
                case StatusCodes.ERROR_ABORT:           throw new libplctag.AbortException();
                case StatusCodes.ERROR_BAD_CONFIG:      throw new libplctag.BadConfigException();
                case StatusCodes.ERROR_BAD_CONNECTION:  throw new libplctag.BadConnectionException();
                case StatusCodes.ERROR_BAD_DATA:        throw new libplctag.BadDataException();
                case StatusCodes.ERROR_BAD_DEVICE:      throw new libplctag.BadDeviceException();
                case StatusCodes.ERROR_BAD_GATEWAY:     throw new libplctag.BadGatewayException();
                case StatusCodes.ERROR_BAD_PARAM:       throw new libplctag.BadParameterException();
                case StatusCodes.ERROR_BAD_REPLY:       throw new libplctag.BadReplyException();
                case StatusCodes.ERROR_BAD_STATUS:      throw new libplctag.BadStatusException();
                case StatusCodes.ERROR_CLOSE:           throw new libplctag.CloseException();
                case StatusCodes.ERROR_CREATE:          throw new libplctag.CreateException();
                case StatusCodes.ERROR_DUPLICATE:       throw new libplctag.DuplicateException();
                case StatusCodes.ERROR_ENCODE:          throw new libplctag.EncodeException();
                case StatusCodes.ERROR_MUTEX_DESTROY:   throw new libplctag.MutexDestroyException();
                case StatusCodes.ERROR_MUTEX_INIT:      throw new libplctag.MutexInitException();
                case StatusCodes.ERROR_MUTEX_LOCK:      throw new libplctag.MutexLockException();
                case StatusCodes.ERROR_MUTEX_UNLOCK:    throw new libplctag.MutexUnlockException();
                case StatusCodes.ERROR_NOT_ALLOWED:     throw new libplctag.NotAllowedException();
                case StatusCodes.ERROR_NOT_FOUND:       throw new libplctag.NotFoundException();
                case StatusCodes.ERROR_NOT_IMPLEMENTED: throw new libplctag.NotImplementedException();
                case StatusCodes.ERROR_NO_DATA:         throw new libplctag.NoDataException();
                case StatusCodes.ERROR_NO_MATCH:        throw new libplctag.NoMatchException();
                case StatusCodes.ERROR_NO_MEM:          throw new libplctag.NoMemoryException();
                case StatusCodes.ERROR_NO_RESOURCES:    throw new libplctag.NoResourcesException();
                case StatusCodes.ERROR_NULL_PTR:        throw new libplctag.NullPointerException();
                case StatusCodes.ERROR_OPEN:            throw new libplctag.OpenException();
                case StatusCodes.ERROR_OUT_OF_BOUNDS:   throw new libplctag.OutOfBoundsException();
                case StatusCodes.ERROR_READ:            throw new libplctag.ReadException();
                case StatusCodes.ERROR_REMOTE_ERR:      throw new libplctag.RemoteErrorException();
                case StatusCodes.ERROR_THREAD_CREATE:   throw new libplctag.ThreadCreateException();
                case StatusCodes.ERROR_THREAD_JOIN:     throw new libplctag.ThreadJoinException();
                case StatusCodes.ERROR_TIMEOUT:         throw new libplctag.TimeoutException();
                case StatusCodes.ERROR_TOO_LARGE:       throw new libplctag.TooLargeException();
                case StatusCodes.ERROR_TOO_SMALL:       throw new libplctag.TooSmallException();
                case StatusCodes.ERROR_UNSUPPORTED:     throw new libplctag.UnsupportedException();
                case StatusCodes.ERROR_WINSOCK:         throw new libplctag.WinsockException();
                case StatusCodes.ERROR_WRITE:           throw new libplctag.WriteException();
                default:                                    throw new System.NotImplementedException();
            }
        }

        public void Dispose() => plctag.destroy(pointer);

        public void Abort() => plctag.abort(pointer);

        public void Read(TimeSpan timeout) => plctag.read(pointer, (int)timeout.TotalMilliseconds);

        public void Write(TimeSpan timeout) => plctag.write(pointer, (int)timeout.TotalMilliseconds);

        public int GetSize() => plctag.get_size(pointer);

        public StatusCodes GetStatus() => (StatusCodes)plctag.status(pointer);

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