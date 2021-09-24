using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag
{
    public class TagEventArgs : EventArgs
    {
        public Event Event { get; set; }
        public Status Status { get; set; }
    }
}
