namespace libplctag.DataTypes
{
    public class DintMarshaller : IMarshaller<int>
    {
        public int? ElementSize => 8;
        public PlcType PlcType { get; set; }
        public int Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt32(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, int value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt32(offset, value);
        }
    }
}
