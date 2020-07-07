using System;

namespace libplctag
{

    public class PartialException : Exception
    {
        public PartialException()
        {
        }

        public PartialException(string message)
            : base(message)
        {
        }

        public PartialException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
