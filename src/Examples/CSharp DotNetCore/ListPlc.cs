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

    class ListPlc
    {
        static public void Run()
        {

            Console.WriteLine("Controller Tags");
            Console.WriteLine("===============");

            var tags = new Tag<TagInfoPlcMapper, TagInfo[]>()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Name = "@tags",
                Timeout = TimeSpan.FromSeconds(10)
            };

            tags.Read();

            foreach (var tag in tags.Value)
                Console.WriteLine($"Id={tag.Id} Name={tag.Name} Type={tag.Type} Length={tag.Length}");




            Console.WriteLine();
            Console.WriteLine("Programs");
            Console.WriteLine("========");

            foreach (var tag in tags.Value)
            {
                if (TagIsProgram(tag, out string programTagListingPrefix))
                {
                    var programsTag = new Tag<TagInfoPlcMapper, TagInfo[]>()
                    {
                        Gateway = "192.168.0.10",
                        Path = "1,0",
                        PlcType = PlcType.ControlLogix,
                        Protocol = Protocol.ab_eip,
                        Name = $"{programTagListingPrefix}.@tags",
                        Timeout = TimeSpan.FromSeconds(10)
                    };

                    programsTag.Read();

                    Console.WriteLine(programTagListingPrefix);
                    foreach (var program in programsTag.Value)
                        Console.WriteLine($"    {program.Name}");

                }
            }


            Console.WriteLine();
            Console.WriteLine("UDTs");
            Console.WriteLine("====");

            var uniqueUdtTypeIds = tags.Value
                .Where(tagInfo => TagIsUdt(tagInfo))
                .Select(tagInfo => GetUdtId(tagInfo))
                .Distinct();

            foreach (var udtId in uniqueUdtTypeIds)
            {
                var udtTag = new Tag<UdtInfoPlcMapper, UdtInfo>()
                {
                    Gateway = "192.168.0.10",
                    Path = "1,0",
                    PlcType = PlcType.ControlLogix,
                    Protocol = Protocol.ab_eip,
                    Name = $"@udt/{udtId}",
                };

                udtTag.Read();
                var udt = udtTag.Value;

                Console.WriteLine($"Id={udt.Id} Name={udt.Name} NumFields={udt.NumFields} Size={udt.Size}");
                foreach (var f in udt.Fields)
                    Console.WriteLine($"    Name={f.Name} Offset={f.Offset} Metadata={f.Metadata} Type={f.Type}");
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

        static bool TagIsProgram(TagInfo tag, out string prefix)
        {
            if (tag.Name.StartsWith("Program:"))
            {
                prefix = tag.Name;
                return true;
            }
            else
            {
                prefix = string.Empty;
                return false;
            }
        }
    }
}
