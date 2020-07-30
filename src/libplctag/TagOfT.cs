using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{
    public class Tag<Marshaller, T> : IDisposable
        where Marshaller : IMarshaller<T>, new()
    {

        private readonly Tag _tag;
        private readonly IMarshaller<T> _marshaller;

        public Tag()
        {
            _marshaller = new Marshaller();
            _tag = new Tag()
            {
                ElementSize = _marshaller.ElementSize
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
        public int? ElementCount
        {
            get => _tag.ElementCount;
            set => _tag.ElementCount = value;
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

            var tempArray = new List<T>();

            var totalTagSize = _tag.GetSize();

            int offset = 0;
            while(offset < totalTagSize)
            {
                tempArray.Add(_marshaller.Decode(_tag, offset, out int elementSize));
                offset += elementSize;
            }

            Value = tempArray.ToArray();
        }

        void EncodeAll()
        {
            int offset = 0;
            for (int ii = 0; ii < ElementCount; ii++)
            {
                _marshaller.Encode(_tag, offset, out int elementSize, Value[ii]);
                offset += elementSize;
            }
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
