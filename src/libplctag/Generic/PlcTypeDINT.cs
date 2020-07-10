using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic
{
    public class PlcTypeDINT : IPlcType<int>
    {
        public int ElementSize { get; } = 4;

        public byte CipCode { get; } = 0xC4;

        public int Decode(Tag tag)
        {
            return tag.GetInt32(0);
        }

        public void Encode(Tag tag, int t)
        {
            tag.SetInt32(0, t);
        }
    }
}
