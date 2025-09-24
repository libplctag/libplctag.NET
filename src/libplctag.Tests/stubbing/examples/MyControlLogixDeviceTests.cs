using FluentAssertions;
using libplctag.NativeImport;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace libplctag.Tests.stubbing.examples
{
    public class MyControlLogixDeviceTests
    {
        [Fact]
        public void TestRoutingWithStubs()
        {
            // ARRANGE
            IDeviceStub myControlLogixDevice = MyControlLogixDevice.Create();
            INative nativeProxy = NativeToDeviceStubsDispatchProxy.Create([myControlLogixDevice]);
            TagBrowsingStub tagBrowsingStub = (myControlLogixDevice.MockedTags[1] as TagBrowsingStub)!;
            int tagBrowsingHandle = 2;
            tagBrowsingStub.Mock.Setup(mock => mock.plc_tag_create_ex(It.IsAny<string>(),
                It.IsAny<plctag.callback_func_ex>(), It.IsAny<IntPtr>(), It.IsAny<int>())).Returns(tagBrowsingHandle);
            tagBrowsingStub.Mock.Setup(mock => mock.plc_tag_get_size(It.IsAny<int>())).Returns(20);
            Tag modeTag = ModeTagStub.CreateModeTag(nativeProxy);
            Tag tagBrowsingTag = new(nativeProxy) { Name = "@tags" };


            // ACT
            modeTag.Initialize();
            byte[] modeValueOne = modeTag.GetBuffer();
            byte[] modeValueTwo = modeTag.GetBuffer();
            Action modeValueThree = () => modeTag.GetBuffer();
            tagBrowsingTag.Initialize();
            


            // ASSERT
            modeTag.IsInitialized.Should().Be(true);
            modeTag.NativeTagHandle.Should().Be(ModeTagStub.ModeTagHandle);
            modeValueOne.Should().Equal(ModeTagStub.ProgramMode);
            modeValueTwo.Should().Equal(ModeTagStub.RunMode);
            modeValueThree.Should().Throw<TargetInvocationException>()
                .WithInnerException<TargetInvocationException>()
                .WithInnerException<TimeoutException>();
            
            tagBrowsingTag.IsInitialized.Should().Be(true);
            tagBrowsingTag.NativeTagHandle.Should().Be(tagBrowsingHandle);
        }
    }
}