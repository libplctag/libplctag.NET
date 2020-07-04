using System;

namespace libplctag
{

    public class WriteException : LibPlcTagException
    {
        public WriteException()
        {
        }

        public WriteException(string message)
            : base(message)
        {
        }

        public WriteException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
