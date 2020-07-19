using libplctag;
using libplctag.Generic;
using libplctag.Generic.DataTypes;
using RandomTestValues;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleGenericTag
    {
        private const int DEFAULT_TIMEOUT = 100;

        public static void Run()
        {
            Random rnd = new Random();
            int testValue = rnd.Next();

            IPAddress gateway = IPAddress.Parse("10.10.10.10");
            const string Path = "1,0";
            const int timeout = 1000;

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

            //Logix doesn't support unsigned

            //Foating Points
            TestTag(new GenericTag<PlcTypeREAL, float>(gateway, Path, CpuType.Logix, "TestREAL", timeout));
            //TestTag(new GenericTag<PlcTypeLREAL, double>(gateway, Path, CpuType.Logix, "TestLREAL", timeout));

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
            tag.Write(DEFAULT_TIMEOUT);

            Console.WriteLine($"Read Value from {tag.Name}");
            tag.Read(100);

            T readback = tag.Value;

            if (readback.Equals(testValue)) Console.WriteLine($"PASS: Read back matched test value");
            else Console.WriteLine($"FAIL: Read back did not match test value - [{readback} != {testValue}]");

            return readback.Equals(testValue);
        }

    }
}
