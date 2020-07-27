namespace libplctag.DataTypes
{
    public class IntMarshaller : IMarshaller<short>
    {
        private const int ElementSize = 2;
        public PlcType PlcType { get; set; }
        public short Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetInt16(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, short value)
        {
            elementSize = ElementSize;
            tag.SetInt16(offset, value);
        }
    }
}
