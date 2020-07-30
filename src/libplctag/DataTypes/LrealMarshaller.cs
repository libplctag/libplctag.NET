namespace libplctag.DataTypes
{
    public class LrealMarshaller : Marshaller<double>
    {

        override public int? ElementSize => 8;

        override public double DecodeOne(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetFloat64(offset * ElementSize.Value);
        }

        override public void EncodeOne(Tag tag, int offset, out int elementSize, double value)
        {
            elementSize = ElementSize.Value;
            tag.SetFloat64(offset, value);
        }

    }
}
