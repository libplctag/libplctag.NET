using System;
using System.Collections.Generic;
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
                    default: throw new NotImplementedException();
                }
            }
        }


        override public string Decode(Tag tag, int offset) => tag.GetString(offset);
        override public void Encode(Tag tag, int offset, string value) => tag.SetString(offset, value);

    }
}