using System;

namespace libplctag
{

    public class NoResourcesException : LibPlcTagException
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
