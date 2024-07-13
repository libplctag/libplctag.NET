using libplctag;
using libplctag.DataTypes.Simple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizedLibplctagReads
{
    internal static class TagInstantation
    {
        internal static async Task<List<TagDint>> Instantiate(int length, TimeSpan timeout)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var allTags = new List<TagDint>();

            for (int i = 0; i < length; i++)
            {
                var myTag = new TagDint()
                {
                    //Name is the full path to tag.
                    Name = $"TestDINT{i.ToString("0000")}",
                    //Gateway is the IP Address of the PLC or communication module.
                    Gateway = "10.10.10.17",
                    //Path is the location in the control plane of the CPU. Almost always "1,0".
                    Path = "1,0",
                    PlcType = PlcType.ControlLogix,
                    Protocol = Protocol.ab_eip,
                    Timeout = timeout,
                };
                allTags.Add(myTag);
            }

            stopwatch.Stop();
            Console.WriteLine($"Tag Instantiation = {stopwatch.ElapsedMilliseconds} msec");

            stopwatch.Restart();
            await Task.WhenAll(allTags.Select(x => x.InitializeAsync()));
            stopwatch.Stop();
            Console.WriteLine($"Tag InitWhenAllAsync = {stopwatch.ElapsedMilliseconds} msec");

            return allTags;
        }
    }
}
