using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.Generic
{
    public interface IPlcType<T>
    {
        int ElementSize { get; }
        byte CipCode { get; }
        void Encode(Tag tag, T t);
        T Decode(Tag tag);
    }
}
