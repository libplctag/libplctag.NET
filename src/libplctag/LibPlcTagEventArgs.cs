using System;

namespace libplctag
{
    public class LibPlcTagEventArgs : EventArgs
    {
        public StatusCode Status { get; set; }
    }

}