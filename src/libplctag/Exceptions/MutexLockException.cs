using System;

namespace libplctag
{

    public class MutexLockException : LibPlcTagException
    {
        public MutexLockException()
        {
        }

        public MutexLockException(string message)
            : base(message)
        {
        }

        public MutexLockException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
