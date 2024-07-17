// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDotNetCore.PlcMapper
{

    class ExampleListPlc
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

    public class TagInfo
    {
        public uint Id { get; set; }
        public ushort Type { get; set; }
        public string Name { get; set; }
        public ushort Length { get; set; }
        public uint[] Dimensions { get; set; }
    }

    public class TagInfoPlcMapper : IPlcMapper<TagInfo[]>
    {

        const int TAG_STRING_SIZE = 200;

        public PlcType PlcType { get; set; }

        //TODO: Is null appropriate since it's unknown?
        public int? ElementSize => null;
        public int[] ArrayDimensions { get => null; set => throw new NotImplementedException("This plcMapper can only be used to read Tag Information"); }

        public TagInfo Decode(Tag tag, int offset, out int elementSize)
        {

            var tagInstanceId = tag.GetUInt32(offset);
            var tagType = tag.GetUInt16(offset + 4);
            var tagLength = tag.GetUInt16(offset + 6);
            var tagArrayDims = new uint[]
            {
                tag.GetUInt32(offset + 8),
                tag.GetUInt32(offset + 12),
                tag.GetUInt32(offset + 16)
            };

            var apparentTagNameLength = (int)tag.GetUInt16(offset + 20);
            var actualTagNameLength = Math.Min(apparentTagNameLength, TAG_STRING_SIZE * 2 - 1);

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

        public TagInfo[] Decode(Tag tag)
        {
            var buffer = new List<TagInfo>();

            var tagSize = tag.GetSize();

            int offset = 0;
            while (offset < tagSize)
            {
                buffer.Add(Decode(tag, offset, out int elementSize));
                offset += elementSize;
            }

            return buffer.ToArray();
        }

        public void Encode(Tag tag, TagInfo[] value)
        {
            throw new NotImplementedException("This plcMapper can only be used to read Tag Information");
        }

        public int? GetElementCount()
        {
            //TODO: We know this value after we decode once. SHould we trigger a decode or cache the value after first decode?
            return null;
        }
    }

    public class UdtFieldInfo
    {
        public string Name { get; set; }
        public ushort Type { get; set; }
        public ushort Metadata { get; set; }
        public uint Offset { get; set; }
    }

    public class UdtInfo
    {
        public uint Size { get; set; }
        public string Name { get; set; }
        public ushort Id { get; set; }
        public ushort NumFields { get; set; }
        public ushort Handle { get; set; }
        public UdtFieldInfo[] Fields { get; set; }
    }

    public class UdtInfoPlcMapper : IPlcMapper<UdtInfo>
    {
        public PlcType PlcType { get; set; }
        //TODO: Is null appropriate since it's unknown?
        public int? ElementSize => null;
        public int[] ArrayDimensions { get => null; set => throw new NotImplementedException("This plcMapper can only be used to read"); }

        public UdtInfo Decode(Tag tag)
        {

            var template_id = tag.GetUInt16(0);
            var member_desc_size = tag.GetUInt32(2);
            var udt_instance_size = tag.GetUInt32(6);
            var num_members = tag.GetUInt16(10);
            var struct_handle = tag.GetUInt16(12);

            var udtInfo = new UdtInfo()
            {
                Fields = new UdtFieldInfo[num_members],
                NumFields = num_members,
                Handle = struct_handle,
                Id = template_id,
                Size = udt_instance_size
            };

            var offset = 14;

            for (int field_index = 0; field_index < num_members; field_index++)
            {
                ushort field_metadata = tag.GetUInt16(offset);
                offset += 2;

                ushort field_element_type = tag.GetUInt16(offset);
                offset += 2;

                ushort field_offset = tag.GetUInt16(offset);
                offset += 4;

                var field = new UdtFieldInfo()
                {
                    Offset = field_offset,
                    Metadata = field_metadata,
                    Type = field_element_type,
                };

                udtInfo.Fields[field_index] = field;
            }

            var name_str = tag.GetString(offset).Split(';')[0];
            udtInfo.Name = name_str;

            offset += tag.GetStringTotalLength(offset);

            for (int field_index = 0; field_index < num_members; field_index++)
            {
                udtInfo.Fields[field_index].Name = tag.GetString(offset);
                offset += tag.GetStringTotalLength(offset);
            }

            return udtInfo;

        }

        public void Encode(Tag tag, UdtInfo value)
        {
            throw new NotImplementedException("This plcMapper can only be used to read");
        }

        public int? GetElementCount()
        {
            //TODO: We know this value after we decode once. SHould we trigger a decode or cache the value after first decode?
            return null;
        }
    }
}
