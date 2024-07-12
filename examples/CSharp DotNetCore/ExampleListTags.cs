// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using System.Net;
using System.Text;
using ConsoleTables;
using libplctag;
using libplctag.DataTypes;

namespace CSharpDotNetCore
{
    static class ExampleListTags
    {
        public static void Run()
        {
            //This example will list all tags at the controller-scoped level

            var tags = new Tag<TagInfoPlcMapper, TagInfo[]>()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Name = "@tags"
            };

            tags.Read();

            ConsoleTable
                .From(tags.Value.Select(t => new
                {
                    t.Id,
                    Type = $"0x{t.Type:X}",
                    t.Name,
                    t.Length,
                    Dimensions = string.Join(',', t.Dimensions)
                }))
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Default);

        }

    }

}