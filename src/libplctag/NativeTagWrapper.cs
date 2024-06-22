// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("libplctag.Tests")]

namespace libplctag
{

    class NativeTagWrapper : IDisposable
    {
        private const int TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION = 0;
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan maxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);

        private int nativeTagHandle;
        private libplctag.NativeImport.plctag.callback_func_ex coreLibCallbackFuncExDelegate;

        private bool _isDisposed = false;
        private bool _isInitialized = false;

        readonly INativeTag _native;

        public NativeTagWrapper(INativeTag nativeMethods)
        {
            _native = nativeMethods;
        }

        ~NativeTagWrapper()
        {
            Dispose();
        }


        // TODO remove. No longer used by Tag but would be a breaking change.
        public bool IsInitialized => _isInitialized;


        private string _name;
        public string Name
        {
            get => GetField(ref _name);
            set => SetField(ref _name, value);
        }

        private Protocol? _protocol;
        public Protocol? Protocol
        {
            get => GetField(ref _protocol);
            set => SetField(ref _protocol, value);
        }

        private string _gateway;
        public string Gateway
        {
            get => GetField(ref _gateway);
            set => SetField(ref _gateway, value);
        }

        private PlcType? _plcType;
        public PlcType? PlcType
        {
            get => GetField(ref _plcType);
            set => SetField(ref _plcType, value);
        }

        private string _path;
        public string Path
        {
            get => GetField(ref _path);
            set => SetField(ref _path, value);
        }

        private int? _elementSize;
        public int? ElementSize
        {
            get => GetField(ref _elementSize);
            set => SetField(ref _elementSize, value);
        }

        private int? _elementCount;
        public int? ElementCount
        {
            get => GetField(ref _elementCount);
            set => SetField(ref _elementCount, value);
        }

        private bool? _useConnectedMessaging;
        public bool? UseConnectedMessaging
        {
            get => GetField(ref _useConnectedMessaging);
            set => SetField(ref _useConnectedMessaging, value);
        }

        private bool? _allowPacking;
        public bool? AllowPacking
        {
            get => GetField(ref _allowPacking);
            set => SetField(ref _allowPacking, value);
        }

        private int? _readCacheMillisecondDuration;
        public int? ReadCacheMillisecondDuration
        {
            get
            {
                ThrowIfAlreadyDisposed();

                if (!_isInitialized)
                    return _readCacheMillisecondDuration;

                return GetIntAttribute("read_cache_ms");
            }
            set
            {
                ThrowIfAlreadyDisposed();

                if (!_isInitialized)
                {
                    _readCacheMillisecondDuration = value;
                    return;
                }

                SetIntAttribute("read_cache_ms", value.Value);
            }
        }

        private TimeSpan _timeout = defaultTimeout;
        public TimeSpan Timeout
        {
            get
            {
                ThrowIfAlreadyDisposed();
                return _timeout;
            }
            set
            {
                ThrowIfAlreadyDisposed();
                if (value <= TimeSpan.Zero || value > maxTimeout)
                    throw new ArgumentOutOfRangeException(nameof(Timeout), value, "Must be greater than 0");
                _timeout = value;
            }
        }

        private TimeSpan? _autoSyncReadInterval;
        public TimeSpan? AutoSyncReadInterval
        {
            get => GetField(ref _autoSyncReadInterval);
            set => SetField(ref _autoSyncReadInterval, value);
        }

        private TimeSpan? _autoSyncWriteInterval;
        public TimeSpan? AutoSyncWriteInterval
        {
            get => GetField(ref _autoSyncWriteInterval);
            set => SetField(ref _autoSyncWriteInterval, value);
        }

        private DebugLevel _debugLevel = DebugLevel.None;
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

                _debugLevel = value;

