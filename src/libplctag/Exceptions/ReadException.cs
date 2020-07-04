using System;

namespace libplctag
{

    public class ReadException : LibPlcTagException
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
