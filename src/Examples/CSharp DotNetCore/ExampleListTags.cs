using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleTables;
using libplctag.NativeImport;

namespace CSharpDotNetCore
{
    static class ExampleListTags
    {


        static int TAG_STRING_SIZE = 200;
        static int TIMEOUT_MS = 5000;


        static IEnumerable<TagInfo> GetTags(string ipAddress, string path)
        {
            

            var tag_string = $"protocol=ab-eip&gateway={ipAddress}&path={path}&cpu=lgx&name=@tags";
            var tag = plctag.plc_tag_create(tag_string, TIMEOUT_MS);
            if (tag < 0)
            {
                throw new Exception($"Unable to open tag!  Return code {plctag.plc_tag_decode_error(tag)}\n");
            }



            int resultCode = plctag.plc_tag_read(tag, TIMEOUT_MS);
            if (resultCode != (int)STATUS_CODES.PLCTAG_STATUS_OK)
            {
                throw new Exception($"Unable to read tag!  Return code {plctag.plc_tag_decode_error(tag)}");
            }



            int offset = 0;

            do
            {

                /* each entry looks like this:
                uint32_t instance_id    monotonically increasing but not contiguous
                uint16_t symbol_type    type of the symbol.
                uint16_t element_length length of one array element in bytes.
                uint32_t array_dims[3]  array dimensions.
                uint16_t string_len     string length count.
                uint8_t string_data[]   string bytes (string_len of them)
                */

                var tagInstanceId =     plctag.plc_tag_get_uint32(tag, offset);
                var tagType =           plctag.plc_tag_get_uint16(tag, offset + 4);
                var tagLength =         plctag.plc_tag_get_uint16(tag, offset + 6);
                var tagArrayDims = new uint[]
                {
                    plctag.plc_tag_get_uint32(tag, offset + 8),
                    plctag.plc_tag_get_uint32(tag, offset + 12),
                    plctag.plc_tag_get_uint32(tag, offset + 16)
                };

                var apparentTagNameLength = plctag.plc_tag_get_uint16(tag, offset + 20);
                var actualTagNameLength = Math.Min(apparentTagNameLength, TAG_STRING_SIZE * 2 - 1);

                var tagNameBytes = Enumerable.Range(offset + 22, actualTagNameLength)
                    .Select(o => plctag.plc_tag_get_uint8(tag, o))
                    .Select(Convert.ToByte)
                    .ToArray();

                var tagName = Encoding.ASCII.GetString(tagNameBytes);

                yield return new TagInfo()
                {
                    Id = tagInstanceId,
                    Type = tagType,
                    Name = tagName,
                    Length = tagLength,
                    Dimensions = tagArrayDims
                };

                offset += 22 + actualTagNameLength;

            } while (resultCode == (int)STATUS_CODES.PLCTAG_STATUS_OK && offset < plctag.plc_tag_get_size(tag));

        }

        class TagInfo
        {
            public uint Id { get; set; }
            public int Type { get; set; }
            public string Name { get; set; }
            public int Length { get; set; }
            public uint[] Dimensions { get; set; }
        }



        public static void Run()
        {
            var host = "192.168.0.10";
            var path = "1,0";
            var tags = GetTags(host, path);

            ConsoleTable
                .From(tags.Select(t => new { t.Id, t.Type, t.Name, t.Length, Dimensions = string.Join(',', t.Dimensions) }))
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Default);

        }

    }
}