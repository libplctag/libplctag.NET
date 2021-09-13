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

    class ListUdtDefinitions
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

            
            var uniqueUdtTypeIds = tags.Value
                .Where(tagInfo => TagIsUdt(tagInfo))
                .Select(tagInfo => GetUdtId(tagInfo))
                .Distinct();

            
            var udtDefinitions = new List<UdtInfo>();
            foreach (var udtId in uniqueUdtTypeIds)
            {
                var udtDefinitionTag = new Tag<UdtInfoPlcMapper, UdtInfo>()
                {
                    Gateway = "192.168.0.10",
                    Path = "1,0",
                    PlcType = PlcType.ControlLogix,
                    Protocol = Protocol.ab_eip,
                    Name = $"@udt/{udtId}",
                };

                udtDefinitionTag.Read();
                udtDefinitions.Add(udtDefinitionTag.Value);
            }

            foreach (var udt in udtDefinitions)
            {
                Console.WriteLine($"Id={udt.Id} Name={udt.Name} NumFields={udt.NumFields}");
                foreach (var f in udt.Fields)
                    Console.WriteLine($"    Name={f.Name} Offset={f.Offset} Metadata={f.Metadata} Type={f.Type} Size={f.Size}");
            }

        }

        static bool TagIsUdt(TagInfo tag)
        {
            const ushort TYPE_IS_STRUCT = 0x8000;
            const ushort TYPE_IS_SYSTEM = 0x1000;

            return ((tag.Type & TYPE_IS_STRUCT) != 0) && !((tag.Type & TYPE_IS_SYSTEM) != 0);
        }

        static int GetUdtId(TagInfo tag)
        {
            const ushort TYPE_UDT_ID_MASK = 0x0FFF;
            return tag.Type & TYPE_UDT_ID_MASK;
        }
    }
}
