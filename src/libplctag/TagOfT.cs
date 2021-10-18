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
            set => _tag.PlcType = value;
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

        /// <inheritdoc cref="Tag.Int16ByteOrder"/>
        public ByteOrder Int16ByteOrder
        {
            get => _tag.Int16ByteOrder;
            set => _tag.Int16ByteOrder = value;
        }

        /// <inheritdoc cref="Tag.Int32ByteOrder"/>
        public ByteOrder Int32ByteOrder
        {
            get => _tag.Int32ByteOrder;
            set => _tag.Int32ByteOrder = value;
        }

        /// <inheritdoc cref="Tag.Int64ByteOrder"/>
        public ByteOrder Int64ByteOrder
        {
            get => _tag.Int64ByteOrder;
            set => _tag.Int64ByteOrder = value;
        }

        /// <inheritdoc cref="Tag.Float32ByteOrder"/>
        public ByteOrder Float32ByteOrder
        {
            get => _tag.Float32ByteOrder;
            set => _tag.Float32ByteOrder = value;
        }

        /// <inheritdoc cref="Tag.Float64ByteOrder"/>
        public ByteOrder Float64ByteOrder
        {
            get => _tag.Float64ByteOrder;
            set => _tag.Float64ByteOrder = value;
        }

        /// <inheritdoc cref="Tag.StringCountWordBytes"/>
        public uint? StringCountWordBytes
        {
            get => _tag.StringCountWordBytes;
            set => _tag.StringCountWordBytes = value;
        }

        /// <inheritdoc cref="Tag.StringIsByteSwapped"/>
        public bool? StringIsByteSwapped
        {
            get => _tag.StringIsByteSwapped;
            set => _tag.StringIsByteSwapped = value;
        }

        /// <inheritdoc cref="Tag.StringIsCounted"/>
        public bool? StringIsCounted
        {
            get => _tag.StringIsCounted;
            set => _tag.StringIsCounted = value;
        }

        /// <inheritdoc cref="Tag.StringIsFixedLength"/>
        public bool? StringIsFixedLength
        {
            get => _tag.StringIsFixedLength;
            set => _tag.StringIsFixedLength = value;
        }

        /// <inheritdoc cref="Tag.StringIsZeroTerminated"/>
        public bool? StringIsZeroTerminated
        {
            get => _tag.StringIsZeroTerminated;
            set => _tag.StringIsZeroTerminated = value;
        }

        /// <inheritdoc cref="Tag.StringMaxCapacity"/>
        public uint? StringMaxCapacity
        {
            get => _tag.StringMaxCapacity;
            set => _tag.StringMaxCapacity = value;
        }

        /// <inheritdoc cref="Tag.StringPadBytes"/>
        public uint? StringPadBytes
        {
            get => _tag.StringPadBytes;
            set => _tag.StringPadBytes = value;
        }

        /// <inheritdoc cref="Tag.StringTotalLength"/>
        public uint? StringTotalLength
        {
            get => _tag.StringTotalLength;
            set => _tag.StringTotalLength = value;
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
        public async Task ReadAsync(CancellationToken token = default)
        {
            await _tag.ReadAsync(token);
            DecodeAll();
        }

        /// <inheritdoc cref="Tag.Read"/>
        public void Read()
        {
            _tag.Read();
            DecodeAll();
        }

        /// <inheritdoc cref="Tag.WriteAsync"/>
        public async Task WriteAsync(CancellationToken token = default)
        {
            if (!_tag.IsInitialized)
                await _tag.InitializeAsync(token);

            EncodeAll();
            await _tag.WriteAsync(token);
        }

        /// <inheritdoc cref="Tag.Write"/>
        public void Write()
        {
            if (!_tag.IsInitialized)
                _tag.Initialize();

            EncodeAll();
            _tag.Write();
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

    }
}
