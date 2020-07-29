namespace libplctag.DataTypes
{
    public class LrealMarshaller : IMarshaller<double>
    {
        public int? ElementSize => 8;
        public PlcType PlcType { get; set; }
        public double Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetFloat64(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, double value)
        {
            elementSize = ElementSize.Value;
            tag.SetFloat64(offset, value);
        }
    }
}
