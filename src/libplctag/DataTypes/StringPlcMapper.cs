using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libplctag.DataTypes
{
    public class StringPlcMapper : PlcMapperBase<string>
    {
        override public int? ElementSize
        {
            get
            {
                switch (PlcType)
                {
                    case PlcType.ControlLogix: return 88;
                    case PlcType.Plc5: return 84;
                    case PlcType.Slc500: return 84;
                    case PlcType.LogixPccc: return 84;
                    case PlcType.Micro800: return 256; //To be Confirmed
                    case PlcType.MicroLogix: return 84;
                    case PlcType.Omron: return 256; //To be Confirmed
                    default: throw new NotImplementedException();
                }
            }
        }

        override public string Decode(Tag tag, int offset)
        {
            if (PlcType == PlcType.Omron)
            {
                var bytes = tag.GetBuffer();
                if (bytes.Length <= 2)
                    return "";
                else
                    return Encoding.ASCII.GetString(bytes, offset + 2, bytes.Length - 2 - offset);
            }
            else
            {
                return tag.GetString(offset);
            }
        }

        override public void Encode(Tag tag, int offset, string value)
        {
            if (PlcType == PlcType.Omron)
            {
                if (value.Length == 0)
                {
                    tag.SetUInt16(0, 0);
                }
                else
                {
                    if (value.Length > 255)
                        value = value.Remove(255);
                    
                    var bytes = new byte[] { Convert.ToByte(value.Length), 0 }
                        .Concat(Encoding.ASCII.GetBytes(value))
                        .ToList();
                    
                    if (bytes.Count % 2 == 1)
                    {
                        bytes.Add(0);
                        bytes[0]++;
                    }

                    tag.SetBuffer(bytes.ToArray());
                }
            }
            else
            {
                tag.SetString(offset, value);
            }
        }
    }
}
