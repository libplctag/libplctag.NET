namespace libplctag
{
    public interface IMarshaller<T>
    {
        int ElementSize { get; }
        PlcType PlcType { get; set; }
        void Encode(Tag tag, int offset, T t);
        T Decode(Tag tag, int offset);
    }
}
