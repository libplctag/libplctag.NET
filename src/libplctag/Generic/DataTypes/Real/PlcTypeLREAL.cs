using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeLREAL : IPlcType<double>
    {
        public int ElementSize { get; } = 8;

        public byte CipCode { get; } = 0xCB;

        public double Decode(Tag tag)
        {
            return tag.GetFloat64(0);
        }

        public void Encode(Tag tag, double t)
        {
            tag.SetFloat64(0, t);
        }
    }
}
