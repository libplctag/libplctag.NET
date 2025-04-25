// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers.Binary;
using libplctag;

namespace CSharp_DotNetCore
{
    class ExampleRaw
    {

        public static void Run()
        {
            // This payload is taken from https://github.com/libplctag/libplctag/blob/release/src/examples/test_raw_cip.c
            // but others can be found by analysing the Rockwell or other manufacturer's documentation
            // https://literature.rockwellautomation.com/idc/groups/literature/documents/pm/1756-pm020_-en-p.pdf pg 39

            ReadOnlySpan<byte> raw_payload = [
                0x55,
                0x03,
                0x20, 0x6b, 0x25, 0x00, 0x00, 0x00,
                0x04, 0x00,
                0x02, 0x00,
                0x07, 0x00,
                0x08, 0x00,
                0x01, 0x00
            ];

            var cipService = new Tag()
            {
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Gateway = "10.10.10.10",
                Path = "1,0",
                Name = "@raw",
            };
            cipService.Initialize();
            cipService.SetSize(raw_payload.Length);
            cipService.SetBuffer(raw_payload);
            cipService.Write();
            Span<byte> res = stackalloc byte[cipService.GetSize()];
            cipService.GetBuffer(res);

            var tagInfos = new List<TagInfo>();
            for (int cursor = 0; cursor < res.Length;)
            {
                var tagNameLength = U16(res, cursor + 20);
                tagInfos.Add(new (
                    Id : U32(res, cursor + 0),
                    Type : U16(res, cursor + 4),
                    Length : U16(res, cursor + 6),
                    Dimensions : [
                        U32(res, cursor + 8),
                        U32(res, cursor + 12),
                        U32(res, cursor + 16)
                        ],
                    Name : ASCII(res, cursor + 22, tagNameLength)
                ));

                cursor += 22 + tagNameLength;
            }

            foreach (var tag in tagInfos)
                Console.WriteLine($"Id={tag.Id} Name={tag.Name} Type={tag.Type} Length={tag.Length}");
        }

        record TagInfo(uint Id, ushort Type, ushort Length, uint[] Dimensions, string Name);
        static ushort U16(ReadOnlySpan<byte> bs, int offset) => BinaryPrimitives.ReadUInt16LittleEndian(bs[offset..]);
        static uint U32(ReadOnlySpan<byte> bs, int offset) => BinaryPrimitives.ReadUInt32LittleEndian(bs[offset..]);
        static string ASCII(ReadOnlySpan<byte> bs, int offset, int len) => Encoding.ASCII.GetString(bs[offset..(offset+len)]);
    }

}
