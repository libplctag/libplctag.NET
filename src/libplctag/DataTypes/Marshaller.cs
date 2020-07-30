using System.Collections.Generic;

namespace libplctag.DataTypes
{
    public abstract class Marshaller<T>
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
        virtual public int? ElementCountFromArrayLength(int? elementCount) => elementCount;





        /// <summary>
        /// The opposite of ElementCountFromArrayLength
        /// </summary>
        virtual public int? ArrayLengthFromElementCount(int? arrayLength) => arrayLength;






        virtual public T[] Decode(Tag tag)
        {

            var buffer = new List<T>();

            var tagSize = tag.GetSize();

            int offset = 0;
            while (offset < tagSize)
            {
                buffer.Add(DecodeOne(tag, offset, out int elementSize));
                offset += elementSize;
            }

            return buffer.ToArray();

        }







        virtual public void Encode(Tag tag, T[] values)
        {
            int offset = 0;
            foreach (var item in values)
            {
                EncodeOne(tag, offset, out int elementSize, item);
                offset += elementSize;
            }
        }





        public abstract T DecodeOne(Tag tag, int offset, out int elementSize);

        public abstract void EncodeOne(Tag tag, int offset, out int elementSize, T value);

    }

}
