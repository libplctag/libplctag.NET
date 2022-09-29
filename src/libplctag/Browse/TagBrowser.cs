using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace libplctag.Browse
{
    /// <summary>
    /// Allows you to load all Inventory data from the PLC
    /// </summary>
    public class TagBrowser
    {
        public ICollection<TagEntry> ControllerTags { get; set; } = new List<TagEntry>();
        public ICollection<ProgramEntry> Programs { get; set; } = new List<ProgramEntry>();
        public IDictionary<int, UdtEntry> Udts { get; set; } = new Dictionary<int, UdtEntry>();
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(1000);

        //These Masks are used to decode the TagEntryFlags of TagEntries
        private int TYPE_IS_STRUCT = 0x8000;
        private int TYPE_IS_SYSTEM = 0x1000;
        private int TYPE_DIM_MASK = 0x6000;
        private int TYPE_UDT_ID_MASK = 0x0FFF;

        private string GateWayIpAddress;
        private string CpuPath;
        private PlcType PlcType;

        /// <summary>
        /// Creates an new Tag browser to upload Tag inventory from the PLC
        /// </summary>
        /// <param name="GateWayIpAddress">Basically the IP address over which the CPU can be reached. Usually the IP of the Connected Communication processor</param>
        /// <param name="CpuPath">
        /// This attribute is required for CompactLogix/ControlLogix tags and for tags using
        /// a DH+ protocol bridge (i.e. a DHRIO module) to get to a PLC/5, SLC 500, or MicroLogix
        /// PLC on a remote DH+ link. The attribute is ignored if it is not a DH+ bridge
        /// route, but will generate a warning if debugging is active. Note that Micro800
        /// connections must not have a path attribute.
        /// 
        /// The format is "Cpu Slot, Cpu Connection" where 
        ///     Cpu Connection: 1 Backplane, 2 Ethernet, 3 Serial
        ///     CpuSlot: the slot where the Cpu is inserted
        ///     
        /// Usually it is "0,1"
        /// </param>
        /// <param name="PlcType">The type of PLC</param>
        public TagBrowser(string GateWayIpAddress, string CpuPath, PlcType PlcType)
        {
            this.GateWayIpAddress = GateWayIpAddress;
            this.CpuPath = CpuPath;
            this.PlcType = PlcType;
        }

        /// <summary>
        /// Connect to the PLC and refresh all data
        /// </summary>
        public void Browse()
        {
            ControllerTags.Clear();
            Programs.Clear();
            Udts.Clear();

            RefreshControllerTags(GateWayIpAddress, CpuPath);
            RefreshProgramsTags(GateWayIpAddress, CpuPath);
            try
            {
                RefreshUdts(GateWayIpAddress, CpuPath);
            }
            catch (Exception)
            {
            }//some plcs do not support Udts, so ignore them
        }

        private void RefreshControllerTags(string GateWayIpAddress, string CpuPath)
        {

            //set up the Tag that will let us upload the Inventory information
            var tag = new libplctag.Tag();
            tag.Name = "@tags"; //this tag gives us all Controller tags
            tag.Gateway = GateWayIpAddress;
            tag.Path = CpuPath;
            tag.PlcType = PlcType;
            tag.Protocol = Protocol.ab_eip;
            tag.Timeout = Timeout;

            //Read the Tags data, which comes as an byte[]
            tag.Initialize();
            tag.Read();
            var v = tag.GetBuffer();
            tag.Dispose();

            //Parse the Tag-List and separate Program Tags from normal Tags
            foreach (var tagentry in ParseTagEntries(v))
            {
                tagentry.TagBrowser = this;
                if (tagentry is ProgramEntry)
                {
                    Programs.Add((ProgramEntry)tagentry);
                }
                else
                {
                    ControllerTags.Add(tagentry);
                }
            }
        }

        private void RefreshProgramsTags(string GateWayIpAddress, string CpuPath)
        {
            foreach (var Program in Programs)
            {
                var tag = new libplctag.Tag();
                tag.Name =String.Format("{0}.@tags", Program.Name); //this tag gives us all Controller tags       
                tag.Gateway = GateWayIpAddress;
                tag.Path = CpuPath;
                tag.PlcType = PlcType;
                tag.Protocol = Protocol.ab_eip;
                tag.Timeout = Timeout;

                //Read the Tags data, which comes as an byte[]
                tag.Initialize();
                tag.Read();
                var v = tag.GetBuffer();
                tag.Dispose();

                foreach (var tagentry in ParseTagEntries(v))
                {
                    tagentry.TagBrowser = this;
                    Program.Tags.Add(tagentry);
                    //if (tagentry is ProgramEntry) Program.Routines.Add(tagentry);
                    //else Program.Tags.Add(tagentry);
                }
            }
        }

        private void RefreshUdts(string GateWayIpAddress, string CpuPath)
        {
            var ProcessedUdtIds = new List<int>();

            foreach (var tag in ControllerTags)
            {
                if (!tag.isStruct) continue;
                if (ProcessedUdtIds.Contains(tag.UdtId)) continue;
                RefreshUdt(tag.UdtId);
            }
            foreach (var Program in Programs)
            {
                foreach (var tag in Program.Tags)
                {
                    if (!tag.isStruct) continue;
                    if (ProcessedUdtIds.Contains(tag.UdtId)) continue;
                    RefreshUdt(tag.UdtId);
                }
            }
        }

        private void RefreshUdt(int UdtId)
        {
            if (Udts.ContainsKey(UdtId)) return;

            var tag = new libplctag.Tag();
            tag.Name =String.Format("@udt/{0}", UdtId); //this tag gives us all Controller tags       
            tag.Gateway = GateWayIpAddress;
            tag.Path = CpuPath;
            tag.PlcType = PlcType;
            tag.Protocol = Protocol.ab_eip;
            tag.Timeout = Timeout;

            //Read the Tags data, which comes as an byte[]
            tag.Initialize();
            tag.Read();
            var v = tag.GetBuffer();
            tag.Dispose();

            var udt = ParseUdtEntry(v);
            udt.TagBrowser = this;
            Udts.Add(udt.ID, udt);

            //go through its fields and Also update the Udt if needed
            foreach (var field in udt.Fields)
            {
                if (!field.isStruct) continue;
                RefreshUdt(field.UdtId);
            }
        }

        private ICollection<TagEntry> ParseTagEntries(byte[] rawBytes)
        {
            //each entry looks like this:
            //uint32_t instance_id    monotonically increasing but not contiguous
            //uint16_t symbol_type    type of the symbol.
            //uint16_t element_length length of one array element in bytes.
            //uint32_t array_dims[3]  array dimensions.
            //uint16_t string_len     string length count.
            //uint8_t string_data[]   string bytes (string_len of them)

            int idx = 0;
            var Tags = new List<TagEntry>();


            int Id;
            int Flags;
            int Size;
            int[] ArrayDims = new int[3];
            int NameLength;
            string Name;

            while (idx < rawBytes.Length)
            {
                //Parse the Basic Information
                Id = BitConverter.ToInt32(rawBytes, idx); idx+=4;
                Flags = BitConverter.ToInt16(rawBytes, idx); idx+=2;
                Size = BitConverter.ToInt16(rawBytes, idx); idx+=2;
                ArrayDims[0] = BitConverter.ToInt32(rawBytes, idx); idx+=4;
                ArrayDims[1] = BitConverter.ToInt32(rawBytes, idx); idx+=4;
                ArrayDims[2] = BitConverter.ToInt32(rawBytes, idx); idx+=4;
                NameLength = BitConverter.ToInt16(rawBytes, idx); idx+=2;

                Name = ASCIIEncoding.ASCII.GetString(rawBytes, idx, NameLength); idx+=NameLength;

                //Filter out invalid Tag Names
                if (Name.StartsWith("Map:Local:")) continue; //i think, these are Aliases
                if (Name.StartsWith("Task:")) continue; //Tasks do not have tags
                if (Name.StartsWith("Routine:")) continue; //Routines do not have tags

                //now it may be an Normal Tag, or an Program Tag
                TagEntry Tag;
                if (Name.StartsWith("Program:")) Tag= new ProgramEntry();
                else Tag = new TagEntry();

                //Assign Information to the Tag
                Tag.ID = Id;
                Tag.Flags = Flags;
                Tag.ElementSize = Size;
                Tag.ArrayDims[0] =  ArrayDims[0];
                Tag.ArrayDims[1] =  ArrayDims[1];
                Tag.ArrayDims[2] =  ArrayDims[2];

                Tag.Name = Name;

                //Now evaluate the Flags
                Tag.ArrayDimCount = (Tag.Flags & TYPE_DIM_MASK) >> 13;
                Tag.isStruct = (Tag.Flags & TYPE_IS_STRUCT) != 0;
                Tag.isSystem = (Tag.Flags & TYPE_IS_SYSTEM) != 0;
                Tag.UdtId = (Tag.Flags & TYPE_UDT_ID_MASK);

                //Count up the element count. Essentially the element count
                //is all Array dimensions multiplied by one another
                Tag.ElementCount = 1;
                for (int i = 0; i < Tag.ArrayDimCount; i++) //go through all Array Dimensions, and multiply them together
                {
                    Tag.ElementCount *= Tag.ArrayDims[i];
                }

                Tags.Add(Tag);
            }
            return Tags;
        }

        private UdtEntry ParseUdtEntry(byte[] rawBytes)
        {
            /* the format in the tag buffer is:
            *
            * A new header of 14 bytes:
            *
            * Bytes   Meaning
            * 0-1     16-bit UDT ID
            * 2-5     32-bit UDT member description size, in 32-bit words.
            * 6-9     32-bit UDT instance size, in bytes.
            * 10-11   16-bit UDT number of members (fields).
            * 12-13   16-bit UDT handle/type.
            *
            * Then the raw field info.
            *
            * N x field info entries
            *     uint16_t field_metadata - array element count or bit field number
            *     uint16_t field_type
            *     uint32_t field_offset
            *
            * int8_t string - zero-terminated string, UDT name, but name stops at first semicolon!
            *
            * N x field names
            *     int8_t string - zero-terminated.
            *
            */
            int idx = 0;
            var udt = new UdtEntry();

            //Parse the Basic Information
            udt.ID = BitConverter.ToInt16(rawBytes, 0);
            udt.InstanceSize = BitConverter.ToInt32(rawBytes, 6);
            udt.FieldCount = BitConverter.ToInt16(rawBytes, 10);
            udt.StructHandle = BitConverter.ToInt16(rawBytes, 12);

            //now skip Header to Field information
            idx = 14; //Header is 14 bytes long

            //Now load all Fields
            for (int i = 0; i <udt.FieldCount; i++)
            {
                var field = new UdtEntry.UdtField();
                field.TagBrowser = this;
                field.Metadata = BitConverter.ToInt16(rawBytes, idx); idx+=2;
                field.Flags = BitConverter.ToInt16(rawBytes, idx); idx+=2;
                field.Offset = BitConverter.ToInt32(rawBytes, idx); idx+=4;

                //Parse Fields
                field.isStruct = (field.Flags & TYPE_IS_STRUCT) != 0;
                field.isSystem = (field.Flags & TYPE_IS_SYSTEM) != 0;
                field.UdtId = (field.Flags & TYPE_UDT_ID_MASK);
                udt.Fields.Add(field);
            }

            /*
             * then get the template/UDT name.   This is weird.
             * Scan until we see a 0x3B, semicolon, byte.   That is the end of the
             * template name.   Actually we should look for ";n" but the semicolon
             * seems to be enough for now.
             */
            var Name = new System.Text.StringBuilder();
            while (rawBytes[idx] != 0x3b & rawBytes[idx] != 0)
            {
                Name.Append(ASCIIEncoding.ASCII.GetString(rawBytes, idx, 1));
                idx++;
            }
            udt.Name = Name.ToString();

            /*
             * This is the second section of the data, the field names.   They appear
             * to be zero terminated.
             */
            idx++; //Skip past last character parsed
            Name.Clear();
            for (int i = 0; i < udt.FieldCount; i++)
            {
                while (rawBytes[idx] != 0)
                {
                    Name.Append(ASCIIEncoding.ASCII.GetString(rawBytes, idx, 1));
                    idx++;
                }
                idx++; //Skip past last character parsed;
                udt.Fields[i].Name = Name.ToString();
                Name.Clear();
            }

            return udt;
        }
    }
}