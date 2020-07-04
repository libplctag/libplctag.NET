using System;

namespace libplctag
{

    public class AbortException : LibPlcTagException
    {
        public AbortException()
        {
        }

        public AbortException(string message)
            : base(message)
        {
        }

        public AbortException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
