﻿// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace libplctag.NativeImport.Benchmarks
{
    [MemoryDiagnoser]
    public class GetStatusRawVsExtractFirst
    {

        public GetStatusRawVsExtractFirst()
        {
            // In order for this to compile you need to make this function public, it is normally a private method.
            // And of course you need to reference the project rather than the package.
            //LibraryExtractor.Init();
            throw new NotImplementedException();
        }

        [Benchmark]
        public int Raw()
        {
            // In order for this to compile you need to make this function public, it is normally a private method.
            // And of course you need to reference the project rather than the package.
            //return plctag.plc_tag_check_lib_version_raw(0, 0, 0);
            throw new NotImplementedException();
        }

        [Benchmark]
        public int ExtractFirst()
        {
            return plctag.plc_tag_check_lib_version(0, 0, 0);
        }

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<GetStatusRawVsExtractFirst>();
        }
    }
}
