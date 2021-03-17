using libplctag.NativeImport;

namespace libplctag
{
    /// <summary>
    /// Debug levels available in the base libplctag library
    /// </summary>
    public enum DebugLevel
    {
        /// <inheritdoc cref="DEBUG_LEVELS.PLCTAG_DEBUG_NONE"/>
        None =      DEBUG_LEVELS.PLCTAG_DEBUG_NONE,

        /// <inheritdoc cref="DEBUG_LEVELS.PLCTAG_DEBUG_ERROR"/>
        Error =     DEBUG_LEVELS.PLCTAG_DEBUG_ERROR,

        /// <inheritdoc cref="DEBUG_LEVELS.PLCTAG_DEBUG_WARN"/>
        Warn =      DEBUG_LEVELS.PLCTAG_DEBUG_WARN,

        /// <inheritdoc cref="DEBUG_LEVELS.PLCTAG_DEBUG_INFO"/>
        Info =      DEBUG_LEVELS.PLCTAG_DEBUG_INFO,

        /// <inheritdoc cref="DEBUG_LEVELS.PLCTAG_DEBUG_DETAIL"/>
        Detail =    DEBUG_LEVELS.PLCTAG_DEBUG_DETAIL,

        /// <inheritdoc cref="DEBUG_LEVELS.PLCTAG_DEBUG_SPEW"/>
        Spew =      DEBUG_LEVELS.PLCTAG_DEBUG_SPEW
    }
}