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
                PlcType = PlcType.ControlLogix
            };
            //Signed Numbers
            var sintTag = new Tag<SintMarshaller, sbyte>()
            {
                Name = "TestSINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            var intTag = new Tag<IntMarshaller, short>()
            {
                Name = "TestINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            var dintTag = new Tag<DintMarshaller, int>()
            {
                Name = "TestBOOL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };
            var lintTag = new Tag<LintMarshaller, long>()
            {
                Name = "TestLINT",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };

            //Floating Points
            var realTag = new Tag<RealMarshaller, float>()
            {
                Name = "TestREAL",
                Gateway = gateway,
                Path = path,
                PlcType = PlcType.ControlLogix
            };


            boolTag.Initialize(timeout);
            boolTag.Read(timeout);

            sintTag.Initialize(timeout);
            sintTag.Read(timeout);

            intTag.Initialize(timeout);
            intTag.Read(timeout);

            dintTag.Initialize(timeout);
            dintTag.Read(timeout);

            lintTag.Initialize(timeout);
            lintTag.Read(timeout);

            realTag.Initialize(timeout);
            realTag.Read(timeout);



        }

    }

}
