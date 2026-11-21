// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Coyote;
using Microsoft.Coyote.SystematicTesting;
using Moq;
using Xunit;
using static libplctag.NativeImport.plctag;

namespace libplctag.Tests
{
    [CollectionDefinition(nameof(TagRaceTests), DisableParallelization = true)]
    public class TagRaceTestsCollection { }

    /// <summary>
    /// Calls that run at the same time. Each test starts them concurrently and states what must hold however they
    /// interleave; Coyote runs it under many schedules (it controls the tasks, locks, Interlocked and Volatile
    /// operations of the rewritten assemblies, see the csproj) and fails with the schedule that broke it.
    /// </summary>
    [Collection(nameof(TagRaceTests))]   // alone: Coyote's hang detection is timing-based for the code it cannot control
    public class TagRaceTests
    {
        static void UnderManySchedules(Func<Task> test, bool reportHangs = true)
        {
            var config = Configuration.Create().WithTestingIterations(100).WithPotentialDeadlocksReportedAsBugs(reportHangs);
            var engine = TestingEngine.Create(config, test);
            engine.Run();
            Assert.True(engine.TestReport.NumOfFoundBugs == 0, string.Join(Environment.NewLine, engine.TestReport.BugReports));
        }

        static int Calls(MockCore core, string method) => core.Native.Invocations.Count(i => i.Method.Name == method);

        [Fact]
        public void Two_Reads_on_a_new_tag_create_it_once() => UnderManySchedules(async () =>
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var read1 = Task.Run(() => tag.Read());
            var read2 = Task.Run(() => tag.Read());
            await Record.ExceptionAsync(() => read1);
            await Record.ExceptionAsync(() => read2);

            // Assert
            Assert.Equal(1, Calls(core, nameof(INative.plc_tag_create_ex)));
        });

        [Fact]
        public void A_ReadAsync_whose_read_finishes_meanwhile_completes() => UnderManySchedules(async () =>
        {
            // A missed wake-up leaves the read waiting forever, which Coyote reports as a deadlock.

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            var read = Task.Run(() => tag.ReadAsync());
            await core.Tickler(read);
            var ex = await Record.ExceptionAsync(() => read);

            // Assert
            Assert.Null(ex);
        });

