using System;

namespace libplctag
{

    public class NoMatchException : Exception
    {
        public NoMatchException()
        {
        }

        public NoMatchException(string message)
            : base(message)
        {
        }

        public NoMatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
