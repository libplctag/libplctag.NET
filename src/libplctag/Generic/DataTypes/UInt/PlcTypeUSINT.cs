using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeUSINT : IPlcType<byte>
    {
        public int ElementSize { get; } = 1;

        public byte CipCode { get; } = 0xC6;

        public byte Decode(Tag tag)
        {
            return tag.GetUInt8(0);
        }

        public void Encode(Tag tag, byte t)
        {
            tag.SetUInt8(0, t);
        }
    }
}
