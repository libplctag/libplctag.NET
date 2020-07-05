using System;

namespace libplctag
{

    public class NotImplementedException : Exception
    {
        public NotImplementedException()
        {
        }

        public NotImplementedException(string message)
            : base(message)
        {
        }

        public NotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
