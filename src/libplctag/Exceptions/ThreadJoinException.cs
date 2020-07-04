using System;

namespace libplctag
{

    public class ThreadJoinException : LibPlcTagException
    {
        public ThreadJoinException()
        {
        }

        public ThreadJoinException(string message)
            : base(message)
        {
        }

        public ThreadJoinException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
