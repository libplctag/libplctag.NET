using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libplctag.Browse
{
    /// <summary>
    /// An common denominator for Tags and Fields. It has an Atomic type or an Udt
    /// </summary>
    public class TypedEntity
    {
        /// <summary>
        /// The Name of this Item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The flags encode various things such as if it is an System type etc.
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// The Identifier of the Actual data type of the Item
        /// </summary>
        public int Type
        {
            get { return Flags & 0xFF; }
        }

        /// <summary>
        /// if it is an Structure (isStruct = true), this holds the ID of the Corresponding Udt
        /// </summary>
        public int UdtId { get; set; }

        /// <summary>
        /// An reference to the owning Browser, so that Udt can be resolved
        /// </summary>
        public TagBrowser TagBrowser { get; set; }

        /// <summary>
        /// The Udt that corresponds to the UdtId if this tag is a structure (isStruct = true)
        /// </summary>
        public UdtEntry Udt
        {
            get
            {
                if (TagBrowser.Udts.ContainsKey(UdtId)) return TagBrowser.Udts[UdtId];
                else return null;
            }
        }

        /// <summary>
        /// if true, means that the type of this tag is defined by the UdtId
        /// </summary>
        public bool isStruct { get; set; }

        /// <summary>
        /// if true, it is an built in type, such as Int, String etc. 
        /// </summary>
        public bool isSystem { get; set; }

        /// <summary>
        /// if true, the Tag is actually an Array of the referenced type. The 
        /// Size of the Array, or its dimensions are in the ArrayDims
        /// </summary>
        public bool isArray { get { return ArrayDimCount > 0; } }

        /// <summary>
        /// If the Tag is an array, this holds up to 3 dimensions of the array
        /// Allen Bradly Plc only support up to 3 Array dimensions
        /// </summary>
        public int[] ArrayDims { get; set; } = new int[3];

        /// <summary>
        /// Indicates how many Array dimensions there are
        /// </summary>
        public int ArrayDimCount { get; set; }

        /// <summary>
        /// Represents an type name and an long description of the Data type of an Tag or field
        /// </summary>
        public struct TypeDescription
        {
            public string Name;
            public string Description;
        }

        /// <summary>
        /// Resolves the data type of the Tag into its name
        /// Data types are:
        /// int, dint, lint, real, lreal,....
        /// </summary>
        /// <returns></returns>
        public TypeDescription GetTypeDescription()
        {
            string value;
            string array = @"";
            string detailedDescription = "";

            if (ArrayDimCount > 0)
            {
                array = @"[" + ArrayDims[0].ToString();
                if (ArrayDims[1] > 0) array += @"," + ArrayDims[1].ToString();
                if (ArrayDims[2] > 0) array += @"," + ArrayDims[2].ToString();
                array += @"]";
            }

            switch (Type)
            {
                case 0xC1:
                    value = @"BOOL{0}";
                    detailedDescription = @"Boolean value, 1 bit";
                    break;
                case 0xC2:
                    value = @"SINT{0}";
                    detailedDescription = @"Signed 8-bit integer value";
                    break;
                case 0xC3:
                    value = @"INT{0}";
                    detailedDescription = @"Signed 16-bit integer value";
                    break;
                case 0xC4:
                    value = @"DINT{0}";
                    detailedDescription = @"Signed 32-bit integer value";
                    break;
                case 0xC5:
                    value = @"LINT{0}";
                    detailedDescription = @"Signed 64-bit integer value";
                    break;
                case 0xC6:
                    value = @"SINT{0}";
                    detailedDescription = @"Unsigned 8-bit integer value";
                    break;
                case 0xC7:
                    value = @"INT{0}";
                    detailedDescription = @"Unsigned 16-bit integer value";
                    break;
                case 0xC8:
                    value = @"DINT{0}";
                    detailedDescription = @"Unsigned 32-bit integer value";
                    break;
                case 0xC9:
                    value = @"LINT{0}";
                    detailedDescription = @"Unsigned 64-bit integer value";
                    break;
                case 0xCA:
                    value = @"REAL{0}";
                    detailedDescription = @"32-bit floating point value, IEEE format";
                    break;
                case 0xCB:
                    value = @"LREAL{0}";
                    detailedDescription = @"64-bit floating point value, IEEE format";
                    break;
                case 0xCC:
                    value = @"LINT{0}";
                    detailedDescription = @"Synchronous time value";
                    break;
                case 0xCD:
                    value = @"LINT{0}";
                    detailedDescription = @"Date value";
                    break;
                case 0xCE:
                    value = @"LINT{0}";
                    detailedDescription = @"Time of day value";
                    break;
                case 0xCF:
                    value = @"LINT{0}";
                    detailedDescription = @"Date and time of day value";
                    break;
                case 0xD0:
                    value = @"STRING{0}";
                    detailedDescription = @"Character string, 1 byte per character";
                    break;
                case 0xD1:
                    value = @"SINT{0}";
                    detailedDescription = @"8-bit bit string";
                    break;
                case 0xD2:
                    value = @"INT{0}";
                    detailedDescription = @"16-bit bit string";
                    break;
                case 0xD3:
                    value = @"DINT{0}";
                    detailedDescription = @"32-bit bit string";
                    break;
                case 0xD4:
                    value = @"LINT{0}";
                    detailedDescription = @"64-bit bit string";
                    break;
                case 0xD5:
                    value = @"STRING{0}";
                    detailedDescription = @"Wide char character string, 2 bytes per character";
                    break;
                case 0xD6:
                    value = @"LINT{0}";
                    detailedDescription = @"High resolution duration value";
                    break;
                case 0xD7:
                    value = @"LINT{0}";
                    detailedDescription = @"Medium resolution duration value";
                    break;
                case 0xD8:
                    value = @"LINT{0}";
                    detailedDescription = @"Low resolution duration value";
                    break;
                case 0xD9:
                    value = @"STRING{0}";
                    detailedDescription = @"N-byte per char character string";
                    break;
                case 0xDA:
                    value = @"STRING{0}";
                    detailedDescription = @"Counted character sting with 1 byte per character and 1 byte length indicator";
                    break;
                case 0xDB:
                    value = @"LINT{0}";
                    detailedDescription = @"Duration in milliseconds";
                    break;
                case 0xDC:
                    value = @"STRING{0}";
                    detailedDescription = @"CIP path segment";
                    break;
                case 0xDD:
                    value = @"STRING{0}";
                    detailedDescription = @"Engineering units";
                    break;
                case 0xDE:
                    value = @"STRING{0}";
                    detailedDescription = @"International character string";
                    break;
                case 0xA0:
                    value = @"UDT{0}";
                    detailedDescription = @"Data is an abbreviated struct type, i.e. a CRC of the actual type descriptor";
                    break;
                case 0xA1:
                    value = @"ARRAY{0}";
                    detailedDescription = @"Data is an abbreviated array type. The limits are left off";
                    break;
                case 0xA2:
                    value = @"UDT{0}";
                    detailedDescription = @"Data is a struct type descriptor";
                    break;
                case 0xA3:
                    value = @"ARRAY{0}";
                    detailedDescription = @"Data is an array type descriptor";
                    break;
                default:
                    value = @"unknown{0}";
                    detailedDescription = @"unknown";
                    break;
            }

            value = String.Format(value, array);

            if (isStruct)
            {
                if (Udt == null)
                {
                    value = "Unknown";
                    detailedDescription = "Unknown";
                }

                value = Udt.Name;
                // the type of the UDT is the name of the template - set the detailedDescription with the template name and the UDT element size in bytes
                detailedDescription = Udt.Name + array + @" - UDT (" + Udt.InstanceSize.ToString() + @" bytes)";
            }

            return new TypeDescription() { Name = value, Description =detailedDescription};
        }

        public override string ToString()
        {
            return Name + ": " + GetTypeDescription().Name;
        }
    }
}
