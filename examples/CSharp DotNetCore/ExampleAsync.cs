// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDotNetCore
{
    class ExampleAsync
    {
        public static async Task Run()
        {
            var myTag = new Tag()
            {
                Name = "PROGRAM:SomeProgram.SomeDINT",
                Gateway = "10.10.10.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip,
                Timeout = TimeSpan.FromMilliseconds(1000),
            };

            await myTag.InitializeAsync();

            myTag.SetInt32(0, 3737);

            await myTag.WriteAsync();

            await myTag.ReadAsync();

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);
        }


        public static void AsyncParallelCancellation(int maxTags = 20, int repetitions = 100)
        {

            var myTags = Enumerable.Range(0, maxTags)
                .Select(i => {
                    var myTag = new Tag()
                    {
                        Name = $"MY_DINT_ARRAY_1000[{i}]",
                        Gateway = "10.10.10.10",
                        Path = "1,0",
                        PlcType = PlcType.ControlLogix,
                        Protocol = Protocol.ab_eip,
                        Timeout = TimeSpan.FromMilliseconds(1000),
                    };
                    myTag.Initialize();
                    return myTag;
                })
                .ToList();


            int cancelAfterSeconds = 5;
            Console.WriteLine($"Starting parallel reads of {maxTags} tags. Will cancel in {cancelAfterSeconds} seconds");

            var cts = new CancellationTokenSource();
            cts.CancelAfter(cancelAfterSeconds * 1000);

            Task.WaitAll(myTags.Select(tag =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        for (int ii = 0; ii < repetitions; ii++)
                        {
                            await tag.ReadAsync(cts.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Successfully Cancelled");
                    }
                });
            }).ToArray());

        }
    }
}
