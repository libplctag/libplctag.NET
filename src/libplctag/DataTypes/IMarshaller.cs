namespace libplctag.DataTypes
{
    public interface IMarshaller<T>
    {
        /// <summary>
        /// You can define different marshalling behaviour for different types
        /// The PlcType is injected during Marshaller instantiation, and
        /// will be available to you in your marshalling logic
        /// </summary>
        PlcType PlcType { get; set; }


    /// <summary>
    /// Provide an integer value for ElementSize if you
    /// want to pass this into the tag constructor
    /// </summary>
    int? ElementSize { get; }
    //PlcType PlcType { get; set; }

    ///// <summary>
    ///// This will return the number of items defined in a tag 
    ///// </summary>
    int[] GetArrayLength(Tag tag);

    /// <summary>
    /// This is used to convert the number of array elements
    /// into the raw element count, which is used by the library.
    /// Most of the time, this will be the same value, but occasionally
    /// it is not (e.g. BOOL arrays).
    /// </summary>
    int[] SetArrayLength(int[] elementCount);

    T Decode(Tag tag);
    //T DecodeOne(Tag tag, int offset, out int elementSize);
    void Encode(Tag tag, T value);
    //void EncodeOne(Tag tag, int offset, out int elementSize, T value);
}
}