namespace libplctag.DataTypes
{
    public class RealMarshaller : IMarshaller<float>
    {
        private const int ElementSize = 4;
        public PlcType PlcType { get; set; }
        public float Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize;
            return tag.GetFloat32(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, float value)
        {
            elementSize = ElementSize;
            tag.SetFloat32(offset, value);
        }
    }
}
