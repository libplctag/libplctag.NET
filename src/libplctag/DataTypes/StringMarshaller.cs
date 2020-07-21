using System;
using System.Text;

namespace libplctag.DataTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class StringMarshaller : IMarshaller<string>
    {

        const int MAX_LOGIX_STRING_LENGTH = 82;
        const int MAX_PCCC_STRING_LENGTH = 80;

        public CpuType CpuType { get; set; }

        public int ElementSize
        {
            get
            {
                switch (CpuType)
                {
                    case CpuType.Logix: return 88;
                    case CpuType.Plc5: return 88;
                    case CpuType.Slc500: return 25;
                    case CpuType.LogixPccc: return 84;
                    case CpuType.Micro800: return 256;
                    case CpuType.MicroLogix: return 256;
                    default: throw new NotImplementedException();
                }
            }
        }


        public string Decode(Tag tag, int offset)
        {
            switch (CpuType)
            {
                case CpuType.Logix: return LogixDecode(tag, offset);
                case CpuType.Plc5: throw new NotImplementedException();
                case CpuType.Slc500: throw new NotImplementedException();
                case CpuType.LogixPccc: return PcccDecode(tag, offset);
                case CpuType.Micro800: throw new NotImplementedException();
                case CpuType.MicroLogix: throw new NotImplementedException();
                default: throw new NotImplementedException();
            }
        }

        public void Encode(Tag tag, int offset, string value)
        {
            switch (CpuType)
            {
                case CpuType.Logix: LogixEncode(tag, offset, value); break;
                case CpuType.Plc5: throw new NotImplementedException();
                case CpuType.Slc500: throw new NotImplementedException();
                case CpuType.LogixPccc: PcccEncode(tag, offset, value); break;
                case CpuType.Micro800: throw new NotImplementedException();
                case CpuType.MicroLogix: throw new NotImplementedException();
                default: break;
            }
        }



        string PcccDecode(Tag tag, int offset)
        {
            var apparentStringLength = (int)tag.GetInt16(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_PCCC_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];
            for (int ii = 0; ii < actualStringLength; ii++)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 2 + 2 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }


        string LogixDecode(Tag tag, int offset)
        {
            var apparentStringLength = tag.GetInt32(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_LOGIX_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];
            for (int ii = 0; ii < actualStringLength; ii++)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 4 + 2 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }





        void PcccEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_PCCC_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int i = 0; i < asciiEncodedString.Length; i++)
            {
                tag.SetUInt8(offset + i + 2 + 2, Convert.ToByte(asciiEncodedString[i]));
            }
        }


        void LogixEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_LOGIX_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int i = 0; i < asciiEncodedString.Length; i++)
            {
                tag.SetUInt8(offset + i + 2 + 2, Convert.ToByte(asciiEncodedString[i]));
            }
        }


    }
}
