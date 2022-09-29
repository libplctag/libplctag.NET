using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libplctag.Browse
{
    /// <summary>
    /// Represents an Program folder in the PLC. It contains Program Tags
    /// </summary>
    public class ProgramEntry: TagEntry
    {
        public ICollection<TagEntry> Tags { get; set; } = new List<TagEntry>();

        public override string ToString()
        {
            return Name;
        }
    }
}
