using System;

namespace libplctag
{

    public class NoDataException : LibPlcTagException
    {
        public NoDataException()
        {
        }

        public NoDataException(string message)
            : base(message)
        {
        }

        public NoDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
