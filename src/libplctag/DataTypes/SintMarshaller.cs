namespace libplctag.DataTypes
{
    public class SintMarshaller : Marshaller<sbyte>, IMarshaller<sbyte>, IMarshaller<sbyte[]>
    {

        override public int? ElementSize => 1;

        override public sbyte Decode(Tag tag, int offset) => tag.GetInt8(offset);

        override public void Encode(Tag tag, int offset, sbyte value) => tag.SetInt8(offset, value);

    }
}
