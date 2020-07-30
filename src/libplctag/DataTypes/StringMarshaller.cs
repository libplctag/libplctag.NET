using System;
using System.Collections.Generic;
using System.Text;

namespace libplctag.DataTypes
{
    public class StringMarshaller : Marshaller<string>
    {

        const int MAX_CONTROLLOGIX_STRING_LENGTH = 82;
        const int MAX_LOGIXPCCC_STRING_LENGTH = 80;

        override public int? ElementSize
        {
            get
            {
                switch (PlcType)
                {
                    case PlcType.ControlLogix: return 88;
                    case PlcType.Plc5: return 88;
                    case PlcType.Slc500: return 25;
                    case PlcType.LogixPccc: return 84;
                    case PlcType.Micro800: return 256;
                    case PlcType.MicroLogix: return 256;
                    default: throw new NotImplementedException();
                }
            }
        }


        override public string DecodeOne(Tag tag, int offset, out int elementSize)
        {
            elementSize = ElementSize.Value;
            switch (PlcType)
            {
                case PlcType.ControlLogix: return ControlLogixDecode(tag, offset);
                case PlcType.Plc5: return Plc5Decode(tag, offset);
                case PlcType.Slc500: return Slc500Decode(tag, offset);
                case PlcType.LogixPccc: return LogixPcccDecode(tag, offset);
                case PlcType.Micro800: return Micro800Decode(tag, offset);
                case PlcType.MicroLogix: return MicroLogixDecode(tag, offset);
                default: throw new NotImplementedException();
            }
        }

        override public void EncodeOne(Tag tag, int offset, out int elementSize, string value)
        {
            elementSize = ElementSize.Value;
            switch (PlcType)
            {
                case PlcType.ControlLogix: ControlLogixEncode(tag, offset, value); break;
                case PlcType.Plc5: Plc5Encode(tag, offset, value); break;
                case PlcType.Slc500: Slc500Encode(tag, offset, value); break;
                case PlcType.LogixPccc: LogixPcccEncode(tag, offset, value); break;
                case PlcType.Micro800: Micro800Encode(tag, offset, value); break;
                case PlcType.MicroLogix: MicroLogixEncode(tag, offset, value); break;
                default: break;
            }
        }






        string ControlLogixDecode(Tag tag, int offset)
        {
            var apparentStringLength = tag.GetInt32(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_CONTROLLOGIX_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];
            for (int ii = 0; ii < actualStringLength; ii++)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 4 + 2 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }

        void ControlLogixEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_CONTROLLOGIX_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int ii = 0; ii < asciiEncodedString.Length; ii++)
            {
                tag.SetUInt8(offset + 4 + 2 + ii, Convert.ToByte(asciiEncodedString[ii]));
            }
        }





        string Plc5Decode(Tag tag, int offset)
        {
            throw new NotImplementedException();
        }

        void Plc5Encode(Tag tag, int offset, string value)
        {
            throw new NotImplementedException();
        }




        string Slc500Decode(Tag tag, int offset)
        {
            throw new NotImplementedException();
        }

        void Slc500Encode(Tag tag, int offset, string value)
        {
            throw new NotImplementedException();
        }





        string LogixPcccDecode(Tag tag, int offset)
        {
            var apparentStringLength = (int)tag.GetInt16(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_LOGIXPCCC_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];

            for (int ii = 0; ii < asciiEncodedString.Length; ii++)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 2 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }

        void LogixPcccEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_LOGIXPCCC_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int ii = 0; ii < asciiEncodedString.Length; ii++)
            {
                tag.SetUInt8(offset + 2 + ii, Convert.ToByte(asciiEncodedString[ii]));
            }
        }




        string Micro800Decode(Tag tag, int offset)
        {
            throw new NotImplementedException();
        }

        void Micro800Encode(Tag tag, int offset, string value)
        {
            throw new NotImplementedException();
        }







        string MicroLogixDecode(Tag tag, int offset)
        {
            var apparentStringLength = (int)tag.GetInt16(offset);

            var actualStringLength = Math.Min(apparentStringLength, MAX_LOGIXPCCC_STRING_LENGTH);

            var asciiEncodedString = new byte[actualStringLength];

            for (int ii = 0; ii < asciiEncodedString.Length; ii++)
            {
                asciiEncodedString[ii] = tag.GetUInt8(offset + 2 + ii + 1);
                asciiEncodedString[ii + 1] = tag.GetUInt8(offset + 2 + ii);
            }

            return Encoding.ASCII.GetString(asciiEncodedString);
        }

        void MicroLogixEncode(Tag tag, int offset, string value)
        {
            if (value.Length > MAX_LOGIXPCCC_STRING_LENGTH)
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");

            var asciiEncodedString = Encoding.ASCII.GetBytes(value);

            tag.SetInt16(offset, Convert.ToInt16(value.Length));

            for (int ii = 0; ii < asciiEncodedString.Length; ii++)
            {
                tag.SetUInt8(offset + 2 + ii, Convert.ToByte(asciiEncodedString[ii + 1]));
                tag.SetUInt8(offset + 2 + ii + 1, Convert.ToByte(asciiEncodedString[ii]));
            }
        }




    }
}
