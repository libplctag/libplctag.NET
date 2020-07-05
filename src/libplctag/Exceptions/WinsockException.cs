using System;

namespace libplctag
{

    public class WinsockException : Exception
    {
        public WinsockException()
        {
        }

        public WinsockException(string message)
            : base(message)
        {
        }

        public WinsockException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
