using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private const int ASYNC_STATUS_POLL_INTERVAL = 2;
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan maxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);

        private int nativeTagHandle;

        private bool _isDisposed = false;

        readonly INativeTag _native;

        public NativeTagWrapper(INativeTag nativeMethods)
        {
            _native = nativeMethods;
        }

        ~NativeTagWrapper()
        {
            Dispose();
        }




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

                if (!IsInitialized)
                    return _readCacheMillisecondDuration;

                return GetIntAttribute("read_cache_ms");
            }
            set
            {
                ThrowIfAlreadyDisposed();

                if (!IsInitialized)
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

            if (IsInitialized)
            {
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


        public bool IsInitialized { get; private set; }

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

            IsInitialized = true;
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

                IsInitialized = true;
            }
        }

        public void Read()
        {
            ThrowIfAlreadyDisposed();

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

                var initialStatus = (Status)_native.plc_tag_read(nativeTagHandle, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);


                Status? statusAfterPending = null;
                try
                {
                    statusAfterPending = await DelayWhilePending(initialStatus, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    if (token.IsCancellationRequested)
                        throw;
                    else
                        throw new LibPlcTagException(Status.ErrorTimeout);
                }

                ThrowIfStatusNotOk(statusAfterPending);

            }
        }

        public void Write()
        {
            ThrowIfAlreadyDisposed();

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

                var initialStatus = (Status)_native.plc_tag_write(nativeTagHandle, TIMEOUT_VALUE_THAT_INDICATES_ASYNC_OPERATION);


                Status? statusAfterPending = null;
                try
                {
                    statusAfterPending = await DelayWhilePending(initialStatus, cts.Token);
                }
                catch (TaskCanceledException)
                {
                    if (token.IsCancellationRequested)
                        throw;
                    else
                        throw new LibPlcTagException(Status.ErrorTimeout);
                }
                
                ThrowIfStatusNotOk(statusAfterPending);

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


        public string GetString(int offset)
        {
            ThrowIfAlreadyDisposed();

            var stringMaxLength = _native.plc_tag_get_string_capacity(nativeTagHandle, offset);
            var stringBuffer = new StringBuilder(stringMaxLength);
            var status = (Status)_native.plc_tag_get_string(nativeTagHandle, offset, stringBuffer, stringBuffer.Capacity);

            ThrowIfStatusNotOk(status);

            return stringBuffer.ToString();
        }

        public void SetString(int offset, string value) => SetNativeTagValue(_native.plc_tag_set_string, offset, value);





        private void ThrowIfAlreadyDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().FullName);
        }

        private void ThrowIfAlreadyInitialized()
        {
            if (IsInitialized)
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
                { "plc",                PlcType?.ToString()?.ToLower() },
                { "elem_size",          ElementSize?.ToString() },
                { "elem_count",         ElementCount?.ToString() },
                { "name",               Name },
                { "read_cache_ms",      ReadCacheMillisecondDuration?.ToString() },
                { "use_connected_msg",  UseConnectedMessaging.HasValue ? (UseConnectedMessaging.Value ? "1" : "0") : null}
            };

            string separator = "&";
            return string.Join(separator, attributes.Where(attr => attr.Value != null).Select(attr => $"{attr.Key}={attr.Value}"));

        }

    }

}