using System;

namespace libplctag
{

    public class BadGatewayException : LibPlcTagException
    {
        public BadGatewayException()
        {
        }

        public BadGatewayException(string message)
            : base(message)
        {
        }

        public BadGatewayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
