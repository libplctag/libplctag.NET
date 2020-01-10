using System;

namespace libplctag
{

    public class ThreadCreateException : Exception
    {
        public ThreadCreateException()
        {
        }

        public ThreadCreateException(string message)
            : base(message)
        {
        }

        public ThreadCreateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
