using System;

namespace libplctag.DataTypes
{

    public class BoolMarshaller : Marshaller<bool>
    {

        public override int? ElementCountFromArrayLength(int? elementCount) => (int)Math.Ceiling((double)elementCount.Value / 32.0);
        public override int? ArrayLengthFromElementCount(int? arrayCount) => arrayCount.Value * 32;

        override public bool[] Decode(Tag tag)
        {

            var buffer = new bool[tag.ElementCount.Value * 32];
            for (int ii = 0; ii < tag.ElementCount.Value * 32; ii++)
            {
                buffer[ii] = tag.GetBit(ii);
            }
            return buffer;
        }



        override public void Encode(Tag tag, bool[] value)
        {
            for (int ii = 0; ii < tag.ElementCount.Value * 32; ii++)
            {
                tag.SetBit(ii, value[ii]);
            }
        }


        public override bool DecodeOne(Tag tag, int offset, out int elementSize)
        {
            // This method is not used because we provided new implementations for Decode()
            throw new System.NotImplementedException();
        }

        public override void EncodeOne(Tag tag, int offset, out int elementSize, bool value)
        {
            // This method is not used because we provided new implementations for Decode()
            throw new System.NotImplementedException();
        }

    }
}
