using System.Collections.Generic;

namespace libplctag.DataTypes
{
    public abstract class Marshaller<T>
    {
        public PlcType PlcType { get; set; }

        virtual public int? SetArrayLength(int? elementCount) => elementCount;

        virtual public int? GetArrayLength(Tag tag) => tag.ElementCount;


        virtual protected T[] DecodeArray(Tag tag, int elementSize)
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

        virtual protected void EncodeArray(Tag tag, T[] values, int elementSize)
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
