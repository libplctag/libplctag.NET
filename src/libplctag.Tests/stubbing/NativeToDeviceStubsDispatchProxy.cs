using System;
using System.Collections.Generic;
using System.Reflection;
using static libplctag.Tests.stubbing.StubRouting;

namespace libplctag.Tests.stubbing
{
    /// <summary>
    /// Responsible for creating a proxy for the INative interface and then delegating calls to it to the matching registered DeviceStub.
    /// </summary>
    public class NativeToDeviceStubsDispatchProxy : DispatchProxy
    {
        private List<IDeviceStub> DeviceMocks { get; set; } = [];

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            IDeviceStub? deviceMock = FindTargetDeviceMock(targetMethod, args);

            return deviceMock == null
                ? throw new Exception($"No device-mock found for handling target method {targetMethod}")
                : targetMethod!.Invoke(deviceMock, args);
        }


        private IDeviceStub? FindTargetDeviceMock(MethodInfo? targetMethod, object?[]? args)
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
                return DeviceMocks.Find(deviceMock => deviceMock.ShouldHandleCallsForLpString(lpsString));
            }

            int tag = (int)args![indexOfTagParameter]!;

            return DeviceMocks.Find(deviceMock => deviceMock.ShouldHandleCallsForTag(tag));
        }


        public static INative Create(List<IDeviceStub> tagMocks)
        {
            var iNativeProxy = Create<INative, NativeToDeviceStubsDispatchProxy>();
            NativeToDeviceStubsDispatchProxy dispatchProxy = (NativeToDeviceStubsDispatchProxy)iNativeProxy;
            dispatchProxy.DeviceMocks = tagMocks;
            return iNativeProxy;
        }
    }
}