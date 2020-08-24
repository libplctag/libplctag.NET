namespace libplctag.DataTypes
{
    public interface IPlcMapper<T>
    {
        /// <summary>
        /// You can define different marshalling behaviour for different types
        /// The PlcType is injected during PlcMapper instantiation, and
        /// will be available to you in your marshalling logic
        /// </summary>
        PlcType PlcType { get; set; }


        /// <summary>
        /// Provide an integer value for ElementSize if you
        /// want to pass this into the tag constructor
        /// </summary>
        int? ElementSize { get; }

        /// <summary>
        /// The dimensions of the array. Null if not an array.
        /// </summary>
        int[] ArrayDimensions { get; set; }

        /// <summary>
        /// This is used to convert the number of array elements
        /// into the raw element count, which is used by the library.
        /// Most of the time, this will be the dimensions multiplied, but occasionally
        /// it is not (e.g. BOOL arrays).
        /// </summary>
        int? GetElementCount();

        T Decode(Tag tag);
        //T DecodeOne(Tag tag, int offset, out int elementSize);
        void Encode(Tag tag, T value);
        //void EncodeOne(Tag tag, int offset, out int elementSize, T value);
    }
}