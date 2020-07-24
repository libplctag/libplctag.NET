using libplctag.NativeImport;

namespace libplctag
{
    public enum DebugLevel
    {
        None =      DEBUG_LEVELS.PLCTAG_DEBUG_NONE,
        Error =     DEBUG_LEVELS.PLCTAG_DEBUG_ERROR,
        Warn =      DEBUG_LEVELS.PLCTAG_DEBUG_WARN,
        Info =      DEBUG_LEVELS.PLCTAG_DEBUG_INFO,
        Detail =    DEBUG_LEVELS.PLCTAG_DEBUG_DETAIL,
        Spew =      DEBUG_LEVELS.PLCTAG_DEBUG_SPEW
    }
}