using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeULINT : IPlcType<ulong>
    {
        public int ElementSize { get; } = 8;

        public byte CipCode { get; } = 0xC9;

        public ulong Decode(Tag tag)
        {
            return tag.GetUInt64(0);
        }

        public void Encode(Tag tag, ulong t)
        {
            tag.SetUInt64(0, t);
        }
    }
}
