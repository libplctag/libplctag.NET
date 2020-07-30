using System;
using System.Linq;
using System.Net;
using System.Text;
using ConsoleTables;
using libplctag;
using libplctag.DataTypes;

namespace CSharpDotNetCore
{
    static class ExampleListTags
    {

        static int TIMEOUT_MS = 5000;

        public static void Run()
        {

            var tags = new Tag<TagInfoMarshaller, TagInfo>()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Name = "@tags"
            };

            tags.Initialize(TIMEOUT_MS);
            tags.Read(TIMEOUT_MS);

            ConsoleTable
                .From(tags.Value.Select(t => new
                {
                    t.Id,
                    Type = $"0x{t.Type:X}",
                    t.Name,
                    t.Length,
                    Dimensions = string.Join(',', t.Dimensions)
                }))
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Default);

        }

    }

}