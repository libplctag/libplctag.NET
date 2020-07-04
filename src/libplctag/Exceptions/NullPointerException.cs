using System;

namespace libplctag
{

    public class NullPointerException : LibPlcTagException
    {
        public NullPointerException()
        {
        }

        public NullPointerException(string message)
            : base(message)
        {
        }

        public NullPointerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
