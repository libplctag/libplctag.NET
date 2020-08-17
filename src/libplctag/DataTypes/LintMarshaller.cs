namespace libplctag.DataTypes
{
    public class LintMarshaller : Marshaller<long>, IMarshaller<long>, IMarshaller<long[]>
    {
        public override int? ElementSize => 8;

        override public long Decode(Tag tag, int offset) => tag.GetInt64(offset);

        override public void Encode(Tag tag, int offset, long value) => tag.SetInt64(offset, value);

    }
}
