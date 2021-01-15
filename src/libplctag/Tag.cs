using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using libplctag.NativeImport;

namespace libplctag
{

    public sealed class Tag : IDisposable
    {

        private const int ASYNC_TIMEOUT = 0;
        private const int ASYNC_STATUS_POLL_INTERVAL = 2;
        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan maxTimeout = TimeSpan.FromMilliseconds(int.MaxValue);


        private int nativeTagHandle;

        private bool _isDisposed = false;



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
                    throw new ArgumentOutOfRangeException(nameof(value));
                _timeout = value;
            }
        }

        

        ~Tag()
        {
            Dispose();
        }

        

        public void Dispose()
        {
            if (_isDisposed)
                return;

            if (!IsInitialized)
                return;

            var result = (Status)plctag.plc_tag_destroy(nativeTagHandle);
            ThrowIfStatusNotOk(result);

            _isDisposed = true;
        }

        public void Abort()
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)plctag.plc_tag_abort(nativeTagHandle);
            ThrowIfStatusNotOk(result);
        }


        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </summary>
        public void Initialize()
        {
            
            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            if (millisecondTimeout <= 0)
                throw new ArgumentOutOfRangeException(nameof(millisecondTimeout), "Must be greater than 0 for a synchronous initialization");

            var attributeString = GetAttributeString();

            var result = plctag.plc_tag_create(attributeString, millisecondTimeout);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                nativeTagHandle = result;

            IsInitialized = true;
        }

        /// <summary>
        /// Initializes the tag by establishing necessary connections.
        /// Can only be called once per instance.
        /// Timeout is controlled via class property.
        /// </summary>
        public async Task InitializeAsync(CancellationToken token = default)
        {
            
            ThrowIfAlreadyDisposed();
            ThrowIfAlreadyInitialized();

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                var attributeString = GetAttributeString();

                var result = plctag.plc_tag_create(attributeString, ASYNC_TIMEOUT);
                if (result < 0)
                    throw new LibPlcTagException((Status)result);
                else
                    nativeTagHandle = result;

                await DelayWhilePending((Status)result, cts.Token);

                ThrowIfStatusNotOk();

                IsInitialized = true;
            }
        }

        /// <summary>
        /// Executes a synchronous read on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public void Read()
        {
            ThrowIfAlreadyDisposed();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            var result = (Status)plctag.plc_tag_read(nativeTagHandle, millisecondTimeout);
            ThrowIfStatusNotOk(result);
        }

        /// <summary>
        /// Executes an asynch read on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public async Task ReadAsync(CancellationToken token = default)
        {
            ThrowIfAlreadyDisposed();

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                var initialStatus = (Status)plctag.plc_tag_read(nativeTagHandle, ASYNC_TIMEOUT);

                var statusAfterPending = await DelayWhilePending(initialStatus, cts.Token);

                ThrowIfStatusNotOk(statusAfterPending);
            }
        }

        /// <summary>
        /// Executes a synchronous write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public void Write()
        {
            ThrowIfAlreadyDisposed();

            var millisecondTimeout = (int)Timeout.TotalMilliseconds;

            var result = (Status)plctag.plc_tag_write(nativeTagHandle, millisecondTimeout);
            ThrowIfStatusNotOk(result);
        }

        /// <summary>
        /// Executes an asynch write on a tag.
        /// Timeout is controlled via class property.
        /// </summary>
        public async Task WriteAsync(CancellationToken token = default)
        {
            ThrowIfAlreadyDisposed();

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(token))
            {
                cts.CancelAfter(Timeout);

                var initialStatus = (Status)plctag.plc_tag_write(nativeTagHandle, ASYNC_TIMEOUT);

                var statusAfterPending = await DelayWhilePending(initialStatus, cts.Token);

                ThrowIfStatusNotOk(statusAfterPending);
            }
        }

        public int GetSize()
        {
            ThrowIfAlreadyDisposed();

            var result = plctag.plc_tag_get_size(nativeTagHandle);
            if (result < 0)
                throw new LibPlcTagException((Status)result);
            else
                return result;
        }

        public Status GetStatus()
        {
            ThrowIfAlreadyDisposed();
            return (Status)plctag.plc_tag_status(nativeTagHandle);
        }

        public byte[] GetBuffer()
        {
            ThrowIfAlreadyDisposed();

            var tagSize = GetSize();
            var temp = new byte[tagSize];

            for (int ii = 0; ii < tagSize; ii++)
                temp[ii] = GetUInt8(ii);

            return temp;
        }


        private int GetIntAttribute(string attributeName)
        {
            ThrowIfAlreadyDisposed();

            var result = plctag.plc_tag_get_int_attribute(nativeTagHandle, attributeName, int.MinValue);
            if (result == int.MinValue)
                ThrowIfStatusNotOk();

            return result;
        }

        private void SetIntAttribute(string attributeName, int value)
        {
            ThrowIfAlreadyDisposed();

            var result = (Status)plctag.plc_tag_set_int_attribute(nativeTagHandle, attributeName, value);
            ThrowIfStatusNotOk(result);
        }

        public bool GetBit(int offset)
        {
            ThrowIfAlreadyDisposed();

            var result = plctag.plc_tag_get_bit(nativeTagHandle, offset);
            if (result == 0)
                return false;
            else if (result == 1)
                return true;
            else
                throw new LibPlcTagException((Status)result);
        }

        public void SetBit(int offset, bool value)          => SetTagValue(plctag.plc_tag_set_bit, offset, value == true ? 1 : 0);

        public ulong GetUInt64(int offset)                  => GetTagValue(plctag.plc_tag_get_uint64, offset, ulong.MaxValue);
        public void SetUInt64(int offset, ulong value)      => SetTagValue(plctag.plc_tag_set_uint64, offset, value);

        public long GetInt64(int offset)                    => GetTagValue(plctag.plc_tag_get_int64, offset, long.MinValue);
        public void SetInt64(int offset, long value)        => SetTagValue(plctag.plc_tag_set_int64, offset, value);

        public uint GetUInt32(int offset)                   => GetTagValue(plctag.plc_tag_get_uint32, offset, uint.MaxValue);
        public void SetUInt32(int offset, uint value)       => SetTagValue(plctag.plc_tag_set_uint32, offset, value);

        public int GetInt32(int offset)                     => GetTagValue(plctag.plc_tag_get_int32, offset, int.MinValue);
        public void SetInt32(int offset, int value)         => SetTagValue(plctag.plc_tag_set_int32, offset, value);

        public ushort GetUInt16(int offset)                 => GetTagValue(plctag.plc_tag_get_uint16, offset, ushort.MaxValue);
        public void SetUInt16(int offset, ushort value)     => SetTagValue(plctag.plc_tag_set_uint16, offset, value);

        public short GetInt16(int offset)                   => GetTagValue(plctag.plc_tag_get_int16, offset, short.MinValue);
        public void SetInt16(int offset, short value)       => SetTagValue(plctag.plc_tag_set_int16, offset, value);

        public byte GetUInt8(int offset)                    => GetTagValue(plctag.plc_tag_get_uint8, offset, byte.MaxValue);
        public void SetUInt8(int offset, byte value)        => SetTagValue(plctag.plc_tag_set_uint8, offset, value);

        public sbyte GetInt8(int offset)                    => GetTagValue(plctag.plc_tag_get_int8, offset, sbyte.MinValue);
        public void SetInt8(int offset, sbyte value)        => SetTagValue(plctag.plc_tag_set_int8, offset, value);

        public double GetFloat64(int offset)                => GetTagValue(plctag.plc_tag_get_float64, offset, double.MinValue);
        public void SetFloat64(int offset, double value)    => SetTagValue(plctag.plc_tag_set_float64, offset, value);

        public float GetFloat32(int offset)                 => GetTagValue(plctag.plc_tag_get_float32, offset, float.MinValue);
        public void SetFloat32(int offset, float value)     => SetTagValue(plctag.plc_tag_set_float32, offset, value);





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



        private void SetTagValue<T>(Func<int, int, T, int> nativeMethod, int offset, T value)
        {
            ThrowIfAlreadyDisposed();
            var result = (Status)nativeMethod(nativeTagHandle, offset, value);
            ThrowIfStatusNotOk(result);
        }

        private T GetTagValue<T>(Func<int, int, T> nativeMethod, int offset, T valueIndicatingPossibleError)
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
                    try
                    {
                        await Task.Delay(ASYNC_STATUS_POLL_INTERVAL, token);
                    }
                    catch (TaskCanceledException)
                    {
                        if (token.IsCancellationRequested)
                            throw;
                        else
                            throw new LibPlcTagException(Status.ErrorTimeout);
                    }
                    status = GetStatus();
                }
            }

            return status;
        }

        private string GetAttributeString()
        {

            var attributes = new Dictionary<string, string>();

            attributes.Add("protocol", Protocol.ToString());
            attributes.Add("gateway", Gateway);
            attributes.Add("path", Path);
            attributes.Add("plc", PlcType.ToString().ToLower());
            attributes.Add("elem_size", ElementSize?.ToString());
            attributes.Add("elem_count", ElementCount?.ToString());
            attributes.Add("name", Name);
            attributes.Add("read_cache_ms", ReadCacheMillisecondDuration?.ToString());
            if (UseConnectedMessaging.HasValue)
                attributes.Add("use_connected_msg", UseConnectedMessaging.Value ? "1" : "0");

            string separator = "&";
            return string.Join(separator, attributes.Where(attr => attr.Value != null).Select(attr => $"{attr.Key}={attr.Value}"));

        }

    }

}