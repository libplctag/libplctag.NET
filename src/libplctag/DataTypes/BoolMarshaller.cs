namespace libplctag.DataTypes
{
    public class BoolMarshaller : IMarshaller<bool>
    {

        private const int ElementSize = 1;

        public PlcType PlcType { get; set; }
        public bool Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetBit(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, bool value)
        {
            elementSize = ElementSize;
            tag.SetBit(offset, value);
        }
    }
}