        [Fact]
        public void Two_Disposes_and_a_ReadAsync_destroy_the_tag_once() => UnderManySchedules(async () =>
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            var read = Task.Run(() => tag.ReadAsync());
            var dispose1 = Task.Run(() => tag.Dispose());
            var dispose2 = Task.Run(() => tag.Dispose());
            await Task.WhenAll(new[] { dispose1, dispose2 });
            var ex = await Record.ExceptionAsync(() => read);

            // Assert
            Assert.IsType<ObjectDisposedException>(ex);
            core.Native.Verify(n => n.plc_tag_destroy(MockCore.Handle), Times.Once);
        });

        [Fact]
        public void Dispose_and_Initialize_leave_no_native_tag() => UnderManySchedules(async () =>
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var init = Task.Run(() => tag.Initialize());
            var dispose = Task.Run(() => tag.Dispose());
            await dispose;
            await Record.ExceptionAsync(() => init);

            // Assert
            Assert.Equal(Calls(core, nameof(INative.plc_tag_create_ex)), Calls(core, nameof(INative.plc_tag_destroy)));
        });

        [Fact]
        public void Dispose_and_InitializeAsync_leave_no_native_tag() => UnderManySchedules(async () =>
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var init = Task.Run(() => tag.InitializeAsync());
            var dispose = Task.Run(() => tag.Dispose());
            await core.Tickler(init);
            await dispose;
            await Record.ExceptionAsync(() => init);

            // Assert
            Assert.Equal(Calls(core, nameof(INative.plc_tag_create_ex)), Calls(core, nameof(INative.plc_tag_destroy)));
        });

        [Fact]
        public void Dispose_and_a_getter_give_the_value_or_ObjectDisposedException() => UnderManySchedules(async () =>
        {
            // int.MinValue may mean an error, so the getter asks for the status, which Dispose may have destroyed
            // by then (status.1).

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_get_int32(MockCore.Handle, 0)).Returns(int.MinValue);

            // Act
            var get = Task.Run(() => tag.GetInt32(0));
            var dispose = Task.Run(() => tag.Dispose());
            await dispose;
            var ex = await Record.ExceptionAsync(() => get);

            // Assert
            Assert.True(ex == null || ex is ObjectDisposedException, $"unexpected {ex}");
        });

        [Fact]
        public void ReadAsync_on_a_new_tag_creates_it_then_reads() => UnderManySchedules(async () =>
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var read = Task.Run(() => tag.ReadAsync());
            await core.Tickler(read, count: 2);   // the creation, then the read
            var ex = await Record.ExceptionAsync(() => read);

            // Assert
            Assert.Null(ex);
            core.Native.Verify(n => n.plc_tag_read(MockCore.Handle, 0), Times.Once);
        });

        [Fact]
        public void WriteAsync_on_a_new_tag_creates_it_then_writes() => UnderManySchedules(async () =>
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var write = Task.Run(() => tag.WriteAsync());
            await core.Tickler(write, count: 2);   // the creation, then the write
            var ex = await Record.ExceptionAsync(() => write);

            // Assert
            Assert.Null(ex);
            core.Native.Verify(n => n.plc_tag_write(MockCore.Handle, 0), Times.Once);
        });

        [Fact]
        public void Two_ReadAsyncs_both_end() => UnderManySchedules(async () =>
        {
            // The second either finds the first in flight (ERR_BUSY), or starts after it, possibly before the first
            // caller has seen its read end; then both wait, and both must be woken.

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            var read1 = Task.Run(() => tag.ReadAsync());
            var read2 = Task.Run(() => tag.ReadAsync());
            await core.Tickler(Task.WhenAll(new[] { read1, read2 }), count: 2);
            var ex1 = await Record.ExceptionAsync(() => read1);
            var ex2 = await Record.ExceptionAsync(() => read2);

            // Assert
            Assert.True(ex1 == null || ex1 is LibPlcTagException { Status: Status.ErrorBusy }, $"unexpected {ex1}");
            Assert.True(ex2 == null || ex2 is LibPlcTagException { Status: Status.ErrorBusy }, $"unexpected {ex2}");
        });

        [Fact]
        public void A1_A5_cancelling_InitializeAsync_while_the_creation_fails_destroys_the_tag() => UnderManySchedules(async () =>
        {
            // The abort may find the tag creating (A1) or already failed (A5).
            // Hangs are not reported here: disposing a token registration waits for a cancellation callback that is
            // still running, inside .NET where Coyote cannot see it, so Coyote mistakes some schedules for deadlocks.
            // Real threads do not hang (checked 20,000 times); the other races cover hangs.

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            var cts = new System.Threading.CancellationTokenSource();

            // Act
            var init = Task.Run(() => tag.InitializeAsync(cts.Token));
            var cancel = Task.Run(() => cts.Cancel());
            await core.Tickler(init, s: Status.ErrorBadConnection);
            await cancel;
            var ex = await Record.ExceptionAsync(() => init);

            // Assert
            Assert.True(ex is OperationCanceledException || ex is LibPlcTagException { Status: Status.ErrorBadConnection }, $"unexpected {ex}");
            Assert.Equal(Calls(core, nameof(INative.plc_tag_create_ex)), Calls(core, nameof(INative.plc_tag_destroy)));
        }, reportHangs: false);

        [Fact]
        public void Dispose_and_a_blocking_Read_give_success_or_ObjectDisposedException() => UnderManySchedules(async () =>
        {
            // Dispose may destroy the tag after Read's own check, so that the core no longer knows the handle (read.1), or while it waits (D4, read.6).

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            var read = Task.Run(() => tag.Read());
            var dispose = Task.Run(() => tag.Dispose());
            await dispose;
            var ex = await Record.ExceptionAsync(() => read);

            // Assert
            Assert.True(ex == null || ex is ObjectDisposedException, $"unexpected {ex}");
            core.Native.Verify(n => n.plc_tag_destroy(MockCore.Handle), Times.Once);
        });
    }
}
