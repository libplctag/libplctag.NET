using libplctag;
using libplctag.Generic;
using libplctag.Generic.DataTypes;
using RandomTestValues;
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
            var boolTag = new GenericTag<PlcTypeBOOL, bool>()
            {
                Name = "TestBOOL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            //Signed Numbers
            var sintTag = new GenericTag<PlcTypeSINT, sbyte>()
            {
                Name = "TestSINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            var intTag = new GenericTag<PlcTypeINT, short>()
            {
                Name = "TestINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            var dintTag = new GenericTag<PlcTypeDINT, int>()
            {
                Name = "TestBOOL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            var lintTag = new GenericTag<PlcTypeLINT, long>()
            {
                Name = "TestLINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };

            //Floating Points
            var realTag = new GenericTag<PlcTypeREAL, float>()
            {
                Name = "TestREAL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };


            boolTag.Initialize(timeout);
            sintTag.Initialize(timeout);
            intTag.Initialize(timeout);
            dintTag.Initialize(timeout);
            lintTag.Initialize(timeout);
            realTag.Initialize(timeout);


            //Random value would look correct 50% of the time
            TestTag(boolTag, true);
            TestTag(boolTag, false);

        }

        private static bool TestTag<T>(IGenericTag<T> tag) where T : struct
        {
            T testValue = RandomValue.Object<T>();
            return TestTag(tag, testValue);
        }

        private static bool TestTag<T>(IGenericTag<T> tag, T testValue) where T : struct
        {
            Console.WriteLine($"\r\n*** {tag.Name} [0x{tag.CipCode:X2}] {typeof(T)} ***");


            tag.Value = testValue;
            Console.WriteLine($"Write Value <{typeof(T)}> {testValue} to '{tag.Name}'");
            tag.Write(timeout);

            Console.WriteLine($"Read Value from {tag.Name}");
            tag.Read(timeout);

            T readback = tag.Value;

            if (readback.Equals(testValue)) Console.WriteLine($"PASS: Read back matched test value");
            else Console.WriteLine($"FAIL: Read back did not match test value - [{readback} != {testValue}]");

            return readback.Equals(testValue);
        }
    }
    
}
