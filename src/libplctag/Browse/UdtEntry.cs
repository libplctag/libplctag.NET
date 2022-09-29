using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libplctag.Browse
{
    /// <summary>
    /// An user defined data type in the PLC
    /// </summary>
    public class UdtEntry
    {
        /// <summary>
        /// the Id of this Udt. You find this Id in the tags definition.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// the name of the Udt
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates how many Fields this Udt has
        /// </summary>
        public int FieldCount { get; set; }

        /// <summary>
        /// How big is this datatype in total during runtime. 
        /// basically it is the sum of all its fields
        /// </summary>
        public int InstanceSize { get; set; }

        /// <summary>
        /// Unknown
        /// </summary>
        public int StructHandle { get; set; }

        /// <summary>
        /// An reference to the owning Browser, so that Udt can be resolved
        /// </summary>
        public TagBrowser TagBrowser { get; set; }

        /// <summary>
        /// The field of this Udt
        /// </summary>
        public IList<UdtField> Fields { get; set; }   = new List<UdtField>();

        public override string ToString()
        {
            return Name;
        }

        public class UdtField : TypedEntity
        {

            /// <summary>
            /// Holds some additional Flags. Their meaning is unknown. 
            /// Probably they are "readWrite", "readOnly" etc. 
            /// </summary>
            public int Metadata { get; set; }

            /// <summary>
            /// The offset of this field inside the Udt
            /// You need this in order to resolve the fields from the 
            /// Byte[] of the Owning Udt
            /// </summary>
            public int Offset { get; set; }
        }
    }
}
