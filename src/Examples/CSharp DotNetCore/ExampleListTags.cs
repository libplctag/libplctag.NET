using System;
using System.Collections.Generic;
using System.Linq;
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

        static int setup_tag(string ipAddress, string path, string program)
        {

            int tag = (int)Status.ErrorCreate;
            string tag_string;

            if (string.IsNullOrEmpty(program))
            {
                tag_string = $"protocol=ab-eip&gateway={ipAddress}&path={path}&cpu=lgx&name=@tags";
            }
            else
            {
                tag_string = $"protocol=ab-eip&gateway={ipAddress}&path={path}&cpu=lgx&name={program}.@tags";
            }

            tag = plctag.create(tag_string, TIMEOUT_MS);
            if (tag < 0)
            {
                Console.WriteLine($"Unable to open tag!  Return code {plctag.decode_error(tag)}\n");
                return 0;
            }

            return tag;

        }


        static IEnumerable<TagStructure> get_list(int tag, string prefix/*, tag_entry_s[][] tag_list, program_entry_s[][] prog_list*/)
        {
            int rc = (int)Status.Ok;
            int offset = 0;

            rc = plctag.read(tag, TIMEOUT_MS);
            if (rc != (int)Status.Ok)
            {
                throw new Exception($"Unable to read tag!  Return code {plctag.decode_error(tag)}");
            }

            /* process each tag entry. */
            do
            {
                uint tag_instance_id = 0;
                ushort tag_type = 0;
                ushort element_length = 0;
                ushort[] array_dims = new ushort[3];
                string tag_name = "";

                /* each entry looks like this:
                uint32_t instance_id    monotonically increasing but not contiguous
                uint16_t symbol_type    type of the symbol.
                uint16_t element_length length of one array element in bytes.
                uint32_t array_dims[3]  array dimensions.
                uint16_t string_len     string length count.
                uint8_t string_data[]   string bytes (string_len of them)
                */

                tag_instance_id = plctag.get_uint32(tag, offset);
                offset += 4;

                tag_type = plctag.get_uint16(tag, offset);
                offset += 2;

                element_length = plctag.get_uint16(tag, offset);
                offset += 2;

                array_dims[0] = (ushort)plctag.get_uint32(tag, offset);
                offset += 4;
                array_dims[1] = (ushort)plctag.get_uint32(tag, offset);
                offset += 4;
                array_dims[2] = (ushort)plctag.get_uint32(tag, offset);
                offset += 4;

                var tag_name_len = plctag.get_uint16(tag, offset);
                offset += 2;

                int prefix_size;
                if (!string.IsNullOrEmpty(prefix))
                {
                    tag_name = $"{prefix}.";
                    prefix_size = prefix.Length + 1;
                }
                else
                {
                    prefix_size = 0;
                }

                byte[] tagNameBytes = new byte[tag_name_len];
                for (int ii = 0; (ii < tag_name_len) && (ii + prefix_size < TAG_STRING_SIZE * 2 - 1); ii++)
                {
                    var character = plctag.get_uint8(tag, offset);
                    tagNameBytes[ii] = Convert.ToByte(character);
                    offset++;
                }

                tag_name = Encoding.ASCII.GetString(tagNameBytes);

                yield return new TagStructure()
                {
                    Id = tag_instance_id,
                    Type = tag_type,
                    Name = tag_name,
                    Length = element_length,
                    Dimensions = array_dims
                };

            } while (rc == (int)Status.Ok && offset < plctag.get_size(tag));

        }

        class TagStructure
        {
            public uint Id { get; set; }
            public int Type { get; set; }
            public string Name { get; set; }
            public int Length { get; set; }
            public ushort[] Dimensions { get; set; }
        }



        public static void Run()
        {
            var host = "192.168.0.10";
            var path = "1,0";
            var tag = setup_tag(host, path, null);

            var tags = get_list(tag, null);

            ConsoleTable
                .From(tags.Select(t => new { t.Id, t.Type, t.Name, t.Length, Dimensions = string.Join(',', t.Dimensions) }))
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Default);

        }

    }
}