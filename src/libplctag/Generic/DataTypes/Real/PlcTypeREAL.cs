using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic.DataTypes
{
    public class PlcTypeREAL : IPlcType<float>
    {
        public int ElementSize { get; } = 4;

        public byte CipCode { get; } = 0xCA;

        public float Decode(Tag tag)
        {
            return tag.GetFloat32(0);
        }

        public void Encode(Tag tag, float t)
        {
            tag.SetFloat32(0, t);
        }
    }
}
