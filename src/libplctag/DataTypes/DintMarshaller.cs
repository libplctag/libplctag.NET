namespace libplctag.DataTypes
{
    public class DintMarshaller : IMarshaller<int>
    {
        private const int ElementSize = 4;
        public PlcType PlcType { get; set; }
        public int Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetInt32(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, int value)
        {
            elementSize = ElementSize;
            tag.SetInt32(offset, value);
        }
    }
}
