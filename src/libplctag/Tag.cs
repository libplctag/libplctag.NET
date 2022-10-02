using System;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{

    public sealed class Tag : IDisposable
    {

        readonly NativeTagWrapper _tag = new NativeTagWrapper(new NativeTag());




        /// <summary>
        /// True if <see cref="Initialize"/> or <see cref="InitializeAsync"/> has been called.
        /// </summary>
        public bool IsInitialized => _tag.IsInitialized;


        /// <summary>
        /// Optional An integer number of elements per tag .
        /// </summary>
        /// 
        /// <remarks>
        /// All tags are treated as arrays.
        /// Tags that are not arrays are considered to have a length of one element.
        /// This attribute determines how many elements are in the tag.
        /// Defaults to one (1) if not found.
        /// </remarks>
        public int? ElementCount
        {
            get => _tag.ElementCount;
            set => _tag.ElementCount = value;
        }


        /// <summary>
        /// Optional An integer number of bytes per element
        /// </summary>
        /// 
        /// <remarks>
        /// This attribute determines the size of a single element of the tag.
        /// Ignored for Modbus and for Allen-Bradley PLCs.
        /// </remarks>
        public int? ElementSize
        {
            get => _tag.ElementSize;
            set => _tag.ElementSize = value;
        }


        /// <summary>
        /// This tells the library what host name or IP address to use for the PLC 
        /// or the gateway to the PLC (in the case that the PLC is remote).
        /// </summary>
        public string Gateway
        {
            get => _tag.Gateway;
            set => _tag.Gateway = value;
        }


        /// <summary>
        /// This is the full name of the tag.
        /// For program tags, prepend `Program:{ProgramName}.` 
        /// where {ProgramName} is the name of the program in which the tag is created.
        /// </summary>
        public string Name
        {
            get => _tag.Name;
            set => _tag.Name = value;
        }


        /// <summary>
        /// This attribute is required for CompactLogix/ControlLogix tags 
        /// and for tags using a DH+ protocol bridge (i.e. a DHRIO module) to get to a PLC/5, SLC 500, or MicroLogix PLC on a remote DH+ link. 
        /// The attribute is ignored if it is not a DH+ bridge route, but will generate a warning if debugging is active. 
        /// Note that Micro800 connections must not have a path attribute.
        /// </summary>
        public string Path
        {
            get => _tag.Path;
            set => _tag.Path = value;
        }

        /// <summary>
        /// The type of PLC
        /// </summary>
        public PlcType? PlcType
        {
            get => _tag.PlcType;
            set => _tag.PlcType = value;
        }


        /// <summary>
        /// Determines the type of the PLC Protocol.
        /// </summary>
        public Protocol? Protocol
        {
            get => _tag.Protocol;
            set => _tag.Protocol = value;
        }

        /// <summary>
        /// Optional. Use this attribute to cause the tag read operations to cache data the requested number of milliseconds. 
        /// This can be used to lower the actual number of requests against the PLC. 
        /// Example read_cache_ms=100 will result in read operations no more often than once every 100 milliseconds.
        /// </summary>
        public int? ReadCacheMillisecondDuration
        {
            get => _tag.ReadCacheMillisecondDuration;
            set => _tag.ReadCacheMillisecondDuration = value;
        }

        /// <summary>
        /// A timeout value that is used for Initialize/Read/Write methods.
        /// It applies to both synchronous and asynchronous calls.
        /// </summary>
        public TimeSpan Timeout
        {
            get => _tag.Timeout;
            set => _tag.Timeout = value;
        }

        /// <summary>
        /// Optional. Control whether to use connected or unconnected messaging. 
        /// Only valid on Logix-class PLCs. Connected messaging is required on Micro800 and DH+ bridged links. 
        /// Default is PLC-specific and link-type specific. Generally you do not need to set this.
        /// </summary>
        public bool? UseConnectedMessaging
        {
            get => _tag.UseConnectedMessaging;
            set => _tag.UseConnectedMessaging = value;
        }

        /// <summary>
        /// Optional. An integer number of milliseconds to periodically read data from the PLC.
        /// </summary>
        /// 
        /// <remarks>
        /// Use this attribute to automatically read data from the PLC on a set interval.
        /// This can be used in conjunction with the <see cref="ReadStarted"/> and <see cref="ReadCompleted"/> events to respond to the data updates.
        /// </remarks>
        public TimeSpan? AutoSyncReadInterval
        {
            get => _tag.AutoSyncReadInterval;
            set => _tag.AutoSyncReadInterval = value;
        }

        /// <summary>
        /// Optional. An integer number of milliseconds to buffer tag data changes before writing to the PLC.
        /// </summary>
        /// 
        /// <remarks>
        /// Use this attribute to automatically write data to the PLC a set duration after setting its value.
        /// This can be used to lower the actual number of write operations by locally buffering local writes, and only writing to the PLC the most recent one when the wait completes.
        /// You can determine when a write starts and completes by catching the <see cref="WriteStarted"/> and <see cref="WriteCompleted"/> events.
        /// </remarks>
        public TimeSpan? AutoSyncWriteInterval
        {
            get => _tag.AutoSyncWriteInterval;
            set => _tag.AutoSyncWriteInterval = value;
        }

        public DebugLevel DebugLevel
        {
            get => _tag.DebugLevel;
            set => _tag.DebugLevel = value;
        }

        /// <summary>
        /// Configures. the byte order of 16-bit integers.
        /// </summary>
        public string Int16ByteOrder
        {
            get => _tag.Int16ByteOrder;
            set => _tag.Int16ByteOrder = value;
        }

        /// <summary>
        /// Optional. Configures the byte order of 32-bit integers.
        /// </summary>
        public string Int32ByteOrder
        {
            get => _tag.Int32ByteOrder;
            set => _tag.Int32ByteOrder = value;
        }

        /// <summary>
        /// Optional. Configures the byte order of 64-bit integers.
        /// </summary>
        public string Int64ByteOrder
        {
            get => _tag.Int64ByteOrder;
            set => _tag.Int64ByteOrder = value;
        }

        /// <summary>
        /// Optional. Configures the byte order of 32-bit floating point values.
        /// </summary>
        public string Float32ByteOrder
        {
            get => _tag.Float32ByteOrder;
            set => _tag.Float32ByteOrder = value;
        }

        /// <summary>
        /// Optional. Configures the byte order of 64-bit floating point values.
        /// </summary>
        public string Float64ByteOrder
        {
            get => _tag.Float64ByteOrder;
            set => _tag.Float64ByteOrder = value;
        }

        /// <summary>
        /// Optional. A positive integer value of 1, 2, 4, or 8 determining how big the leading count word is in a string.
        /// </summary>
        public uint? StringCountWordBytes
        {
            get => _tag.StringCountWordBytes;
            set => _tag.StringCountWordBytes = value;
        }

        /// <summary>
        /// Optional. Determines whether character bytes are swapped within 16-bit words.
        /// </summary>
        public bool? StringIsByteSwapped
        {
            get => _tag.StringIsByteSwapped;
            set => _tag.StringIsByteSwapped = value;
        }

        /// <summary>
        /// Optional. Determines whether strings have a count word or not.
        /// </summary>
        public bool? StringIsCounted
        {
            get => _tag.StringIsCounted;
            set => _tag.StringIsCounted = value;
        }

        /// <summary>
        /// Optional. Determines whether strings have a fixed length that they occupy.
        /// </summary>
        public bool? StringIsFixedLength
        {
            get => _tag.StringIsFixedLength;
            set => _tag.StringIsFixedLength = value;
        }

        /// <summary>
        /// Optional. Determines whether strings are zero-terminated as is done in C.
        /// </summary>
        public bool? StringIsZeroTerminated
        {
            get => _tag.StringIsZeroTerminated;
            set => _tag.StringIsZeroTerminated = value;
        }

        /// <summary>
        /// Optional. Determines the maximum number of character bytes in a string.
        /// </summary>
        public uint? StringMaxCapacity
        {
            get => _tag.StringMaxCapacity;
            set => _tag.StringMaxCapacity = value;
        }

        /// <summary>
        /// Optional. A positive integer value determining the total number of padding bytes at the end of a string.
        /// </summary>
        public uint? StringPadBytes
        {
            get => _tag.StringPadBytes;
            set => _tag.StringPadBytes = value;
        }

        /// <summary>
        /// Optional. A positive integer value determining the total number of bytes used in the tag buffer by a string. Must be used with str_is_fixed_length.
        /// </summary>
        public uint? StringTotalLength
        {
            get => _tag.StringTotalLength;
            set => _tag.StringTotalLength = value;
        }

        /// <summary>
        /// Creates the underlying data structures and references required before tag operations.
        /// </summary>
        /// 
        /// <remarks>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </remarks>
        /// <exception cref="LibPlcTagException"></exception>
        public void Initialize() => _tag.Initialize();

        /// <summary>
        /// Creates the underlying data structures and references required before tag operations.
        /// </summary>
        /// 
        /// <remarks>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </remarks>
        public Task InitializeAsync(CancellationToken token = default) => _tag.InitializeAsync(token);

        /// <summary>
        /// Executes a synchronous read on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        /// 
        /// <remarks>
        /// Reading a tag brings the data at the time of read into the local memory of the PC running the library. 
        /// The data is not automatically kept up to date. 
        /// If you need to find out the data periodically, you need to read the tag periodically.
        /// </remarks>
        public void Read() => _tag.Read();

        /// <summary>
        /// Executes an asynchronous read on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        /// 
        /// <remarks>
        /// Reading a tag brings the data at the time of read into the local memory of the PC running the library. 
        /// The data is not automatically kept up to date. 
        /// If you need to find out the data periodically, you need to read the tag periodically.
        /// </remarks>
        public Task ReadAsync(CancellationToken token = default) => _tag.ReadAsync(token);

        /// <summary>
        /// Executes a synchronous write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        /// 
        /// <remarks>
        /// Writing a tag sends the data from local memory to the target PLC.
        /// </remarks>
        public void Write() => _tag.Write();

        /// <summary>
        /// Executes an asynchronous write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        /// 
        /// <remarks>
        /// Writing a tag sends the data from local memory to the target PLC.
        /// </remarks>
        public Task WriteAsync(CancellationToken token = default) => _tag.WriteAsync(token);

        /// <summary>
        /// Creates the underlying data structures and references required before tag operations.
        /// </summary>
        /// 
        /// <returns>
        /// Whether the operation was successful. 
        /// </returns>
        /// 
        /// <remarks>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </remarks>
        public bool TryInitialize()
        {
            var status = _tag.TryInitialize();
            return _tag.IsStatusOk(status);
        }

        public async Task<bool> TryInitializeAsync(CancellationToken token = default)
        {
            var status = await _tag.TryInitializeAsync(token);
            return _tag.IsStatusOk(status);
        }

        public bool TryRead()
        {
            var status = _tag.TryRead();
            return _tag.IsStatusOk(status);
        }

        public async Task<bool> TryReadAsync(CancellationToken token = default)
        {
            var status = await _tag.TryReadAsync(token);
            return _tag.IsStatusOk(status);
        }

        public bool TryWrite()
        {
            var status = _tag.TryWrite();
            return _tag.IsStatusOk(status);
        }

        public async Task<bool> TryWriteAsync(CancellationToken token = default)
        {
            var status = await _tag.TryWriteAsync(token);
            return _tag.IsStatusOk(status);
        }

        public void Abort()                                 => _tag.Abort();
        public void Dispose()                               => _tag.Dispose();

        /// <summary>
        /// This function retrieves a segment of raw, unprocessed bytes from the tag buffer.
        /// </summary>
        public byte[] GetBuffer()                           => _tag.GetBuffer();
        public int GetSize()                                => _tag.GetSize();
        public void SetSize(int newSize)                    => _tag.SetSize(newSize);

        /// <summary>
        /// Check the operational status of the tag
        /// </summary>
        /// <returns>Tag's current status</returns>
        public Status GetStatus()                           => _tag.GetStatus();

        public bool IsStatusOk(Status status)               => _tag.IsStatusOk(status);

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


        public void SetString(int offset, string value)     => _tag.SetString(offset, value);
        public int GetStringLength(int offset)              => _tag.GetStringLength(offset);
        public int GetStringTotalLength(int offset)         => _tag.GetStringTotalLength(offset);
        public int GetStringCapacity(int offset)            => _tag.GetStringCapacity(offset);
        public string GetString(int offset)                 => _tag.GetString(offset);


        public event EventHandler<TagEventArgs> ReadStarted
        {
            add => _tag.ReadStarted += value;
            remove => _tag.ReadStarted -= value;
        }
        public event EventHandler<TagEventArgs> ReadCompleted
        {
            add => _tag.ReadCompleted += value;
            remove => _tag.ReadCompleted -= value;
        }
        public event EventHandler<TagEventArgs> WriteStarted
        {
            add => _tag.WriteStarted += value;
            remove => _tag.WriteStarted -= value;
        }
        public event EventHandler<TagEventArgs> WriteCompleted
        {
            add => _tag.WriteCompleted += value;
            remove => _tag.WriteCompleted -= value;
        }
        public event EventHandler<TagEventArgs> Aborted
        {
            add => _tag.Aborted += value;
            remove => _tag.Aborted -= value;
        }
        public event EventHandler<TagEventArgs> Destroyed
        {
            add => _tag.Destroyed += value;
            remove => _tag.Destroyed -= value;
        }

        ~Tag()
        {
            Dispose();
        }

    }
}