using System;

namespace libplctag
{

    public class BusyException : LibPlcTagException
    {
        public BusyException()
        {
        }

        public BusyException(string message)
            : base(message)
        {
        }

        public BusyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
