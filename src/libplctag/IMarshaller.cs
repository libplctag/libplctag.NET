namespace libplctag
{
    public interface IMarshaller<DotNetType>
    {
        int ElementSize { get; }
        void Encode(Tag tag, int offset, DotNetType t);
        DotNetType Decode(Tag tag, int offset);
    }
}
