namespace libplctag.DataTypes
{
    public class DintMarshaller : Marshaller<int>
    {

        override public int? ElementSize => 4;

        override public int DecodeOne(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetInt32(offset * ElementSize.Value);
        }

        override public void EncodeOne(Tag tag, int offset, out int elementSize, int value)
        {
            elementSize = ElementSize.Value;
            tag.SetInt32(offset, value);
        }


    }
}
