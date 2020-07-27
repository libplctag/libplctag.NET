using libplctag;
using libplctag.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CSharpDotNetCore
{
    class ExampleGenericTag
    {
        public static void Run()
        {

            var MyDintArray = new Tag<DintMarshaller, int>(IPAddress.Parse("192.168.0.10"), "1,0", PlcType.ControlLogix, "MY_DINT_ARRAY_1000[0]", 1000, 1000);

            for (int ii = 0; ii < 1000; ii++)
                MyDintArray.Value[ii] = ii;

            MyDintArray.Write(1000);

            // Illustrate use of Linq
            var myDintsAbove200 = MyDintArray.Value.Where(x => x > 200).ToList();




            var MyStringArray = new Tag<StringMarshaller, string>(IPAddress.Parse("192.168.0.10"), "1,0", PlcType.ControlLogix, "MY_STRING_ARRAY_10[0]", 1000, 10);
            for (int ii = 0; ii < 10; ii++)
                MyStringArray.Value[ii] = (ii * 111).ToString();
            MyStringArray.Write(1000);



            var MySequence = new Tag<SequenceMarshaller, Sequence>(IPAddress.Parse("192.168.0.10"), "1,0", PlcType.ControlLogix, "Seq_1", 1000, 1);
            MySequence.Read(1000);
            Console.WriteLine(
                $"Command: {MySequence.Value[0].Command}\n" +
                $"Fault: {MySequence.Value[0].Fault}\n" +
                $"Fault_Step: {MySequence.Value[0].Fault_Step}\n" +
                $"Hold: {MySequence.Value[0].Hold}\n" +
                $"Stop: {MySequence.Value[0].Stop}\n" +
                $"Idle_Step: {MySequence.Value[0].Idle_Step}\n" +
                $"Init_Step: {MySequence.Value[0].Init_Step}\n" +
                $"Next_Step: {MySequence.Value[0].Next_Step}\n" +
                $"Step_No: {MySequence.Value[0].Step_No}\n" +
                $"Timer6.Preset: {MySequence.Value[0].Timer[6].Preset}\n" +
                $"Timer7.Preset: {MySequence.Value[0].Timer[7].Preset}\n"
                );

        }
    }

    
}
