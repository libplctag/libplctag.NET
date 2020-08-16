namespace libplctag.DataTypes
{
    public class DintMarshaller : Marshaller<int>, IMarshaller<int>, IMarshaller<int[]>
    {
        override public int? ElementSize => 4;

        override public int Decode(Tag tag, int offset) => tag.GetInt32(offset);

        override public void Encode(Tag tag, int offset, int value) => tag.SetInt32(offset, value);

        int[] IMarshaller<int[]>.Decode(Tag tag)=> DecodeArray(tag, ElementSize.Value);

        void IMarshaller<int[]>.Encode(Tag tag, int[] value) => EncodeArray(tag, value, ElementSize.Value);
    }
}
