namespace libplctag.DataTypes
{
    public class SintMarshaller : IMarshaller<sbyte>
    {
        public int? ElementSize => 1;
        public PlcType PlcType { get; set; }
        public sbyte Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt8(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, sbyte value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt8(offset, value);
        }
    }
}
