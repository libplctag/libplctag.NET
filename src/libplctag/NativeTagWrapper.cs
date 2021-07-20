using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("libplctag.Tests")]

namespace libplctag
{

    class NativeTagWrapper : IDisposable
    {


        private const int TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION = 0;
        private const int ASYNC_STATUS_POLL_INTERVAL = 2;
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan maxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);

        private int nativeTagHandle;
        private libplctag.NativeImport.plctag.callback_func coreLibCallbackFuncDelegate;

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





        public void Dispose()
        {
            if (_isDisposed)
                return;

            if (_isInitialized)
            {
                RemoveEvents();
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

            var attributeString = GetAttributeString();

            var result = _native.plc_tag_create(attributeString, millisecondTimeout);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                nativeTagHandle = result;

            SetUpEvents();

            _isInitialized = true;
        }

        public async Task InitializeAsync(CancellationToken token = default)
        {
            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                var attributeString = GetAttributeString();

                var result = _native.plc_tag_create(attributeString, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);
                if (result < 0)
                    throw new LibPlcTagException((Status)result);
                else
                    nativeTagHandle = result;

                SetUpEvents();

                Status? statusAfterPending = null;
                try
                {
                    statusAfterPending = await DelayWhilePending(GetStatus(), cts.Token);
                }
                catch (TaskCanceledException)
                {
                    if (token.IsCancellationRequested)
                        throw;
                    else
                        throw new LibPlcTagException(Status.ErrorTimeout);
                }

                ThrowIfStatusNotOk(statusAfterPending);

                _isInitialized = true;
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

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                await InitializeAsyncIfRequired(cts.Token);

                using (cts.Token.Register(() =>
                {
                    Abort();

                    if (readTasks.TryPop(out var readTask))
                    {
                        if (token.IsCancellationRequested)
                            readTask.SetCanceled();
                        else
                            readTask.SetException(new LibPlcTagException(Status.ErrorTimeout));
                    }
                }))
                {
                    var readTask = new TaskCompletionSource<object>();
                    readTasks.Push(readTask);
                    _native.plc_tag_read(nativeTagHandle, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);
                    await readTask.Task;
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

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                await InitializeAsyncIfRequired(cts.Token);

                using (cts.Token.Register(() =>
                {
                    Abort();

                    if (writeTasks.TryPop(out var writeTask))
                    {
                        if (token.IsCancellationRequested)
                            writeTask.SetCanceled();
                        else
                            writeTask.SetException(new LibPlcTagException(Status.ErrorTimeout));
                    }
                }))
                {
                    var writeTask = new TaskCompletionSource<object>();
                    writeTasks.Push(writeTask);
                    _native.plc_tag_write(nativeTagHandle, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);
                    await writeTask.Task;
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

        public Status GetStatus()
        {
            ThrowIfAlreadyDisposed();
            return (Status)_native.plc_tag_status(nativeTagHandle);
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


        private int GetIntAttribute(string attributeName)
        {
            ThrowIfAlreadyDisposed();

            var result = _native.plc_tag_get_int_attribute(nativeTagHandle, attributeName, int.MinValue);
            if (result == int.MinValue)
                ThrowIfStatusNotOk();

            return result;
        }

        private void SetIntAttribute(string attributeName, int value)
        {
            ThrowIfAlreadyDisposed();

            var result = (Status)_native.plc_tag_set_int_attribute(nativeTagHandle, attributeName, value);
            ThrowIfStatusNotOk(result);
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

        public ulong GetUInt64(int offset)                  => GetNativeTagValue(_native.plc_tag_get_uint64, offset, ulong.MaxValue);
        public void SetUInt64(int offset, ulong value)      => SetNativeTagValue(_native.plc_tag_set_uint64, offset, value);

        public long GetInt64(int offset)                    => GetNativeTagValue(_native.plc_tag_get_int64, offset, long.MinValue);
        public void SetInt64(int offset, long value)        => SetNativeTagValue(_native.plc_tag_set_int64, offset, value);

        public uint GetUInt32(int offset)                   => GetNativeTagValue(_native.plc_tag_get_uint32, offset, uint.MaxValue);
        public void SetUInt32(int offset, uint value)       => SetNativeTagValue(_native.plc_tag_set_uint32, offset, value);

        public int GetInt32(int offset)                     => GetNativeTagValue(_native.plc_tag_get_int32, offset, int.MinValue);
        public void SetInt32(int offset, int value)         => SetNativeTagValue(_native.plc_tag_set_int32, offset, value);

        public ushort GetUInt16(int offset)                 => GetNativeTagValue(_native.plc_tag_get_uint16, offset, ushort.MaxValue);
        public void SetUInt16(int offset, ushort value)     => SetNativeTagValue(_native.plc_tag_set_uint16, offset, value);

        public short GetInt16(int offset)                   => GetNativeTagValue(_native.plc_tag_get_int16, offset, short.MinValue);
        public void SetInt16(int offset, short value)       => SetNativeTagValue(_native.plc_tag_set_int16, offset, value);

        public byte GetUInt8(int offset)                    => GetNativeTagValue(_native.plc_tag_get_uint8, offset, byte.MaxValue);
        public void SetUInt8(int offset, byte value)        => SetNativeTagValue(_native.plc_tag_set_uint8, offset, value);

        public sbyte GetInt8(int offset)                    => GetNativeTagValue(_native.plc_tag_get_int8, offset, sbyte.MinValue);
        public void SetInt8(int offset, sbyte value)        => SetNativeTagValue(_native.plc_tag_set_int8, offset, value);

        public double GetFloat64(int offset)                => GetNativeTagValue(_native.plc_tag_get_float64, offset, double.MinValue);
        public void SetFloat64(int offset, double value)    => SetNativeTagValue(_native.plc_tag_set_float64, offset, value);

        public float GetFloat32(int offset)                 => GetNativeTagValue(_native.plc_tag_get_float32, offset, float.MinValue);
        public void SetFloat32(int offset, float value)     => SetNativeTagValue(_native.plc_tag_set_float32, offset, value);


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

        private T GetNativeTagValue<T>(Func<int, int, T> nativeMethod, int offset, T valueIndicatingPossibleError)
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

        private async Task<Status> DelayWhilePending(Status initialStatus, CancellationToken token)
        {

            if (initialStatus != Status.Pending)
                return initialStatus;

            var status = initialStatus;

            using (token.Register(() => Abort()))
            {
                while (status == Status.Pending)
                {
                    await Task.Delay(ASYNC_STATUS_POLL_INTERVAL, token);
                    status = GetStatus();
                }
            }

            return status;
        }

        private string GetAttributeString()
        {

            var attributes = new Dictionary<string, string>
            {
                { "protocol",           Protocol?.ToString() },
                { "gateway",            Gateway },
                { "path",               Path },
                { "plc",                PlcType == libplctag.PlcType.Omron ? "omron-njnx" : PlcType?.ToString()?.ToLower() },
                { "elem_size",          ElementSize?.ToString() },
                { "elem_count",         ElementCount?.ToString() },
                { "name",               Name },
                { "read_cache_ms",      ReadCacheMillisecondDuration?.ToString() },
                { "use_connected_msg",  UseConnectedMessaging.HasValue ? (UseConnectedMessaging.Value ? "1" : "0") : null}
            };

            string separator = "&";
            return string.Join(separator, attributes.Where(attr => attr.Value != null).Select(attr => $"{attr.Key}={attr.Value}"));

        }




        void SetUpEvents()
        {

            // Used to finalize the asynchronous read/write task completion sources
            ReadCompleted += ReadTaskCompleter;
            WriteCompleted += WriteTaskCompleter;

            // Need to keep a reference to the delegate in memory so it doesn't get garbage collected
            coreLibCallbackFuncDelegate = new libplctag.NativeImport.plctag.callback_func(coreLibEventCallback);

            var callbackRegistrationResult = (Status)_native.plc_tag_register_callback(nativeTagHandle, coreLibCallbackFuncDelegate);
            ThrowIfStatusNotOk(callbackRegistrationResult);

        }

        void RemoveEvents()
        {

            // Used to finalize the  read/write task completion sources
            ReadCompleted -= ReadTaskCompleter;
            WriteCompleted -= WriteTaskCompleter;

            var callbackRemovalResult = (Status)_native.plc_tag_unregister_callback(nativeTagHandle);
            ThrowIfStatusNotOk(callbackRemovalResult);

        }

        private ConcurrentStack<TaskCompletionSource<object>> readTasks = new ConcurrentStack<TaskCompletionSource<object>>();
        void ReadTaskCompleter(object sender, LibPlcTagEventArgs e)
        {
            if (readTasks.TryPop(out var readTask))
            {
                switch (e.Status)
                {
                    case Status.Ok:
                        readTask?.SetResult(null);
                        break;
                    case Status.Pending:
                        // Do nothing, wait for another ReadCompleted callback when Status is Ok.
                        break;
                    default:
                        readTask?.SetException(new LibPlcTagException(e.Status));
                        break;
                }
            }
        }

        private ConcurrentStack<TaskCompletionSource<object>> writeTasks = new ConcurrentStack<TaskCompletionSource<object>>();
        void WriteTaskCompleter(object sender, LibPlcTagEventArgs e)
        {
            if (writeTasks.TryPop(out var writeTask))
            {
                switch (e.Status)
                {
                    case Status.Ok:
                        writeTask?.SetResult(null);
                        break;
                    case Status.Pending:
                        // Do nothing, wait for another WriteCompleted callback when Status is Ok.
                        break;
                    default:
                        writeTask?.SetException(new LibPlcTagException(e.Status));
                        break;

                }
            }
        }

        event EventHandler<LibPlcTagEventArgs> ReadStarted;
        event EventHandler<LibPlcTagEventArgs> ReadCompleted;
        event EventHandler<LibPlcTagEventArgs> WriteStarted;
        event EventHandler<LibPlcTagEventArgs> WriteCompleted;
        event EventHandler<LibPlcTagEventArgs> Aborted;
        event EventHandler<LibPlcTagEventArgs> Destroyed;

        void coreLibEventCallback(int eventTagHandle, int eventCode, int statusCode)
        {

            var @event = (Event)eventCode;
            var status = (Status)statusCode;
            var eventArgs = new LibPlcTagEventArgs() { Status = status };

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
                default:
                    throw new NotImplementedException();
            }
        }

    }

}