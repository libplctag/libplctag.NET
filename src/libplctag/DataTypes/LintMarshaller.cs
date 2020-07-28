namespace libplctag.DataTypes
{
    public class LintMarshaller : IMarshaller<long>
    {
        private const int ElementSize = 8;
        public PlcType PlcType { get; set; }
        public long Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetInt64(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, long value)
        {
            elementSize = ElementSize;
            tag.SetInt64(offset, value);
        }
    }
}
