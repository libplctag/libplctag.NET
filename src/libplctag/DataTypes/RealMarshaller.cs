namespace libplctag.DataTypes
{
    public class RealMarshaller : Marshaller<float>
    {

        override public int? ElementSize => 4;

        override public float DecodeOne(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetFloat32(offset* ElementSize.Value);
        }

        override public void EncodeOne(Tag tag, int offset, out int elementSize, float value)
        {
            elementSize = ElementSize.Value;
            tag.SetFloat32(offset, value);
        }

    }
}
