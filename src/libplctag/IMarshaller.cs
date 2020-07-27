namespace libplctag
{
    public interface IMarshaller<T>
    {
        PlcType PlcType { get; set; }
        void Encode(Tag tag, int offset, out int elementSize, T t);
        T Decode(Tag tag, int offset, out int elementSize);
    }
}
