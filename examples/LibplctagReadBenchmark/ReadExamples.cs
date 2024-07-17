using libplctag;
using System.Diagnostics;

namespace OptimizedLibplctagReads
{
    internal static class ReadExamples
    {
        internal static void ReadForEach(List<Tag> allTags)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            foreach (var item in allTags)
            {
                item.Read();
            }
            stopwatch.Stop();
            Console.WriteLine($"Tag ReadForEach = {stopwatch.ElapsedMilliseconds} msec");
        }

        internal static async Task ReadWhenAllAsync(List<Tag> allTags)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            await Task.WhenAll(allTags.Select(x => x.ReadAsync()));
            stopwatch.Stop();
            Console.WriteLine($"Tag ReadWhenAllAsync = {stopwatch.ElapsedMilliseconds} msec");
        }

        internal static async Task ReadForEachAsync(List<Tag> allTags)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Restart();
            foreach (var item in allTags)
            {
                await item.ReadAsync();
            }
            stopwatch.Stop();
            Console.WriteLine($"Tag ReadForEachAsync = {stopwatch.ElapsedMilliseconds} msec");
        }

        internal static void ReadParallelForEach(List<Tag> allTags)
        {
            var stopwatch = new Stopwatch();

            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = 3 };

            stopwatch.Restart();
            Parallel.ForEach(allTags, parallelOptions, x => x.Read());
            stopwatch.Stop();
            Console.WriteLine($"Tag ReadParallelForEach = {stopwatch.ElapsedMilliseconds} msec");
        }

        //        internal static async Task ReadAsyncParallelForEach(List<TagDint> allTags)
        //        {
        //            var stopwatch = new Stopwatch();

        //            ParallelOptions parallelOptions = new()
        //            {
        //                MaxDegreeOfParallelism = 3
        //            };

        //#error This code is not actually waiting for the reads to occur. It's bad.

        //            stopwatch.Restart();
        //            var result = Parallel.ForEach(allTags, parallelOptions, async x => await x.ReadAsync());
        //            stopwatch.Stop();
        //            Console.WriteLine($"Tag ReadAsyncParallelForEach = {stopwatch.ElapsedMilliseconds} msec");
        //        }

        internal static async Task ReadAsyncParallelForEachAsync(List<Tag> allTags)
        {
            var stopwatch = new Stopwatch();
            ParallelOptions parallelOptions = new() { MaxDegreeOfParallelism = allTags.Count(), };

            stopwatch.Restart();

            await Parallel.ForEachAsync(allTags, parallelOptions, async (tag, token) => await tag.ReadAsync(token));
            stopwatch.Stop();
            Console.WriteLine($"Tag ReadAsyncParallelForEachAsync = {stopwatch.ElapsedMilliseconds} msec");
        }
    }
}
