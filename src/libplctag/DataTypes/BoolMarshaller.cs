namespace libplctag.DataTypes
{
    public class BoolMarshaller : IMarshaller<bool>
    {

        public int? ElementSize => 1;
        public PlcType PlcType { get; set; }
        public bool Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetBit(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, bool value)
        {
            elementSize = ElementSize.Value;
            tag.SetBit(offset, value);
        }
    }
}
