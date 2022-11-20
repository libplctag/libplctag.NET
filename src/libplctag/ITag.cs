// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace libplctag
{
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

        Status GetStatus();
        void Initialize();
        Task InitializeAsync(CancellationToken token = default);
        object Read();
        Task<object> ReadAsync(CancellationToken token = default);
        void Write();
        Task WriteAsync(CancellationToken token = default);

        Object Value { get; set; }
    }
}