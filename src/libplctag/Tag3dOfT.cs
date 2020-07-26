using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace libplctag
{
    public class Tag3d<Marshaller, T>
        where Marshaller : IMarshaller<T>, new()
    {

        Tag _tag;

        IMarshaller<T> _marshaller;

        /// <summary>
        /// Provides a new tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="gateway">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <param name="plcType">Allen-Bradley CPU model</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="dimension1Length">Length of array dimension 1</param>
        /// <param name="dimension2Length">Length of array dimension 2</param>
        /// <param name="dimension3Length">Length of array dimension 3</param>
        /// <param name="millisecondTimeout"></param>
        /// <param name="protocol">Currently only ab_eip supported.</param>
        /// <param name="readCacheMillisecondDuration">Set the amount of time to cache read results</param>
        /// <param name="useConnectedMessaging">Control whether to use connected or unconnected messaging.</param>
        public Tag3d(IPAddress gateway,
                   string path,
                   PlcType plcType,
                   string name,
                   int dimension1Length,
                   int dimension2Length,
                   int dimension3Length,
                   int millisecondTimeout,
                   Protocol protocol = Protocol.ab_eip,
                   int readCacheMillisecondDuration = default,
                   bool useConnectedMessaging = true)
        {

            Dimension1Length = dimension1Length;
            Dimension2Length = dimension2Length;
            Dimension3Length = dimension3Length;

            _marshaller = new Marshaller()
            {
                PlcType = plcType
            };

            _tag = new Tag(
                gateway,
                path,
                plcType,
                _marshaller.ElementSize,
                name,
                millisecondTimeout,
                Dimension1Length * Dimension2Length * Dimension3Length,
                protocol,
                readCacheMillisecondDuration,
                useConnectedMessaging);

            Value = new T[Dimension1Length, Dimension2Length, Dimension3Length];
            DecodeAll();
        }

        public Protocol Protocol => _tag.Protocol;
        public IPAddress Gateway => _tag.Gateway;
        public string Path => _tag.Path;
        public PlcType PlcType => _tag.PlcType;
        public int Dimension1Length { get; }
        public int Dimension2Length { get; }
        public int Dimension3Length { get; }
        public string Name => _tag.Name;
        public bool UseConnectedMessaging => _tag.UseConnectedMessaging;
        public int ReadCacheMillisecondDuration
        {
            get => _tag.ReadCacheMillisecondDuration;
            set => _tag.ReadCacheMillisecondDuration = value;
        }


        public void Read(int millisecondTimeout)
        {
            _tag.Read(millisecondTimeout);
            DecodeAll();
        }

        public void Write(int millisecondTimeout)
        {
            EncodeAll();
            _tag.Write(millisecondTimeout);
        }

        void DecodeAll()
        {
            for (int ii = 0; ii < Dimension1Length; ii++)
                for (int jj = 0; jj < Dimension2Length; jj++)
                    for (int kk = 0; kk < Dimension3Length; kk++)
                        Value[ii, jj, kk] = _marshaller.Decode(_tag, GetOffset(ii, jj, kk));
        }
        void EncodeAll()
        {
            for (int ii = 0; ii < Dimension1Length; ii++)
                for (int jj = 0; jj < Dimension2Length; jj++)
                    for (int kk = 0; kk < Dimension3Length; kk++)
                        _marshaller.Encode(_tag, GetOffset(ii, jj, kk), Value[ii, jj, kk]);
        }
        int GetOffset(int i, int j, int k) => _marshaller.ElementSize * (i * Dimension2Length * Dimension3Length + j * Dimension3Length + k);

        public Status GetStatus() => _tag.GetStatus();

        public T[,,] Value { get; set; }

    }
}
