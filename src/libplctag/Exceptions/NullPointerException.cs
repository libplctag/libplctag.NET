using System;

namespace libplctag
{

    public class NullPointerException : Exception
    {
        public NullPointerException()
        {
        }

        public NullPointerException(string message)
            : base(message)
        {
        }

        public NullPointerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
