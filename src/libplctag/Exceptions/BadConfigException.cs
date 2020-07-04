using System;

namespace libplctag
{

    public class BadConfigException : LibPlcTagException
    {
        public BadConfigException()
        {
        }

        public BadConfigException(string message)
            : base(message)
        {
        }

        public BadConfigException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
