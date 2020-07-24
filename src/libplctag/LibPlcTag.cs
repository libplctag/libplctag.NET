using libplctag.NativeImport;
using System;

namespace libplctag
{
    public static class LibPlcTag
    {
        private const int LIB_ATTRIBUTE_POINTER = 0;

        static public int VersionMajor => plctag.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "version_major", int.MinValue);
        static public int VersionMinor => plctag.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "version_minor", int.MinValue);
        static public int VersionPatch => plctag.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "version_patch", int.MinValue);
        static public bool IsRequiredVersion(int requiredMajor, int requiredMinor, int requiredPatch)
        {
            var result = (Status)plctag.plc_tag_check_lib_version(requiredMajor, requiredMinor, requiredPatch);

            if (result == Status.Ok)
                return true;
            else if (result == Status.ErrorUnsupported)
                return false;
            else
                throw new NotImplementedException();
        }

        static public DebugLevel DebugLevel
        {
            get => (DebugLevel)plctag.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "debug", int.MinValue);
            set => plctag.plc_tag_set_debug_level((int)value);
        }

    }
}
