// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDotNetCore.PlcMapper
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

        /// <inheritdoc cref="Tag.AllowPacking"/>
        public bool? AllowPacking
        {
            get => _tag.AllowPacking;
            set => _tag.AllowPacking = value;
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
            await _tag.InitializeAsync(token).ConfigureAwait(false);
            DecodeAll();
        }

        /// <inheritdoc cref="Tag.ReadAsync"/>
        public async Task<T> ReadAsync(CancellationToken token = default)
        {
            await _tag.ReadAsync(token).ConfigureAwait(false);
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

        async Task<object> ITag.ReadAsync(CancellationToken token) => await ReadAsync().ConfigureAwait(false);

        /// <inheritdoc cref="Tag.WriteAsync"/>
        public async Task WriteAsync(CancellationToken token = default)
        {
            if (!_tag.IsInitialized)
                await _tag.InitializeAsync(token).ConfigureAwait(false);

            EncodeAll();
            await _tag.WriteAsync(token).ConfigureAwait(false);
        }

        /// <inheritdoc cref="Tag.WriteAsync"/>
        public async Task WriteAsync(T value, CancellationToken token = default)
        {
            Value = value;
            await WriteAsync(token).ConfigureAwait(false);
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

    /// <summary>
    /// An interface to represent any generic tag without
    /// exposing its value
    /// </summary>
    public interface ITag : IDisposable
    {
        int[] ArrayDimensions { get; set; }
        string Gateway { get; set; }
        string Name { get; set; }
        string Path { get; set; }
        PlcType? PlcType { get; set; }
        Protocol? Protocol { get; set; }
        int? ReadCacheMillisecondDuration { get; set; }
        TimeSpan Timeout { get; set; }
        bool? UseConnectedMessaging { get; set; }
        bool? AllowPacking { get; set; }
        TimeSpan? AutoSyncReadInterval { get; set; }
        TimeSpan? AutoSyncWriteInterval { get; set; }
        DebugLevel DebugLevel { get; set; }

        event EventHandler<TagEventArgs> ReadStarted;
        event EventHandler<TagEventArgs> ReadCompleted;
        event EventHandler<TagEventArgs> WriteStarted;
        event EventHandler<TagEventArgs> WriteCompleted;
        event EventHandler<TagEventArgs> Aborted;
        event EventHandler<TagEventArgs> Destroyed;

        Status GetStatus();
        void Initialize();
        Task InitializeAsync(CancellationToken token = default);
        object Read();
        Task<object> ReadAsync(CancellationToken token = default);
        void Write();
        Task WriteAsync(CancellationToken token = default);

        object Value { get; set; }
    }

    public class TagBool : Tag<BoolPlcMapper, bool> { }
    public class TagBool1D : Tag<BoolPlcMapper, bool[]> { }
    public class TagBool2D : Tag<BoolPlcMapper, bool[,]> { }
    public class TagBool3D : Tag<BoolPlcMapper, bool[,,]> { }


    public class TagDint : Tag<DintPlcMapper, int> { }
    public class TagDint1D : Tag<DintPlcMapper, int[]> { }
    public class TagDint2D : Tag<DintPlcMapper, int[,]> { }
    public class TagDint3D : Tag<DintPlcMapper, int[,,]> { }


    public class TagInt : Tag<IntPlcMapper, short> { }
    public class TagInt1D : Tag<IntPlcMapper, short[]> { }
    public class TagInt2D : Tag<IntPlcMapper, short[,]> { }
    public class TagInt3D : Tag<IntPlcMapper, short[,,]> { }


    public class TagLint : Tag<LintPlcMapper, long> { }
    public class TagLint1D : Tag<LintPlcMapper, long[]> { }
    public class TagLint2D : Tag<LintPlcMapper, long[,]> { }
    public class TagLint3D : Tag<LintPlcMapper, long[,,]> { }


    public class TagLreal : Tag<LrealPlcMapper, double> { }
    public class TagLreal1D : Tag<LrealPlcMapper, double[]> { }
    public class TagLreal2D : Tag<LrealPlcMapper, double[,]> { }
    public class TagLreal3D : Tag<LrealPlcMapper, double[,,]> { }


    public class TagReal : Tag<RealPlcMapper, float> { }
    public class TagReal1D : Tag<RealPlcMapper, float[]> { }
    public class TagReal2D : Tag<RealPlcMapper, float[,]> { }
    public class TagReal3D : Tag<RealPlcMapper, float[,,]> { }


    public class TagSint : Tag<SintPlcMapper, sbyte> { }
    public class TagSint1D : Tag<SintPlcMapper, sbyte[]> { }
    public class TagSint2D : Tag<SintPlcMapper, sbyte[,]> { }
    public class TagSint3D : Tag<SintPlcMapper, sbyte[,,]> { }


    public class TagString : Tag<StringPlcMapper, string> { }
    public class TagString1D : Tag<StringPlcMapper, string[]> { }
    public class TagString2D : Tag<StringPlcMapper, string[,]> { }
    public class TagString3D : Tag<StringPlcMapper, string[,,]> { }


    public class TagTagInfo : Tag<TagInfoPlcMapper, TagInfo[]> { }

    public class TagTimer : Tag<TimerPlcMapper, AbTimer> { }

    public class TagUdtInfo : Tag<UdtInfoPlcMapper, UdtInfo> { }
}
