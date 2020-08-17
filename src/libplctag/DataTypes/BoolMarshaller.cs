using System;

namespace libplctag.DataTypes
{

    public class BoolMarshaller : IMarshaller<bool>, IMarshaller<bool[]>
    {
        public int? ElementSize => throw new NotImplementedException();

        public PlcType PlcType { get; set; }

        public int? SetArrayLength(int? elementCount) => (int)Math.Ceiling((double)elementCount.Value / 32.0);
        public int? GetArrayLength(Tag tag) => tag.ElementCount.Value * 32;

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
