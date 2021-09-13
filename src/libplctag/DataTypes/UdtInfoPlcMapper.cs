using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libplctag.DataTypes
{

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

            var template_id =       tag.GetUInt16(0);
            var member_desc_size =  tag.GetUInt32(2);
            var udt_instance_size = tag.GetUInt32(6);
            var num_members =       tag.GetUInt16(10);
            var struct_handle =     tag.GetUInt16(12);

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
