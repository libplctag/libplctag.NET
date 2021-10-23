using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag
{
    public class TagEventArgs : EventArgs
    {
        public Status Status { get; set; }
    }
}
