// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using static libplctag.NativeImport.plctag;

namespace libplctag.Tests
{
    public class TagContractTests
    {
        const Status AnyError = Status.ErrorBadConnection;

        [Fact(Timeout = 2000)]
        public async Task create_ex1_InitializeAsync_throws_the_error_when_create_ex_fails()
        {
            // Arrange
            var core = new MockCore();
            core.Choose.CreateFails = AnyError;
            var tag = core.NewTag();

            // Act
            var init = tag.InitializeAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => init);
            Assert.Equal(AnyError, ex.Status);
        }

        [Fact]
        public void create_ex1_Initialize_throws_the_error_when_create_ex_fails()
        {
            // Arrange
            var core = new MockCore();
            core.Choose.CreateFails = Status.ErrorTimeout;
            var tag = core.NewTag();

            // Act
            var ex = Record.Exception(() => tag.Initialize());

            // Assert
            Assert.Equal(Status.ErrorTimeout, Assert.IsType<LibPlcTagException>(ex).Status);
        }

        [Fact(Timeout = 2000)]
        public async Task create_ex2_F1_InitializeAsync_completes_when_the_tag_is_created()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            var init = tag.InitializeAsync();

            // Act
            core.Model.Finish(Status.Ok);

            // Assert
            await init;
            Assert.True(tag.IsInitialized);
        }

        [Fact(Timeout = 2000)]
        public async Task F2_InitializeAsync_throws_the_error_and_destroys_the_tag_when_creation_fails()
        {
            // Arrange
            var core = new MockCore();
            var init = core.NewTag().InitializeAsync();

            // Act
            core.Model.Finish(AnyError);

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => init);
            Assert.Equal(AnyError, ex.Status);
            core.Native.Verify(n => n.plc_tag_destroy(MockCore.Handle), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task A1_cancelling_InitializeAsync_aborts_and_destroys_the_tag()
        {
            // Arrange
            var core = new MockCore();
            var cts = new CancellationTokenSource();
            var init = core.NewTag().InitializeAsync(cts.Token);

            // Act
            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => init);
            core.Native.Verify(n => n.plc_tag_abort(MockCore.Handle), Times.Once);
            core.Native.Verify(n => n.plc_tag_destroy(MockCore.Handle), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task Timeout_during_InitializeAsync_aborts_destroys_the_tag_and_throws_ErrorTimeout()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Timeout = TimeSpan.FromMilliseconds(100);

            // Act
            var init = tag.InitializeAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => init);
            Assert.Equal(Status.ErrorTimeout, ex.Status);
            core.Native.Verify(n => n.plc_tag_abort(MockCore.Handle), Times.Once);
            core.Native.Verify(n => n.plc_tag_destroy(MockCore.Handle), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task InitializeAsync_a_second_time_throws_InvalidOperationException()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            var init = tag.InitializeAsync();
            core.Model.Finish(Status.Ok);
            await init;

            // Act
            var again = tag.InitializeAsync();

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => again);
        }

        [Fact(Timeout = 2000)]
        public async Task InitializeAsync_can_be_retried_after_the_creation_failed()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            var first = tag.InitializeAsync();
            core.Model.Finish(AnyError);
            await Assert.ThrowsAsync<LibPlcTagException>(() => first);

            // Act
            var second = tag.InitializeAsync();
            core.Model.Finish(Status.Ok);
            await second;

            // Assert
            Assert.True(tag.IsInitialized);
            core.Native.Verify(n => n.plc_tag_destroy(1), Times.Once);
            core.Native.Verify(n => n.plc_tag_destroy(2), Times.Never);
        }

        [Fact]
        public void Initialize_while_InitializeAsync_is_creating_throws_ErrorBusy_and_creates_no_second_tag()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            _ = tag.InitializeAsync();

            // Act
            var ex = Record.Exception(() => tag.Initialize());

            // Assert
            Assert.Equal(Status.ErrorBusy, Assert.IsType<LibPlcTagException>(ex).Status);
            core.Native.Verify(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Read_while_InitializeAsync_is_creating_throws_ErrorBusy_and_creates_no_second_tag()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            _ = tag.InitializeAsync();

            // Act
            var ex = Record.Exception(() => tag.Read());

            // Assert
            Assert.Equal(Status.ErrorBusy, Assert.IsType<LibPlcTagException>(ex).Status);
            core.Native.Verify(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task read3_F3_ReadAsync_completes_when_the_read_succeeds()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var read = tag.ReadAsync();

            // Act
            core.Model.Finish(Status.Ok);

            // Assert
            Assert.Null(await Record.ExceptionAsync(() => read));
        }

        [Fact(Timeout = 2000)]
        public async Task read3_F3_ReadAsync_throws_the_error_when_the_read_fails()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var read = tag.ReadAsync();

            // Act
            core.Model.Finish(AnyError);

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => read);
            Assert.Equal(AnyError, ex.Status);
        }

        [Fact(Timeout = 2000)]
        public async Task write3_F4_WriteAsync_completes_when_the_write_succeeds()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var write = tag.WriteAsync();

            // Act
            core.Model.Finish(Status.Ok);

            // Assert
            Assert.Null(await Record.ExceptionAsync(() => write));
        }

        [Fact(Timeout = 2000)]
        public async Task write3_F4_WriteAsync_throws_the_error_when_the_write_fails()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var write = tag.WriteAsync();

            // Act
            core.Model.Finish(AnyError);

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => write);
            Assert.Equal(AnyError, ex.Status);
        }

        [Fact(Timeout = 2000)]
        public async Task read7_ReadAsync_completes_at_once_when_the_read_is_served_from_the_cache()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_read(MockCore.Handle, 0)).Returns((int)Status.Ok);

            // Act
            var read = tag.ReadAsync();

            // Assert
            Assert.Null(await Record.ExceptionAsync(() => read));
        }

        [Fact(Timeout = 2000)]
        public async Task read8_ReadAsync_throws_ErrorBusy_at_once_when_the_tag_has_unwritten_changes()
        {

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_read(MockCore.Handle, 0)).Returns((int)Status.ErrorBusy);

            // Act
            var read = tag.ReadAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => read);
            Assert.Equal(Status.ErrorBusy, ex.Status);
        }

        [Fact(Timeout = 2000)]
        public async Task read9_ReadAsync_throws_the_error_when_the_read_cannot_start()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_read(MockCore.Handle, 0)).Returns((int)AnyError);

            // Act
            var read = tag.ReadAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => read);
            Assert.Equal(AnyError, ex.Status);
        }

        [Theory(Timeout = 2000)]
        [InlineData(Status.Ok)]
        [InlineData(AnyError)]
        public async Task write7_WriteAsync_ends_at_once_when_the_write_finishes_during_the_call(Status result)
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_write(MockCore.Handle, 0)).Returns((int)result);

            // Act
            var ex = await Record.ExceptionAsync(() => tag.WriteAsync());

            // Assert
            Assert.Equal(result, ex == null ? Status.Ok : Assert.IsType<LibPlcTagException>(ex).Status);
        }

        [Fact(Timeout = 2000)]
        public async Task read2_a_read_overlapping_another_gets_ErrorBusy_from_the_core()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            _ = tag.ReadAsync();

            // Act
            var second = tag.ReadAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => second);
            Assert.Equal(Status.ErrorBusy, ex.Status);
            core.Native.Verify(n => n.plc_tag_read(MockCore.Handle, 0), Times.Exactly(2));
        }

        [Fact(Timeout = 2000)]
        public async Task write2_WriteAsync_throws_ErrorBusy_at_once_when_the_core_is_busy()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            _ = tag.ReadAsync();

            // Act
            var write = tag.WriteAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => write);
            Assert.Equal(Status.ErrorBusy, ex.Status);
        }

        [Fact(Timeout = 2000)]
        public async Task A2_ReadAsync_aborted_by_another_caller_ends()
        {

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var read = tag.ReadAsync();

            // Act
            core.Native.Object.plc_tag_abort(MockCore.Handle);
            var ex = await Record.ExceptionAsync(() => read);

            // Assert
            Assert.True(ex == null || ex is LibPlcTagException, $"unexpected {ex}");
        }

        [Fact(Timeout = 2000)]
        public async Task An_event_that_does_not_end_the_operation_is_ignored()
        {

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var read = tag.ReadAsync();

            // Act
            core.Model.Raise(Event.ReadStarted, Status.Ok);
            core.Model.Raise(Event.ReadCompleted, Status.Ok);
            await Task.Delay(100);

            // Assert
            Assert.False(read.IsCompleted);
        }

        [Fact(Timeout = 2000)]
        public async Task An_event_delivered_inside_the_read_call_does_not_end_the_read()
        {

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            var read = tag.ReadAsync();
            await Task.Delay(100);

            // Assert
            Assert.False(read.IsCompleted);
        }

        [Fact(Timeout = 2000)]
        public async Task An_exception_from_a_user_event_handler_does_not_reach_the_core()
        {

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            tag.ReadCompleted += (_, __) => throw new InvalidOperationException("user handler");
            var read = tag.ReadAsync();

            // Act
            var ex = Record.Exception(() => core.Model.Finish(Status.Ok));

            // Assert
            Assert.Null(ex);
            Assert.Null(await Record.ExceptionAsync(() => read));
        }

        [Theory]
        [InlineData(100)]
        [InlineData(105)]
        [InlineData(9999)]
        public void An_event_the_wrapper_does_not_know_is_ignored(int code)
        {
            // Arrange
            var core = new MockCore();
            core.NewTag().Initialize();

            // Act
            var ex = Record.Exception(() => core.Model.Raise((Event)code, Status.Ok));

            // Assert
            Assert.Null(ex);
        }

        [Fact(Timeout = 2000)]
        public async Task A2_cancelling_ReadAsync_aborts_the_read()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var cts = new CancellationTokenSource();
            var read = tag.ReadAsync(cts.Token);

            // Act
            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => read);
            core.Native.Verify(n => n.plc_tag_abort(MockCore.Handle), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task A3_cancelling_WriteAsync_aborts_the_write()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var cts = new CancellationTokenSource();
            var write = tag.WriteAsync(cts.Token);

            // Act
            cts.Cancel();

            // Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => write);
            core.Native.Verify(n => n.plc_tag_abort(MockCore.Handle), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task Timeout_aborts_and_throws_ErrorTimeout()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Timeout = TimeSpan.FromMilliseconds(100);
            tag.Initialize();

            // Act
            var read = tag.ReadAsync();

            // Assert
            var ex = await Assert.ThrowsAsync<LibPlcTagException>(() => read);
            Assert.Equal(Status.ErrorTimeout, ex.Status);
            core.Native.Verify(n => n.plc_tag_abort(MockCore.Handle), Times.Once);
        }

        [Fact(Timeout = 2000)]
        public async Task InitializeAsync_with_a_token_that_is_already_cancelled_does_not_create_the_native_tag()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var task = tag.InitializeAsync(new CancellationToken(canceled: true));

            // Assert
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
            core.Native.Verify(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Never);
        }

        [Fact(Timeout = 2000)]
        public async Task ReadAsync_with_a_token_that_is_already_cancelled_does_not_create_the_native_tag()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var task = tag.ReadAsync(new CancellationToken(canceled: true));

            // Assert
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
            core.Native.Verify(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Never);
        }

        [Fact(Timeout = 2000)]
        public async Task WriteAsync_with_a_token_that_is_already_cancelled_does_not_create_the_native_tag()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            var task = tag.WriteAsync(new CancellationToken(canceled: true));

            // Assert
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
            core.Native.Verify(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>()), Times.Never);
        }

        [Fact(Timeout = 2000)]
        public async Task Cancelling_as_the_read_completes_either_succeeds_or_cancels_and_leaves_the_tag_usable()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var cts = new CancellationTokenSource();
            var read = tag.ReadAsync(cts.Token);

            // Act
            core.Model.Finish(Status.Ok);
            cts.Cancel();
            var ex = await Record.ExceptionAsync(() => read);
            var next = tag.ReadAsync();
            core.Model.Finish(Status.Ok);

            // Assert
            Assert.True(ex == null || ex is TaskCanceledException, $"unexpected {ex}");
            Assert.Null(await Record.ExceptionAsync(() => next));
        }

        [Fact]
        public void A4_Abort_on_an_idle_tag_calls_the_core()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            tag.Abort();

            // Assert
            core.Native.Verify(n => n.plc_tag_abort(MockCore.Handle), Times.Once);
        }

        [Fact]
        public void destroy3_Dispose_unregisters_the_callback_then_destroys_the_tag()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();

            // Act
            tag.Dispose();

            // Assert
            var calls = core.Native.Invocations.Select(i => i.Method.Name).Where(n => n == "plc_tag_unregister_callback" || n == "plc_tag_destroy");
            Assert.Equal(new[] { "plc_tag_unregister_callback", "plc_tag_destroy" }, calls);
        }

        [Fact(Timeout = 2000)]
        public async Task Dispose_ends_a_waiting_ReadAsync_and_destroys_the_tag_once()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            var read = tag.ReadAsync();

            // Act
            tag.Dispose();

            // Assert
            await Assert.ThrowsAsync<ObjectDisposedException>(() => read);
            core.Native.Verify(n => n.plc_tag_destroy(MockCore.Handle), Times.Once);
        }

        [Fact]
        public void Abort_after_Dispose_throws_ObjectDisposedException()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            tag.Dispose();

            // Act
            var ex = Record.Exception(() => tag.Abort());

            // Assert
            Assert.IsType<ObjectDisposedException>(ex);
        }

        [Theory]
        [InlineData(Status.Ok)]   // read.6 F3
        [InlineData(AnyError)]   // read.6 F3
        [InlineData(Status.ErrorTimeout)]   // read.5
        [InlineData(Status.ErrorBusy)]   // read.2
        public void Read_returns_or_throws_what_the_blocking_read_returns(Status result)
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            if (result == Status.ErrorBusy) _ = tag.ReadAsync();
            else core.Choose.BlockedCallEndsWith = result == Status.ErrorTimeout ? null : result;

            // Act
            var ex = Record.Exception(() => tag.Read());

            // Assert
            Assert.Equal(result, ex == null ? Status.Ok : Assert.IsType<LibPlcTagException>(ex).Status);
        }

        [Theory]
        [InlineData(Status.Ok)]   // write.6 F4
        [InlineData(AnyError)]   // write.6 F4
        [InlineData(Status.ErrorTimeout)]   // write.5
        [InlineData(Status.ErrorBusy)]   // write.2
        public void Write_returns_or_throws_what_the_blocking_write_returns(Status result)
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            if (result == Status.ErrorBusy) _ = tag.ReadAsync();
            else core.Choose.BlockedCallEndsWith = result == Status.ErrorTimeout ? null : result;

            // Act
            var ex = Record.Exception(() => tag.Write());

            // Assert
            Assert.Equal(result, ex == null ? Status.Ok : Assert.IsType<LibPlcTagException>(ex).Status);
        }

        [Fact]
        public void Read_on_a_new_tag_creates_it_then_reads()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();

            // Act
            tag.Read();

            // Assert
            core.Native.Verify(n => n.plc_tag_create_ex(It.IsAny<string>(), It.IsAny<callback_func_ex>(), It.IsAny<IntPtr>(), It.Is<int>(t => t > 0)), Times.Once);
        }

        [Fact]
        public void get1_a_getter_throws_the_status_when_it_fails()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_get_int32(MockCore.Handle, 99)).Returns(int.MinValue);
            core.Native.Setup(n => n.plc_tag_status(MockCore.Handle)).Returns((int)Status.ErrorOutOfBounds);

            // Act
            var ex = Record.Exception(() => tag.GetInt32(99));

            // Assert
            Assert.Equal(Status.ErrorOutOfBounds, Assert.IsType<LibPlcTagException>(ex).Status);
        }

        [Fact]
        public void set1_a_setter_throws_the_error_it_returns()
        {
            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_set_int32(MockCore.Handle, 99, It.IsAny<int>())).Returns((int)Status.ErrorOutOfBounds);

            // Act
            var ex = Record.Exception(() => tag.SetInt32(99, 1));

            // Assert
            Assert.Equal(Status.ErrorOutOfBounds, Assert.IsType<LibPlcTagException>(ex).Status);
        }

        [Fact]
        public void status2_a_getter_returns_int_MinValue_while_a_read_is_in_flight()
        {

            // Arrange
            var core = new MockCore();
            var tag = core.NewTag();
            tag.Initialize();
            core.Native.Setup(n => n.plc_tag_get_int32(MockCore.Handle, 0)).Returns(int.MinValue);
            _ = tag.ReadAsync();

            // Act
            var value = tag.GetInt32(0);

            // Assert
            Assert.Equal(int.MinValue, value);
        }
    }
}
