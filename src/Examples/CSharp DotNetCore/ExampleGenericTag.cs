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

        const int timeout = 1000;
        const string gateway = "10.10.10.10";
        const string path = "1,0";


        public static void Run()
        {

            //Bool - Test both cases
            var boolTag = new Tag<BoolMarshaller, bool>()
            {
                Name = "TestBOOL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };
            //Signed Numbers
            var sintTag = new Tag<SintMarshaller, sbyte>()
            {
                Name = "TestSINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };
            var intTag = new Tag<IntMarshaller, short>()
            {
                Name = "TestINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };
            var dintTag = new Tag<DintMarshaller, int>()
            {
                Name = "TestBOOL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };
            var lintTag = new Tag<LintMarshaller, long>()
            {
                Name = "TestLINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            //Floating Points
            var realTag = new Tag<RealMarshaller, float>()
            {
                Name = "TestREAL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };


            boolTag.Initialize();
            boolTag.Read();

            sintTag.Initialize();
            sintTag.Read();

            intTag.Initialize();
            intTag.Read();

            dintTag.Initialize();
            dintTag.Read();

            lintTag.Initialize();
            lintTag.Read();

            realTag.Initialize();
            realTag.Read();



        }


        public static void StringArray()
        {

            var stringTag = new Tag<StringMarshaller, string[]>()
            {
                Name = "MY_STRING_1D[0]",
                Gateway = gateway,
                Path = path,
                Protocol = Protocol.ab_eip,
                PlcType = PlcType.ControlLogix,
                ArrayLength = 100
            };

            stringTag.Initialize();

            var r = new Random((int)DateTime.Now.ToBinary());

            for (int ii = 0; ii < 100; ii++)
                stringTag.Value[ii] = r.Next().ToString();

            stringTag.Write();

            Console.WriteLine("DONE");


        }


        public static void UDT_Array()
        {

            var sequenceArray = new Tag<SequenceMarshaller, Sequence[]>()
            {
                Name = "MY_SEQUENCE_3D[0,0,0]",
                Gateway = gateway,
                Path = path,
                Protocol = Protocol.ab_eip,
                PlcType = PlcType.ControlLogix,
                ArrayLength = 8
            };
            sequenceArray.Initialize();

            for (int ii = 0; ii < 8; ii++)
                sequenceArray.Value[ii].Command = ii * 2;

            sequenceArray.Write();


            Console.WriteLine("DONE! Check values in RsLogix");

        }


        public static void MyBool()
        {

            var myBool = new Tag<BoolMarshaller, bool>()
            {
                Name = "MY_BOOL",
                Gateway = gateway,
                Path = path,
                Protocol = Protocol.ab_eip,
                PlcType = PlcType.ControlLogix,
                ArrayLength = 1
            };
            myBool.Initialize();

            myBool.Value = true;

            myBool.Write();

            Console.WriteLine("DONE! Check values in RsLogix");

        }

        public static void MyBoolArray()
        {

            var myBools = new Tag<BoolMarshaller, bool[]>()
            {
                Name = "MY_BOOL_1D[0]",
                Gateway = gateway,
                Path = path,
                Protocol = Protocol.ab_eip,
                PlcType = PlcType.ControlLogix,
                ArrayLength = 30
            };
            myBools.Initialize();

            for (int ii = 0; ii < 30; ii++)
            {
                myBools.Value[ii] = !myBools.Value[ii];
            }

            myBools.Write();

            Console.WriteLine("DONE! Check values in RsLogix");

        }

    }

}
