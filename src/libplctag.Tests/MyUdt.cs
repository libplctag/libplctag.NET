using System.Runtime.InteropServices;

namespace libplctag.Tests
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MyUdt
    {
        public short shortField;
        public int intField;
    }
}