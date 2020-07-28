namespace libplctag.DataTypes
{
    public class LrealMarshaller : IMarshaller<double>
    {
        private const int ElementSize = 4;
        public PlcType PlcType { get; set; }
        public double Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetFloat64(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, double value)
        {
            elementSize = ElementSize;
            tag.SetFloat64(offset, value);
        }
    }
}
