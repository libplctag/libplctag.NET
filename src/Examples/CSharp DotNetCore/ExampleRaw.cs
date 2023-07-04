using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDotNetCore
{
    class ExampleRaw
    {
        public static void Run()
        {
            var tags = new Tag<RawTagListingMapper, TagInfo[]>()
            {
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromMilliseconds(1000),
            };

            tags.Read();

            foreach (var tag in tags.Value)
                Console.WriteLine($"Id={tag.Id} Name={tag.Name} Type={tag.Type} Length={tag.Length}");
        }

    }

    class RawTagListingMapper : IPlcMapper<TagInfo[]>
    {
        public PlcType PlcType { get; set; }

        public int? ElementSize => null;

        public int[] ArrayDimensions { get => null; set => throw new NotImplementedException("This plcMapper can only be used to read Tag Information"); }

        public void Configure(Tag tag)
        {
            // This payload is taken from https://github.com/libplctag/libplctag/blob/release/src/examples/test_raw_cip.c
            // but others can be found by analysing the Rockwell or other manufacturer's documentation
            // https://literature.rockwellautomation.com/idc/groups/literature/documents/pm/1756-pm020_-en-p.pdf

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

            tag.Name = "@raw";
            tag.SetSize(raw_payload.Length);

            for (int ii = 0; ii < raw_payload.Length; ii++)
                tag.SetUInt8(ii, raw_payload[ii]);

        }

        public TagInfo[] Decode(Tag tag)
        {
            // This mapper makes use of a pre-existing mapper that decodes an array of TagInfo
            // We could instead make this mapper target a different CLR type, and decode into that instead.
            var tagInfoMapper = new TagInfoPlcMapper() { PlcType = PlcType };
            return tagInfoMapper.Decode(tag);
        }

        public void Encode(Tag tag, TagInfo[] value)
            => throw new NotImplementedException();

        public int? GetElementCount()
            => null;
    }
}
