using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeINT : IPlcType<short>
    {
        public int ElementSize { get; } = 2;

        public byte CipCode { get; } = 0xC3;

        public short Decode(Tag tag)
        {
            return tag.GetInt16(0);
        }

        public void Encode(Tag tag, short t)
        {
            tag.SetInt16(0, t);
        }
    }
}
