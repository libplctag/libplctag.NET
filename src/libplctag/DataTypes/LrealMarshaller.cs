namespace libplctag.DataTypes
{
    public class LrealMarshaller : Marshaller<double>
    {

        override public int? ElementSize => 8;

        override public double Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetFloat64(offset * ElementSize.Value);
        }

        override public void Encode(Tag tag, int offset, out int elementSize, double value)
        {
            elementSize = ElementSize.Value;
            tag.SetFloat64(offset, value);
        }

    }
}
