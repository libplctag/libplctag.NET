namespace libplctag.DataTypes
{
    public class LrealMarshaller : Marshaller<double>, IMarshaller<double>, IMarshaller<double[]>
    {

        override public int? ElementSize => 8;

        override public double Decode(Tag tag, int offset) => tag.GetFloat64(offset);

        override public void Encode(Tag tag, int offset, double value)=> tag.SetFloat64(offset, value);
    }
}
