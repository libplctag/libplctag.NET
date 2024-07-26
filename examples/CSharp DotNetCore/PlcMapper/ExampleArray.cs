// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;

namespace CSharpDotNetCore.PlcMapper
{
    class ExampleArray
    {
        public static void Run()
        {
            //This example shows reading/writing to a 1D DINT array of 5 elements

            Console.WriteLine($"\r\n*** ExampleArray ***");

            //Set some constants for our Tag
            const int ARRAY_LENGTH = 5;
            const int TIMEOUT = 1000;
            const string gateway = "10.10.10.10";
            const string path = "1,0";

            var dintTag = new Tag<DintPlcMapper, int[]>()
            {
                Name = "TestDINTArray2",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                ArrayDimensions = new int[] { ARRAY_LENGTH },
                Timeout = TimeSpan.FromMilliseconds(TIMEOUT),
            };

            dintTag.Value = new int[] { 1, 2, 3, 4, 5 };
            dintTag.Write();

            dintTag.Read();
            
            //Read back value from local memory
            for (int i = 0; i < dintTag.Value.Length; i++)
            {
                Console.WriteLine($"Value[{i}]: {dintTag.Value[i]}");
            }
        }

    }
}
