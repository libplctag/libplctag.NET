namespace libplctag.DataTypes
{
    public class DINTIntMarshaller : IMarshaller<int>
    {
        public int ElementSize => 4;
        public int Decode(Tag tag, int index) => tag.GetInt32(ElementSize * index);
        public void Encode(Tag tag, int index, int value) => tag.SetInt32(ElementSize * index, value);
    }
}
