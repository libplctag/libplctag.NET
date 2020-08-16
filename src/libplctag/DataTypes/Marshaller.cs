using System.Collections.Generic;

namespace libplctag.DataTypes
{
    public abstract class Marshaller<T> : IMarshaller<T>
    {


        /// <summary>
        /// You can define different marshalling behaviour for different types
        /// The PlcType is injected during Marshaller instantiation, and
        /// will be available to you in your marshalling logic
        /// </summary>
        public PlcType PlcType { get; set; }



        /// <summary>
        /// Provide an integer value for ElementSize if you
        /// want to pass this into the tag constructor
        /// </summary>
        virtual public int? ElementSize => null;




        /// <summary>
        /// This is used to convert the number of array elements
        /// into the element count, which is used by the library.
        /// Most of the time, this will be the same value, but occasionally
        /// it is not (e.g. BOOL arrays).
        /// </summary>
        virtual public int? SetArrayLength(int? elementCount) => elementCount;



        /// <summary>
        /// The opposite of ElementCountFromArrayLength
        /// </summary>
        virtual public int? GetArrayLength(Tag tag) => tag.ElementCount;


        virtual public T[] DecodeArray(Tag tag, int elementSize)
        {

            var buffer = new List<T>();

            var tagSize = tag.GetSize();

            int offset = 0;
            while (offset < tagSize)
            {
                buffer.Add(Decode(tag, offset));
                offset += elementSize;
            }

            return buffer.ToArray();

        }

        virtual public void EncodeArray(Tag tag, T[] values, int elementSize)
        {
            int offset = 0;
            foreach (var item in values)
            {
                Encode(tag, offset, item);
                offset += elementSize;
            }
        }

        virtual public T Decode(Tag tag) => Decode(tag, 0);
        public abstract T Decode(Tag tag, int offset);

        virtual public void Encode(Tag tag, T value) => Encode(tag, 0, value);
        public abstract void Encode(Tag tag, int offset, T value);

    }

}
