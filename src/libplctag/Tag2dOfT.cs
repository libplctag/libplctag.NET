using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace libplctag
{
    public class Tag2d<Marshaller, T>
        where Marshaller : IMarshaller<T>, new()
        where T : new()
    {

        Tag _tag;

        IMarshaller<T> _marshaller = new Marshaller();

        /// <summary>
        /// Provides a new tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="gateway">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <param name="cpuType">Allen-Bradley CPU model</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="dimension1Length">elements count: 1- single, n-array.</param>
        /// <param name="dimension2Length">elements count: 1- single, n-array.</param>
        /// <param name="millisecondTimeout"></param>
        /// <param name="protocol">Currently only ab_eip supported.</param>
        /// <param name="readCacheMillisecondDuration">Set the amount of time to cache read results</param>
        /// <param name="useConnectedMessaging">Control whether to use connected or unconnected messaging.</param>
        public Tag2d(IPAddress gateway,
                   string path,
                   CpuType cpuType,
                   string name,
                   int dimension1Length,
                   int dimension2Length,
                   int millisecondTimeout,
                   Protocol protocol = Protocol.ab_eip,
                   int readCacheMillisecondDuration = default,
                   bool useConnectedMessaging = true)
        {

            Dimension1Length = dimension1Length;
            Dimension2Length = dimension2Length;

            _tag = new Tag(
                gateway,
                path,
                cpuType,
                _marshaller.ElementSize,
                name,
                millisecondTimeout,
                dimension1Length * dimension2Length,
                protocol,
                readCacheMillisecondDuration,
                useConnectedMessaging);

            Value = new T[Dimension1Length, Dimension2Length];
            for (int ii = 0; ii < Dimension1Length; ii++)
                for (int jj = 0; jj < Dimension2Length; jj++)
                        Value[ii, jj] = new T();
        }

        public Protocol Protocol => _tag.Protocol;
        public IPAddress Gateway => _tag.Gateway;
        public string Path => _tag.Path;
        public CpuType CPU => _tag.CPU;
        public int Dimension1Length { get; }
        public int Dimension2Length { get; }
        public string Name => _tag.Name;
        public bool UseConnectedMessaging => _tag.UseConnectedMessaging;
        public int ReadCacheMillisecondDuration
        {
            get => _tag.ReadCacheMillisecondDuration;
            set => _tag.ReadCacheMillisecondDuration = value;
        }

        int GetUnderlyingArrayIndex(int i, int j) => i * Dimension2Length + j;

        public void Read(int millisecondTimeout)
        {
            _tag.Read(millisecondTimeout);

            for (int ii = 0; ii < Dimension1Length; ii++)
                for (int jj = 0; jj < Dimension2Length; jj++)
                    Value[ii,jj] = _marshaller.Decode(_tag, _marshaller.ElementSize * GetUnderlyingArrayIndex(ii,jj));
        }

        public void Write(int millisecondTimeout)
        {
            for (int ii = 0; ii < Dimension1Length; ii++)
                for (int jj = 0; jj < Dimension2Length; jj++)
                     _marshaller.Encode(_tag, _marshaller.ElementSize * GetUnderlyingArrayIndex(ii, jj), Value[ii,jj]);

            _tag.Write(millisecondTimeout);
        }

        public Status GetStatus() => _tag.GetStatus();

        public T[,] Value { get; set; }

    }
}