                if (_isInitialized)
                    SetDebugLevel(value);
            }
        }

        private string _int16ByteOrder;
        public string Int16ByteOrder
        {
            get => GetField(ref _int16ByteOrder);
            set => SetField(ref _int16ByteOrder, value);
        }

        private string _int32ByteOrder;
        public string Int32ByteOrder
        {
            get => GetField(ref _int32ByteOrder);
            set => SetField(ref _int32ByteOrder, value);
        }

        private string _int64ByteOrder;
        public string Int64ByteOrder
        {
            get => GetField(ref _int64ByteOrder);
            set => SetField(ref _int64ByteOrder, value);
        }

        private string _float32ByteOrder;
        public string Float32ByteOrder
        {
            get => GetField(ref _float32ByteOrder);
            set => SetField(ref _float32ByteOrder, value);
        }

        private string _float64ByteOrder;
        public string Float64ByteOrder
        {
            get => GetField(ref _float64ByteOrder);
            set => SetField(ref _float64ByteOrder, value);
        }


        private uint? _stringCountWordBytes;
        public uint? StringCountWordBytes
        {
            get => GetField(ref _stringCountWordBytes);
            set => SetField(ref _stringCountWordBytes, value);
        }

        private bool? _stringIsByteSwapped;
        public bool? StringIsByteSwapped
        {
            get => GetField(ref _stringIsByteSwapped);
            set => SetField(ref _stringIsByteSwapped, value);
        }

        private bool? _stringIsCounted;
        public bool? StringIsCounted
        {
            get => GetField(ref _stringIsCounted);
            set => SetField(ref _stringIsCounted, value);
        }

        private bool? _stringIsFixedLength;
        public bool? StringIsFixedLength
        {
            get => GetField(ref _stringIsFixedLength);
            set => SetField(ref _stringIsFixedLength, value);
        }

        private bool? _stringIsZeroTerminated;
        public bool? StringIsZeroTerminated
        {
            get => GetField(ref _stringIsZeroTerminated);
            set => SetField(ref _stringIsZeroTerminated, value);
        }

        private uint? _stringMaxCapacity;
        public uint? StringMaxCapacity
        {
            get => GetField(ref _stringMaxCapacity);
            set => SetField(ref _stringMaxCapacity, value);
        }

        private uint? _stringPadBytes;
        public uint? StringPadBytes
        {
            get => GetField(ref _stringPadBytes);
            set => SetField(ref _stringPadBytes, value);
        }

        private uint? _stringTotalLength;
        public uint? StringTotalLength
        {
            get => GetField(ref _stringTotalLength);
            set => SetField(ref _stringTotalLength, value);
        }


        private uint? _maxRequetsInFlight;
        public uint? MaxRequestsInFlight
        {
            get => GetField (ref _maxRequetsInFlight);
            set => SetField(ref _maxRequetsInFlight, value);
        }



        public void Dispose()
        {
            if (_isDisposed)
                return;

            if (_isInitialized)
            {
                RemoveEventsAndRemoveCallback();
                var result = (Status)_native.plc_tag_destroy(nativeTagHandle);
                ThrowIfStatusNotOk(result);
            }

            _isDisposed = true;
        }

        public void Abort()
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_abort(nativeTagHandle);
            ThrowIfStatusNotOk(result);
        }



        public void Initialize()
        {

            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            SetUpEvents();

            var attributeString = GetAttributeString();
            
            var result = _native.plc_tag_create_ex(attributeString, coreLibCallbackFuncExDelegate, IntPtr.Zero, millisecondTimeout);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                nativeTagHandle = result;


            _isInitialized = true;
        }

        public async Task InitializeAsync(CancellationToken token = default)
        {

            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                using (cts.Token.Register(() =>
                {
                    if (createTasks.TryPop(out var createTask))
                    {
                        Abort();
                        RemoveEventsAndRemoveCallback();

                        if (token.IsCancellationRequested)
                            createTask.SetCanceled();
                        else
                            createTask.SetException(new LibPlcTagException(Status.ErrorTimeout));
                    }
                }))
                {
                    SetUpEvents();

                    var createTask = new TaskCompletionSource<Status>(TaskCreationOptions.RunContinuationsAsynchronously);
                    createTasks.Push(createTask);
                    
                    var attributeString = GetAttributeString();

                    var result = _native.plc_tag_create_ex(attributeString, coreLibCallbackFuncExDelegate, IntPtr.Zero, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);
                    if (result < 0)
                        throw new LibPlcTagException((Status)result);
                    else
                        nativeTagHandle = result;

                    if(GetStatus() == Status.Pending)
                        await createTask.Task.ConfigureAwait(false);

                    ThrowIfStatusNotOk(createTask.Task.Result);

                    _isInitialized = true;
                }
            }
        }

        public void Read()
        {
            ThrowIfAlreadyDisposed();
            InitializeIfRequired();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            var result = (Status)_native.plc_tag_read(nativeTagHandle, millisecondTimeout);
            ThrowIfStatusNotOk(result);
        }

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

        public void Write()
        {
            ThrowIfAlreadyDisposed();
            InitializeIfRequired();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            var result = (Status)_native.plc_tag_write(nativeTagHandle, millisecondTimeout);
            ThrowIfStatusNotOk(result);
        }

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

        public Status GetStatus()
        {
            ThrowIfAlreadyDisposed();
            return (Status)_native.plc_tag_status(nativeTagHandle);
        }

        public void GetBuffer(byte[] buffer)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)_native.plc_tag_get_raw_bytes(nativeTagHandle, 0, buffer, buffer.Length);
            ThrowIfStatusNotOk(result);
        }

        public byte[] GetBuffer()
        {
            ThrowIfAlreadyDisposed();

            var tagSize = GetSize();
            var temp = new byte[tagSize];

            var result = (Status)_native.plc_tag_get_raw_bytes(nativeTagHandle, 0, temp, temp.Length);
            ThrowIfStatusNotOk(result);

            return temp;
        }

        public void SetBuffer(byte[] buffer)
        {
            ThrowIfAlreadyDisposed();

            GetNativeValueAndThrowOnNegativeResult(_native.plc_tag_set_size, buffer.Length);
            var result = (Status)_native.plc_tag_set_raw_bytes(nativeTagHandle, 0, buffer, buffer.Length);
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

        public byte[] GetByteArrayAttribute(string attributeName)
        {
            ThrowIfAlreadyDisposed();

            var bufferLengthAttributeName = attributeName + ".length";
            var bufferLength = GetIntAttribute(bufferLengthAttributeName);
            var buffer = new byte[bufferLength];

            var result = (Status)_native.plc_tag_get_byte_array_attribute(nativeTagHandle, attributeName, buffer, buffer.Length);
            ThrowIfStatusNotOk(result);

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
                { "auto_sync_read_ms",      AutoSyncReadInterval?.TotalMilliseconds.ToString() },
                { "auto_sync_write_ms",     AutoSyncWriteInterval?.TotalMilliseconds.ToString() },
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

            // Need to keep a reference to the delegate in memory so it doesn't get garbage collected
            coreLibCallbackFuncExDelegate = new libplctag.NativeImport.plctag.callback_func_ex(coreLibEventCallback);

        }

        void RemoveEventsAndRemoveCallback()
        {

            // Used to finalize the  read/write task completion sources
            ReadCompleted -= ReadTaskCompleter;
            WriteCompleted -= WriteTaskCompleter;
            Created -= CreatedTaskCompleter;

            var callbackRemovalResult = (Status)_native.plc_tag_unregister_callback(nativeTagHandle);
            ThrowIfStatusNotOk(callbackRemovalResult);

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
