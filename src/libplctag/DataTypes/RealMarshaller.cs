namespace libplctag.DataTypes
{
    public class RealMarshaller : Marshaller<float>, IMarshaller<float>, IMarshaller<float[]>
    {

        override public int? ElementSize => 4;

        override public float Decode(Tag tag, int offset) => tag.GetFloat32(offset);

        override public void Encode(Tag tag, int offset, float value) => tag.SetFloat32(offset, value);

    }
}
