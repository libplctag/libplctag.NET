// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;
using libplctag;

namespace CSharpDotNetCore.Inheritance
{

    abstract class TagSingle<T> : BaseTag
    {
        private readonly int elementSize;
        public TagSingle(int elementSize)
            : base()
        {
            this.elementSize = elementSize;
            tag.ElementSize = elementSize;
        }

        protected abstract T Decode(int offset);
        protected abstract void Encode(int offset, T value);

        public T Value
        {
            get => Decode(0);
            set => Encode(0, value);
        }

    }

    abstract class Tag1D<T> : BaseTag
    {
        private readonly int elementSize;
        public Tag1D(int length, int elementSize)
            : base()
        {
            this.elementSize = elementSize;
            tag.ElementSize = elementSize;
            Length = length;
        }

        protected abstract T Decode(int offset);
        protected abstract void Encode(int offset, T value);

        public T GetValue(int index) => Decode(index * elementSize);
        public void SetValue(int index, T value) => Encode(index * elementSize, value);

        public int Length
        {
            get => tag.ElementCount.Value;
            set => tag.ElementCount = value;
        }
    }


    abstract class Tag2D<T> : BaseTag
    {
        private readonly int elementSize;
        private int lengthDimension1;
        private int lengthDimension2;

        public Tag2D(int lengthDimension1, int lengthDimension2, int elementSize)
            : base()
        {
            this.elementSize = elementSize;
            tag.ElementSize = elementSize;
            LengthDimension1 = lengthDimension1;
            LengthDimension2 = lengthDimension1;
        }

        protected abstract T Decode(int offset);
        protected abstract void Encode(int offset, T value);

        private int GetOffset(int i, int j) => this.elementSize * (i + j * LengthDimension1);

        public T GetValue(int i, int j) => Decode(GetOffset(i, j));
        public void SetValue(int i, int j, T value) => Encode(GetOffset(i, j), value);

        public int LengthDimension1
        {
            get => lengthDimension1;
            set
            {
                tag.ElementCount = value * lengthDimension2;
                lengthDimension1 = value;
            }
        }

        public int LengthDimension2
        {
            get => lengthDimension2;
            set
            {
                tag.ElementCount = lengthDimension1 * value;
                lengthDimension2 = value;
            }
        }
    }

    abstract class Tag3D<T> : BaseTag
    {
        private readonly int elementSize;
        private int lengthDimension1;
        private int lengthDimension2;
        private int lengthDimension3;

        public Tag3D(int lengthDimension1, int lengthDimension2, int lengthDimension3, int elementSize)
            : base()
        {
            this.elementSize = elementSize;
            tag.ElementSize = elementSize;
            LengthDimension1 = lengthDimension1;
            LengthDimension2 = lengthDimension2;
            LengthDimension3 = lengthDimension3;
        }

        protected abstract T Decode(int offset);
        protected abstract void Encode(int offset, T value);

        private int GetOffset(int i, int j, int k) => elementSize * (i + j * LengthDimension1 + k * LengthDimension1 * LengthDimension2);

        public T GetValue(int i, int j, int k) => Decode(GetOffset(i, j, k));
        public void SetValue(int i, int j, int k, T value) => Encode(GetOffset(i, j, k), value);

        public int LengthDimension1
        {
            get => lengthDimension1;
            set
            {
                tag.ElementCount = value * lengthDimension2 * lengthDimension3;
                lengthDimension1 = value;
            }
        }

        public int LengthDimension2
        {
            get => lengthDimension2;
            set
            {
                tag.ElementCount = lengthDimension1 * value * lengthDimension3;
                lengthDimension2 = value;
            }
        }

        public int LengthDimension3
        {
            get => lengthDimension3;
            set
            {
                tag.ElementCount = lengthDimension1 * lengthDimension2 * value;
                lengthDimension3 = value;
            }
        }
    }

    abstract class BaseTag : IDisposable
    {

        protected readonly Tag tag = new Tag();

        public Protocol? Protocol
        {
            get => tag.Protocol;
            set => tag.Protocol = value;
        }

        public string Gateway
        {
            get => tag.Gateway;
            set => tag.Gateway = value;
        }

        public string Path
        {
            get => tag.Path;
            set => tag.Path = value;
        }

        public PlcType? PlcType
        {
            get => tag.PlcType;
            set => tag.PlcType = value;
        }

        public string Name
        {
            get => tag.Name;
            set => tag.Name = value;
        }

        public bool? UseConnectedMessaging
        {
            get => tag.UseConnectedMessaging;
            set => tag.UseConnectedMessaging = value;
        }

        public bool? AllowPacking
        {
            get => tag.AllowPacking;
            set => tag.AllowPacking = value;
        }

        public int? ReadCacheMillisecondDuration
        {
            get => tag.ReadCacheMillisecondDuration;
            set => tag.ReadCacheMillisecondDuration = value;
        }

        public TimeSpan Timeout
        {
            get => tag.Timeout;
            set => tag.Timeout = value;
        }

        public TimeSpan? AutoSyncReadInterval
        {
            get => tag.AutoSyncReadInterval;
            set => tag.AutoSyncReadInterval = value;
        }

        public TimeSpan? AutoSyncWriteInterval
        {
            get => tag.AutoSyncWriteInterval;
            set => tag.AutoSyncWriteInterval = value;
        }

        public DebugLevel DebugLevel
        {
            get => tag.DebugLevel;
            set => tag.DebugLevel = value;
        }

        public void Dispose() => tag.Dispose();
        public Task InitializeAsync(CancellationToken token = default) => tag.InitializeAsync(token);
        public Task ReadAsync(CancellationToken token = default) => tag.ReadAsync(token);
        public Task WriteAsync(CancellationToken token = default) => tag.WriteAsync(token);

        ~BaseTag()
        {
            Dispose();
        }
    }
}
