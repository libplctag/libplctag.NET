using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeUINT : IPlcType<ushort>
    {
        public int ElementSize { get; } = 2;

        public byte CipCode { get; } = 0xC7;

        public ushort Decode(Tag tag)
        {
            return tag.GetUInt16(0);
        }

        public void Encode(Tag tag, ushort t)
        {
            tag.SetUInt16(0, t);
        }
    }
}
