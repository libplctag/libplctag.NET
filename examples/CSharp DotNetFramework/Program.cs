// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;

namespace CSharpDotNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {

            // Example tag configuration for a global DINT tag in an Allen-Bradley CompactLogix/ControlLogix PLC
            var myTag = new Tag()
            {
                Name = "SomeDINT",
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            // Read the value from the PLC and output to console
            myTag.Read();
            int originalValue = myTag.GetInt32(0);
            Console.WriteLine($"Original value: {originalValue}");

            // Write a new value to the PLC, then read it back, and output to console
            int updatedValue = 1234;
            myTag.SetInt32(0, updatedValue);
            myTag.Write();
            Console.WriteLine($"Updated value: {updatedValue}");
        }
    }
} 