/*###################################################################################################*/
/*#                                                                                                 #*/
/*#                (c) Jochen Haar                                                                  #*/
/*#                ===============                                                                  #*/
/*#                                                                                                 #*/
/*# Class-Name   : EipTagListFactory                                                                #*/
/*#                                                                                                 #*/
/*# Derived from : none                                                                             #*/
/*#                                                                                                 #*/
/*# File-Name    : EipTagListFactory.cs                                                             #*/
/*#                                                                                                 #*/
/*# Author       : Dipl.-Ing. Jochen Haar                                                           #*/
/*#                                                                                                 #*/
/*# Date         : 14.03.2021                                                                       #*/
/*#                                                                                                 #*/
/*# Tool/Compiler: Visual Studio 2019                                                               #*/
/*#                                                                                                 #*/
/*# Exceptions   : will be thrown on CIP protocol errors                                            #*/
/*#                                                                                                 #*/
/*#=================================================================================================#*/
/*#                                                                                                 #*/
/*# Introduction : Class to read the Tag and UDT template information from an Ethernet/IP PLC       #*/
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
using System.Linq;
using System.Net.Sockets;
using System.Text;

//-----------------------------------------------------------------------------------------------------
namespace IIoT_EipDeviceScanner
//-----------------------------------------------------------------------------------------------------
{
    /// <summary>
    /// Class to read the Tag and UDT template information from an Ethernet/IP PLC via a separate TcpClient
    /// connection to avoid any side effects in the library. This implementation is meant as a work-around
    /// only until the library provides a similar solution. It should work at least for CompactLogix and 
    /// ControlLogix devices connected to the PLC on-board Ethernet adapter. Backplane routing is not supported.
    /// Usage: Just instantiate an object by calling the Constructor with the IPAddress and Port of the
    /// PLC. The Constructor opens the connection, reads the data of interest, analyses the data and closes
    /// the connection. All Tag and Structured Tag information is available via the public methods and 
    /// public members.
    /// 
    /// Reference: 1756-PM020G-EN-P.pdf - published September 2020
    /// 
    /// Feel free to use this code as is and on your own risk. The risk is low, as we are reading data only.
    /// 
    /// A matter of course and very fair is to keep the name of the original author in the header of the code.
    /// March 2021 - Jochen Haar
    /// </summary>
    //-------------------------------------------------------------------------------------------------
    public class EipTagListFactory
    //-------------------------------------------------------------------------------------------------
    {
        /// <summary>
        /// The EipTag object instance with the member properties
        /// </summary>
        //---------------------------------------------------------------------------------------------
        public class EipTag
        //---------------------------------------------------------------------------------------------
        {
            /// <summary>
            /// The SymbolName of the Tag
            /// </summary>
            public string SymbolName { private set; get; }
            /// <summary>
            /// The InstanceId of the Tag
            /// </summary>
            public int InstanceId { private set; get; }
            /// <summary>
            /// The SymbolType of the Tag
            /// </summary>
            public UInt16 SymbolType { private set; get; }
            /// <summary>
            /// The ElementSize of the Tag
            /// </summary>
            public UInt16 ElementSize { private set; get; }
            /// <summary>
            /// The Dimensions of the Tag
            /// </summary>
            public uint[] Dimensions { private set; get; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="symbolName">The SymbolName of the Tag</param>
            /// <param name="instanceId">The InstanceId of the Tag</param>
            /// <param name="symbolType">The SymbolType of the Tag</param>
            /// <param name="elementSize">The ElementSize of the Tag</param>
            /// <param name="dimensions">The Dimensions of the Tag</param>
            //-----------------------------------------------------------------------------------------
            public EipTag(string symbolName, int instanceId, UInt16 symbolType, UInt16 elementSize, uint[] dimensions)
            //-----------------------------------------------------------------------------------------
            {
                this.SymbolName = symbolName;
                this.InstanceId = instanceId;
                this.SymbolType = symbolType;
                this.ElementSize = elementSize;
                this.Dimensions = dimensions;
            }
        }

        /// <summary>
        /// The EipTemplateMember object instance with the member properties
        /// </summary>
        //---------------------------------------------------------------------------------------------
        public class EipTemplateMember
        //---------------------------------------------------------------------------------------------
        {
            /// <summary>
            /// The MemberName property member
            /// </summary>
            public string MemberName { set; get; }
            /// <summary>
            /// The MemberInfo property member
            /// </summary>
            public UInt16 MemberInfo { private set; get; }
            /// <summary>
            /// The MemberType property member
            /// </summary>
            public UInt16 MemberType { private set; get; }
            /// <summary>
            /// The MemberOffset property member
            /// </summary>
            public int MemberOffset { private set; get; }
            /// <summary>
            /// The EipTemplate object instance of the member
            /// </summary>
            public EipTemplate ChildTemplate { set; get; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="memberName">The MemberName property member</param>
            /// <param name="memberInfo">The MemberInfo property member</param>
            /// <param name="memberType">The MemberType property member</param>
            /// <param name="memberOffset">The MemberOffset property member</param>
            //-----------------------------------------------------------------------------------------
            public EipTemplateMember(string memberName, UInt16 memberInfo, UInt16 memberType, int memberOffset)
            //-----------------------------------------------------------------------------------------
            {
                this.MemberName = memberName;
                this.MemberInfo = memberInfo;
                this.MemberType = memberType;
                this.MemberOffset = memberOffset;
                this.ChildTemplate = null;
            }
        }

        /// <summary>
        /// The EipTemplate object instance with the template properties
        /// </summary>
        //---------------------------------------------------------------------------------------------
        public class EipTemplate
        //---------------------------------------------------------------------------------------------
        {
            /// <summary>
            /// The InstanceId of the template
            /// </summary>
            public UInt16 InstanceId { set; get; }
            /// <summary>
            /// The Name of the template
            /// </summary>
            public string TemplateName { set; get; }
            /// <summary>
            /// The amount of members in the structure
            /// </summary>
            public UInt16 MemberCount { set; get; }
            /// <summary>
            /// The amount of bytes for the definition of the structure
            /// </summary>
            public int TemplateDefinitionStructureSize { set; get; }
            /// <summary>
            /// The amount of bytes transferred on the wire on read
            /// </summary>
            public int TransferredWireBytesOnRead { set; get; }
            /// <summary>
            /// The structure handle of the template. Remember: 0xFCE is a STRING type
            /// </summary>
            public UInt16 StructureHandle { set; get; }
            /// <summary>
            /// The MemberList of the template
            /// </summary>
            public List<EipTemplateMember> TemplateMemberList { set; get; } = null;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="instanceId">The InstanceId of the template</param>
            /// <param name="templateName">The Name of the template</param>
            //-----------------------------------------------------------------------------------------
            public EipTemplate(UInt16 instanceId = 0, string templateName = null)
            //-----------------------------------------------------------------------------------------
            {
                this.InstanceId = instanceId;
                this.TemplateName = templateName;
                this.TemplateMemberList = new List<EipTemplateMember>();
            }
        }

        /// <summary>
        /// The IP Address of the Ethernet/IP device we want to deal with
        /// </summary>
        public string DeviceIPAddress { private set; get; }
        /// <summary>
        /// The Port Number we have to use to communicate with the Ethernet/IP device
        /// </summary>
        public int DevicePort { private set; get; }
        /// <summary>
        /// The collection for fast hash-access to the EipTag instance of the tag via the TagName
        /// </summary>
        public Dictionary<string, EipTag> TagNameCollection { private set; get; } = new Dictionary<string, EipTag>();
        /// <summary>
        /// The collection for fast hash-access to the EipTag instance of the tag via the InstanceId of the tag
        /// </summary>
        public Dictionary<int, EipTag> TagInstanceIdCollection { private set; get; } = new Dictionary<int, EipTag>();
        /// <summary>
        /// The collection for fast hash-access to the EipTemplate instance of the tag via the TemplateName
        /// </summary>
        public Dictionary<string, EipTemplate> TemplateNameCollection { private set; get; } = new Dictionary<string, EipTemplate>();
        /// <summary>
        /// The collection for fast hash-access to the EipTemplate instance of the tag via the InstanceId of the tag
        /// </summary>
        public Dictionary<int, EipTemplate> TemplateInstanceIdCollection { private set; get; } = new Dictionary<int, EipTemplate>();
        /// <summary>
        /// The spent time to explore the Ethernet/IP device
        /// </summary>
        public TimeSpan Duration { private set; get; }
        /// <summary>
        /// The TcpClient object for the communication
        /// </summary>
        private TcpClient Client;
        /// <summary>
        /// The handle of the communication session with the PLC
        /// </summary>
        private UInt32 SessionHandle;
        /// <summary>
        /// The stream of the TcpClient object
        /// </summary>
        private NetworkStream Stream = null;
        /// <summary>
        /// The last InstanceId for the tag ID's - used during browsing only to set the InstanceId of the tags
        /// </summary>
        private int LastInstanceId = 0;
        /// <summary>
        /// The tag list of the PLC we are connected to
        /// </summary>
        private EipTagList EipTagList = null;

        /// <summary>
        /// Forbidden Constructor
        /// </summary>
        //---------------------------------------------------------------------------------------------
        private EipTagListFactory()
        //---------------------------------------------------------------------------------------------
        {
        }

        /// <summary>
        /// The one and only Constructor
        /// </summary>
        /// <param name="deviceIPAddress">The IP address of the device of interest</param>
        /// <param name="port">The Port number to connect to</param>
        //---------------------------------------------------------------------------------------------
        public EipTagListFactory(string deviceIPAddress, int devicePort)
        //---------------------------------------------------------------------------------------------
        {
            this.DeviceIPAddress = deviceIPAddress;
            this.DevicePort = devicePort;
            DateTime start = DateTime.Now;

            this.RegisterSession();                 // register for a small talk with the Ethernet/IP device
            this.ReadTags();                        // read controller tags
            this.ReadProgramTags();                 // read program tags
            this.AnalyseTagListAndReadTemplates();  // deep dive
            this.UnregisterSession();               // we're done - close the session and the connection

            this.Duration = DateTime.Now.Subtract(start);  // just to get an impression how long it will take with huge tag lists
        }

        /// <summary>
        /// Registers a communication session with the Ethernet/IP device
        /// </summary>
        /// <returns>The session handle</returns>
        //---------------------------------------------------------------------------------------------
        private UInt32 RegisterSession()
        //---------------------------------------------------------------------------------------------
        {
            if (this.SessionHandle != 0) return this.SessionHandle;

            EipEncapsulation encapsulation = new EipEncapsulation();
            encapsulation.Command = EipEncapsulation.CommandsEnum.RegisterSession;
            encapsulation.Length = 4;
            encapsulation.CommandSpecificData.Add(1);  // protocol version - should be set to 1
            encapsulation.CommandSpecificData.Add(0);
            encapsulation.CommandSpecificData.Add(0);  // session options shall be set to "0"
            encapsulation.CommandSpecificData.Add(0);

            this.Client = new TcpClient(this.DeviceIPAddress, this.DevicePort);
            this.Stream = this.Client.GetStream();
            this.Stream.Write(encapsulation.ToBytes(), 0, encapsulation.ToBytes().Length);
            byte[] data = new Byte[256];
            Int32 lng = this.Stream.Read(data, 0, data.Length);
            UInt32 sessionHandle = 0;
            if (lng >= 8) sessionHandle = (UInt32)data[4] + (((UInt32)data[5]) << 8) + (((UInt32)data[6]) << 16) + (((UInt32)data[7]) << 24);
            this.SessionHandle = sessionHandle;

            return sessionHandle;
        }

        /// <summary>
        /// Unregisters a communication session with the Ethernet/IP device
        /// </summary> 
        //---------------------------------------------------------------------------------------------
        private void UnregisterSession()
        //---------------------------------------------------------------------------------------------
        {
            EipEncapsulation encapsulation = new EipEncapsulation();
            encapsulation.Command = EipEncapsulation.CommandsEnum.UnregisterSession;
            encapsulation.Length = 0;
            encapsulation.SessionHandle = this.SessionHandle;

            // this try/catch is just to allow to close the stream if the connection was already closed by the Ethernet/IP device
            try { this.Stream.Write(encapsulation.ToBytes(), 0, encapsulation.ToBytes().Length); }
            catch (Exception) { }

            this.Client.Close();
            this.Stream.Close();
            this.SessionHandle = 0;
        }

        /// <summary>
        /// Analyses the TagList, removes forbidden tags and creates the Template objects of structured Tags.
        /// See Rockwell publication: 1756-PM020G-EN-P.pdf
        /// </summary>
        //---------------------------------------------------------------------------------------------
        private void AnalyseTagListAndReadTemplates()
        //---------------------------------------------------------------------------------------------
        {
            bool forbiddenTag;
            string tagName;
            UInt16 symbolType;
            EipTag eipTag;
            List<string> tagsToRemove = new List<string>();
            
            // analyse the TagList and read the template information
            foreach (var pair in this.TagNameCollection)
            {
                tagName = pair.Key;
                eipTag = pair.Value;
                forbiddenTag = false;
                symbolType = eipTag.SymbolType;

                // remove the tag if it is a reserved tag - bit 12 = 1
                if (forbiddenTag == false) forbiddenTag = this.IsReservedTag(symbolType);
                // remove the tag if it is a structured tag in the forbidden range of 0x100 >= x <= 0xEFF
                if (forbiddenTag == false) if (this.IsStructuredTag(symbolType)) forbiddenTag = this.IsTagInForbiddenRange(symbolType);
                // remove the tag if the tag has two leading underscores, e.g. "__AnyTag"
                if (forbiddenTag == false) forbiddenTag = this.HasLeadingDoubleUnderscores(tagName);
                // remove the tag if the tag is a Routine"
                if (forbiddenTag == false) forbiddenTag = this.IsRoutine(tagName);  // is already covered by IsReservedTag(...)
                // remove the tag if the tag has ":" in the tag name, e.g. Local:1:C
                //if (forbiddenTag == false) forbiddenTag = this.HasColon(tagName);  // keep these tags for now

                if (forbiddenTag)
                {
                    // remember to get rid of this forbidden tag
                    tagsToRemove.Add(tagName);
                }
                else
                {
                    // if the tag is a structured tag, we need to read the template data
                    if (this.IsStructuredTag(symbolType))
                    {
                        // create the instanceId of the template by switching off bit 15
                        symbolType = this.GetTemplateInstanceFromSymbolType(symbolType);
                        // read the template if not already in the collection
                        if (this.TemplateInstanceIdCollection.ContainsKey(symbolType) == false)
                        {
                            EipTemplate template = this.ReadTemplate(symbolType);
                            this.TemplateNameCollection.Add(template.TemplateName, template);
                            this.TemplateInstanceIdCollection.Add(template.InstanceId, template);
                        }
                    }
                }
            }

            // remove the forbidden tags from the TagList
            foreach (string tag in tagsToRemove) this.TagNameCollection.Remove(tag);
            // fill the TagInstanceCollection for tag access via the InstanceId
            foreach (var pair in this.TagNameCollection) this.TagInstanceIdCollection.Add(pair.Value.InstanceId, pair.Value);
            // read all nested templates and add the new found templates to the TemplateInstanceIdCollection
            foreach (var pair in this.TemplateNameCollection) this.ReadNestedTemplates(pair.Value);
            // clear and refill the TemplateNameCollection as the TemplateInstanceIdCollection has grown meanwhile in the ReadNestedTemplates() method
            this.TemplateNameCollection.Clear();  // prepare template collections synchronization
            foreach (var pair in this.TemplateInstanceIdCollection) this.TemplateNameCollection.Add(pair.Value.TemplateName, pair.Value);  // synchronize the template collections
        }

        /// <summary>
        /// Reads the tags which are in Program scope only
        /// </summary>
        //---------------------------------------------------------------------------------------------
        private void ReadProgramTags()
        //---------------------------------------------------------------------------------------------
        {
            List<string> programTags = new List<string>();
            foreach (var pair in this.TagNameCollection) if (pair.Value.SymbolType == EipConstants.PROGRAM_SYMBOL_TYPE) programTags.Add(pair.Value.SymbolName);
            foreach (string programName in programTags) this.ReadTags(0, programName);
        }

        /// <summary>
        /// Reads the nested templates if the members of the structured tag are structured tags as well
        /// </summary>
        /// <param name="template">The template object instance of the structured tag</param>
        //---------------------------------------------------------------------------------------------
        private void ReadNestedTemplates(EipTemplate template)
        //---------------------------------------------------------------------------------------------
        {
            UInt16 symbolType;

            foreach (EipTemplateMember member in template.TemplateMemberList)
            {
                symbolType = member.MemberType;

                if (this.IsStructuredTag(symbolType))
                {
                    // get the TemplateInstanceId from the SymbolType by switching off bit 15
                    symbolType = this.GetTemplateInstanceFromSymbolType(symbolType);

                    // read the template if not already done and available in the TemplateNameCollection
                    if (this.TemplateNameCollection.ContainsKey(member.MemberName) == false)
                    {
                        //--------------------------------------------------------------------------------------------------------------//
                        // PLEASE BEAR IN MIND: The TemplateInstanceIdCollection is growing here and the TemplateNameCollection not!!!  //
                        // As we are iterating thru the TemplateNameCollection, we can't add any objects to the TemplateNameCollection. //
                        // The TemplateNameCollection will be synchronized once we are done. This circumstance is worthwhile to explain //
                        //--------------------------------------------------------------------------------------------------------------//

                        // create and add the child template to the TemplateInstanceIdCollection, if not already done previously
                        if (this.TemplateInstanceIdCollection.ContainsKey(symbolType) == false)
                        {
                            // create the child template
                            EipTemplate childTemplate = this.ReadTemplate(symbolType);
                            // add the template to the collection
                            this.TemplateInstanceIdCollection.Add(childTemplate.InstanceId, childTemplate);
                            // assign the template to the member
                            member.ChildTemplate = childTemplate;
                            // hip hip hooray - here we go ... drill one level down with a recursive call and the childTemplate
                            this.ReadNestedTemplates(childTemplate);
                        }
                        else
                        {
                            // template is already in the collection, assign the one which is in the TemplateInstanceIdCollection
                            member.ChildTemplate = this.GetTemplateByInstance(symbolType);
                        }
                    }
                    else
                    {
                        // template is already in the collection, assign the one which is in the TemplateInstanceIdCollection
                        member.ChildTemplate = this.GetTemplateByInstance(symbolType);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the tag is a reserved tag
        /// </summary>
        /// <param name="symbolType">The SymbolType of the tag</param>
        /// <returns>True if it is reserved tag</returns>
        //---------------------------------------------------------------------------------------------
        private bool IsReservedTag(UInt16 symbolType)
        //---------------------------------------------------------------------------------------------
        {
            return (symbolType & EipConstants.RESERVED_TAG_TYPE) == EipConstants.RESERVED_TAG_TYPE;
        }

        /// <summary>
        /// Checks if the tag is aa array
        /// </summary>
        /// <param name="symbolType">The SymbolType of the tag</param>
        /// <returns>True if it is an array</returns>
        //---------------------------------------------------------------------------------------------
        public bool IsArrayTag(UInt16 symbolType)
        //---------------------------------------------------------------------------------------------
        {
            return (symbolType & EipConstants.ARRAY_TAG_TYPE) == EipConstants.ARRAY_TAG_TYPE;
        }

        /// <summary>
        /// Checks if the tag is a structured tag
        /// </summary>
        /// <param name="symbolType">The SymbolType of the tag</param>
        /// <returns>True if it is a structured tag</returns>
        //---------------------------------------------------------------------------------------------
        public bool IsStructuredTag(UInt16 symbolType)
        //---------------------------------------------------------------------------------------------
        {
            return (symbolType & EipConstants.STRUCTURED_TAG_TYPE) == EipConstants.STRUCTURED_TAG_TYPE;
        }

        /// <summary>
        /// Checks if the tag is in the range of the forbidden tags
        /// </summary>
        /// <param name="symbolType">The SymbolType of the tag</param>
        /// <returns>True if in the forbidden range</returns>
        //---------------------------------------------------------------------------------------------
        private bool IsTagInForbiddenRange(UInt16 symbolType)
        //---------------------------------------------------------------------------------------------
        {
            return (symbolType >= 0x100) && (symbolType <= 0xEFF);
        }

        /// <summary>
        /// Checks if the tag name is a routine
        /// </summary>
        /// <param name="tagName">The Name of the tag</param>
        /// <returns>True if the tag name starts with "Routine:"</returns>
        //---------------------------------------------------------------------------------------------
        private bool IsRoutine(string tagName)
        //---------------------------------------------------------------------------------------------
        {
            return (tagName.StartsWith(@"Routine:"));
        }

        /// <summary>
        /// Checks if the tag name has two leading underscores
        /// </summary>
        /// <param name="tagName">The Name of the tag</param>
        /// <returns>True if the tag has two leading underscores</returns>
        //---------------------------------------------------------------------------------------------
        private bool HasLeadingDoubleUnderscores(string tagName)
        //---------------------------------------------------------------------------------------------
        {
            return (tagName.StartsWith(@"__"));
        }

        /// <summary>
        /// Checks if the tag name contains colons
        /// </summary>
        /// <param name="tagName">The Name of the tag</param>
        /// <returns>True if the tag name contains colons</returns>
        //---------------------------------------------------------------------------------------------
        private bool HasColon(string tagName)
        //---------------------------------------------------------------------------------------------
        {
            return (tagName.Contains(@":"));
        }

        /// <summary>
        /// Returns the EipTag object instance with TagName as key
        /// </summary>
        /// <param name="tagName">The TagName of interest</param>
        /// <returns>The EipTag object instance, or null if unknown</returns>
        //---------------------------------------------------------------------------------------------
        public EipTag GetTagByName(string tagName)
        //---------------------------------------------------------------------------------------------
        {
            EipTag value = null;
            if (this.TagNameCollection.ContainsKey(tagName)) value = this.TagNameCollection[tagName];
            return value;
        }

        /// <summary>
        /// Returns the EipTag object instance with InstanceId as key
        /// </summary>
        /// <param name="instanceId">The InstanceId of interest</param>
        /// <returns>The EipTag object instance, or null if unknown</returns>
        //---------------------------------------------------------------------------------------------
        public EipTag GetTagByInstance(int instanceId)
        //---------------------------------------------------------------------------------------------
        {
            EipTag value = null;
            if (this.TagInstanceIdCollection.ContainsKey(instanceId)) value = this.TagInstanceIdCollection[instanceId];
            return value;
        }

        /// <summary>
        /// Returns the EipTemplate object instance with TemplateName as key
        /// </summary>
        /// <param name="templateName">The TemplateName of interest</param>
        /// <returns>The EipTemplate object instance, or null if unknown</returns>
        //---------------------------------------------------------------------------------------------
        public EipTemplate GetTemplateByName(string templateName)
        //---------------------------------------------------------------------------------------------
        {
            EipTemplate value = null;
            if (this.TemplateNameCollection.ContainsKey(templateName)) value = this.TemplateNameCollection[templateName];
            return value;
        }

        /// <summary>
        /// Returns the EipTemplate object instance with InstanceId as key
        /// </summary>
        /// <param name="instanceId">The InstanceId of interest</param>
        /// <returns>The EipTemplate object instance, or null if unknown</returns>
        //---------------------------------------------------------------------------------------------
        public EipTemplate GetTemplateByInstance(UInt16 instanceId)
        //---------------------------------------------------------------------------------------------
        {
            EipTemplate value = null;
            instanceId = this.GetTemplateInstanceFromSymbolType(instanceId);  // ensure valid InstanceId range
            if (this.TemplateInstanceIdCollection.ContainsKey(instanceId)) value = this.TemplateInstanceIdCollection[instanceId];
            return value;
        }

        /// <summary>
        /// Returns the TemplateInstanceId of a given SymbolType by switching bit 15 off
        /// </summary>
        /// <param name="instanceId">The SymbolType of the tag</param>
        /// <returns>The InstanceId of the template</returns>
        //---------------------------------------------------------------------------------------------
        public UInt16 GetTemplateInstanceFromSymbolType(UInt16 symbolType)
        //---------------------------------------------------------------------------------------------
        {
            return symbolType &= 0xFFF;  // ensure valid InstanceId range
        }

        /// <summary>
        /// Returns the EipTagList of the PLC we're connected to
        /// </summary>
        /// <param name="rootName">The name of the root</param>
        /// <returns>The EipTagList with the tags in the PLC we are connected to</returns>
        //-----------------------------------------------------------------------------------------
        public EipTagList GetEipTagList(string rootName)
        //-----------------------------------------------------------------------------------------
        {
            if (this.EipTagList == null)
            {
                EipTag tag;
                EipTemplate template;
                EipTagList childEipTagList;
                UInt16 structureHandle;

                // create the EipTagList instance
                this.EipTagList = new EipTagList(rootName);  // the name of the root

                // run thru the TagNameCollection instance and fill the EipTagList instance
                foreach (var pair in this.TagNameCollection)
                {
                    tag = pair.Value;  // just to understand it better
                    structureHandle = 0;  // reset previous setting
                    // get the template object instance or null if it is not a template tag
                    template = this.GetTemplateByInstance(tag.SymbolType);
                    // if it is a template tag, set the StructureHandle for standard STRING indication - remember: handle 0xFCE is a STRING type
                    if (template != null) structureHandle = template.StructureHandle;
                    // add the tag to the list
                    childEipTagList = this.EipTagList.AddTag(this.EipTagList, tag.SymbolName, tag.ElementSize, (uint)tag.InstanceId, tag.SymbolType, tag.Dimensions, structureHandle);
                    // if it is a template tag, we drill down
                    if (template != null) this.AddTemplateToTag(template, childEipTagList);
                }
            }

            return this.EipTagList;
        }

        /// <summary>
        /// Adds the template data to the parent EipTagList
        /// </summary>
        /// <param name="template">The template instance object</param>
        /// <param name="parent">The EipTagList instance object of the parent</param>
        //-----------------------------------------------------------------------------------------
        private void AddTemplateToTag(EipTemplate template, EipTagList parent)
        //-----------------------------------------------------------------------------------------
        {
            string plcAccessTagName;
            EipTemplate childTemplate;
            EipTagList childEipTagList;
            UInt16 structureHandle;
            uint[] dimensions = new uint[] { 0, 0, 0 };  // the dimensions of the tag

            // run thru the TemplateMemberList instance and fill the EipTagList instance
            foreach (EipTemplateMember member in template.TemplateMemberList)
            {
                // reset the previous settings of the structured member
                structureHandle = 0;
                // reset the previous dimension of the structured member
                dimensions[0] = 0;
                // get the template instance or null if it is not a template
                childTemplate = this.GetTemplateByInstance(member.MemberType);
                // if it is an array set the array dimension which is defined in the member.MemberInfo property. Struct member arrays are always one-dimensional
                if (this.IsArrayTag(member.MemberType)) dimensions[0] = member.MemberInfo;
                // if it is a template tag, set the StructureHandle
                if (childTemplate != null) structureHandle = childTemplate.StructureHandle;
                // set the during drill down growing PlcAccessTagName for the element-wise access to the structured tag
                plcAccessTagName = parent.PlcAccessTagName + @"." + member.MemberName;
                // add the tag to the list - length and instanceId are not relevant here as the length is defined with the data type and the instanceId is the parent instanceId
                childEipTagList = parent.AddTag(parent, member.MemberName, 0, 0, member.MemberType, dimensions, structureHandle, plcAccessTagName);
                // if it is a template tag, we need to place a recursive call to drill further down
                if (childTemplate != null) this.AddTemplateToTag(childTemplate, childEipTagList);
            }
        }

        /// <summary>
        /// Creates the encrypted request path in CIP notation for logical segment addressing 
        /// </summary>
        /// <param name="classId">The ClassId</param>
        /// <param name="instanceId">The InstanceId</param>
        /// <returns>The byte buffer of the created path</returns>
        //---------------------------------------------------------------------------------------------
        private byte[] GetEncryptedRequestPath(byte classId, UInt16 instanceId = 0)
        //---------------------------------------------------------------------------------------------
        {
            byte[] path = new byte[6];
            path[0] = 0x20;     // logical SegmentId
            path[1] = classId;  // ClassId
            path[2] = 0x25;     // InstanceId in 16 bit notation
            path[3] = 0x00;     // padding byte
            Buffer.BlockCopy(BitConverter.GetBytes((UInt16)instanceId), 0, path, 4, sizeof(UInt16));
            return path;
        }

        /// <summary>
        /// Creates the encrypted request path in CIP notation for ANSI symbolic segment addressing
        /// </summary>
        /// <param name="segment">ANSI symbolic segment</param>
        /// <returns>The byte buffer of the created path</returns>
        //---------------------------------------------------------------------------------------------
        private byte[] GetEncryptedRequestPath(string segment)
        //---------------------------------------------------------------------------------------------
        {
            int lng = ((segment.Length + 1) / 2) * 2 + 2;
            byte[] path = new byte[lng];
            path[0] = 0x91;  // ANSI symbolic segment
            path[1] = (byte)segment.Length;
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(segment), 0, path, 2, segment.Length);
            return path;
        }

        /// <summary>
        /// Creates the requested data buffer for attribute lists
        /// </summary>
        /// <param name="attributes">The requested list of attributes of interest</param>
        /// <returns>The byte buffer of the requested data</returns>
        //---------------------------------------------------------------------------------------------
        private byte[] GetRequestedData(UInt16[] attributes)
        //---------------------------------------------------------------------------------------------
        {
            byte[] data = new byte[(attributes.Length * sizeof(UInt16)) + sizeof(UInt16)];
            Buffer.BlockCopy(BitConverter.GetBytes((UInt16)attributes.Length), 0, data, 0, sizeof(UInt16));
            for (int i = 0; i < attributes.Length; i++) Buffer.BlockCopy(BitConverter.GetBytes((UInt16)attributes[i]), 0, data, (i * sizeof(UInt16) + sizeof(UInt16)), sizeof(UInt16));
            return data;
        }

        /// <summary>
        /// Creates the Encapsulation header, the Common Packet Format and the requested data in CIP frame notation 
        /// </summary>
        /// <param name="eipServiceCallCommand">The CIP service call command</param>
        /// <param name="requestedPath">The requested encrypted path</param>
        /// <param name="requestedData">The requested data</param>
        /// <returns>The created CIP message frame</returns>
        //---------------------------------------------------------------------------------------------
        private byte[] CreateRRDataMessage(byte eipServiceCallCommand, byte[] requestedPath, byte[] requestedData)
        //---------------------------------------------------------------------------------------------
        {
            EipEncapsulation encapsulation = new EipEncapsulation();
            encapsulation.Command = EipEncapsulation.CommandsEnum.SendRRData;
            encapsulation.SessionHandle = this.SessionHandle;
            encapsulation.Length = (UInt16)(18 + requestedPath.Length + requestedData.Length);
            encapsulation.CommandSpecificData.Add(0);  // interface handle CIP
            encapsulation.CommandSpecificData.Add(0);  // interface handle CIP
            encapsulation.CommandSpecificData.Add(0);  // interface handle CIP
            encapsulation.CommandSpecificData.Add(0);  // interface handle CIP
            encapsulation.CommandSpecificData.Add(0);  // timeout
            encapsulation.CommandSpecificData.Add(0);  // timeout
            byte[] encapsulationData = encapsulation.ToBytes();

            EipEncapsulation.CommonPacketFormat commonPacketFormat = new EipEncapsulation.CommonPacketFormat();
            commonPacketFormat.ItemCount = 0x02;
            commonPacketFormat.AddressItem = 0x0000;  // null - used for UCMM Messages
            commonPacketFormat.AddressLength = 0x0000;
            commonPacketFormat.DataItem = 0xB2;
            commonPacketFormat.DataLength = (UInt16)(2 + requestedPath.Length + requestedData.Length);
            commonPacketFormat.Data.Add((byte)eipServiceCallCommand);
            commonPacketFormat.Data.Add((byte)(requestedPath.Length / 2));  // requested path size - amount of 16 bit words
            for (int i = 0; i < requestedPath.Length; i++) commonPacketFormat.Data.Add(requestedPath[i]);
            byte[] commonPacketFormatData = commonPacketFormat.ToBytes();

            // create the message frame data buffer to be sent to the Ethernet/IP device
            byte[] message = new byte[encapsulationData.Length + commonPacketFormatData.Length + requestedData.Length];
            Buffer.BlockCopy(encapsulationData, 0, message, 0, encapsulationData.Length);
            Buffer.BlockCopy(commonPacketFormatData, 0, message, encapsulationData.Length, commonPacketFormatData.Length);
            Buffer.BlockCopy(requestedData, 0, message, encapsulationData.Length + commonPacketFormatData.Length, requestedData.Length);

            return message;
        }

        /// <summary>
        /// Reads the TagList from the Ethernet/IP device
        /// </summary>
        /// <param name="instanceId">The instanceId plus one of the last received Tag. Is set to zero for the first call.</param>
        /// <param name="symbolicSegment">The ANSI symbolic segment name for reading program tags or null for controller tags</param>
        //---------------------------------------------------------------------------------------------
        private void ReadTags(UInt16 instanceId = 0, string symbolicSegment = null)
        //---------------------------------------------------------------------------------------------
        {
            byte[] requestedPath;

            // check if we are evaluating controller tags or program tags
            if (symbolicSegment == null)
            {
                // specify the requested path for the controller tags
                requestedPath = this.GetEncryptedRequestPath(0x6B, instanceId);
            }
            else
            {
                // specify the requested path for the program tags
                byte[] path1 = this.GetEncryptedRequestPath(symbolicSegment);
                byte[] path2 = this.GetEncryptedRequestPath(0x6B, instanceId);
                byte[] path = new byte[path1.Length + path2.Length];
                Buffer.BlockCopy(path1, 0, path, 0, path1.Length);
                Buffer.BlockCopy(path2, 0, path, path1.Length, path2.Length);
                requestedPath = path;
            }

            // specifiy the attributes of interest. 1 = Symbol Name, 2 = Symbol Type, 7 = Element Size, 8 = Dimensions
            byte[] requestedData = this.GetRequestedData(new UInt16[] { 1, 2, 7, 8 });  
            // create the CIP service call message
            byte[] snd = this.CreateRRDataMessage(EipConstants.COMMAND_GET_INSTANCE_ATTRIBUTE_LIST, requestedPath, requestedData);
            // send the message
            this.Stream.Write(snd, 0, snd.Length);
            byte[] rcv = new byte[1024];
            // read the response
            int lng = this.Stream.Read(rcv, 0, rcv.Length);
            // get the status
            byte status = rcv[42];
            // throw an Exception if something went wrong
            if (status != 0 && status != 6) throw new Exception(this.GetStatusCode(status));
            // extract the data of interest
            byte[] data = new byte[lng - 44];
            // decode the response. status = 6 indicates remaining data to be read
            Buffer.BlockCopy(rcv, 44, data, 0, lng - 44);
            this.DecodeTagListData(status, data, symbolicSegment);
        }

        /// <summary>
        /// Decodes the received data of the TagList service call
        /// </summary>
        /// <param name="status">The status of the service call</param>
        /// <param name="data">The received data of the service call</param>
        /// <param name="symbolicSegment">The ANSI symbolic segment name for reading program tags or null for controller tags</param>
        //---------------------------------------------------------------------------------------------
        private void DecodeTagListData(byte status, byte[] data, string symbolicSegment)
        //---------------------------------------------------------------------------------------------
        {
            int offset = 0;
            int instanceId = 0;
            string symbolName;
            UInt16 symbolType;
            UInt16 nameLength;
            UInt16 elementSize;

            while (offset < data.Length)
            {
                instanceId = BitConverter.ToInt32(data, offset);
                offset += sizeof(int);

                nameLength = BitConverter.ToUInt16(data, offset);
                offset += sizeof(UInt16);

                byte[] buffer = new byte[nameLength];
                Buffer.BlockCopy(data, offset, buffer, 0, nameLength);
                ASCIIEncoding encode = new System.Text.ASCIIEncoding();
                symbolName = encode.GetString(buffer);
                offset += nameLength;

                symbolType = BitConverter.ToUInt16(data, offset);
                offset += sizeof(UInt16);

                elementSize = BitConverter.ToUInt16(data, offset);
                offset += sizeof(UInt16);

                uint[] dimensions = new uint[3];
                Buffer.BlockCopy(data, offset, dimensions, 0, dimensions.Length * sizeof(uint));
                offset += dimensions.Length * sizeof(uint);

                if (symbolicSegment != null) symbolName = symbolicSegment + @"." + symbolName;
                EipTag tag = new EipTag(symbolName, (this.LastInstanceId + instanceId), symbolType, elementSize, dimensions);
                this.TagNameCollection.Add(symbolName, tag);
            }

            // if there is remaining data, we continue to read with a recursive call
            if (status == 6) this.ReadTags((UInt16)(++instanceId), symbolicSegment);
            this.LastInstanceId += instanceId;
        }

        /// <summary>
        /// Returns the Program InstanceId of a dedicated Program Name
        /// </summary>
        /// <param name="programName">The ProgramName of interest</param>
        //---------------------------------------------------------------------------------------------
        private UInt16 GetProgramInstanceId(string programName)
        //---------------------------------------------------------------------------------------------
        {
            // specify the requested path
            byte[] requestedPath = this.GetEncryptedRequestPath(programName);
            // specifiy the attributes of interest. 22 = InstanceId
            byte[] requestedData = this.GetRequestedData(new UInt16[] { 22 });  
            // create the CIP service call message
            byte[] snd = this.CreateRRDataMessage(EipConstants.COMMAND_GET_INSTANCE_ATTRIBUTE_LIST, requestedPath, requestedData);
            // send the message
            this.Stream.Write(snd, 0, snd.Length);
            byte[] rcv = new byte[1024];
            // read the response
            int lng = this.Stream.Read(rcv, 0, rcv.Length);
            // get the status
            byte status = rcv[42];
            // throw an Exception if something went wrong
            if (status != 0) throw new Exception(this.GetStatusCode(status));
            // extract the data of interest
            byte[] data = new byte[lng - 44];
            // decode the response
            Buffer.BlockCopy(rcv, 44, data, 0, lng - 44);

            // decode the received data
            UInt16 programInstanceId = 0;  // the third UInt16 word in the response buffer is the Program InstanceId
            int offset = 0;
            int cnt = 0;

            while (offset < data.Length)
            {
                programInstanceId = BitConverter.ToUInt16(data, offset);
                offset += sizeof(UInt16);
                cnt++;
                if (cnt == 3) break;  
            }

            return programInstanceId;
        }

        /// <summary>
        /// Reads the template data from the Ethernet/IP device
        /// </summary>
        /// <param name="instanceId">The InstanceID of the requested tag</param>
        /// <returns>The EipTemplate object instance</returns>
        //---------------------------------------------------------------------------------------------
        private EipTemplate ReadTemplate(UInt16 instanceId)
        //---------------------------------------------------------------------------------------------
        {
            instanceId = this.GetTemplateInstanceFromSymbolType(instanceId);  // ensure valid InstanceId range
            EipTemplate template = new EipTemplate(instanceId);

            // specify the requested path
            byte[] requestedPath = this.GetEncryptedRequestPath(0x6C, instanceId);
            // specifiy the attributes of interest. 4 = Object definition size, 5 = Structure size, 2 = Member count, 1 = Structure handle
            byte[] requestedData = this.GetRequestedData(new UInt16[] { 4, 5, 2, 1 });  
            // create the CIP service call message
            byte[] snd = this.CreateRRDataMessage(EipConstants.COMMAND_GET_ATTRIBUTE_LIST, requestedPath, requestedData);
            // send the message
            this.Stream.Write(snd, 0, snd.Length);
            byte[] rcv = new byte[1024];
            // read the response
            int lng = this.Stream.Read(rcv, 0, rcv.Length);
            // get the status
            byte status = rcv[42];
            // throw an Exception if something went wrong
            if (status != 0) throw new Exception(this.GetStatusCode(status));
            // extract the data of interest
            byte[] data = new byte[lng - 44];
            // decode the response
            Buffer.BlockCopy(rcv, 44, data, 0, lng - 44);
            template.TemplateDefinitionStructureSize = BitConverter.ToInt32(data, 6);
            template.TransferredWireBytesOnRead = BitConverter.ToInt32(data, 14);
            template.MemberCount = BitConverter.ToUInt16(data, 22);
            template.StructureHandle = BitConverter.ToUInt16(data, 28);
            UInt16 numberOfBytesToBeRead = (UInt16)((template.TemplateDefinitionStructureSize * 4) - 23);
            byte[] dataBuffer = new byte[numberOfBytesToBeRead];
            // read the template members
            this.ReadTemplateMember(template, template.MemberCount, dataBuffer, numberOfBytesToBeRead);
            // decode the template member response
            this.DecodeTemplateMemberData(template, template.MemberCount, dataBuffer);
            // remove the forbidden template members
            this.RemoveForbiddenMembersFromTemplate(template);

            return template;
        }

        /// <summary>
        /// Reads the template member and the member properties from the Ethernet/IP device
        /// </summary>
        /// <param name="template">The template as the information container</param>
        /// <param name="memberCount">The amount of members</param>
        /// <param name="dataBuffer">The data buffer to be filled</param>
        /// <param name="numberOfBytesToBeRead">The expected amount of byte as the response of the service call</param>
        /// <param name="offset">The offset in the data buffer, if the device is sending in packages</param>
        //---------------------------------------------------------------------------------------------
        private void ReadTemplateMember(EipTemplate template, UInt16 memberCount, byte[] dataBuffer, UInt16 numberOfBytesToBeRead, int offset = 0)
        //---------------------------------------------------------------------------------------------
        {
            UInt16 readLength = (UInt16)(numberOfBytesToBeRead - (UInt16)offset);
            // specify the requested path
            byte[] requestedPath = this.GetEncryptedRequestPath(0x6C, (UInt16)template.InstanceId);
            // specifiy the requested data
            byte[] requestedData = new byte[6];
            Buffer.BlockCopy(BitConverter.GetBytes((int)offset), 0, requestedData, 0, sizeof(int));
            Buffer.BlockCopy(BitConverter.GetBytes((UInt16)readLength), 0, requestedData, sizeof(int), sizeof(UInt16));
            // create the CIP service call message
            byte[] snd = this.CreateRRDataMessage(EipConstants.COMMAND_READ_TEMPLATE, requestedPath, requestedData);
            // send the message
            this.Stream.Write(snd, 0, snd.Length);
            byte[] rcv = new byte[1024];
            // read the response
            int lng = this.Stream.Read(rcv, 0, rcv.Length);
            // get the status
            byte status = rcv[42];
            // throw an Exception if something went wrong
            if (status != 0 && status != 6) throw new Exception(this.GetStatusCode(status));
            // extract the data of interest
            Buffer.BlockCopy(rcv, 44, dataBuffer, offset, lng - 44);
            offset += lng - 44;
            // if there is more data to read, we need to place a recursive call 
            if (status == 6) this.ReadTemplateMember(template, memberCount, dataBuffer, numberOfBytesToBeRead, offset);
        }

        /// <summary>
        /// Decodes the template data of the ReadTemplate service call 
        /// </summary>
        /// <param name="template">The template as the information container</param>
        /// <param name="memberCount">The amount of members</param>
        /// <param name="data">The data buffer to be analysed</param>
        //---------------------------------------------------------------------------------------------
        private void DecodeTemplateMemberData(EipTemplate template, UInt16 memberCount, byte[] data)
        //---------------------------------------------------------------------------------------------
        {
            int offset = 0;
            UInt16 memberInfo;
            UInt16 memberType;
            int memberOffset;
            UInt16 memberIndex = 0;

            while (offset < data.Length)
            {
                if (template.TemplateMemberList.Count < memberCount)
                {
                    // get the member properties
                    memberInfo = BitConverter.ToUInt16(data, offset);
                    offset += sizeof(UInt16);
                    memberType = BitConverter.ToUInt16(data, offset);
                    offset += sizeof(UInt16);
                    memberOffset = BitConverter.ToInt32(data, offset);
                    offset += sizeof(Int32);
                    template.TemplateMemberList.Add(new EipTemplateMember(null, memberInfo, memberType, memberOffset));
                }
                else
                {
                    if (template.TemplateName == null)
                    {
                        // get the template name and remove the unused data
                        template.TemplateName = this.GetNullTerminatedStringFromDataBuffer(data, ref offset);
                        int pos = template.TemplateName.IndexOf(@";");
                        if (pos != -1) template.TemplateName = template.TemplateName.Substring(0, pos);
                    }
                    else
                    {
                        if (memberIndex < memberCount)
                        {
                            // throw an Exception if an index error is detected - we should never see this Exception
                            if ((memberIndex + 1) > template.TemplateMemberList.Count) throw new Exception(@"Error in Template Member decoding!");
                            // get the member name and update the member name in the member object
                            EipTemplateMember member = template.TemplateMemberList.ElementAt(memberIndex);
                            member.MemberName = this.GetNullTerminatedStringFromDataBuffer(data, ref offset);
                            memberIndex++;
                        }
                        else offset = data.Length;  // we're done - no more data to analyse
                    }
                }
            }
        }

        /// <summary>
        /// Removes the forbidden member tags from the template - see Rockwell publication: 1756-PM020G-EN-P.pdf
        /// </summary>
        /// <param name="template">The template as the information container</param>
        //---------------------------------------------------------------------------------------------
        private void RemoveForbiddenMembersFromTemplate(EipTemplate template)
        //---------------------------------------------------------------------------------------------
        {
            bool forbiddenMember;
            List<EipTemplateMember> allowedMembers = new List<EipTemplateMember>();

            foreach (EipTemplateMember member in template.TemplateMemberList)
            {
                forbiddenMember = false;
                // remove the member if it is a reserved tag - bit 12 = 1
                if (forbiddenMember == false) forbiddenMember = this.IsReservedTag(member.MemberType);
                // remove the member if it is a structured tag in the forbidden range of 0x100 >= x <= 0xEFF
                if (forbiddenMember == false) if (this.IsStructuredTag(member.MemberType)) forbiddenMember = this.IsTagInForbiddenRange(member.MemberType);
                // remove the member if the tag has two leading underscores, e.g. "__AnyTag"
                if (forbiddenMember == false) forbiddenMember = this.HasLeadingDoubleUnderscores(member.MemberName);
                // remove the member if the tag has ":" in the tag name, e.g. Local:1:C
                //if (forbiddenTag == false) forbiddenTag = this.HasColon(tagName);  // keep these tags for now
                if (forbiddenMember == false) allowedMembers.Add(member);
            }

            // assign the list of the allowed members to the template
            template.TemplateMemberList = allowedMembers;
        }

        /// <summary>
        /// Reads a null terminated string from a byte buffer starting at the offset until a null byte is found
        /// </summary>
        /// <param name="data">The data buffer where to be read from</param>
        /// <param name="offset">The offset in the data buffer where to start from</param>
        /// <returns>The requested string</returns>
        //---------------------------------------------------------------------------------------------
        private string GetNullTerminatedStringFromDataBuffer(byte[] data, ref int offset)
        //---------------------------------------------------------------------------------------------
        {
            int lng = 0;
            string value = null;

            for (int i = offset; i < data.Length; i++)
            {
                if (data[i] == 0) break;
                else lng++;
            }

            if (lng > 0)
            {
                byte[] buffer = new byte[lng];
                Buffer.BlockCopy(data, offset, buffer, 0, lng);
                ASCIIEncoding encode = new System.Text.ASCIIEncoding();
                value = encode.GetString(buffer);
                offset += lng + 1;
            }

            return value;
        }

        /// <summary>
        /// Returns the Data Type of the PLC object as a string
        /// </summary>
        /// <param name="type">The Type of the PLC oject</param>
        /// <param name="dimensions">The Dimensions of the PLC object</param>
        /// <param name="detailledDescription">Reference string to provide more detailled information about the tag type</param>
        /// <returns>The tag data type as string</returns>
        //-----------------------------------------------------------------------------------------
        public string GetTagDataType(int type, uint[] dimensions, out string detailledDescription)
        //-----------------------------------------------------------------------------------------
        {
            string value;
            string array = @"";

            bool isReserved = this.IsReservedTag((UInt16)type);
            bool isArray = this.IsArrayTag((UInt16)type);
            bool isUDT = this.IsStructuredTag((UInt16)type);

            if (isArray)
            {
                array = @"[" + dimensions[0].ToString();
                if (dimensions[1] > 0) array += @"," + dimensions[1].ToString();
                if (dimensions[2] > 0) array += @"," + dimensions[2].ToString();
                array += @"]";
            }

            switch (type & 0xFFF)
            {
                case 0xC1:
                    value = @"BOOL{0}";
                    detailledDescription = @"Boolean value, 1 bit";
                    break;
                case 0xC2:
                    value = @"SINT{0}";
                    detailledDescription = @"Signed 8–bit integer value";
                    break;
                case 0xC3:
                    value = @"INT{0}";
                    detailledDescription = @"Signed 16–bit integer value";
                    break;
                case 0xC4:
                    value = @"DINT{0}";
                    detailledDescription = @"Signed 32–bit integer value";
                    break;
                case 0xC5:
                    value = @"LINT{0}";
                    detailledDescription = @"Signed 64–bit integer value";
                    break;
                case 0xC6:
                    value = @"SINT{0}";
                    detailledDescription = @"Unsigned 8–bit integer value";
                    break;
                case 0xC7:
                    value = @"INT{0}";
                    detailledDescription = @"Unsigned 16–bit integer value";
                    break;
                case 0xC8:
                    value = @"DINT{0}";
                    detailledDescription = @"Unsigned 32–bit integer value";
                    break;
                case 0xC9:
                    value = @"LINT{0}";
                    detailledDescription = @"Unsigned 64–bit integer value";
                    break;
                case 0xCA:
                    value = @"REAL{0}";
                    detailledDescription = @"32–bit floating point value, IEEE format";
                    break;
                case 0xCB:
                    value = @"LREAL{0}";
                    detailledDescription = @"64–bit floating point value, IEEE format";
                    break;
                case 0xCC:
                    value = @"LINT{0}";
                    detailledDescription = @"Synchronous time value";
                    break;
                case 0xCD:
                    value = @"LINT{0}";
                    detailledDescription = @"Date value";
                    break;
                case 0xCE:
                    value = @"LINT{0}";
                    detailledDescription = @"Time of day value";
                    break;
                case 0xCF:
                    value = @"LINT{0}";
                    detailledDescription = @"Date and time of day value";
                    break;
                case 0xD0:
                    value = @"STRING{0}";
                    detailledDescription = @"Character string, 1 byte per character";
                    break;
                case 0xD1:
                    value = @"SINT{0}";
                    detailledDescription = @"8-bit bit string";
                    break;
                case 0xD2:
                    value = @"INT{0}";
                    detailledDescription = @"16-bit bit string";
                    break;
                case 0xD3:
                    value = @"DINT{0}";
                    detailledDescription = @"32-bit bit string";
                    break;
                case 0xD4:
                    value = @"LINT{0}";
                    detailledDescription = @"64-bit bit string";
                    break;
                case 0xD5:
                    value = @"STRING{0}";
                    detailledDescription = @"Wide char character string, 2 bytes per character";
                    break;
                case 0xD6:
                    value = @"LINT{0}";
                    detailledDescription = @"High resolution duration value";
                    break;
                case 0xD7:
                    value = @"LINT{0}";
                    detailledDescription = @"Medium resolution duration value";
                    break;
                case 0xD8:
                    value = @"LINT{0}";
                    detailledDescription = @"Low resolution duration value";
                    break;
                case 0xD9:
                    value = @"STRING{0}";
                    detailledDescription = @"N-byte per char character string";
                    break;
                case 0xDA:
                    value = @"STRING{0}";
                    detailledDescription = @"Counted character sting with 1 byte per character and 1 byte length indicator";
                    break;
                case 0xDB:
                    value = @"LINT{0}";
                    detailledDescription = @"Duration in milliseconds";
                    break;
                case 0xDC:
                    value = @"STRING{0}";
                    detailledDescription = @"CIP path segment";
                    break;
                case 0xDD:
                    value = @"STRING{0}";
                    detailledDescription = @"Engineering units";
                    break;
                case 0xDE:
                    value = @"STRING{0}";
                    detailledDescription = @"International character string";
                    break;
                case 0xA0:
                    value = @"UDT{0}";
                    detailledDescription = @"Data is an abbreviated struct type, i.e. a CRC of the actual type descriptor";
                    break;
                case 0xA1:
                    value = @"ARRAY{0}";
                    detailledDescription = @"Data is an abbreviated array type. The limits are left off";
                    break;
                case 0xA2:
                    value = @"UDT{0}";
                    detailledDescription = @"Data is a struct type descriptor";
                    break;
                case 0xA3:
                    value = @"ARRAY{0}";
                    detailledDescription = @"Data is an array type descriptor";
                    break;
                default:
                    value = @"unknown{0}";
                    detailledDescription = @"unknown";
                    break;
            }

            value = String.Format(value, array);

            if (isReserved)
            {
                value = @"RESERVED";
                detailledDescription = @"RESERVED TAG - don't play with it!";
            }

            if (isUDT)
            {
                value = @"UDT";
                // the type of the UDT is the name of the template - set the detailledDescription with the template name and the UDT size in bytes
                EipTemplate template = this.GetTemplateByInstance(this.GetTemplateInstanceFromSymbolType((UInt16)type));
                if (template != null) detailledDescription = template.TemplateName + array + @" - UDT (" + template.TransferredWireBytesOnRead.ToString() + @" Bytes)";
                else detailledDescription = @"UNKNOWN UDT - there seems to be something wrong!";  // we should never get this
            }

            return value;
        }

        /// <summary>
        /// Returns the status code of the CIP service call in a human readable form
        /// </summary>
        /// <param name="code">The status code</param>
        /// <returns>The status code in a human readable form</returns>
        //---------------------------------------------------------------------------------------------
        public string GetStatusCode(byte code)
        //---------------------------------------------------------------------------------------------
        {
            switch (code)
            {
                case 0x00: return @"Success";
                case 0x01: return @"Connection failure";
                case 0x02: return @"Resource unavailable";
                case 0x03: return @"Invalid parameter value";
                case 0x04: return @"Path segment error";
                case 0x05: return @"Path destination unknown";
                case 0x06: return @"Partial transfer - more data to read";
                case 0x07: return @"Connection lost";
                case 0x08: return @"Service not supported";
                case 0x09: return @"Invalid attribute value";
                case 0x0A: return @"Attribute list error";
                case 0x0B: return @"Already in requested mode/state";
                case 0x0C: return @"Object state conflict";
                case 0x0D: return @"Object already exists";
                case 0x0E: return @"Attribute not settable";
                case 0x0F: return @"Privilege violation";
                case 0x10: return @"Device state conflict";
                case 0x11: return @"Reply data too large";
                case 0x12: return @"Fragmentation of a primitive value";
                case 0x13: return @"Not enough data";
                case 0x14: return @"Attribute not supported";
                case 0x15: return @"Too much data";
                case 0x16: return @"Object does not exist";
                case 0x17: return @"Service fragmentation sequence not in progress";
                case 0x18: return @"No stored attribute data";
                case 0x19: return @"Store operation failure";
                case 0x1A: return @"Routing failure, request packet too large";
                case 0x1B: return @"Routing failure, response packet too large";
                case 0x1C: return @"Missing attribute list entry data";
                case 0x1D: return @"Invalid attribute value list";
                case 0x1E: return @"Embedded service error";
                case 0x1F: return @"Vendor specific error";
                case 0x20: return @"Invalid parameter";
                case 0x21: return @"Write-once value or medium already written";
                case 0x22: return @"Invalid reply received";
                case 0x23: return @"Buffer overflow";
                case 0x24: return @"Message format error";
                case 0x25: return @"Key failure path";
                case 0x26: return @"Path size invalid";
                case 0x27: return @"Unexpected attribute list";
                case 0x28: return @"Invalid Member ID";
                case 0x29: return @"Member not setable";
                case 0x2A: return @"Group 2 only Server failure";
                case 0x2B: return @"Unknown Modbus Error";
                default: return @"unknown";
            }
        }
    }
}
