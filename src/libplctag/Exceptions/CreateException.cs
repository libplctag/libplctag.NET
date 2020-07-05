using System;

namespace libplctag
{

    public class CreateException : Exception
    {
        public CreateException()
        {
        }

        public CreateException(string message)
            : base(message)
        {
        }

        public CreateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
