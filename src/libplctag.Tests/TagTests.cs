using FluentAssertions;
using libplctag.NativeImport;
using Moq;
using System;
using System.Net;
using System.Runtime.InteropServices;
using Xunit;

namespace libplctag.Tests
{
    public class TagTests : IDisposable
    {
        private const int TimeoutInMilliSeconds = 1000;
        private const int NativeTagHandle = 1;
        private readonly MockRepository _mockRepository;
        private readonly Mock<INative> _iNativeMock;
        private readonly Tag _underTest;


        public TagTests()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _iNativeMock = _mockRepository.Create<INative>();
            _underTest = new Tag(_iNativeMock.Object);
            _underTest.Timeout = TimeSpan.FromMilliseconds(TimeoutInMilliSeconds);
        }

        [Fact]
        public void TagCanBeInitialized()
        {
            // ARRANGE
            GivenTagCanBeInitializedWithHandle(NativeTagHandle);

            // ACT
            _underTest.Initialize();


            // ASSERT
            _underTest.IsInitialized.Should().BeTrue();
            _underTest.NativeTagHandle.Should().Be(NativeTagHandle);
        }

        private void GivenTagCanBeInitializedWithHandle(int nativeTagHandle)
        {
            _iNativeMock.Setup(native => native.plc_tag_create_ex(
                    It.IsAny<string>(),
                    It.IsAny<plctag.callback_func_ex>(),
                    IntPtr.Zero,
                    TimeoutInMilliSeconds))
                .Returns(nativeTagHandle);
        }


        [Fact]
        public void TagForMyUdtShouldReturnMockedValue()
        {
            // ARRANGE
            MyUdt expectedValue = new() { intField = 1, shortField = 2 };
            _underTest.ElementSize = Marshal.SizeOf(typeof(MyUdt));

            GivenTagCanBeInitializedWithHandle(NativeTagHandle);
            GivenMarshalledDataIsReturnedInBuffer(NativeTagHandle, expectedValue);
            
            // ACT
            _underTest.Initialize();
            MyUdt currentTagValue = _underTest.GetValue<MyUdt>();

            // ASSERT
            currentTagValue.Should().Be(expectedValue);
        }

        private void GivenMarshalledDataIsReturnedInBuffer<T>(int nativeTagHandle, T expectedData) where T : struct
        {
            byte[] byteData = expectedData.ToByteArray();
            int size = Marshal.SizeOf(typeof(T));

            _iNativeMock.Setup(native => native.plc_tag_get_size(nativeTagHandle)).Returns(size);
            _iNativeMock.Setup(native => native.plc_tag_get_raw_bytes(nativeTagHandle, 0, It.IsAny<byte[]>(), size))
                .Returns((int)Status.Ok)
                .Callback((int tag, int start_offset, byte[] buffer, int buffer_length) =>
                {
                    Array.Copy(byteData, buffer, byteData.Length);
                });
        }


        public void Dispose()
        {
            _mockRepository.VerifyAll();
            GC.SuppressFinalize(this);
        }
    }
}