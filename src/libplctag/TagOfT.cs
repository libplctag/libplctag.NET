using libplctag.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{
    public class Tag<M, T> : IDisposable
        where M : Marshaller<T>, new()
    {

        private readonly Tag _tag;
        private readonly Marshaller<T> _marshaller;

        public Tag()
        {
            _marshaller = new M();
            _tag = new Tag()
            {
                ElementSize = _marshaller.ElementSize,
            };
        }

        public Protocol? Protocol
        {
            get => _tag.Protocol;
            set => _tag.Protocol = value;
        }
        public string Gateway
        {
            get => _tag.Gateway;
            set => _tag.Gateway = value;
        }
        public string Path
        {
            get => _tag.Path;
            set => _tag.Path = value;
        }
        public PlcType? PlcType
        {
            get => _tag.PlcType;
            set => _tag.PlcType = value;
        }
        public int? ArrayLength
        {
            get => _marshaller.ArrayLengthFromElementCount(_tag.ElementCount);
            set => _tag.ElementCount = _marshaller.ElementCountFromArrayLength(value);
        }
        public string Name
        {
            get => _tag.Name;
            set => _tag.Name = value;
        }
        public bool? UseConnectedMessaging
        {
            get => _tag.UseConnectedMessaging;
            set => _tag.UseConnectedMessaging = value;
        }
        public int? ReadCacheMillisecondDuration
        {
            get => _tag.ReadCacheMillisecondDuration;
            set => _tag.ReadCacheMillisecondDuration = value;
        }


        public void Initialize(int millisecondTimeout)
        {
            _tag.Initialize(millisecondTimeout);
            DecodeAll();
        }

        public async Task InitializeAsync(int millisecondTimeout, CancellationToken token = default)
        {
            await _tag.InitializeAsync(millisecondTimeout, token);
            DecodeAll();
        }

        public async Task InitializeAsync(CancellationToken token = default)
        {
            await _tag.InitializeAsync(token);
            DecodeAll();
        }

        public async Task ReadAsync(int millisecondTimeout, CancellationToken token = default)
        {
            await _tag.ReadAsync(millisecondTimeout, token);
            DecodeAll();
        }

        public async Task ReadAsync(CancellationToken token = default)
        {
            await _tag.ReadAsync(token);
            DecodeAll();
        }

        public void Read(int millisecondTimeout)
        {
            _tag.Read(millisecondTimeout);
            DecodeAll();
        }

        public async Task WriteAsync(int millisecondTimeout, CancellationToken token = default)
        {
            EncodeAll();
            await _tag.WriteAsync(millisecondTimeout, token);
        }

        public async Task WriteAsync(CancellationToken token = default)
        {
            EncodeAll();
            await _tag.ReadAsync(token);
        }

        public void Write(int millisecondTimeout)
        {
            EncodeAll();
            _tag.Write(millisecondTimeout);
        }

        void DecodeAll()
        {
            Value = _marshaller.Decode(_tag);
        }

        void EncodeAll()
        {
            _marshaller.Encode(_tag, Value);
        }

        public Status GetStatus() => _tag.GetStatus();

        public void Dispose() => _tag.Dispose();

        ~Tag()
        {
            Dispose();
        }

        public T[] Value { get; set; }

    }
}
