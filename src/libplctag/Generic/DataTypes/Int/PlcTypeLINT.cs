using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeLINT : IPlcType<long>
    {
        public int ElementSize { get; } = 8;

        public byte CipCode { get; } = 0xC5;

        public long Decode(Tag tag)
        {
            return tag.GetInt64(0);
        }

        public void Encode(Tag tag, long t)
        {
            tag.SetInt64(0, t);
        }
    }
}
