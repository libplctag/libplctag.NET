using System;

namespace libplctag
{

    public class TooSmallException : Exception
    {
        public TooSmallException()
        {
        }

        public TooSmallException(string message)
            : base(message)
        {
        }

        public TooSmallException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
