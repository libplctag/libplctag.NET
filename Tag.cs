using System;
using System.Net;
using System.Text;

namespace libplctag
{

    public class Tag
    {
        public IPAddress IPAddress { get; }

        public CpuType Cpu { get; }

        public string Name { get; }

        public int ElementSize { get; }

        public int ElementCount { get; }

        public string UniqueKey { get; }

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
        public Tag(IPAddress ipAddress, string path, CpuType cpuType, string name, int elementSize, int elementCount, int debugLevel = 0)
        {

            if (cpuType == CpuType.LGX && string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("PortType and Slot must be specified for Controllogix / Compactlogix processors");
            }

            IPAddress = ipAddress;
            Cpu = cpuType;
            Name = name;
            ElementSize = elementSize;
            ElementCount = elementCount;
            UniqueKey = GetUniqueKey(IPAddress.ToString(), path, cpuType, name, elementSize, elementCount, debugLevel);
            
        }

        private string GetUniqueKey(string ipAddress, string path, CpuType cpuType, string name, int elementSize, int elementCount, int debugLevel)
        {

            var sb = new StringBuilder();

            var protocol = "ab_eip";
            sb.Append($"protocol={protocol}");
            sb.Append($"gateway={ipAddress}");

            if (!string.IsNullOrEmpty(path))
            {
                sb.Append($"&path={path}");
            }

            sb.Append($"&cpu={cpuType}");
            sb.Append($"&elem_size={elementSize}");
            sb.Append($"&elem_count={elementCount}");
            sb.Append($"&name={name}");

            if (debugLevel > 0)
            {
                sb.Append($"&debug={debugLevel}");
            }

            return sb.ToString();

        }
    }

}