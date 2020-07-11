using System;

namespace libplctag
{
    public class LibPlcTagEventArgs : EventArgs
    {
        public StatusCode StatusCode { get; set; }
    }

}