using System.Collections.Generic;

namespace libplctag.Tests.stubbing
{
    //
        
    public interface IDeviceStub : INative
    {
        List<TagStub> MockedTags { get; }
    }


    public static class DeviceStubExtensions
    {
        public static bool ShouldHandleCallsForTag(this IDeviceStub stub, int tag)
        {
            return stub.MockedTags.Exists(tagHandle => tagHandle.IsResponsibleForTag(tag));
        }

        public static bool ShouldHandleCallsForLpString(this IDeviceStub stub, string lpString)
        {
            return stub.MockedTags.Exists(tagHandle => tagHandle.IsResponsibleForLpString(lpString));
        }
    }
}