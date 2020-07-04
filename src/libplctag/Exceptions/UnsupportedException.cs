using System;

namespace libplctag
{

    public class UnsupportedException : LibPlcTagException
    {
        public UnsupportedException()
        {
        }

        public UnsupportedException(string message)
            : base(message)
        {
        }

        public UnsupportedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
