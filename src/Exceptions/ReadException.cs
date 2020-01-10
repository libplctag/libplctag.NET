using System;

namespace libplctag
{

    public class ReadException : Exception
    {
        public ReadException()
        {
        }

        public ReadException(string message)
            : base(message)
        {
        }

        public ReadException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
