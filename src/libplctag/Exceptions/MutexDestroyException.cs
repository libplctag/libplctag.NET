using System;

namespace libplctag
{

    public class MutexDestroyException : LibPlcTagException
    {
        public MutexDestroyException()
        {
        }

        public MutexDestroyException(string message)
            : base(message)
        {
        }

        public MutexDestroyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
