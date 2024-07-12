// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpDotNetCore
{
    class ExampleCustomUdt
    {
        public static void Run()
        {
            const string gateway = "10.10.10.10";
            const string path = "1,0";

            //This example shows the use of a custom mapper created to match a UDT called "Sequence"

            var sequenceArray = new Tag<SequencePlcMapper, Sequence[]>()
            {
                Name = "MY_SEQUENCE_3D[0,0,0]",
                Gateway = gateway,
                Path = path,
                Protocol = Protocol.ab_eip,
                PlcType = PlcType.ControlLogix,
                ArrayDimensions = new int[] { 8 },
            };

            for (int ii = 0; ii < 8; ii++)
                sequenceArray.Value[ii].Command = ii * 2;

            sequenceArray.Write();


            Console.WriteLine("DONE! Check values in RsLogix");

        }

    }
}
