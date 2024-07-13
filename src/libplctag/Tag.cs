// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{

    public sealed class Tag : IDisposable
    {

        readonly NativeTagWrapper _tag;

        public Tag()
        {
            _tag = new NativeTagWrapper(new NativeTag());

            _tag.ReadStarted += (s, e) => ReadStarted?.Invoke(this, e);
            _tag.ReadCompleted += (s, e) => ReadCompleted?.Invoke(this, e);
            _tag.WriteStarted += (s, e) => WriteStarted?.Invoke(this, e);
            _tag.WriteCompleted += (s, e) => WriteCompleted?.Invoke(this, e);
            _tag.Aborted += (s, e) => Aborted?.Invoke(this, e);
            _tag.Destroyed += (s, e) => Destroyed?.Invoke(this, e);
        }


        /// <summary>
        /// True if <see cref="Initialize"/> or <see cref="InitializeAsync"/> has been called.
        /// </summary>
        public bool IsInitialized => _tag.IsInitialized;


        /// <summary>
        /// [OPTIONAL]
        /// An integer number of elements per tag.
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
        /// [REQUIRED/OPTIONAL]
        /// An integer number of bytes per element
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
        /// [REQUIRED]
        /// IP address or host name
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// This tells the library what host name or IP address to use for the PLC or the gateway to the PLC (in the case that the PLC is remote).
        /// </para>
        /// <para>
        /// [AB-SPECIFIC]
        /// Only for PLCs with additional routing.
        /// This attribute is required for CompactLogix/ControlLogix tags and for tags using a DH+ protocol bridge (i.e. a DHRIO module) to get to a PLC/5, SLC 500, or MicroLogix PLC on a remote DH+ link. The attribute is ignored if it is not a DH+ bridge route, but will generate a warning if debugging is active.
        /// Note that Micro800 connections must not have a path attribute.
        /// </para>
        /// </remarks>
        public string Gateway
        {
            get => _tag.Gateway;
            set => _tag.Gateway = value;
        }


        /// <summary>
        /// <list type="table">
        /// <listheader>
        ///    <term>REQUIRED | AB-SPECIFIC</term>
        ///    <description>
        ///     The full name of the tag, e.g. <c>`TestBigTag[10,42]`</c> for CIP-based PLCs.
        ///     <para>
        ///         For program tags, prepend <c>`Program:{program name}.`</c> where {program name} is the name of the program in which the tag is created.
        ///         Example: <c>`Program:MyProgram.MyTag`</c>.
        ///         This will access the tag MyTag in the program MyProgram.
        ///         For PCCC-based PLCs, examples include <c>`N7:4`</c>, <c>`ST18:26`</c>, <c>`L20:2`</c>, <c>`B3:4/2`</c>, <c>`MG14:6.en`</c> etc.
        ///         Many common field name abbeviations are supported. 
        ///     </para>
        /// </description>
        /// </listheader>
        /// <listheader>
        ///    <term>REQUIRED | MODBUS-SPECIFIC</term>
        ///    <description>
        ///     The type and first register number of a tag, e.g. co42 for coil 42 (counts from zero).
        ///     <para>
        ///         The supported register type prefixes are co for coil, di for discrete inputs, hr for holding registers and ir for input registers.
        ///         The type prefix must be present and the register number must be greater than or equal to zero and less than or equal to 65535.
        ///         Modbus examples: co21 - coil 21, di22 - discrete input 22, hr66 - holding register 66, ir64000 - input register 64000.
        ///     </para>
        /// </description>
        /// </listheader>
        /// </list>
        /// </summary>
        public string Name
        {
            get => _tag.Name;
            set => _tag.Name = value;
        }


        /// <summary>
        /// <list type="table">
        /// <listheader>
        ///     <term>OPTIONAL | AB-SPECIFIC</term>
        ///     <description>
        ///     Only for PLCs with additional routing.
        ///     <para>
        ///         This attribute is required for CompactLogix/ControlLogix tags and for tags using a DH+ protocol bridge (i.e. a DHRIO module) to get to a PLC/5, SLC 500, or MicroLogix PLC on a remote DH+ link. 
        ///         The attribute is ignored if it is not a DH+ bridge route, but will generate a warning if debugging is active. 
        ///         Note that Micro800 connections must not have a path attribute.
        ///     </para>
        ///     </description>
        /// </listheader>
        /// <listheader>
        ///     <term>OPTIONAL | MODBUS-SPECIFIC</term>
        ///     <description>
        ///     The server/unit ID.
        ///     Must be an integer value between 0 and 255.
        ///     <para>
        ///         Servers may support more than one unit or may bridge to other units. Example: path=4 for accessing device unit ID 4.
        ///     </para>
        ///     </description>
        /// </listheader>
        /// </list>
        /// </summary>
        public string Path
        {
            get => _tag.Path;
            set => _tag.Path = value;
        }

        /// <summary>
        /// [REQUIRED | AB-SPECIFIC]
        /// Determines the type of the PLC.
        /// </summary>
        public PlcType? PlcType
        {
            get => _tag.PlcType;
            set => _tag.PlcType = value;
        }


        /// <summary>
        /// [REQUIRED]
        /// Determines the type of the PLC Protocol.
        /// </summary>
        public Protocol? Protocol
        {
            get => _tag.Protocol;
            set => _tag.Protocol = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// Use this attribute to cause the tag read operations to cache data the requested number of milliseconds. 
        /// </summary>
        /// 
        /// <remarks>
        /// This can be used to lower the actual number of requests against the PLC. 
        /// Example read_cache_ms=100 will result in read operations no more often than once every 100 milliseconds.
        /// </remarks>
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
        /// [OPTIONAL | AB-SPECIFIC]
        /// True = use CIP connection.
        /// False = use UCMM.
        /// </summary>
        /// <remarks>
        /// Control whether to use connected or unconnected messaging. 
        /// Only valid on Logix-class PLCs.
        /// Connected messaging is required on Micro800 and DH+ bridged links. 
        /// Default is PLC-specific and link-type specific.
        /// Generally you do not need to set this.
        /// </remarks>
        public bool? UseConnectedMessaging
        {
            get => _tag.UseConnectedMessaging;
            set => _tag.UseConnectedMessaging = value;
        }

        /// <summary>
        /// [OPTIONAL | AB-SPECIFIC]
        /// True = (default) allow use of multi-request CIP command.
        /// False = use only one CIP request per packet.
        /// </summary>
        /// <remarks>
        /// This is specific to individual PLC models.
        /// Generally only Control Logix-class PLCs support it.
        /// It is the default for those PLCs that support it as it greatly increases the performance of the communication channel to the PLC.
        /// </remarks>
        public bool? AllowPacking
        {
            get => _tag.AllowPacking;
            set => _tag.AllowPacking = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// An integer number of milliseconds to periodically read data from the PLC.
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
        /// [OPTIONAL]
        /// An integer number of milliseconds to buffer tag data changes before writing to the PLC.
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
        /// [OPTIONAL]
        /// A string indicating the byte order of 16-bit integers.
        /// </summary>
        /// 
        /// <remarks>
        /// Allowable values include `01` (little-endian) and `10` (big-endian).
        /// Defaults to `10` for Modbus and `01` for Allen-Bradley.
        /// </remarks>
        public string Int16ByteOrder
        {
            get => _tag.Int16ByteOrder;
            set => _tag.Int16ByteOrder = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A string indicating the byte order of 32-bit integers.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults to `3210`, strict big-endian, for Modbus and `0123` for Allen-Bradley PLCs.
        /// </remarks>
        public string Int32ByteOrder
        {
            get => _tag.Int32ByteOrder;
            set => _tag.Int32ByteOrder = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A string indicating the byte order of 64-bit integers.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults to `76543210`, strict big-endian, for Modbus and `01234567` for Allen-Bradley PLCs.
        /// </remarks>
        public string Int64ByteOrder
        {
            get => _tag.Int64ByteOrder;
            set => _tag.Int64ByteOrder = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A string indicating the byte order of 32-bit floating point values.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults to `3210`, strict big-endian, for Modbus. Allen-Bradley PLCs default to the PLC-native order.
        /// </remarks>
        public string Float32ByteOrder
        {
            get => _tag.Float32ByteOrder;
            set => _tag.Float32ByteOrder = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A string indicating the byte order of 64-bit floating point values.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults to `76543210`, strict big-endian, for Modbus and `01234567` for Allen-Bradley PLCs.
        /// </remarks>
        public string Float64ByteOrder
        {
            get => _tag.Float64ByteOrder;
            set => _tag.Float64ByteOrder = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A positive integer value of 1, 2, 4, or 8 determining how big the leading count word is in a string.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB Logix PLCs default to 4.
        /// AB PCCC PLCs default to 2.
        /// Must be used with <see cref="StringIsCounted"/>.
        /// </remarks>
        public uint? StringCountWordBytes
        {
            get => _tag.StringCountWordBytes;
            set => _tag.StringCountWordBytes = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// Determines whether character bytes are swapped within 16-bit words.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB Logix PLCs default to false and PCCC PLCs default to true.
        /// </remarks>
        public bool? StringIsByteSwapped
        {
            get => _tag.StringIsByteSwapped;
            set => _tag.StringIsByteSwapped = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// Determines whether strings have a count word or not.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB PLCs default to true.
        /// If set true, must be used with <see cref="StringCountWordBytes"/>.
        /// </remarks>
        public bool? StringIsCounted
        {
            get => _tag.StringIsCounted;
            set => _tag.StringIsCounted = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// Determines whether strings have a fixed length that they occupy.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC and tag type.
        /// AB defaults to true for ControlLogix and CompactLogix and 84 for PCCC-based PLCs.
        /// Listing tags is an exception as the tag names are counted, but not fixed length.
        /// </remarks>
        public bool? StringIsFixedLength
        {
            get => _tag.StringIsFixedLength;
            set => _tag.StringIsFixedLength = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// Determines whether strings are zero-terminated as is done in C.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB defaults to false.
        /// </remarks>
        public bool? StringIsZeroTerminated
        {
            get => _tag.StringIsZeroTerminated;
            set => _tag.StringIsZeroTerminated = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// Determines the maximum number of character bytes in a string.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB Logix and PCCC PLCs default to 82.
        /// </remarks>
        public uint? StringMaxCapacity
        {
            get => _tag.StringMaxCapacity;
            set => _tag.StringMaxCapacity = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A positive integer value determining the total number of padding bytes at the end of a string.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB Logix PLCs default to 2.
        /// AB PCCC PLCs default to 0.
        /// </remarks>
        public uint? StringPadBytes
        {
            get => _tag.StringPadBytes;
            set => _tag.StringPadBytes = value;
        }

        /// <summary>
        /// [OPTIONAL]
        /// A positive integer value determining the total number of bytes used in the tag buffer by a string.
        /// Must be used with <see cref="StringIsFixedLength"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// Defaults are set per PLC type.
        /// AB Logix PLCs default to 88.
        /// AB PCCC PLCs default to 84.
        /// </remarks>
        public uint? StringTotalLength
        {
            get => _tag.StringTotalLength;
            set => _tag.StringTotalLength = value;
        }

        /// <summary>
        /// [OPTIONAL | MODBUS-SPECIFIC]
        /// The Modbus specification allows devices to queue up to 16 requests at once.
        /// </summary>
        ///
        /// <remarks>
        /// The default is 1 and the maximum is 16.
        /// This allows the host to send multiple requests without waiting for the device to respond.
        /// Not all devices support up to 16 requests in flight.
        /// </remarks>
        public uint? MaxRequestsInFlight
        {
            get => _tag.MaxRequestsInFlight;
            set => _tag.MaxRequestsInFlight = value;
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

        public void Abort()                                 => _tag.Abort();
        public void Dispose()                               => _tag.Dispose();

        /// <summary>
        /// This function retrieves a segment of raw, unprocessed bytes from the tag buffer.
        /// </summary>
        /// <remarks>
        /// Note; allocates a new block of memory.
        /// If this is problematic, use <see cref="GetBuffer(byte[])"/> instead.
        /// </remarks>
        public byte[] GetBuffer()                           => _tag.GetBuffer();

        /// <summary>
        /// Fills the supplied buffer with the raw, unprocessed bytes from the tag buffer.
        /// </summary>
        /// <remarks>
        /// Use this instead of <see cref="GetBuffer()"/> to avoid creating a new block of memory.
        /// </remarks>
        public void GetBuffer(byte[] buffer)                => _tag.GetBuffer(buffer);

        /// <summary>
        /// Fills the supplied buffer with the raw, unprocessed bytes from the tag buffer.
        /// </summary>
        /// <remarks>
        /// Use this instead of <see cref="GetBuffer()"/> to avoid creating a new block of memory.
        /// </remarks>
        public void GetBuffer(int offset, byte[] buffer, int length)    => _tag.GetBuffer(offset, buffer, length);

        public void SetBuffer(byte[] newBuffer)                         => _tag.SetBuffer(newBuffer);
        public void SetBuffer(int offset, byte[] buffer, int length)    => _tag.SetBuffer(offset, buffer, length);

        /// <summary>
        /// This function retrieves an attribute of the raw tag byte array.
        /// </summary>
        public byte[] GetByteArrayAttribute(string attributeName)   => _tag.GetByteArrayAttribute(attributeName);
        public int GetSize()                                => _tag.GetSize();
        public int SetSize(int newSize)                    => _tag.SetSize(newSize);

        /// <summary>
        /// Check the operational status of the tag
        /// </summary>
        /// <returns>Tag's current status</returns>
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


        public void SetString(int offset, string value)     => _tag.SetString(offset, value);
        public int GetStringLength(int offset)              => _tag.GetStringLength(offset);
        public int GetStringTotalLength(int offset)         => _tag.GetStringTotalLength(offset);
        public int GetStringCapacity(int offset)            => _tag.GetStringCapacity(offset);
        public string GetString(int offset)                 => _tag.GetString(offset);

        public event EventHandler<TagEventArgs> ReadStarted;
        public event EventHandler<TagEventArgs> ReadCompleted;
        public event EventHandler<TagEventArgs> WriteStarted;
        public event EventHandler<TagEventArgs> WriteCompleted;
        public event EventHandler<TagEventArgs> Aborted;
        public event EventHandler<TagEventArgs> Destroyed;

        ~Tag()
        {
            Dispose();
        }

    }
}
