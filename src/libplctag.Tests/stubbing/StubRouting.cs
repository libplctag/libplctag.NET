using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace libplctag.Tests.stubbing
{
    /// <summary>
    /// Helper methods to determine which stub should be called for invocations of methods of the INative interface
    /// depending on the tag and the lpString parameters.
    /// </summary>
    public static class StubRouting
    {
        private const string TagParameterName = "tag";
        private const string LpStringParameterName = "lpString";

        public static int FindIndexOfTagParameter(MethodInfo? targetMethod)
        {
            List<ParameterInfo> parameters = targetMethod?.GetParameters().ToList() ?? [];
            return parameters.FindIndex(IsTagParameter);
        }

        public static int FindIndexOfLpStringParameter(MethodInfo? targetMethod)
        {
            List<ParameterInfo> parameters = targetMethod?.GetParameters().ToList() ?? [];
            return parameters.FindIndex(IsLpStringParameter);
        }

        private static bool IsLpStringParameter(ParameterInfo info)
        {
            return info.Name == LpStringParameterName && info.ParameterType == typeof(string);
        }

        private static bool IsTagParameter(ParameterInfo info)
        {
            return info.Name == TagParameterName && info.ParameterType == typeof(int);
        }
    }
}