using System;

namespace libplctag
{

    public class BadReplyException : Exception
    {
        public BadReplyException()
        {
        }

        public BadReplyException(string message)
            : base(message)
        {
        }

        public BadReplyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
