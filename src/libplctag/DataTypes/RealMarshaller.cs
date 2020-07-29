namespace libplctag.DataTypes
{
    public class RealMarshaller : IMarshaller<float>
    {
        public int? ElementSize => 4;
        public PlcType PlcType { get; set; }
        public float Decode(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            return tag.GetFloat32(offset);
        }

        public void Encode(Tag tag, int offset, out int elementSize, float value)
        {
            elementSize = ElementSize.Value;
            tag.SetFloat32(offset, value);
        }
    }
}
