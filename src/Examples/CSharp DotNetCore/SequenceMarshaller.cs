using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections;

namespace CSharpDotNetCore
{
    /// <summary>
    /// This is a User Defined Type and is just included as an example of how to define a custom marshaller
    /// </summary>
    public class SequenceMarshaller : IMarshaller<Sequence>
    {
        
        public int ElementSize => 268;

        public PlcType PlcType { get; set; }

        public Sequence Decode(Tag tag, int offset)
        {

            var DINT0 = tag.GetInt32(offset + 0);
            var DINT1 = tag.GetInt32(offset + 4);
            var DINT2 = tag.GetInt32(offset + 8);
            var DINT3 = tag.GetInt32(offset + 12);
            var DINT4 = tag.GetInt32(offset + 16);
            var DINT5 = tag.GetInt32(offset + 20);
            var DINT6 = tag.GetInt32(offset + 24);

            var bitArray = new BitArray(new int[] { DINT6 });

            var timerMarshaller = new TimerMarshaller()
            { PlcType = this.PlcType };

            var TIMERS = new AbTimer[20];
            for (int i = 0; i < 20; i++)
            {
                var timerOffset = offset + 28 + i * timerMarshaller.ElementSize;
                TIMERS[i] = timerMarshaller.Decode(tag, timerOffset);
            }

            return new Sequence()
            {
                Step_No = DINT0,
                Next_Step = DINT1,
                Command = DINT2,
                Idle_Step = DINT3,
                Fault_Step = DINT4,
                Init_Step = DINT5,
                Stop = bitArray[0],
                Hold = bitArray[1],
                Fault = bitArray[2],
                Timer = TIMERS
            };

        }

        public void Encode(Tag tag, int offset, Sequence value)
        {

            var DINT0 = value.Step_No;
            var DINT1 = value.Next_Step;
            var DINT2 = value.Command;
            var DINT3 = value.Idle_Step;
            var DINT4 = value.Fault_Step;
            var DINT5 = value.Init_Step;

            var temp = new BitArray(32);
            temp[0] = value.Stop;
            temp[1] = value.Hold;
            temp[2] = value.Fault;

            var DINT6 = BitArrayToInt(temp);

            tag.SetInt32(offset + 0, DINT0);
            tag.SetInt32(offset + 4, DINT1);
            tag.SetInt32(offset + 8, DINT2);
            tag.SetInt32(offset + 12, DINT3);
            tag.SetInt32(offset + 16, DINT4);
            tag.SetInt32(offset + 20, DINT5);
            tag.SetInt32(offset + 24, DINT6);

            var timerMarshaller = new TimerMarshaller();
            for (int i = 0; i < 20; i++)
            {
                var timerOffset = offset + 28 + i * timerMarshaller.ElementSize;
                timerMarshaller.Encode(tag, timerOffset, value.Timer[i]);
            }

        }

        static int BitArrayToInt(BitArray binary)
        {
            if (binary == null)
                throw new ArgumentNullException("binary");
            if (binary.Length > 32)
                throw new ArgumentException("Must be at most 32 bits long");

            var result = new int[1];
            binary.CopyTo(result, 0);
            return result[0];
        }

    }

    public class Sequence
    {
        public int Command { get; set; }
        public bool Fault { get; set; }
        public int Fault_Step { get; set; }
        public bool Hold { get; set; }
        public int Idle_Step { get; set; }
        public int Next_Step { get; set; }
        public int Init_Step { get; set; }
        public int Step_No { get; set; }
        public bool Stop { get; set; }
        public AbTimer[] Timer { get; set; }
    }
}
