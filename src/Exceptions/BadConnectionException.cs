using System;

namespace libplctag
{

    public class BadConnectionException : Exception
    {
        public BadConnectionException()
        {
        }

        public BadConnectionException(string message)
            : base(message)
        {
        }

        public BadConnectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
