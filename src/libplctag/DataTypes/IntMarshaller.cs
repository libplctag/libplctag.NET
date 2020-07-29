namespace libplctag.DataTypes
{
    public class IntMarshaller : IMarshaller<short>
    {
        public int? ElementSize => 8;
        public PlcType PlcType { get; set; }
        public short Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt16(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, short value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt16(offset, value);
        }
    }
}
