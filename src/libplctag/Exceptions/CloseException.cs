using System;

namespace libplctag
{

    public class CloseException : Exception
    {
        public CloseException()
        {
        }

        public CloseException(string message)
            : base(message)
        {
        }

        public CloseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
