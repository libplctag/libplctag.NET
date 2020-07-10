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
        TimeSpan ReadCacheDuration { get; set; }
        bool UseConnectedMessaging { get; }

        void Abort();
        void Dispose();
        int GetSize();
        StatusCode GetStatus();
        void Read(TimeSpan timeout);
        void Write(TimeSpan timeout);
    }
}