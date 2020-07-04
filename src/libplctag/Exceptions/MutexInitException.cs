using System;

namespace libplctag
{

    public class MutexInitException : LibPlcTagException
    {
        public MutexInitException()
        {
        }

        public MutexInitException(string message)
            : base(message)
        {
        }

        public MutexInitException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
