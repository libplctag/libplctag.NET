using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace libplctag.Generic
{
    public class GenericTag<TPlcType, TDotNetType> : ITag, IGenericTag<TDotNetType> where TPlcType : IPlcType<TDotNetType>, new()
    {

        private readonly TPlcType plcDataType;
        private readonly Tag tag;

        /// <summary>
        /// Provides a new tag. If the PLC type is Logix, the port type and slot has to be specified.
        /// </summary>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        public GenericTag(string name)
        {
            //Instantiate our definition
            //TODO: These could be singleton or a private static lookup for performance

            plcDataType = new TPlcType();
            var elementSize = plcDataType.ElementSize;


            this.tag = new Tag(name);
        }

        public TDotNetType Value { get => plcDataType.Decode(tag); set => plcDataType.Encode(tag, value); }
        public byte CipCode => plcDataType.CipCode;

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

        public PlcType? PlcType => ((ITag)tag).PlcType;

        public int? ElementCount => ((ITag)tag).ElementCount;

        public int? ElementSize => ((ITag)tag).ElementSize;

        public string Gateway => ((ITag)tag).Gateway;

        public string Name => ((ITag)tag).Name;

        public string Path => ((ITag)tag).Path;

        public Protocol? Protocol => ((ITag)tag).Protocol;

        public int? ReadCacheMillisecondDuration { get => ((ITag)tag).ReadCacheMillisecondDuration; set => ((ITag)tag).ReadCacheMillisecondDuration = value; }

        public bool? UseConnectedMessaging => ((ITag)tag).UseConnectedMessaging;

    }

}
