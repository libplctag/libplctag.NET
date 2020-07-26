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

            var MyDintArray = new Tag1d<DintMarshaller, int>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "MY_DINT_ARRAY_1000[0]", 1000, 1000);

            //Bool - Test both cases
            //Random value would look correct 50% of the time
            var boolTag = new GenericTag<PlcTypeBOOL, bool>(gateway, Path, CpuType.Logix, "TestBOOL", timeout);
            TestTag(boolTag, true);
            TestTag(boolTag, false);

            //Signed Numbers
            TestTag(new GenericTag<PlcTypeSINT, sbyte>(gateway, Path, CpuType.Logix, "TestSINT", timeout));
            TestTag(new GenericTag<PlcTypeINT, short>(gateway, Path, CpuType.Logix, "TestINT", timeout));
            TestTag(new GenericTag<PlcTypeDINT, int>(gateway, Path, CpuType.Logix, "TestDINT", timeout));
            TestTag(new GenericTag<PlcTypeLINT, long>(gateway, Path, CpuType.Logix, "TestLINT", timeout));

            // Illustrate use of Linq
            var myDintsAbove200 = MyDintArray.Value.Where(x => x > 200).ToList();

            //Foating Points
            TestTag(new GenericTag<PlcTypeREAL, float>(gateway, Path, CpuType.Logix, "TestREAL", timeout));
            //TestTag(new GenericTag<PlcTypeLREAL, double>(gateway, Path, CpuType.Logix, "TestLREAL", timeout));



            var MyStringArray = new Tag1d<StringMarshaller, string>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "MY_STRING_ARRAY_10[0]", 1000, 10);
            for (int ii = 0; ii < 10; ii++)
                MyStringArray.Value[ii] = (ii * 111).ToString();
            MyStringArray.Write(1000);



//<<<<<<< HEAD

//            var MyTimer = new Tag<TimerMarshaller, AbTimer>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "T_MinorFaultCheck", 1000);
//            MyTimer.Read(1000);
//            Console.WriteLine(
//                $"Preset: {MyTimer.Value.Preset}\n" +
//                $"Accumulated: {MyTimer.Value.Accumulated}\n" +
//                $"Enabled: {MyTimer.Value.Enabled}\n" +
//                $"InProgress: {MyTimer.Value.InProgress}\n" +
//                $"Done: {MyTimer.Value.Done}"
//                );
//=======
//            tag.Value = testValue;
//            Console.WriteLine($"Write Value <{typeof(T)}> {testValue} to '{tag.Name}'");
//            tag.Write(DEFAULT_TIMEOUT);

//            Console.WriteLine($"Read Value from {tag.Name}");
//            tag.Read(DEFAULT_TIMEOUT);
//>>>>>>> f300ab0b5437f4c23e31d718edeed4358f28b819


            var MySequence = new Tag<SequenceMarshaller, Sequence>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "Seq_1", 1000);
            MySequence.Read(1000);
            Console.WriteLine(
                $"Command: {MySequence.Value.Command}\n" +
                $"Fault: {MySequence.Value.Fault}\n" +
                $"Fault_Step: {MySequence.Value.Fault_Step}\n" +
                $"Hold: {MySequence.Value.Hold}\n" +
                $"Stop: {MySequence.Value.Stop}\n" +
                $"Idle_Step: {MySequence.Value.Idle_Step}\n" +
                $"Init_Step: {MySequence.Value.Init_Step}\n" +
                $"Next_Step: {MySequence.Value.Next_Step}\n" +
                $"Step_No: {MySequence.Value.Step_No}\n" +
                $"Timer6.Preset: {MySequence.Value.Timer[6].Preset}\n" +
                $"Timer7.Preset: {MySequence.Value.Timer[7].Preset}\n"
                );

            var MySequenceArray = new Tag3d<SequenceMarshaller, Sequence>(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.Logix, "Seq_3dArray[0,0,0]", 2, 2, 2, 1000);
            
            MySequenceArray.Read(1000);
            Console.WriteLine(
                $"Command: {MySequenceArray.Value[1, 0, 1].Command}\n" +
                $"Fault: {MySequenceArray.Value[1, 0, 1].Fault}\n" +
                $"Fault_Step: {MySequenceArray.Value[1, 0, 1].Fault_Step}\n" +
                $"Hold: {MySequenceArray.Value[1, 0, 1].Hold}\n" +
                $"Stop: {MySequenceArray.Value[1, 0, 1].Stop}\n" +
                $"Idle_Step: {MySequenceArray.Value[1, 0, 1].Idle_Step}\n" +
                $"Init_Step: {MySequenceArray.Value[1, 0, 1].Init_Step}\n" +
                $"Next_Step: {MySequenceArray.Value[1, 0, 1].Next_Step}\n" +
                $"Step_No: {MySequenceArray.Value[1, 0, 1].Step_No}\n" +
                $"Timer6.Preset: {MySequenceArray.Value[1, 0, 1].Timer[6].Preset}\n" +
                $"Timer7.Preset: {MySequenceArray.Value[1, 0, 1].Timer[7].Preset}\n"
                );

            MySequenceArray.Value[0, 1, 1].Command = 123456;
            MySequenceArray.Value[0, 1, 1].Fault = true;
            MySequenceArray.Value[0, 1, 1].Idle_Step = 54321;
            MySequenceArray.Write(1000);

        }
    }

    
}
