using System;

namespace libplctag
{

    public class BadDataException : Exception
    {
        public BadDataException()
        {
        }

        public BadDataException(string message)
            : base(message)
        {
        }

        public BadDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
