using libplctag.DataTypes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{
    /// <summary>
    /// A class that allows for strongly-typed objects tied to PLC tags
    /// </summary>
    /// <typeparam name="M">A <see cref="IPlcMapper{T}"/> class that handles data conversion</typeparam>
    /// <typeparam name="T">The desired C# type of Tag.Value</typeparam>
    public class Tag<M, T> : IDisposable, ITag where M : IPlcMapper<T>, new()
    {

        private readonly Tag _tag;
        private readonly IPlcMapper<T> _plcMapper;

        public Tag()
        {
            _plcMapper = new M();
            _tag = new Tag()
            {
                ElementSize = _plcMapper.ElementSize,
            };

            _tag.ReadStarted += (s, e) => ReadStarted?.Invoke(this, e);
            _tag.ReadCompleted += (s, e) =>
            {
                // If AutoSyncReadInterval is configured, then this event was almost certainly not triggered
                // by a call to Read/ReadAsync - and therefore the data needs to be decoded.
                if(AutoSyncReadInterval != null) DecodeAll();
                ReadCompleted?.Invoke(this, e);
            };
            _tag.WriteStarted += (s, e) => WriteStarted?.Invoke(this, e);
            _tag.WriteCompleted += (s, e) => WriteCompleted?.Invoke(this, e);
            _tag.Aborted += (s, e) => Aborted?.Invoke(this, e);
            _tag.Destroyed += (s, e) => Destroyed?.Invoke(this, e);
        }

        /// <inheritdoc cref="Tag.Protocol"/>
        public Protocol? Protocol
        {
            get => _tag.Protocol;
            set => _tag.Protocol = value;
        }

        /// <inheritdoc cref="Tag.Gateway"/>
        public string Gateway
        {
            get => _tag.Gateway;
            set => _tag.Gateway = value;
        }

        /// <inheritdoc cref="Tag.Path"/>
        public string Path
        {
            get => _tag.Path;
            set => _tag.Path = value;
        }

        /// <inheritdoc cref="Tag.PlcType"/>
        public PlcType? PlcType
        {
            get => _tag.PlcType;
            set
            {
                _tag.PlcType = value;
                if(value.HasValue)
                    _plcMapper.PlcType = value.Value;
            }
        }

        /// <inheritdoc cref="Tag.Name"/>
        public string Name
        {
            get => _tag.Name;
            set => _tag.Name = value;
        }

        /// <inheritdoc cref="Tag.UseConnectedMessaging"/>
        public bool? UseConnectedMessaging
        {
            get => _tag.UseConnectedMessaging;
            set => _tag.UseConnectedMessaging = value;
        }

        /// <inheritdoc cref="Tag.ReadCacheMillisecondDuration"/>
        public int? ReadCacheMillisecondDuration
        {
            get => _tag.ReadCacheMillisecondDuration;
            set => _tag.ReadCacheMillisecondDuration = value;
        }

        /// <inheritdoc cref="Tag.Timeout"/>
        public TimeSpan Timeout
        {
            get => _tag.Timeout;
            set => _tag.Timeout = value;
        }

        /// <inheritdoc cref="Tag.AutoSyncReadInterval"/>
        public TimeSpan? AutoSyncReadInterval
        {
            get => _tag.AutoSyncReadInterval;
            set => _tag.AutoSyncReadInterval = value;
        }

        /// <inheritdoc cref="Tag.AutoSyncWriteInterval"/>
        public TimeSpan? AutoSyncWriteInterval
        {
            get => _tag.AutoSyncWriteInterval;
            set => _tag.AutoSyncWriteInterval = value;
        }

        /// <inheritdoc cref="Tag.DebugLevel"/>
        public DebugLevel DebugLevel
        {
            get => _tag.DebugLevel;
            set => _tag.DebugLevel = value;
        }

        /// <summary>
        /// Dimensions of Value if it is an array
        /// Ex. {2, 10} for a 2 column, 10 row array
        /// Non-arrays can use null (default)
        /// </summary>
        public int[] ArrayDimensions
        {
            get => _plcMapper.ArrayDimensions;
            set
            {
                _plcMapper.ArrayDimensions = value;
                _tag.ElementCount = _plcMapper.GetElementCount();
            }
        }

        /// <inheritdoc cref="Tag.Initialize"/>
        public void Initialize()
        {
            _tag.Initialize();
            DecodeAll();
        }

        /// <inheritdoc cref="Tag.InitializeAsync"/>
        public async Task InitializeAsync(CancellationToken token = default)
        {
            await _tag.InitializeAsync(token);
            DecodeAll();
        }

        /// <inheritdoc cref="Tag.ReadAsync"/>
        public async Task<T> ReadAsync(CancellationToken token = default)
        {
            await _tag.ReadAsync(token);
            DecodeAll();
            return Value;
        }

        /// <inheritdoc cref="Tag.Read"/>
        public T Read()
        {
            _tag.Read();
            DecodeAll();
            return Value;
        }

        object ITag.Read() => Read();

        async Task<object> ITag.ReadAsync(CancellationToken token) => await ReadAsync();

        /// <inheritdoc cref="Tag.WriteAsync"/>
        public async Task WriteAsync(CancellationToken token = default)
        {
            if (!_tag.IsInitialized)
                await _tag.InitializeAsync(token);

            EncodeAll();
            await _tag.WriteAsync(token);
        }

        /// <inheritdoc cref="Tag.WriteAsync"/>
        public async Task WriteAsync(T value, CancellationToken token = default)
        {
            Value = value;
            await WriteAsync(token);
        }

        /// <inheritdoc cref="Tag.Write"/>
        public void Write()
        {
            if (!_tag.IsInitialized)
                _tag.Initialize();

            EncodeAll();
            _tag.Write();
        }

        /// <inheritdoc cref="Tag.Write"/>
        public void Write(T value)
        {
            Value = value;
            Write();
        }

        void DecodeAll()
        {
            Value = _plcMapper.Decode(_tag);
        }

        void EncodeAll()
        {
            _plcMapper.Encode(_tag, Value);
        }

        /// <inheritdoc cref="Tag.GetStatus"/>
        public Status GetStatus() => _tag.GetStatus();

        public void Dispose() => _tag.Dispose();

        ~Tag()
        {
            Dispose();
        }

        /// <summary>
        /// The local memory value that can be transferred to/from the PLC
        /// </summary>
        public T Value { get; set; }
        object ITag.Value { get => Value; set => Value = (T)value; }
        public event EventHandler<TagEventArgs> ReadStarted;
        public event EventHandler<TagEventArgs> ReadCompleted;
        public event EventHandler<TagEventArgs> WriteStarted;
        public event EventHandler<TagEventArgs> WriteCompleted;
        public event EventHandler<TagEventArgs> Aborted;
        public event EventHandler<TagEventArgs> Destroyed;

    }
}
