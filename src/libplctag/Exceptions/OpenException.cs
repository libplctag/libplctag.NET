using System;

namespace libplctag
{

    public class OpenException : Exception
    {
        public OpenException()
        {
        }

        public OpenException(string message)
            : base(message)
        {
        }

        public OpenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
