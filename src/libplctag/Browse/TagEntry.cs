using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libplctag.Browse
{
    /// <summary>
    /// An ordinary Tag in the PLC
    /// </summary>
    public class TagEntry: TypedEntity
    {
        /// <summary>
        /// The id of this Tag
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The size of this Tag or the size of one Element in the case it is an array
        /// </summary>
        public int ElementSize { get; set; }

        /// <summary>
        /// how many Elements there are. Basically it is the product of all array dimensions
        /// </summary>
        public int ElementCount { get; set; }
    }
}