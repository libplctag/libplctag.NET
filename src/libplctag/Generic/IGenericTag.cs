namespace libplctag.Generic
{
    public interface IGenericTag<T> : ITag
    {
        T Value { get; set; }
        byte CipCode { get; }
        void Read(int timeout);
        void Write(int timeout);
    }
}