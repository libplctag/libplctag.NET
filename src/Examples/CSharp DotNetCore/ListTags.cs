using ConsoleTables;
using libplctag;
using libplctag.DataTypes;
using libplctag.NativeImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDotNetCore
{

    class ProgramEntry
    {
        public string Name { get;set; }
        public ProgramEntry Next { get; set; }
    }

    class TagEntry
    {
        public TagEntry Next { get; set; }
        public string Name { get; set; }
        public TagEntry Parent { get; set; }
        public ushort Type { get; set; }
        public ushort ElemSize { get; set; }
        public ushort ElemCount { get; set; }
        public ushort NumDimensions { get; set; }
        public ushort[] Dimensions { get; set; }
    }

    class UdtFieldEntry
    {
        public string Name { get; set; }
        public ushort Type { get; set; }
        public ushort Metadata { get; set; }
        public uint Size { get; set; }
        public uint Offset { get; set; }
    }

    class UdtEntry
    {
        public string Name { get; set; }
        public ushort Id { get; set; }
        public ushort NumFields { get; set; }
        public ushort Handle { get; set; }
        public UdtFieldEntry[] Fields { get; set; }
    }




    class ListTags
    {
        static public void Run()
        {

            plctag.ForceExtractLibrary = false;

            var tags = new Tag<TagInfoPlcMapper, TagInfo[]>()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Name = "@tags"
            };

            tags.Read();

            const ushort TYPE_IS_STRUCT = 0x8000;
            const ushort TYPE_IS_SYSTEM = 0x1000;
            const ushort TYPE_DIM_MASK = 0x6000;
            const ushort TYPE_UDT_ID_MASK = 0x0FFF;
            const ushort TAG_DIM_MASK = 0x6000;

            var uniqueUdtTypeIds = tags.Value
                .Where(tagInfo => ((tagInfo.Type & TYPE_IS_STRUCT) != 0) && !((tagInfo.Type & TYPE_IS_SYSTEM) != 0))
                .Select(tagInfo => tagInfo.Type & TYPE_UDT_ID_MASK)
                .Distinct();

            var udtDefinitions = new List<UdtEntry>();

            foreach (var udtId in uniqueUdtTypeIds)
            {
                var udtDefinitionTag = new Tag()
                {
                    Gateway = "192.168.0.10",
                    Path = "1,0",
                    PlcType = PlcType.ControlLogix,
                    Protocol = Protocol.ab_eip,
                    Name = $"@udt/{udtId}",
                };

                udtDefinitionTag.Read();

                var template_id =       udtDefinitionTag.GetUInt16(0);
                var member_desc_size =  udtDefinitionTag.GetUInt32(2);
                var udt_instance_size = udtDefinitionTag.GetUInt32(6);
                var num_members =       udtDefinitionTag.GetUInt16(10);
                var struct_handle =     udtDefinitionTag.GetUInt16(12);


                var template = new UdtEntry()
                {
                    Fields = new UdtFieldEntry[num_members],
                    NumFields = num_members,
                    Handle = struct_handle,
                    Id = template_id,
                };

                var offset = 14;

                for (int field_index = 0; field_index < num_members; field_index++)
                {
                    ushort field_metadata =         udtDefinitionTag.GetUInt16(offset);
                    offset += 2;

                    ushort field_element_type =     udtDefinitionTag.GetUInt16(offset);
                    offset += 2;
                    
                    ushort field_offset =           udtDefinitionTag.GetUInt16(offset);
                    offset += 4;

                    var field = new UdtFieldEntry()
                    {
                        Offset = field_offset,
                        Metadata = field_metadata,
                        Type = field_element_type,
                    };

                    template.Fields[field_index] = field;
                }


                var name_str = udtDefinitionTag.GetString(offset).Split(';')[0];
                template.Name = name_str;

                offset += udtDefinitionTag.GetStringTotalLength(offset);

                for (int field_index = 0; field_index < num_members; field_index++)
                {
                    template.Fields[field_index].Name = udtDefinitionTag.GetString(offset);
                    offset += udtDefinitionTag.GetStringTotalLength(offset);
                }

                udtDefinitions.Add(template);

            }



            foreach (var udt in udtDefinitions)
            {

                Console.WriteLine($"Id={udt.Id} Name={udt.Name} NumFields={udt.NumFields}");
                foreach (var f in udt.Fields)
                    Console.WriteLine($"    Name={f.Name} Offset={f.Offset} Metadata={f.Metadata} Type={f.Type}");


            }

            Console.WriteLine("SUCESS");

        }
    }
}
