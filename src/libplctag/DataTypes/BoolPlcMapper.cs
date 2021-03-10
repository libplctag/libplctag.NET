using libplctag.DataTypes.Extensions;
using System;
using System.Linq;

namespace libplctag.DataTypes
{
    public class BoolPlcMapper : IPlcMapper<bool>
    {
        public int? ElementSize => 1;

        public PlcType PlcType { get; set; }
        public int[] ArrayDimensions { get; set; }

        public int? GetElementCount() => 1;

        bool IPlcMapper<bool>.Decode(Tag tag) => tag.GetUInt8(0) == 255;

        void IPlcMapper<bool>.Encode(Tag tag, bool value) => tag.SetUInt8(0, value == true ? (byte)255 : (byte)0 );
    }
}
