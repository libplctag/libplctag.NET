using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeUDINT : IPlcType<uint>
    {
        public int ElementSize { get; } = 4;

        public byte CipCode { get; } = 0xC8;

        public uint Decode(Tag tag)
        {
            return tag.GetUInt32(0);
        }

        public void Encode(Tag tag, uint t)
        {
            tag.SetUInt32(0, t);
        }
    }
}
