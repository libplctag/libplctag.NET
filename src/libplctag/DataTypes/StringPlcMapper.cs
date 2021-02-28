using System;

namespace libplctag.DataTypes
{
    public class StringPlcMapper : PlcMapperBase<string>, IPlcMapper<string>, IPlcMapper<string[]>
    {

        override public int? ElementSize
        {
            get
            {
                // TODO get this from the native library which now has an API for this
                // https://github.com/libplctag/libplctag/wiki/API#handling-strings
                // Some of these values have not been confirmed (Micro800)

                switch (PlcType)
                {
                    case PlcType.ControlLogix: return 88;
                    case PlcType.Plc5: return 84;
                    case PlcType.Slc500: return 84;
                    case PlcType.LogixPccc: return 84;
                    case PlcType.Micro800: return 256;
                    case PlcType.MicroLogix: return 84;
                    default: throw new NotImplementedException();
                }
            }
        }

        override public string Decode(Tag tag, int offset)
        {
            return tag.GetString(offset);
        }

        override public void Encode(Tag tag, int offset, string value)
        {
            tag.SetString(offset, value);
        }

    }
}