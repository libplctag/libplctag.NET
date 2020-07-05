using System;

namespace libplctag
{

    public class NoResourcesException : Exception
    {
        public NoResourcesException()
        {
        }

        public NoResourcesException(string message)
            : base(message)
        {
        }

        public NoResourcesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
