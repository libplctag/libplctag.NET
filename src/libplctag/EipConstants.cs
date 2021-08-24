/*###################################################################################################*/
/*#                                                                                                 #*/
/*#                (c) Jochen Haar                                                                  #*/
/*#                ===============                                                                  #*/
/*#                                                                                                 #*/
/*# Class-Name   : EipConstants                                                                     #*/
/*#                                                                                                 #*/
/*# Derived from : none                                                                             #*/
/*#                                                                                                 #*/
/*# File-Name    : EipConstants.cs                                                                  #*/
/*#                                                                                                 #*/
/*# Author       : Dipl.-Ing. Jochen Haar                                                           #*/
/*#                                                                                                 #*/
/*# Date         : 14.03.2021                                                                       #*/
/*#                                                                                                 #*/
/*# Tool/Compiler: Visual Studio 2019                                                               #*/
/*#                                                                                                 #*/
/*# Exceptions   : -                                                                                #*/
/*#                                                                                                 #*/
/*#=================================================================================================#*/
/*#                                                                                                 #*/
/*# Introduction : Definition of constants for the Ethernet/IP protocol.                            #*/
/*#                                                                                                 #*/
/*#=================================================================================================#*/
/*# Change report                                                                                   #*/
/*#----------+-------------------+------------------------------------------------------------------#*/
/*# Date     | Name              | description of the reason for changes                            #*/
/*#----------+-------------------+------------------------------------------------------------------#*/
/*#          |                   |                                                                  #*/
/*#----------+-------------------+------------------------------------------------------------------#*/
/*###################################################################################################*/

using System;

//-----------------------------------------------------------------------------------------------------
namespace IIoT_EipDeviceScanner
//-----------------------------------------------------------------------------------------------------
{
    /// <summary>
    /// Constants for the Ethernet/IP driver
    /// </summary>
    class EipConstants
    {
        /// <summary>
        /// Standard port number for Ethernet/IP
        /// </summary>
        public const int ETHERNETIP_PORT = 44818;
        /// <summary>
        /// Header size for commands, e.g. ListIdentity call
        /// </summary>
        public const byte HEADER_BYTE_SIZE = 24;
        /// <summary>
        /// Identifier for the GetAttributeList service call
        /// </summary>
        public const byte COMMAND_GET_ATTRIBUTE_LIST = 0x03;
        /// <summary>
        /// Identifier for the ReadTemplate service call
        /// </summary>
        public const byte COMMAND_READ_TEMPLATE = 0x4C;
        /// <summary>
        /// Identifier for the GetInstanceAttributeList service call
        /// </summary>
        public const byte COMMAND_GET_INSTANCE_ATTRIBUTE_LIST = 0x55;
        /// <summary>
        /// Identifier for the ListIdentity service call
        /// </summary>
        public const byte COMMAND_LIST_IDENTITY = 0x63;
        /// <summary>
        /// Definition of a structured tag of the symbol type UInt16 word
        /// </summary>
        public const UInt16 STRUCTURED_TAG_TYPE = 0x8000;  // bit 15 set to 1
        /// <summary>
        /// Definition of a structured tag of the symbol type UInt16 word
        /// </summary>
        public const UInt16 ARRAY_TAG_TYPE = 0x2000;  // bit 13 set to 1
        /// <summary>
        /// Definition of a reserved tag of the symbol type UInt16 word
        /// </summary>
        public const UInt16 RESERVED_TAG_TYPE = 0x1000;  // bit 12 set to 1
        /// <summary>
        /// Definition of the Program symbol type
        /// </summary>
        public const UInt16 PROGRAM_SYMBOL_TYPE = 0x1068;  // 4200 in decimal
        /// <summary>
        /// Definition of the string structure handle for the EipTemplate.StructureHandle property
        /// </summary>
        public const UInt16 STRING_STRUCTURE_HANDLE = 0xFCE;  // 4046 in decimal
    }
}
