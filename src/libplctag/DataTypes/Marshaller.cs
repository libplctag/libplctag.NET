using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace libplctag.DataTypes
{
    public abstract class Marshaller<T>: IMarshaller<T>, IMarshaller<T[]>, IMarshaller<T[,]>
    {
        public PlcType PlcType { get; set; }

        abstract public int? ElementSize { get; }

        public int[] ArrayDimensions { get; set; }

        //Multiply all the dimensions to get total elements
        virtual public int? GetElementCount() => ArrayDimensions?.Aggregate(1, (x, y) => x * y);

        virtual protected T[] DecodeArray(Tag tag)
        {
            if (ElementSize is null)
            {
                throw new ArgumentNullException($"{nameof(ElementSize)} cannot be null for array decoding");
            }

            var buffer = new List<T>();

            var tagSize = tag.GetSize();

            int offset = 0;
            while (offset < tagSize)
            {
                buffer.Add(Decode(tag, offset));
                offset += ElementSize.Value;
            }

            return buffer.ToArray();

        }

        virtual protected void EncodeArray(Tag tag, T[] values)
        {
            if (ElementSize is null)
            {
                throw new ArgumentNullException($"{nameof(ElementSize)} cannot be null for array encoding");
            }

            int offset = 0;
            foreach (var item in values)
            {
                Encode(tag, offset, item);
                offset += ElementSize.Value;
            }
        }

        private static T[,] To2DArray(T[] input, int height, int width)
        {
            T[,] output = new T[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }

        private static T[] To1DArray(T[,] input)
        {
            // Step 1: get total size of 2D array, and allocate 1D array.
            int size = input.Length;
            T[] result = new T[size];

            // Step 2: copy 2D array elements into a 1D array.
            int write = 0;
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                for (int z = 0; z <= input.GetUpperBound(1); z++)
                {
                    result[write++] = input[i, z];
                }
            }
            // Step 3: return the new array.
            return result;
        }

        virtual public T Decode(Tag tag) => Decode(tag, 0);
        public abstract T Decode(Tag tag, int offset);


        virtual public void Encode(Tag tag, T value) => Encode(tag, 0, value);
        public abstract void Encode(Tag tag, int offset, T value);

        virtual public void Encode(Tag tag, T[] value) => EncodeArray(tag, value);

        T[] IMarshaller<T[]>.Decode(Tag tag) => DecodeArray(tag);


        T[,] IMarshaller<T[,]>.Decode(Tag tag)
        {
            var array1D = DecodeArray(tag);
            return To2DArray(array1D, ArrayDimensions[0], ArrayDimensions[1]);
        }

        void IMarshaller<T[,]>.Encode(Tag tag, T[,] value) => EncodeArray(tag, To1DArray(value));
    }

}
