using System;

namespace libplctag
{

    public class MutexUnlockException : Exception
    {
        public MutexUnlockException()
        {
        }

        public MutexUnlockException(string message)
            : base(message)
        {
        }

        public MutexUnlockException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
