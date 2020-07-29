namespace libplctag.DataTypes
{
    public class LintMarshaller : IMarshaller<long>
    {
        public int? ElementSize => 8;

        public PlcType PlcType { get; set; }
        public long Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt64(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, long value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt64(offset, value);
        }
    }
}
