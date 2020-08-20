using System;
using System.Linq;

namespace libplctag.DataTypes
{

    public class BoolMarshaller : IMarshaller<bool>, IMarshaller<bool[]>
    {
        public int? ElementSize => 1;

        public PlcType PlcType { get; set; }
        public int[] ArrayDimensions { get; set; }

        public int? GetElementCount()
        {
            if (ArrayDimensions == null)
                return null;

            //TODO: Test -> I'm not confident that the overall bool count is packed as a 1D array and not packed by dimension.
            //Multiply dimensions for total elements
            var totalElements = ArrayDimensions.Aggregate(1, (x, y) => x * y);
            return (int)Math.Ceiling((double)totalElements / 32.0);
        }

        public int? SetArrayLength(int? elementCount) => (int)Math.Ceiling((double)elementCount.Value / 32.0);

        bool IMarshaller<bool>.Decode(Tag tag) => tag.GetBit(0);

        void IMarshaller<bool>.Encode(Tag tag, bool value) => tag.SetBit(0, value);

        bool[] IMarshaller<bool[]>.Decode(Tag tag)
        {
            var buffer = new bool[tag.ElementCount.Value * 32];
            for (int ii = 0; ii < tag.ElementCount.Value * 32; ii++)
            {
                buffer[ii] = tag.GetBit(ii);
            }
            return buffer;
        }

        void IMarshaller<bool[]>.Encode(Tag tag, bool[] value)
        {
            for (int ii = 0; ii < tag.ElementCount.Value * 32; ii++)
            {
                tag.SetBit(ii, value[ii]);
            }
        }

    }
}
