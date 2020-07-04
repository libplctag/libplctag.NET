using System;

namespace libplctag
{

    public class BadConnectionException : LibPlcTagException
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
