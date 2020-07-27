using System;
using System.Net;

namespace libplctag
{
    public interface ITag
    {
        PlcType? PlcType { get; set; }
        int? ElementCount { get; set; }
        int? ElementSize { get; set; }
        string Gateway { get; set; }
        string Name { get; set; }
        string Path { get; set; }
        Protocol? Protocol { get; set; }
        int? ReadCacheMillisecondDuration { get; set; }
        bool? UseConnectedMessaging { get; set; }

        void Abort();
        void Dispose();
        int GetSize();
        Status GetStatus();
        void Read(int timeout);
        void Write(int timeout);
        void Initialize(int timeout);
    }
}