using System;

namespace libplctag
{

    public class TooLargeException : LibPlcTagException
    {
        public TooLargeException()
        {
        }

        public TooLargeException(string message)
            : base(message)
        {
        }

        public TooLargeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
