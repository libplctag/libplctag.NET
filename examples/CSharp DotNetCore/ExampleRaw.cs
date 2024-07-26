// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libplctag;

namespace CSharp_DotNetCore
{
    class ExampleRaw
    {
        public static void Run()
        {
            var lister = new LogixTagListing()
            {
                Gateway = "192.168.0.1",
                Path = "1,0",
                Timeout = TimeSpan.FromMilliseconds(1000),
            };

            var tags = lister.ListTags();

            foreach (var tag in tags)
                Console.WriteLine($"Id={tag.Id} Name={tag.Name} Type={tag.Type} Length={tag.Length}");
        }

        class LogixTagListing
        {
            readonly Tag _rawCip = new Tag()
            {
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Name = "@raw",
            };

            public string Gateway { get => _rawCip.Gateway; set => _rawCip.Gateway = value; }
            public string Path { get => _rawCip.Path; set => _rawCip.Path = value; }
            public TimeSpan Timeout { get => _rawCip.Timeout; set => _rawCip.Timeout = value; }

            public List<TagInfo> ListTags()
            {
                // This payload is taken from https://github.com/libplctag/libplctag/blob/release/src/examples/test_raw_cip.c
                // but others can be found by analysing the Rockwell or other manufacturer's documentation
                // https://literature.rockwellautomation.com/idc/groups/literature/documents/pm/1756-pm020_-en-p.pdf pg 39

                var raw_payload = new byte[] {
                    0x55,
                    0x03,
                    0x20,
                    0x6b,
                    0x25,
                    0x00,
                    0x00,
                    0x00,
                    0x04,
                    0x00,
                    0x02,
                    0x00,
                    0x07,
                    0x00,
                    0x08,
                    0x00,
                    0x01,
                    0x00
                };

                _rawCip.Initialize();
                _rawCip.SetSize(raw_payload.Length);
                _rawCip.SetBuffer(raw_payload);
                _rawCip.Write();

                var responseSize = _rawCip.GetSize();

                var tagInfos = new List<TagInfo>();
                int offset = 0;
                while (offset < responseSize)
                    tagInfos.Add(DecodeOneTagInfo(ref offset));

                return tagInfos;
            }

            public class TagInfo
            {
                public uint Id { get; set; }
                public ushort Type { get; set; }
                public string Name { get; set; }
                public ushort Length { get; set; }
                public uint[] Dimensions { get; set; }
            }

            TagInfo DecodeOneTagInfo(ref int offset)
            {

                var tagInstanceId = _rawCip.GetUInt32(offset);
                var tagType = _rawCip.GetUInt16(offset + 4);
                var tagLength = _rawCip.GetUInt16(offset + 6);
                var tagArrayDims = new uint[]
                {
                    _rawCip.GetUInt32(offset + 8),
                    _rawCip.GetUInt32(offset + 12),
                    _rawCip.GetUInt32(offset + 16)
                };

                var apparentTagNameLength = (int)_rawCip.GetUInt16(offset + 20);
                const int TAG_STRING_SIZE = 200;
                var actualTagNameLength = Math.Min(apparentTagNameLength, TAG_STRING_SIZE * 2 - 1);

                var tagNameBytes = Enumerable.Range(offset + 22, actualTagNameLength)
                    .Select(o => _rawCip.GetUInt8(o))
                    .Select(Convert.ToByte)
                    .ToArray();

                var tagName = Encoding.ASCII.GetString(tagNameBytes);

                offset = 22 + actualTagNameLength;

                return new TagInfo()
                {
                    Id = tagInstanceId,
                    Type = tagType,
                    Name = tagName,
                    Length = tagLength,
                    Dimensions = tagArrayDims
                };

            }

        }

    }

}
