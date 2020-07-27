namespace libplctag.DataTypes
{
    public class SintMarshaller : IMarshaller<sbyte>
    {
        private const int ElementSize = 1;
        public PlcType PlcType { get; set; }
        public sbyte Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetInt8(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, sbyte value)
        {
            elementSize = ElementSize;
            tag.SetInt8(offset, value);
        }
    }
}
