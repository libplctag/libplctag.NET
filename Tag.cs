using System;
using System.Text;

namespace libplctag
{
    //$"protocol=ab_eip&gateway=192.168.0.100&cpu=SLC&elem_size={ELEM_SIZE}&elem_count={ELEM_COUNT}&name=F8:0&debug=1";
    public class Tag
    {
        public string IpAddress { get; }

        public CpuType Cpu { get; }

        public string Name { get; }

        public int ElementSize { get; }

        public int ElementCount { get; }

        public string UniqueKey { get; }

        /// <summary>
        /// Creates a tag for PLC5 / SLC / MicroLogix processor types (the path is not specified)
        /// </summary>
        /// <param name="ipAddress">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="cpuType">AB CPU models</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="elementSize">The size of an element in bytes. The tag is assumed to be composed of elements of the same size. For structure tags, use the total size of the structure.</param>
        /// <param name="elementCount">elements count: 1- single, n-array.</param>
        /// <param name="debugLevel"></param>
        public Tag(string ipAddress, CpuType cpuType, string name, int elementSize, int elementCount, int debugLevel = 0)
            : this(ipAddress, string.Empty, cpuType, name, elementSize, elementCount, debugLevel)
        {

        }

        /// <summary>
        /// Creates a tag. If the CPU type is LGX, the port type and slot has to be specified.
        /// </summary>
        /// <param name="ipAddress">IP address of the gateway for this protocol. Could be the IP address of the PLC you want to access.</param>
        /// <param name="path">Required for LGX, Optional for PLC/SLC/MLGX IOI path to access the PLC from the gateway.
        /// <para></para>Communication Port Type: 1- Backplane, 2- Control Net/Ethernet, DH+ Channel A, DH+ Channel B, 3- Serial
        /// <para></para> Slot number where cpu is installed: 0,1.. </param>
        /// <param name="slot"></param>
        /// <param name="cpuType">AB CPU models</param>
        /// <param name="name">The textual name of the tag to access. The name is anything allowed by the protocol. E.g. myDataStruct.rotationTimer.ACC, myDINTArray[42] etc.</param>
        /// <param name="elementSize">The size of an element in bytes. The tag is assumed to be composed of elements of the same size. For structure tags, use the total size of the structure.</param>
        /// <param name="elementCount">elements count: 1- single, n-array.</param>
        /// <param name="debugLevel"></param>
        public Tag(string ipAddress, string path, CpuType cpuType, string name, int elementSize, int elementCount, int debugLevel = 0)
        {
            if (cpuType == CpuType.LGX && string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("PortType and Slot must be specified for Controllogix / Compactlogix processors");
            }
            IpAddress = ipAddress;
            Cpu = cpuType;
            Name = name;
            ElementSize = elementSize;
            ElementCount = elementCount;
            var sb = new StringBuilder();
            sb.Append($"protocol=ab_eip&gateway={ipAddress}");
            if (!string.IsNullOrEmpty(path))
            {
                sb.Append($"&path={path}");
            }
            sb.Append($"&cpu={cpuType}&elem_size={ElementSize}&elem_count={elementCount}&name={name}");
            if (debugLevel > 0)
            {
                sb.Append($"&debug={debugLevel}");
            }
            UniqueKey = sb.ToString();
        }
    }
}