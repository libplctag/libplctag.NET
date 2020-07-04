using System;

namespace libplctag
{

    public class TimeoutException : LibPlcTagException
    {
        public TimeoutException()
        {
        }

        public TimeoutException(string message)
            : base(message)
        {
        }

        public TimeoutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
