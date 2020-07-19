using System;
using System.Text;

namespace libplctag.DataTypes
{
    public class StringMarshaller: IMarshaller<string>
    {

        const int MAX_AB_STRING_LENGTH = 82;

        public int ElementSize => 88;

        public string Decode(Tag tag, int offset)
        {

            // The first two bytes of a STRING tag encode its length.
            var apparentStringLength = tag.GetInt32(offset);

            // A STRING is a struct, and the length might be corrupted by faulty PLC programming.
            var stringLength = Math.Min(apparentStringLength, MAX_AB_STRING_LENGTH);

            var returnString = new StringBuilder();

            for (int i = 0; i < stringLength; i++)
            {
                // There is a 2 byte padding, then each byte afterwards is an ASCII-encoded (a.k.a UTF-8) value.
                returnString.Append(Convert.ToChar(tag.GetUInt8(offset + i + 2 + 2)));
            }

            return returnString.ToString();
        }

        public void Encode(Tag tag, int offset, string value)
        {

            foreach (var ch in value)
            {
                if ((int)ch >= 256)
                {
                    throw new ArgumentException("String is not UTF-8 compatible");
                }
            }

            var valueToWrite = Encoding.ASCII.GetBytes(value);

            if (valueToWrite.Length > MAX_AB_STRING_LENGTH)
            {
                throw new ArgumentException("String length exceeds maximum for a tag of type STRING");
            }

            tag.SetInt32(offset, value.Length);

            for (int i = 0; i < valueToWrite.Length; i++)
            {
                tag.SetUInt8(offset + i + 2 + 2, Convert.ToByte(valueToWrite[i]));
            }
        }
    }
}
