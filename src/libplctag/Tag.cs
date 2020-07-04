using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using libplctag.NativeImport;

namespace libplctag
{

    public sealed class Tag : IDisposable
    {

        public Protocols Protocol { get; }
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
        public Tag(IPAddress gateway, string path, CpuTypes cpuType, int elementSize, string name, int elementCount = 1, TimeSpan timeout = default, DebugLevels debugLevel = DebugLevels.None, Protocols protocol = Protocols.ab_eip)
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

        private static string GetAttributeString(Protocols protocol, IPAddress gateway, string path, CpuTypes CPU, int elementSize, int elementCount, string name, DebugLevels debugLevel)
        {

            var attributes = new Dictionary<string, string>();

            attributes.Add("protocol", protocol.ToString());
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
                case StatusCodes.StatusPending:         break;
                case StatusCodes.StatusOk:              break;
                case StatusCodes.ErrorAbort:            throw new libplctag.AbortException();
                case StatusCodes.ErrorBadConfig:        throw new libplctag.BadConfigException();
                case StatusCodes.ErrorBadConnection:    throw new libplctag.BadConnectionException();
                case StatusCodes.ErrorBadData:          throw new libplctag.BadDataException();
                case StatusCodes.ErrorBadDevice:        throw new libplctag.BadDeviceException();
                case StatusCodes.ErrorBadGateway:       throw new libplctag.BadGatewayException();
                case StatusCodes.ErrorBadParam:         throw new libplctag.BadParameterException();
                case StatusCodes.ErrorBadReply:         throw new libplctag.BadReplyException();
                case StatusCodes.ErrorBadStatus:        throw new libplctag.BadStatusException();
                case StatusCodes.ErrorClose:            throw new libplctag.CloseException();
                case StatusCodes.ErrorCreate:           throw new libplctag.CreateException();
                case StatusCodes.ErrorDuplicate:        throw new libplctag.DuplicateException();
                case StatusCodes.ErrorEncode:           throw new libplctag.EncodeException();
                case StatusCodes.ErrorMutexDestroy:     throw new libplctag.MutexDestroyException();
                case StatusCodes.ErrorMutexInit:        throw new libplctag.MutexInitException();
                case StatusCodes.ErrorMutexLock:        throw new libplctag.MutexLockException();
                case StatusCodes.ErrorMutexUnlock:      throw new libplctag.MutexUnlockException();
                case StatusCodes.ErrorNotAllowed:       throw new libplctag.NotAllowedException();
                case StatusCodes.ErrorNotFound:         throw new libplctag.NotFoundException();
                case StatusCodes.ErrorNotImplemented:   throw new libplctag.NotImplementedException();
                case StatusCodes.ErrorNoData:           throw new libplctag.NoDataException();
                case StatusCodes.ErrorNoMatch:          throw new libplctag.NoMatchException();
                case StatusCodes.ErrorNoMem:            throw new libplctag.NoMemoryException();
                case StatusCodes.ErrorNoResources:      throw new libplctag.NoResourcesException();
                case StatusCodes.ErrorNullPtr:          throw new libplctag.NullPointerException();
                case StatusCodes.ErrorOpen:             throw new libplctag.OpenException();
                case StatusCodes.ErrorOutOfBounds:      throw new libplctag.OutOfBoundsException();
                case StatusCodes.ErrorRead:             throw new libplctag.ReadException();
                case StatusCodes.ErrorRemoteErr:        throw new libplctag.RemoteErrorException();
                case StatusCodes.ErrorThreadCreate:     throw new libplctag.ThreadCreateException();
                case StatusCodes.ErrorThreadJoin:       throw new libplctag.ThreadJoinException();
                case StatusCodes.ErrorTimeout:          throw new libplctag.TimeoutException();
                case StatusCodes.ErrorTooLarge:         throw new libplctag.TooLargeException();
                case StatusCodes.ErrorTooSmall:         throw new libplctag.TooSmallException();
                case StatusCodes.ErrorUnsupported:      throw new libplctag.UnsupportedException();
                case StatusCodes.ErrorWinsock:          throw new libplctag.WinsockException();
                case StatusCodes.ErrorWrite:            throw new libplctag.WriteException();
                default:                                throw new System.NotImplementedException();
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