using System;

namespace libplctag
{

    public class NotAllowedException : LibPlcTagException
    {
        public NotAllowedException()
        {
        }

        public NotAllowedException(string message)
            : base(message)
        {
        }

        public NotAllowedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
