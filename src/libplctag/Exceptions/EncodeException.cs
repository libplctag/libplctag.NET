using System;

namespace libplctag
{

    public class EncodeException : Exception
    {
        public EncodeException()
        {
        }

        public EncodeException(string message)
            : base(message)
        {
        }

        public EncodeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
