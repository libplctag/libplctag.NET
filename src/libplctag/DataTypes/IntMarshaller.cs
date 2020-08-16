namespace libplctag.DataTypes
{
    public class IntMarshaller : Marshaller<short>
    {
        override public int? ElementSize => 2;

        override public short Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt16(offset * ElementSize.Value);
        }

        override public void Encode(Tag tag, int offset, out int elementSize, short value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt16(offset, value);
        }
    }
}
