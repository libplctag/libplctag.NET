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
        where M : IMarshaller<T>, new()
    {

        private readonly Tag _tag;
        private readonly IMarshaller<T> _marshaller;

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
        public int[] ArrayLength
        {
            get => _marshaller.GetArrayLength(_tag);
            set => _tag.ElementCount = _marshaller.SetArrayLength(value);
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
        public TimeSpan Timeout
        {
            get => _tag.Timeout;
            set => _tag.Timeout = value;
        }


        public void Initialize()
        {
            _tag.Initialize();
            DecodeAll();
        }

        public async Task InitializeAsync(CancellationToken token = default)
        {
            await _tag.InitializeAsync(token);
            DecodeAll();
        }

        public async Task ReadAsync(CancellationToken token = default)
        {
            await _tag.ReadAsync(token);
            DecodeAll();
        }

        public void Read()
        {
            _tag.Read();
            DecodeAll();
        }

        public async Task WriteAsync(CancellationToken token = default)
        {
            EncodeAll();
            await _tag.WriteAsync(token);
        }

        public void Write()
        {
            EncodeAll();
            _tag.Write();
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

        public T Value { get; set; }

    }
}
