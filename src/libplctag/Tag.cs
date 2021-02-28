using System;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{
    public sealed class Tag : IDisposable
    {

        NativeTagWrapper _tag = new NativeTagWrapper(new NativeTag());

        public bool IsInitialized => _tag.IsInitialized;
        public int? ElementCount
        {
            get => _tag.ElementCount;
            set => _tag.ElementCount = value;
        }
        public int? ElementSize
        {
            get => _tag.ElementSize;
            set => _tag.ElementSize = value;
        }
        public string Gateway
        {
            get => _tag.Gateway;
            set => _tag.Gateway = value;
        }
        public string Name
        {
            get => _tag.Name;
            set => _tag.Name = value;
        }
        public string Path
        {
            get => _tag.Path;
            set => _tag.Path = value;
        }
        public PlcType? PlcType
        {
            get => _tag.PlcType;
            set => _tag.PlcType = value;
        }
        public Protocol? Protocol
        {
            get => _tag.Protocol;
            set => _tag.Protocol = value;
        }
        public int? ReadCacheMillisecondDuration
        {
            get => _tag.ReadCacheMillisecondDuration;
            set => _tag.ReadCacheMillisecondDuration = value;
        }
        public TimeSpan Timeout
        {
            get => _tag.Timeout;
            set => _tag.Timeout = value;
        }
        public bool? UseConnectedMessaging
        {
            get => _tag.UseConnectedMessaging;
            set => _tag.UseConnectedMessaging = value;
        }

        /// <summary>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </summary>
        public void Initialize() => _tag.Initialize();

        /// <summary>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </summary>
        public Task InitializeAsync(CancellationToken token = default) => _tag.InitializeAsync(token);

        /// <summary>
        /// Executes a synchronous read on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public void Read() => _tag.Read();

        /// <summary>
        /// Executes an asynch read on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public Task ReadAsync(CancellationToken token = default) => _tag.ReadAsync(token);

        /// <summary>
        /// Executes a synchronous write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public void Write() => _tag.Write();

        /// <summary>
        /// Executes an asynch write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public Task WriteAsync(CancellationToken token = default) => _tag.WriteAsync(token);

        public void Abort()                                 => _tag.Abort();
        public void Dispose()                               => _tag.Dispose();
        public byte[] GetBuffer()                           => _tag.GetBuffer();
        public int GetSize()                                => _tag.GetSize();
        public Status GetStatus()                           => _tag.GetStatus();

        public bool GetBit(int offset)                      => _tag.GetBit(offset);
        public void SetBit(int offset, bool value)          => _tag.SetBit(offset, value);

        public float GetFloat32(int offset)                 => _tag.GetFloat32(offset);
        public void SetFloat32(int offset, float value)     => _tag.SetFloat32(offset, value);

        public double GetFloat64(int offset)                => _tag.GetFloat64(offset);
        public void SetFloat64(int offset, double value)    => _tag.SetFloat64(offset, value);

        public sbyte GetInt8(int offset)                    => _tag.GetInt8(offset);
        public void SetInt8(int offset, sbyte value)        => _tag.SetInt8(offset, value);

        public short GetInt16(int offset)                   => _tag.GetInt16(offset);
        public void SetInt16(int offset, short value)       => _tag.SetInt16(offset, value);

        public int GetInt32(int offset)                     => _tag.GetInt32(offset);
        public void SetInt32(int offset, int value)         => _tag.SetInt32(offset, value);

        public long GetInt64(int offset)                    => _tag.GetInt64(offset);
        public void SetInt64(int offset, long value)        => _tag.SetInt64(offset, value);

        public byte GetUInt8(int offset)                    => _tag.GetUInt8(offset);
        public void SetUInt8(int offset, byte value)        => _tag.SetUInt8(offset, value);

        public ushort GetUInt16(int offset)                 => _tag.GetUInt16(offset);
        public void SetUInt16(int offset, ushort value)     => _tag.SetUInt16(offset, value);

        public uint GetUInt32(int offset)                   => _tag.GetUInt32(offset);
        public void SetUInt32(int offset, uint value)       => _tag.SetUInt32(offset, value);

        public ulong GetUInt64(int offset)                  => _tag.GetUInt64(offset);
        public void SetUInt64(int offset, ulong value)      => _tag.SetUInt64(offset, value);

        public string GetString(int offset)                 => _tag.GetString(offset);
        public void SetString(int offset, string value)     => _tag.SetString(offset, value);

    }
}