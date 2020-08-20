using System;
using System.Collections.Generic;

namespace libplctag.DataTypes
{
    public abstract class Marshaller<T>: IMarshaller<T>, IMarshaller<T[]>
    {
        public PlcType PlcType { get; set; }

        abstract public int? ElementSize { get; }

        virtual public int? SetArrayLength(int? elementCount) => elementCount;

        virtual public int? GetArrayLength(Tag tag) => tag.ElementCount;


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

        virtual public T Decode(Tag tag) => Decode(tag, 0);
        public abstract T Decode(Tag tag, int offset);


        virtual public void Encode(Tag tag, T value) => Encode(tag, 0, value);
        public abstract void Encode(Tag tag, int offset, T value);

        virtual public void Encode(Tag tag, T[] value) => EncodeArray(tag, value);

        T[] IMarshaller<T[]>.Decode(Tag tag) => DecodeArray(tag);
    }

}
