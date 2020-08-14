namespace libplctag.DataTypes
{
    public interface IMarshaller<T>
    {
        int? ElementSize { get; }
        //PlcType PlcType { get; set; }

        int? ArrayLengthFromElementCount(int? arrayLength);
        T Decode(Tag tag);
        //T DecodeOne(Tag tag, int offset, out int elementSize);
        int? ElementCountFromArrayLength(int? elementCount);
        void Encode(Tag tag, T value);
        //void EncodeOne(Tag tag, int offset, out int elementSize, T value);
    }
}