using System;

namespace libplctag
{

    public class BadDeviceException : LibPlcTagException
    {
        public BadDeviceException()
        {
        }

        public BadDeviceException(string message)
            : base(message)
        {
        }

        public BadDeviceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
