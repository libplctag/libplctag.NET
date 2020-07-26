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


        static int TAG_STRING_SIZE = 200;
        static int TIMEOUT_MS = 5000;


        static IEnumerable<TagInfo> GetTags(string ipAddress, string path)
        {
            

            var tag_string = $"protocol=ab-eip&gateway={ipAddress}&path={path}&plc=lgx&name=@tags";
            int resultCode = plctag.plc_tag_create(tag_string, TIMEOUT_MS);
            if (resultCode < 0)
            {
                throw new Exception($"Unable to open tag!  Return code {plctag.plc_tag_decode_error(resultCode)}\n");
            }

            // Else since resultCode >= 0, it is our tag handle
            var tagHandle = resultCode;

            resultCode = plctag.plc_tag_read(tagHandle, TIMEOUT_MS);
            if (resultCode != (int)STATUS_CODES.PLCTAG_STATUS_OK)
            {
                throw new Exception($"Unable to read tag!  Return code {plctag.plc_tag_decode_error(resultCode)}");
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

                var tagInstanceId =     plctag.plc_tag_get_uint32(tagHandle, offset);
                var tagType =           plctag.plc_tag_get_uint16(tagHandle, offset + 4);
                var tagLength =         plctag.plc_tag_get_uint16(tagHandle, offset + 6);
                var tagArrayDims = new uint[]
                {
                    plctag.plc_tag_get_uint32(tagHandle, offset + 8),
                    plctag.plc_tag_get_uint32(tagHandle, offset + 12),
                    plctag.plc_tag_get_uint32(tagHandle, offset + 16)
                };

                var apparentTagNameLength = plctag.plc_tag_get_uint16(tagHandle, offset + 20);
                var actualTagNameLength = Math.Min(apparentTagNameLength, TAG_STRING_SIZE * 2 - 1);

                var tagNameBytes = Enumerable.Range(offset + 22, actualTagNameLength)
                    .Select(o => plctag.plc_tag_get_uint8(tagHandle, o))
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

            } while (resultCode == (int)STATUS_CODES.PLCTAG_STATUS_OK && offset < plctag.plc_tag_get_size(tagHandle));

        }

        public static void Run()
        {
            var host = "192.168.0.10";
            var path = "1,0";
            //var tags = GetTags(host, path);

            var tagList = new Tag<TagListingMarshaller, IEnumerable<TagInfo>>(IPAddress.Parse(host), path, PlcType.ControlLogix, "@tags", 1000);

            var tags = tagList.Value.ToList();

            ConsoleTable
                .From(tags.Select(t => new { t.Id, Type = $"0x{t.Type:X}", t.Name, t.Length, Dimensions = string.Join(',', t.Dimensions) }))
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

    public class TagListingMarshaller : IMarshaller<IEnumerable<TagInfo>>
    {

        public int ElementSize => 0;

        public PlcType PlcType { get; set; }

        public IEnumerable<TagInfo> Decode(Tag tag, int offset)
        {

            var TAG_STRING_SIZE = 200;

            do
            {

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

                yield return new TagInfo()
                {
                    Id = tagInstanceId,
                    Type = tagType,
                    Name = tagName,
                    Length = tagLength,
                    Dimensions = tagArrayDims
                };

                offset += 22 + actualTagNameLength;

            } while (tag.GetStatus() == Status.Ok && offset < tag.GetSize());

        }

        public void Encode(Tag tag, int offset, IEnumerable<TagInfo> t)
        {
            throw new NotImplementedException();
        }
    }
}