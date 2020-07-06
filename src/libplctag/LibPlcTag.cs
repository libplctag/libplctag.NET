using libplctag.NativeImport;

namespace libplctag
{
    public static class LibPlcTag
    {

        static public int VersionMajor => plctag.get_int_attribute(0, "version_major", int.MinValue);
        static public int VersionMinor => plctag.get_int_attribute(0, "version_minor", int.MinValue);
        static public int VersionPatch => plctag.get_int_attribute(0, "version_patch", int.MinValue);
        static public bool IsRequiredVersion(int requiredMajor, int requiredMinor, int requiredPatch)
        {
            var result = (StatusCode)plctag.check_lib_version(requiredMajor, requiredMinor, requiredPatch);

            if (result == StatusCode.PLCTAG_STATUS_OK)
                return true;
            else if (result == StatusCode.PLCTAG_ERR_UNSUPPORTED)
                return false;
            else
                throw new NotImplementedException();
        }

    }
}
