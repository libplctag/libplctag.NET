/*###################################################################################################*/
/*#                                                                                                 #*/
/*#                (c) Jochen Haar                                                                  #*/
/*#                ===============                                                                  #*/
/*#                                                                                                 #*/
/*# Class-Name   : EipTagList                                                                       #*/
/*#                                                                                                 #*/
/*# Derived from : none                                                                             #*/
/*#                                                                                                 #*/
/*# File-Name    : EipTagList.cs                                                                    #*/
/*#                                                                                                 #*/
/*# Author       : Dipl.-Ing. Jochen Haar                                                           #*/
/*#                                                                                                 #*/
/*# Date         : 31.01.2021                                                                       #*/
/*#                                                                                                 #*/
/*# Tool/Compiler: Visual Studio 2019                                                               #*/
/*#                                                                                                 #*/
/*# Exceptions   : -                                                                                #*/
/*#                                                                                                 #*/
/*#=================================================================================================#*/
/*#                                                                                                 #*/
/*# Introduction : Class to create the Tag Tree List  for Ethernet/IP devices                       #*/
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
using System.Collections.Generic;

//-------------------------------------------------------------------------------------------------
namespace IIoT_EipDeviceScanner
//-------------------------------------------------------------------------------------------------
{
    /// <summary>
    /// Class to create the Tag Tree List for Ethernet/IP device objects
    /// </summary>
    //---------------------------------------------------------------------------------------------
    public class EipTagList
    //---------------------------------------------------------------------------------------------
    {
        /// <summary>
        /// TagName of the PLC object
        /// </summary>
        public string TagName { private set; get; }
        /// <summary>
        /// Length in bytes of the tag
        /// </summary>
        public int Length { private set; get; }
        /// <summary>
        /// InstanceId of the tag
        /// </summary>
        public uint InstanceId { private set; get; }
        /// <summary>
        /// Symbol Type of the tag in Ethernet/IP notation
        /// </summary>
        public int Type { private set; get; }
        /// <summary>
        /// Dimensions of the tag
        /// </summary>
        public uint[] Dimensions { private set; get; }
        /// <summary>
        /// The structure handle of the template or 0 for plain tags. Remember: handle 0xFCE is a STRING type
        /// </summary>
        public UInt16 StructureHandle { private set; get; }
        /// <summary>
        /// The PLC access tag name for element-wise access, e.g. StructA.StructB.StructC.TagName
        /// </summary>
        public string PlcAccessTagName { private set; get; }
        /// <summary>
        /// Current value of the tag
        /// </summary>
        public object Value { set; get; }
        /// <summary>
        /// The timestamp of the tag value in DateTime ticks
        /// </summary>
        public long TimestampTicks { set; get; }
        /// <summary>
        /// Collection of the child tags of the structured tags or empty for plain tags
        /// </summary>
        public Dictionary<string, EipTagList> ChildTagCollection { private set; get; } = null;

        /// <summary>
        /// Instantiates a Tag tree list
        /// </summary>
        /// <param name="tagName">TagName of the tag</param>
        /// <param name="length">Length in bytes of the tag</param>
        /// <param name="instanceId">InstanceId of the tag</param>
        /// <param name="type">Type of the tag</param>
        /// <param name="dimensions">Dimensions of the tag</param>
        /// <param name="structureHandle">StructureHandle of the tag or 0 for plain tags. Remember: 0xFCE is a STRING type</param>
        /// <param name="plcAccessTagName">The PLC access tag name for element-wise access, e.g. StructA.StructB.StructC.TagName</param>
        /// <param name="value">Current value of the tag</param>
        /// <param name="timestampTicks">The timestamp of the tag value in DateTime ticks</param>
        //-----------------------------------------------------------------------------------------
        public EipTagList(string tagName, int length = 0, uint instanceId = 0, int type = 0, uint[] dimensions = null, UInt16 structureHandle = 0, string plcAccessTagName = null, object value = null, long timestampTicks = 0)
        //-----------------------------------------------------------------------------------------
        {
            this.TagName = tagName;
            this.Length = length;
            this.InstanceId = instanceId;
            this.Type = type;
            this.Dimensions = dimensions;
            this.StructureHandle = structureHandle;
            if (plcAccessTagName == null) this.PlcAccessTagName = tagName;
            else this.PlcAccessTagName = plcAccessTagName;
            this.Value = value;
            this.TimestampTicks = timestampTicks;
            this.ChildTagCollection = new Dictionary<string, EipTagList>();
        }

        /// <summary>
        /// Creates a Tag tree list and adds the new created tree list to the ChildTagList of the parent
        /// </summary>
        /// <param name="parent">Parent Tag tree list, or null for the root</param>
        /// <param name="tagName">TagName of the tag</param>
        /// <param name="length">Length in bytes of the tag</param>
        /// <param name="instanceId">InstanceId of the tag</param>
        /// <param name="type">Type of the tag</param>
        /// <param name="dimensions">Dimensions of the tag</param>
        /// <param name="structureHandle">StructureHandle of the tag. Remember: 0xFCE is a STRING type</param>
        /// <param name="plcAccessTagName">The PLC access tag name for element-wise access, e.g. StructA.StructB.StructC.TagName</param>
        /// <param name="value">Current value of the tag</param>
        /// <param name="timestampTicks">The timestamp of the tag value in DateTime ticks</param>
        /// <returns>The Tag tree list of the Tag</returns>
        //--------------------------------------------------------------------------------------------
        public EipTagList AddTag(EipTagList parent, string tagName, int length = 0, uint instanceId = 0, int type = 0, uint[] dimensions = null, UInt16 structureHandle = 0, string plcAccessTagName = null, object value = null, long timestampTicks = -1)
        //--------------------------------------------------------------------------------------------
        {
            EipTagList tagList = new EipTagList(tagName, length, instanceId, type, dimensions, structureHandle, plcAccessTagName, value, timestampTicks);
            if (parent != null) parent.ChildTagCollection.Add(tagName, tagList);
            return tagList;
        }

        /// <summary>
        /// Returns the tag list of a dedicated TagName or null if it is an unknown tag.
        /// This method might be helpful if an user wants to read the values of the UDT structure
        /// members element-wise from the PLC.
        /// </summary>
        /// <param name="tagName">The TagName of interest</param>
        /// <returns>The tag list of the tag or null if the tag is unknown</returns>
        //--------------------------------------------------------------------------------------------
        public EipTagList GetTagListOfTagName(string tagName)
        //--------------------------------------------------------------------------------------------
        {
            EipTagList tagList = null;
            if (this.ChildTagCollection.ContainsKey(tagName)) tagList = this.ChildTagCollection[tagName];
            return tagList;
        }
    }
}
