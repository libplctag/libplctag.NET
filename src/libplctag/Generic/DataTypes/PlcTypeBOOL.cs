using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeBOOL : IPlcType<bool>
    {
        public int ElementSize { get; } = 1;

        public byte CipCode { get; } = 0xC1;

        public bool Decode(Tag tag)
        {
            //TODO: Check that this is the correct read/design
            return tag.GetBit(0) == 1;
        }

        public void Encode(Tag tag, bool t)
        {
            tag.SetBit(0, t ? (byte)1 : (byte)0);
        }
    }
}
