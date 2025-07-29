// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static libplctag.NativeImport.plctag;

[assembly: InternalsVisibleTo("libplctag.Tests")]

namespace libplctag
{

    public sealed class Tag : IDisposable
    {
        private const int TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION = 0;
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan maxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);

        private readonly INative _native;

        private int nativeTagHandle;
        private callback_func_ex coreLibCallbackFuncExDelegate;

        private bool _isDisposed = false;
        private bool _isInitialized = false;

        private string _name;
        private Protocol? _protocol;
        private string _gateway;
        private PlcType? _plcType;
        private string _path;
        private int? _elementSize;
        private int? _elementCount;
        private bool? _useConnectedMessaging;
        private bool? _allowPacking;
        private bool? _allowFieldResize;
        private int? _readCacheMillisecondDuration;
        private uint? _maxRequestsInFlight;
        private TimeSpan _timeout = defaultTimeout;
        private TimeSpan? _autoSyncReadInterval;
        private TimeSpan? _autoSyncWriteInterval;
        private DebugLevel _debugLevel = DebugLevel.None;
        private string _int16ByteOrder;
        private string _int32ByteOrder;
        private string _int64ByteOrder;
        private string _float32ByteOrder;
        private string _float64ByteOrder;
        private uint? _stringCountWordBytes;
        private bool? _stringIsByteSwapped;
        private bool? _stringIsCounted;
        private bool? _stringIsFixedLength;
        private bool? _stringIsZeroTerminated;
        private uint? _stringPadBytes;
        private uint? _stringTotalLength;

        public Tag() : this(new Native())
        {
        }

        internal Tag(INative nativeMethods)
        {
            _native = nativeMethods;

            // Need to keep a reference to the delegate in memory so it doesn't get garbage collected
            coreLibCallbackFuncExDelegate = new callback_func_ex(coreLibEventCallback);
        }

        ~Tag()
        {
            Dispose();
        }

