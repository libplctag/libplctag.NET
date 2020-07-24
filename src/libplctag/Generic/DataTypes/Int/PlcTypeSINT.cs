using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeSINT : IPlcType<sbyte>
    {
        public int ElementSize { get; } = 1;

        public byte CipCode { get; } = 0xC2;

        public sbyte Decode(Tag tag)
        {
            return tag.GetInt8(0);
        }

        public void Encode(Tag tag, sbyte t)
        {
            tag.SetInt8(0, t);
        }
    }
}
