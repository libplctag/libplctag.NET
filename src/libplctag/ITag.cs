using System;
using System.Net;

namespace libplctag
{
    public interface ITag
    {
        CpuType CPU { get; }
        DebugLevel DebugLevel { get; set; }
        int ElementCount { get; }
        int ElementSize { get; }
        IPAddress Gateway { get; }
        string Name { get; }
        string Path { get; }
        Protocol Protocol { get; }
        int ReadCacheMillisecondDuration { get; set; }
        bool UseConnectedMessaging { get; }

        void Abort();
        void Dispose();
        int GetSize();
        Status GetStatus();
        void Read(int timeout);
        void Write(int timeout);
    }
}