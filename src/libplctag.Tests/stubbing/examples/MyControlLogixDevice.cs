using System;
using System.Collections.Generic;
using System.Reflection;
using static libplctag.Tests.stubbing.StubRouting;

namespace libplctag.Tests.stubbing.examples
{
    public class MyControlLogixDevice : DispatchProxy
    {
        public static readonly TimeSpan Timeout = TimeSpan.FromSeconds(2);
        public const string Gateway = "127.0.0";
        public const string Path = "1,0";
        public const Protocol Protocol = libplctag.Protocol.ab_eip;
        public const PlcType PlcType = libplctag.PlcType.ControlLogix;

        public List<TagStub> MockedTags { get; set; } = [];

        public static IDeviceStub Create()
        {
            var tagProxy = Create<IDeviceStub, MyControlLogixDevice>();
            MyControlLogixDevice device = (MyControlLogixDevice)tagProxy;

            Tag mode = ModeTagStub.CreateModeTag(new Native());
            // Register additional TagStubs here that should be available on the device

            TagBrowsingStub tagBrowsingStub = new(2, new Tag(new Native()) { Name = TagBrowsingStub.TagBrowsingParameterName });
            device.MockedTags = [new ModeTagStub(1, mode), tagBrowsingStub];
            return tagProxy;
        }

        private TagStub? FindTargetTagMock(MethodInfo? targetMethod, object?[]? args)
        {
            int indexOfTagParameter = FindIndexOfTagParameter(targetMethod);

            if (indexOfTagParameter == -1)
            {
                int indexOfLpStringParameter = FindIndexOfLpStringParameter(targetMethod);

                if (indexOfLpStringParameter == -1)
                {
                    return null;
                }

                string lpsString = (string)args![indexOfLpStringParameter]!;
                return MockedTags.Find(deviceMock => deviceMock.IsResponsibleForLpString(lpsString));
            }

            int tag = (int)args![indexOfTagParameter]!;

            return MockedTags.Find(deviceMock => deviceMock.IsResponsibleForTag(tag));
        }


        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            var type = typeof(MyControlLogixDevice);
            // Get the PropertyInfo
            PropertyInfo property = type.GetProperty(nameof(MockedTags))!;

            // Get the MethodInfo for the getter
            MethodInfo getter = property.GetMethod!; // or property.GetGetMethod()

            if (getter.Name.Equals(targetMethod?.Name))
            {
                return MockedTags;
            }

            TagStub? deviceMock = FindTargetTagMock(targetMethod, args);

            return deviceMock == null
                ? throw new Exception($"No tag-mock found for handling target method {targetMethod}")
                : targetMethod!.Invoke(deviceMock, args);
        }
    }
}