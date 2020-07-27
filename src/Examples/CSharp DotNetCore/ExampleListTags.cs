using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ConsoleTables;
using libplctag;
using libplctag.NativeImport;

namespace CSharpDotNetCore
{
    static class ExampleListTags
    {

        static int TIMEOUT_MS = 5000;

        public static void Run()
        {
            var host = "192.168.0.10";
            var path = "1,0";

            // The current constructor forces us to choose the element count, but @tags is special
            // so ideally we don't supply elem_count in the attribute string
            // For now, this works as long as there are at least 5 tags in the controller
            var elementCount = 5;

            var tags = new Tag<TagListingMarshaller, TagInfo>(IPAddress.Parse(host), path, PlcType.ControlLogix, "@tags", TIMEOUT_MS, elementCount);

            ConsoleTable
                .From(tags.Value.Select(t => new { t.Id, Type = $"0x{t.Type:X}", t.Name, t.Length, Dimensions = string.Join(',', t.Dimensions) }))
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Default);

        }

    }

    public class TagInfo
    {
        public uint Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public uint[] Dimensions { get; set; }
    }

    public class TagListingMarshaller : IMarshaller<TagInfo>
    {

        public int ElementSize => 0;

        public PlcType PlcType { get; set; }

        public TagInfo Decode(Tag tag, int offset, out int elementSize)
        {

            var TAG_STRING_SIZE = 200;

            var tagInstanceId = tag.GetUInt32(offset);
            var tagType = tag.GetUInt16(offset+4);
            var tagLength = tag.GetUInt16(offset + 6);
            var tagArrayDims = new uint[]
            {
                tag.GetUInt32(offset + 8),
                tag.GetUInt32(offset + 12),
                tag.GetUInt32(offset + 16)
            };

            var apparentTagNameLength = tag.GetUInt16(offset + 20);
            var actualTagNameLength = (int)Math.Min(apparentTagNameLength, TAG_STRING_SIZE * 2 - 1);

            var tagNameBytes = Enumerable.Range(offset + 22, actualTagNameLength)
                .Select(o => tag.GetUInt8(o))
                .Select(Convert.ToByte)
                .ToArray();

            var tagName = Encoding.ASCII.GetString(tagNameBytes);

            elementSize = 22 + actualTagNameLength;

            return new TagInfo()
            {
                Id = tagInstanceId,
                Type = tagType,
                Name = tagName,
                Length = tagLength,
                Dimensions = tagArrayDims
            };

        }

        public void Encode(Tag tag, int offset, out int elementSize, TagInfo t)
        {
            throw new NotImplementedException();
        }
    }
}