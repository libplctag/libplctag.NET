using System;

namespace libplctag
{

    public class AbortException : Exception
    {
        public AbortException()
        {
        }

        public AbortException(string message)
            : base(message)
        {
        }

        public AbortException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
