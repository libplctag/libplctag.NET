namespace libplctag.DataTypes
{
    public class BoolMarshaller : IMarshaller<bool>
    {
        public int ElementSize => 1;
        public PlcType PlcType { get; set; }
        public bool Decode(Tag tag, int offset) => tag.GetBit(offset);
        public void Encode(Tag tag, int offset, bool value) => tag.SetBit(offset, value);
    }
}