        /// <summary>
        /// True if <see cref="Initialize"/> or <see cref="InitializeAsync"/> has completed succesfully.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                ThrowIfAlreadyDisposed();
                return _isInitialized;
            }
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
            get => GetField(ref _name);
            set => SetField(ref _name, value);
        }

        /// <summary>
        /// [REQUIRED]
        /// Determines the type of the PLC Protocol.
        /// </summary>
        public Protocol? Protocol
        {
            get => GetField(ref _protocol);
            set => SetField(ref _protocol, value);
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
            get => GetField(ref _gateway);
            set => SetField(ref _gateway, value);
        }

        /// <summary>
        /// [REQUIRED | AB-SPECIFIC]
        /// Determines the type of the PLC.
        /// </summary>
        public PlcType? PlcType
        {
            get => GetField(ref _plcType);
            set => SetField(ref _plcType, value);
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
            get => GetField(ref _path);
            set => SetField(ref _path, value);
        }
        
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
            get => GetField(ref _elementCount);
            set => SetField(ref _elementCount, value);
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
            get => GetField(ref _elementSize);
            set => SetField(ref _elementSize, value);
        }

        /// <summary>
        /// A timeout value that is used for Initialize/Read/Write methods.
        /// It applies to both synchronous and asynchronous calls.
        /// </summary>
        public TimeSpan Timeout
        {
            get => GetField(ref _timeout);
            set => SetField(ref _timeout, value);
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
            get => GetField(ref _useConnectedMessaging);
            set => SetField(ref _useConnectedMessaging, value);
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
            get => GetField(ref _allowPacking);
            set => SetField(ref _allowPacking, value);
        }

        /// <summary>
        /// [OPTIONAL]
        /// True = Allow data functions to change the tag’s buffer size.
        /// False = Prevent functions like plc_tag_set_string() from changing the tag’s buffer size.
        /// </summary>
        /// <remarks>
        /// When set allows the library to resize a tag’s data buffer when a field within it changes size.
        /// This is the case when a string is set to a different size than it was when first read.
        /// When a resize happens, the buffer is split at the site of the field and the part before the field is not changed.
        /// The part after the field does not have changed contents, but the location of that contents will shift.
        /// </remarks>
        public bool? AllowFieldResize
        {
            get => GetField(ref _allowFieldResize);
            set => SetField(ref _allowFieldResize, value);
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
            get
            {
                ThrowIfAlreadyDisposed();
                return _readCacheMillisecondDuration;

            }
            set
            {
                ThrowIfAlreadyDisposed();

                if (_isInitialized)
                    SetIntAttribute("read_cache_ms", value.Value);
                
                // Set after writing to underlying tag in case SetIntAttribute fails.
                // Ensures the two have the same value.
                _readCacheMillisecondDuration = value;

            }
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
            get
            {
                ThrowIfAlreadyDisposed();
                return _autoSyncReadInterval;
            }
            set
            {
                ThrowIfAlreadyDisposed();

                if (_isInitialized)
                {
                    if (value is null)
                        SetIntAttribute("auto_sync_read_ms", 0);    // 0 is a special value that turns off auto sync
                    else
                        SetIntAttribute("auto_sync_read_ms", (int)value.Value.TotalMilliseconds);
                }
                
                // Set after writing to underlying tag in case SetIntAttribute fails.
                // Ensures the two have the same value.
                _autoSyncReadInterval = value;
            }
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
            get
            {
                ThrowIfAlreadyDisposed();
                return _autoSyncWriteInterval;
            }
            set
            {
                ThrowIfAlreadyDisposed();

                if (_isInitialized)
                {
                    if (value is null)
                        SetIntAttribute("auto_sync_write_ms", 0);    // 0 is a special value that turns off auto sync
                    else
                        SetIntAttribute("auto_sync_write_ms", (int)value.Value.TotalMilliseconds);
                }
                
                // Set after writing to underlying tag in case SetIntAttribute fails.
                // Ensures the two have the same value.
                _autoSyncWriteInterval = value;
            }
        }

        public DebugLevel DebugLevel
        {
            get
            {
                ThrowIfAlreadyDisposed();
                return _debugLevel;
            }
            set
            {
                ThrowIfAlreadyDisposed();

                if (_isInitialized)
                    SetDebugLevel(value);

                // Set after writing to underlying tag in case SetDebugLevel fails.
                // Ensures the two have the same value.
                _debugLevel = value;
            }
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
            get => GetField(ref _int16ByteOrder);
            set => SetField(ref _int16ByteOrder, value);
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
            get => GetField(ref _int32ByteOrder);
            set => SetField(ref _int32ByteOrder, value);
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
            get => GetField(ref _int64ByteOrder);
            set => SetField(ref _int64ByteOrder, value);
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
            get => GetField(ref _float32ByteOrder);
            set => SetField(ref _float32ByteOrder, value);
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
            get => GetField(ref _float64ByteOrder);
            set => SetField(ref _float64ByteOrder, value);
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
            get => GetField(ref _stringCountWordBytes);
            set => SetField(ref _stringCountWordBytes, value);
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
            get => GetField(ref _stringIsByteSwapped);
            set => SetField(ref _stringIsByteSwapped, value);
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
            get => GetField(ref _stringIsCounted);
            set => SetField(ref _stringIsCounted, value);
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
            get => GetField(ref _stringIsFixedLength);
            set => SetField(ref _stringIsFixedLength, value);
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
            get => GetField(ref _stringIsZeroTerminated);
            set => SetField(ref _stringIsZeroTerminated, value);
        }

        private uint? _stringMaxCapacity;
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
            get => GetField(ref _stringMaxCapacity);
            set => SetField(ref _stringMaxCapacity, value);
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
            get => GetField(ref _stringPadBytes);
            set => SetField(ref _stringPadBytes, value);
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
            get => GetField(ref _stringTotalLength);
            set => SetField(ref _stringTotalLength, value);
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
            get => GetField(ref _maxRequestsInFlight);
            set => SetField(ref _maxRequestsInFlight, value);
        }


        public void Dispose()
        {
            if (_isDisposed)
                return;

            if (_isInitialized)
            {
                // These should always succeed unless bugs exist in this wrapper or the core library
                var removeCallbackResult = RemoveCallback();
                var destroyResult = (Status)_native.plc_tag_destroy(nativeTagHandle);

                // However, we cannot recover if they do fail, so ignore except during development
                Debug.Assert(removeCallbackResult == Status.Ok);
                Debug.Assert(destroyResult == Status.Ok);

                RemoveEvents();
            }

            _isDisposed = true;
        }

        public void Abort()
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_abort(nativeTagHandle);
            ThrowIfStatusNotOk(result);
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
        public void Initialize()
        {

            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            SetUpEvents();

            var attributeString = GetAttributeString();
            
            var result = _native.plc_tag_create_ex(attributeString, coreLibCallbackFuncExDelegate, IntPtr.Zero, millisecondTimeout);
            if (result < 0)
            {
                RemoveEvents();
                throw new LibPlcTagException((Status)result);
            }
            else
            {
                nativeTagHandle = result;
            }


            _isInitialized = true;
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
        public async Task InitializeAsync(CancellationToken token = default)
        {
            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            SetUpEvents();

            var createTask = new TaskCompletionSource<Status>(TaskCreationOptions.RunContinuationsAsynchronously);
            createTasks.Push(createTask);

            var attributeString = GetAttributeString();
            var result = _native.plc_tag_create_ex(attributeString, coreLibCallbackFuncExDelegate, IntPtr.Zero, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);

            // Something went wrong locally
            if (result < 0)
            {
                // shouldn't we pop here ?!?
                RemoveEvents();
                throw new LibPlcTagException((Status)result);
            }
            else
            {
                nativeTagHandle = result;
            }

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                using (cts.Token.Register(() =>
                {
                    if (createTasks.TryPop(out _))
                    {
                        RemoveCallback();
                        Abort();
                        var destroyResult = (Status)_native.plc_tag_destroy(result);
                        Debug.Assert(destroyResult == Status.Ok);
                        RemoveEvents();

                        if (token.IsCancellationRequested)
                            createTask.SetCanceled();
                        else
                            createTask.SetException(new LibPlcTagException(Status.ErrorTimeout));
                    }
                }))
                {
                    // Await while Pending
                    if (GetStatus() == Status.Pending) // wouldn't it be safer to await anyway to avoid possible race conditions?
                    {
                        await createTask.Task.ConfigureAwait(false);
                    }
                }
            }

            // On error, tear down and throw
            if (createTask.Task.Result != Status.Ok)
            {
                RemoveCallback();
                var destroyResult = (Status)_native.plc_tag_destroy(nativeTagHandle);
                Debug.Assert(destroyResult == Status.Ok);
                RemoveEvents();
                throw new LibPlcTagException(createTask.Task.Result);
            }

            _isInitialized = true;
        }

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
        public void Read()
        {
            ThrowIfAlreadyDisposed();
            InitializeIfRequired();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            var result = (Status)_native.plc_tag_read(nativeTagHandle, millisecondTimeout);
            ThrowIfStatusNotOk(result);
        }

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
        public async Task ReadAsync(CancellationToken token = default)
        {
            ThrowIfAlreadyDisposed();
            await InitializeAsyncIfRequired(token).ConfigureAwait(false);

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                using (cts.Token.Register(() =>
                {
                    if (readTasks.TryPop(out var readTask))
                    {
                        Abort();

                        if (token.IsCancellationRequested)
                            readTask.SetCanceled();
                        else
                            readTask.SetException(new LibPlcTagException(Status.ErrorTimeout));
                    }
                }))
                {
                    var readTask = new TaskCompletionSource<Status>(TaskCreationOptions.RunContinuationsAsynchronously);
                    readTasks.Push(readTask);
                    _native.plc_tag_read(nativeTagHandle, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);
                    await readTask.Task.ConfigureAwait(false);
                    ThrowIfStatusNotOk(readTask.Task.Result);
                }
            }
        }

        /// <summary>
        /// Executes a synchronous write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        /// 
        /// <remarks>
        /// Writing a tag sends the data from local memory to the target PLC.
        /// </remarks>
        public void Write()
        {
            ThrowIfAlreadyDisposed();
            InitializeIfRequired();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            var result = (Status)_native.plc_tag_write(nativeTagHandle, millisecondTimeout);
            ThrowIfStatusNotOk(result);
        }

        /// <summary>
        /// Executes an asynchronous write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        /// 
        /// <remarks>
        /// Writing a tag sends the data from local memory to the target PLC.
        /// </remarks>
        public async Task WriteAsync(CancellationToken token = default)
        {
            ThrowIfAlreadyDisposed();
            await InitializeAsyncIfRequired(token).ConfigureAwait(false);

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                using (cts.Token.Register(() =>
                {
                    if (writeTasks.TryPop(out var writeTask))
                    {
                        Abort();

                        if (token.IsCancellationRequested)
                            writeTask.SetCanceled();
                        else
                            writeTask.SetException(new LibPlcTagException(Status.ErrorTimeout));
                    }
                }))
                {
                    var writeTask = new TaskCompletionSource<Status>(TaskCreationOptions.RunContinuationsAsynchronously);
                    writeTasks.Push(writeTask);
                    _native.plc_tag_write(nativeTagHandle, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);
                    await writeTask.Task.ConfigureAwait(false);
                    ThrowIfStatusNotOk(writeTask.Task.Result);
                }
            }
        }

        public int GetSize()
        {
            ThrowIfAlreadyDisposed();

            var result = _native.plc_tag_get_size(nativeTagHandle);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                return result;
        }

        public int SetSize(int newSize)
        {
            ThrowIfAlreadyDisposed();
            var result = _native.plc_tag_set_size(nativeTagHandle, newSize);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                return result;
        }

        /// <summary>
        /// Check the operational status of the tag
        /// </summary>
        /// <returns>Tag's current status</returns>
        public Status GetStatus()
        {
            ThrowIfAlreadyDisposed();
            return (Status)_native.plc_tag_status(nativeTagHandle);
        }

        /// <summary>
        /// This function retrieves a segment of raw, unprocessed bytes from the tag buffer.
        /// </summary>
        /// <remarks>
        /// Note; allocates a new block of memory.
        /// If this is problematic, use <see cref="GetBuffer(byte[])"/> instead.
        /// </remarks>
        public byte[] GetBuffer()
        {
            ThrowIfAlreadyDisposed();

            var tagSize = GetSize();
            var temp = new byte[tagSize];

            var result = (Status)_native.plc_tag_get_raw_bytes(nativeTagHandle, 0, temp, temp.Length);
            ThrowIfStatusNotOk(result);

            return temp;
        }

        /// <summary>
        /// Fills the supplied buffer with the raw, unprocessed bytes from the tag buffer.
        /// </summary>
        /// <remarks>
        /// Use this instead of <see cref="GetBuffer()"/> to avoid creating a new block of memory.
        /// </remarks>
        public void GetBuffer(byte[] buffer)
        {
            GetBuffer(0, buffer, buffer.Length);
        }

        /// <summary>
        /// Fills the supplied buffer with the raw, unprocessed bytes from the tag buffer.
        /// </summary>
        /// <remarks>
        /// Use this instead of <see cref="GetBuffer()"/> to avoid creating a new block of memory.
        /// </remarks>
        public void GetBuffer(int offset, byte[] buffer, int length)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_get_raw_bytes(nativeTagHandle, offset, buffer, length);
            ThrowIfStatusNotOk(result);
        }

        public void GetBuffer(Span<byte> buffer)
        {
            GetBuffer(0, buffer);
        }

        public void GetBuffer(int offset, Span<byte> buffer)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_get_raw_bytes(nativeTagHandle, offset, buffer);
            ThrowIfStatusNotOk(result);
        }

        public void SetBuffer(ReadOnlySpan<byte> buffer)
        {
            SetBuffer(0, buffer);
        }

        public void SetBuffer(int start_offset, ReadOnlySpan<byte> buffer)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_set_raw_bytes(nativeTagHandle, start_offset, buffer);
            ThrowIfStatusNotOk(result);
        }

        public void SetBuffer(byte[] buffer)
        {
            SetBuffer(0, buffer, buffer.Length);
        }

        public void SetBuffer(int start_offset, byte[] buffer, int length)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_set_raw_bytes(nativeTagHandle, start_offset, buffer, length);
            ThrowIfStatusNotOk(result);
        }


        private int GetIntAttribute(string attributeName)
        {
            var result = _native.plc_tag_get_int_attribute(nativeTagHandle, attributeName, int.MinValue);
            if (result == int.MinValue)
                ThrowIfStatusNotOk();

            return result;
        }

        private void SetIntAttribute(string attributeName, int value)
        {
            var result = (Status)_native.plc_tag_set_int_attribute(nativeTagHandle, attributeName, value);
            ThrowIfStatusNotOk(result);
        }


        /// <summary>
        /// This function retrieves an attribute of the raw tag byte array.
        /// </summary>
        public byte[] GetByteArrayAttribute(string attributeName)
        {
            ThrowIfAlreadyDisposed();

            var bufferLengthAttributeName = attributeName + ".length";
            var bufferLength = GetIntAttribute(bufferLengthAttributeName);
            var buffer = new byte[bufferLength];

            var result = _native.plc_tag_get_byte_array_attribute(nativeTagHandle, attributeName, buffer, buffer.Length);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                return buffer;
        }

        private void SetDebugLevel(DebugLevel level)
        {
            _native.plc_tag_set_debug_level((int)level);
        }

        public bool GetBit(int offset)
        {
            ThrowIfAlreadyDisposed();

            var result = _native.plc_tag_get_bit(nativeTagHandle, offset);
            if (result == 0)
                return false;
            else if (result == 1)
                return true;
            else
                throw new LibPlcTagException((Status)result);
        }

        public void SetBit(int offset, bool value)          => SetNativeTagValue(_native.plc_tag_set_bit, offset, value == true ? 1 : 0);

        public ulong GetUInt64(int offset)                  => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_uint64, offset, ulong.MaxValue);
        public void SetUInt64(int offset, ulong value)      => SetNativeTagValue(_native.plc_tag_set_uint64, offset, value);

        public long GetInt64(int offset)                    => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_int64, offset, long.MinValue);
        public void SetInt64(int offset, long value)        => SetNativeTagValue(_native.plc_tag_set_int64, offset, value);

        public uint GetUInt32(int offset)                   => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_uint32, offset, uint.MaxValue);
        public void SetUInt32(int offset, uint value)       => SetNativeTagValue(_native.plc_tag_set_uint32, offset, value);

        public int GetInt32(int offset)                     => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_int32, offset, int.MinValue);
        public void SetInt32(int offset, int value)         => SetNativeTagValue(_native.plc_tag_set_int32, offset, value);

        public ushort GetUInt16(int offset)                 => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_uint16, offset, ushort.MaxValue);
        public void SetUInt16(int offset, ushort value)     => SetNativeTagValue(_native.plc_tag_set_uint16, offset, value);

        public short GetInt16(int offset)                   => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_int16, offset, short.MinValue);
        public void SetInt16(int offset, short value)       => SetNativeTagValue(_native.plc_tag_set_int16, offset, value);

        public byte GetUInt8(int offset)                    => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_uint8, offset, byte.MaxValue);
        public void SetUInt8(int offset, byte value)        => SetNativeTagValue(_native.plc_tag_set_uint8, offset, value);

        public sbyte GetInt8(int offset)                    => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_int8, offset, sbyte.MinValue);
        public void SetInt8(int offset, sbyte value)        => SetNativeTagValue(_native.plc_tag_set_int8, offset, value);

        public double GetFloat64(int offset)                => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_float64, offset, double.MinValue);
        public void SetFloat64(int offset, double value)    => SetNativeTagValue(_native.plc_tag_set_float64, offset, value);

        public float GetFloat32(int offset)                 => GetNativeValueAndThrowOnSpecificResult(_native.plc_tag_get_float32, offset, float.MinValue);
        public void SetFloat32(int offset, float value)     => SetNativeTagValue(_native.plc_tag_set_float32, offset, value);



        public void SetString(int offset, string value)     => SetNativeTagValue(_native.plc_tag_set_string, offset, value);
        public int GetStringLength(int offset)              => GetNativeValueAndThrowOnNegativeResult(_native.plc_tag_get_string_length, offset);
        public int GetStringCapacity(int offset)            => GetNativeValueAndThrowOnNegativeResult(_native.plc_tag_get_string_capacity, offset);
        public int GetStringTotalLength(int offset)         => GetNativeValueAndThrowOnNegativeResult(_native.plc_tag_get_string_total_length, offset);
        public string GetString(int offset)
        {
            ThrowIfAlreadyDisposed();
            var stringLength = GetStringLength(offset);
            var sb = new StringBuilder(stringLength);
            var status = (Status)_native.plc_tag_get_string(nativeTagHandle, offset, sb, stringLength);
            ThrowIfStatusNotOk(status);
            return sb.ToString().Substring(0, stringLength < sb.Length ? stringLength : sb.Length);
        }


        private void ThrowIfAlreadyDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private void InitializeIfRequired()
        {
            if (!_isInitialized)
                Initialize();
        }

        private Task InitializeAsyncIfRequired(CancellationToken token)
        {
            if (!_isInitialized)
                return InitializeAsync(token);
            else
                return Task.CompletedTask;
        }

        private void ThrowIfAlreadyInitialized()
        {
            if (_isInitialized)
                throw new InvalidOperationException("Already initialized");
        }

        private void ThrowIfStatusNotOk(Status? status = null)
        {
            var statusToCheck = status ?? GetStatus();
            if (statusToCheck != Status.Ok)
                throw new LibPlcTagException(statusToCheck);
        }



        private void SetNativeTagValue<T>(Func<int, int, T, int> nativeMethod, int offset, T value)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)nativeMethod(nativeTagHandle, offset, value);
            ThrowIfStatusNotOk(result);
        }

        private int GetNativeValueAndThrowOnNegativeResult(Func<int, int, int> nativeMethod, int offset)
        {
            ThrowIfAlreadyDisposed();
            var result = nativeMethod(nativeTagHandle, offset);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            return result;
        }

        private T GetNativeValueAndThrowOnSpecificResult<T>(Func<int, int, T> nativeMethod, int offset, T valueIndicatingPossibleError)
            where T : struct
        {
            ThrowIfAlreadyDisposed();
            var result = nativeMethod(nativeTagHandle, offset);
            if (result.Equals(valueIndicatingPossibleError))
                ThrowIfStatusNotOk();
            return result;
        }

        private T GetField<T>(ref T field)
        {
            ThrowIfAlreadyDisposed();
            return field;
        }

        private void SetField<T>(ref T field, T value)
        {
            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();
            field = value;
        }

        private string GetAttributeString()
        {

            string FormatNullableBoolean(bool? value)
                => value.HasValue ? (value.Value ? "1" : "0") : null;

            string FormatPlcType(PlcType? type)
            {
                if (type == libplctag.PlcType.Omron)
                    return "omron-njnx";
                else
                    return type?.ToString().ToLowerInvariant();
            }

            string FormatTimeSpan(TimeSpan? timespan)
            {
                if(timespan.HasValue)
                    return ((int)timespan.Value.TotalMilliseconds).ToString();
                else
                    return null;
            }

            var attributes = new Dictionary<string, string>
            {
                { "protocol",               Protocol?.ToString() },
                { "gateway",                Gateway },
                { "path",                   Path },
                { "plc",                    FormatPlcType(PlcType) },
                { "elem_size",              ElementSize?.ToString() },
                { "elem_count",             ElementCount?.ToString() },
                { "name",                   Name },
                { "read_cache_ms",          ReadCacheMillisecondDuration?.ToString() },
                { "use_connected_msg",      FormatNullableBoolean(UseConnectedMessaging) },
                { "allow_packing",          FormatNullableBoolean(AllowPacking) },
                { "auto_sync_read_ms",      FormatTimeSpan(AutoSyncReadInterval) },
                { "auto_sync_write_ms",     FormatTimeSpan(AutoSyncWriteInterval) },
                { "debug",                  DebugLevel == DebugLevel.None ? null : ((int)DebugLevel).ToString() },
                { "int16_byte_order",       Int16ByteOrder },
                { "int32_byte_order",       Int32ByteOrder },
                { "int64_byte_order",       Int64ByteOrder },
                { "float32_byte_order",     Float32ByteOrder },
                { "float64_byte_order",     Float64ByteOrder },
                { "str_count_word_bytes",   StringCountWordBytes?.ToString() },
                { "str_is_byte_swapped",    FormatNullableBoolean(StringIsByteSwapped)  },
                { "str_is_counted",         FormatNullableBoolean(StringIsCounted) },
                { "str_is_fixed_length",    FormatNullableBoolean(StringIsFixedLength)  },
                { "str_is_zero_terminated", FormatNullableBoolean(StringIsFixedLength)  },
                { "str_max_capacity",       StringMaxCapacity?.ToString() },
                { "str_pad_bytes",          StringPadBytes?.ToString() },
                { "str_total_length",       StringTotalLength?.ToString() },
                { "max_requests_in_flight", MaxRequestsInFlight?.ToString() },
                { "allow_field_resize",     FormatNullableBoolean(AllowFieldResize) },
            };

            string separator = "&";
            return string.Join(separator, attributes.Where(attr => attr.Value != null).Select(attr => $"{attr.Key}={attr.Value}"));

        }




        void SetUpEvents()
        {

            // Used to finalize the asynchronous read/write task completion sources
            ReadCompleted += ReadTaskCompleter;
            WriteCompleted += WriteTaskCompleter;
            Created += CreatedTaskCompleter;


        }

        void RemoveEvents()
        {

            // Used to finalize the  read/write task completion sources
            ReadCompleted -= ReadTaskCompleter;
            WriteCompleted -= WriteTaskCompleter;
            Created -= CreatedTaskCompleter;
        }

        Status RemoveCallback()
        {
            return (Status)_native.plc_tag_unregister_callback(nativeTagHandle);
        }

        private readonly ConcurrentStack<TaskCompletionSource<Status>> createTasks = new ConcurrentStack<TaskCompletionSource<Status>>();
        void CreatedTaskCompleter(object sender, TagEventArgs e)
        {
            if (createTasks.TryPop(out var createTask))
            {
                switch (e.Status)
                {
                    case Status.Pending:
                        // Do nothing, wait for another ReadCompleted callback
                        break;
                    default:
                        createTask?.SetResult(e.Status);
                        break;
                }
            }
        }

        private readonly ConcurrentStack<TaskCompletionSource<Status>> readTasks = new ConcurrentStack<TaskCompletionSource<Status>>();
        void ReadTaskCompleter(object sender, TagEventArgs e)
        {
            if (readTasks.TryPop(out var readTask))
            {
                switch (e.Status)
                {
                    case Status.Pending:
                        // Do nothing, wait for another ReadCompleted callback
                        break;
                    default:
                        readTask?.SetResult(e.Status);
                        break;
                }
            }
        }

        private readonly ConcurrentStack<TaskCompletionSource<Status>> writeTasks = new ConcurrentStack<TaskCompletionSource<Status>>();
        void WriteTaskCompleter(object sender, TagEventArgs e)
        {
            if (writeTasks.TryPop(out var writeTask))
            {
                switch (e.Status)
                {
                    case Status.Pending:
                        // Do nothing, wait for another WriteCompleted callback
                        break;
                    default:
                        writeTask?.SetResult(e.Status);
                        break;

                }
            }
        }

        public event EventHandler<TagEventArgs> ReadStarted;
        public event EventHandler<TagEventArgs> ReadCompleted;
        public event EventHandler<TagEventArgs> WriteStarted;
        public event EventHandler<TagEventArgs> WriteCompleted;
        public event EventHandler<TagEventArgs> Aborted;
        public event EventHandler<TagEventArgs> Destroyed;
        public event EventHandler<TagEventArgs> Created;

        void coreLibEventCallback(int eventTagHandle, int eventCode, int statusCode, IntPtr userdata)
        {
            var @event = (Event)eventCode;
            var status = (Status)statusCode;
            var eventArgs = new TagEventArgs() { Status = status };

            switch (@event)
            {
                case Event.ReadCompleted:
                    ReadCompleted?.Invoke(this, eventArgs);
                    break;
                case Event.ReadStarted:
                    ReadStarted?.Invoke(this, eventArgs);
                    break;
                case Event.WriteStarted:
                    WriteStarted?.Invoke(this, eventArgs);
                    break;
                case Event.WriteCompleted:
                    WriteCompleted?.Invoke(this, eventArgs);
                    break;
                case Event.Aborted:
                    Aborted?.Invoke(this, eventArgs);
                    break;
                case Event.Destroyed:
                    Destroyed?.Invoke(this, eventArgs);
                    break;
                case Event.Created:
                    Created?.Invoke(this, eventArgs);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }

}
