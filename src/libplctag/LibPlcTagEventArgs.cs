using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag
{
    public class LibPlcTagEventArgs : EventArgs
    {
        public Status Status { get; set; }
    }
}
