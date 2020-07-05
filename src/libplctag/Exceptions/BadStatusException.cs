using System;

namespace libplctag
{

    public class BadStatusException : Exception
    {
        public BadStatusException()
        {
        }

        public BadStatusException(string message)
            : base(message)
        {
        }

        public BadStatusException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
