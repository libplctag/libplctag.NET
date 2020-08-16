namespace libplctag.DataTypes
{
    public class LintMarshaller : Marshaller<long>
    {
        override public int? ElementSize => 8;

        override public long Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt64(offset * ElementSize.Value);
        }

        override public void Encode(Tag tag, int offset, out int elementSize, long value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt64(offset, value);
        }

    }
}
