using System;

namespace libplctag
{

    public class BadParameterException : LibPlcTagException
    {
        public BadParameterException()
        {
        }

        public BadParameterException(string message)
            : base(message)
        {
        }

        public BadParameterException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
