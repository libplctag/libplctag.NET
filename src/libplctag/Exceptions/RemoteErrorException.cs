using System;

namespace libplctag
{

    public class RemoteErrorException : LibPlcTagException
    {
        public RemoteErrorException()
        {
        }

        public RemoteErrorException(string message)
            : base(message)
        {
        }

        public RemoteErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
