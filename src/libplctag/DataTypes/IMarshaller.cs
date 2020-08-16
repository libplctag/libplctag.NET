namespace libplctag.DataTypes
{
    public interface IMarshaller<T>
    {
        int? ElementSize { get; }
        //PlcType PlcType { get; set; }

        int? GetArrayLength(Tag tag);
        int? SetArrayLength(int? elementCount);

        T Decode(Tag tag);
        //T DecodeOne(Tag tag, int offset, out int elementSize);
        void Encode(Tag tag, T value);
        //void EncodeOne(Tag tag, int offset, out int elementSize, T value);
    }
}