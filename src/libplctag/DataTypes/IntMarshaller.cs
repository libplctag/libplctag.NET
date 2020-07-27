namespace libplctag.DataTypes
{
    public class IntMarshaller : IMarshaller<short>
    {
        public int ElementSize => 2;
        public PlcType PlcType { get; set; }
        public short Decode(Tag tag, int offset) => tag.GetInt16(offset);
        public void Encode(Tag tag, int offset, short value) => tag.SetInt16(offset, value);
    }
}
