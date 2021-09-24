using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag
{
    public class LogEventArgs : EventArgs
    {
        public DebugLevel DebugLevel { get; set; }
        public string Message { get; set; }
    }
}
