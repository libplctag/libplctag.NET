namespace libplctag
{
    public interface IMarshaller<DotNetType>
    {
        int ElementSize { get; }
        void Encode(Tag tag, int index, DotNetType t);
        DotNetType Decode(Tag tag, int index);
    }
}
