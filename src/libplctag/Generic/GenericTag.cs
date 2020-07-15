using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace libplctag.Generic
{
    public class GenericTag<TPlcType, TDotNetType> : ITag, IGenericTag<TDotNetType> where TPlcType : IPlcType<TDotNetType>, new()
    {

        private readonly TPlcType plcType;
        private readonly Tag tag;

        /// <summary>
        /// Provides a new tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="gateway">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <param name="cpuType">Allen-Bradley CPU model</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="elementCount">elements count: 1- single, n-array.</param>
        /// <param name="millisecondTimeout"></param>
        /// <param name="debugLevel"></param>
        /// <param name="protocol">Currently only ab_eip supported.</param>
        /// <param name="readCacheMillisecondDuration">Set the amount of time to cache read results</param>
        /// <param name="useConnectedMessaging">Control whether to use connected or unconnected messaging.</param>
        public GenericTag(IPAddress gateway,
                   string path,
                   CpuType cpuType,
                   string name,
                   int millisecondTimeout,
                   int elementCount = 1,
                   DebugLevel debugLevel = DebugLevel.None,
                   Protocol protocol = Protocol.ab_eip,
                   int readCacheMillisecondDuration = default,
                   bool useConnectedMessaging = true)
        {
            //Instantiate our definition
            //TODO: These could be singleton or a private static lookup for performance

            plcType = new TPlcType();
            var elementSize = plcType.ElementSize;


            this.tag = new Tag(gateway,
                               path,
                               cpuType,
                               elementSize,
                               name,
                               millisecondTimeout,
                               elementCount,
                               debugLevel,
                               protocol,
                               readCacheMillisecondDuration,
                               useConnectedMessaging);
        }

        public TDotNetType Value { get => plcType.Decode(tag); set => plcType.Encode(tag, value); }
        public byte CipCode => plcType.CipCode;

        public void Read(int timeout)
        {
            ////HACK: To properly deal with zero timeout, this should be an async
            //if (timeout == TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout), "Currently reads must have timeout > 0");

            tag.Read(timeout);
        }

        public void Write(int timeout)
        {
            ////HACK: To properly deal with zero timeout, this should be an async
            //if (timeout == TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout), "Currently writes must have timeout > 0");

            tag.Write(timeout);
        }

        public void Abort()
        {
            ((ITag)tag).Abort();
        }

        public void Dispose()
        {
            ((ITag)tag).Dispose();
        }

        public int GetSize()
        {
            return ((ITag)tag).GetSize();
        }

        public Status GetStatus()
        {
            return ((ITag)tag).GetStatus();
        }

        public CpuType CPU => ((ITag)tag).CPU;

        public DebugLevel DebugLevel { get => ((ITag)tag).DebugLevel; set => ((ITag)tag).DebugLevel = value; }

        public int ElementCount => ((ITag)tag).ElementCount;

        public int ElementSize => ((ITag)tag).ElementSize;

        public IPAddress Gateway => ((ITag)tag).Gateway;

        public string Name => ((ITag)tag).Name;

        public string Path => ((ITag)tag).Path;

        public Protocol Protocol => ((ITag)tag).Protocol;

        public int ReadCacheMillisecondDuration { get => ((ITag)tag).ReadCacheMillisecondDuration; set => ((ITag)tag).ReadCacheMillisecondDuration = value; }

        public bool UseConnectedMessaging => ((ITag)tag).UseConnectedMessaging;

    }

}
