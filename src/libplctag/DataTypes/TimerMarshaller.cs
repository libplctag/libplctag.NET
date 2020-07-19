using System;
using System.Collections;
using System.Text;

namespace libplctag.DataTypes
{
    public class TimerMarshaller: IMarshaller<AbTimer>
    {

        public int ElementSize => 12;

        public AbTimer Decode(Tag tag, int offset)
        {

            // Needed to look at RsLogix documentation for structure of TIMER
            var DINT2 = tag.GetInt32(offset);
            var DINT1 = tag.GetInt32(offset + 4);
            var DINT0 = tag.GetInt32(offset + 8);

            // The third DINT packs a few BOOLs into it
            var bitArray = new BitArray(new int[] { DINT2 });

            var timer = new AbTimer
            {
                Accumulated = DINT0,         // ACC
                Preset = DINT1,              // PRE
                Done = bitArray[29],         // DN
                InProgress = bitArray[30],   // TT
                Enabled = bitArray[31]       // EN
            };

            return timer;

        }

        public void Encode(Tag tag, int index, AbTimer value)
        {
            throw new NotImplementedException();
        }
    }

    public class AbTimer
    {
        public int Preset { get; set; }
        public int Accumulated { get; set; }
        public bool Enabled { get; set; }
        public bool InProgress { get; set; }
        public bool Done { get; set; }
    }
}
