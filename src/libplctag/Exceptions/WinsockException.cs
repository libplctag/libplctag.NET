using System;

namespace libplctag
{

    public class WinsockException : LibPlcTagException
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
