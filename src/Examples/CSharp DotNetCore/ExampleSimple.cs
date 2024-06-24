// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleSimple
    {
        public static void Run()
        {
            //This is the absolute most simplified example code
            //Please see the other examples for more features/optimizations

            //Instantiate the tag with the proper mapper and datatype
            var myTag = new Tag()
            {
                Name = "PROGRAM:SomeProgram.SomeDINT",
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromSeconds(5)
            };

            //Write value to PLC
            //This will call Initialize internally since it's the first use of this tag
            //myTag.Value will be set to 3737 before being transferred to PLC
            myTag.SetInt32(0, 3737);

            //Read value from PLC
            //Value will also be accessible at myTag.Value
            myTag.Read();
            int myDint = myTag.GetInt32(0);

            //Write to console
            Console.WriteLine(myDint);
        }
    }
}
