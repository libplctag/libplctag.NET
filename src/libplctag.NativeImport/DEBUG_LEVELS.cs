namespace libplctag.NativeImport
{
    public enum DEBUG_LEVELS
    {
        /// <summary>
        /// Disables debugging output.
        /// </summary>
        PLCTAG_DEBUG_NONE = 0,

        /// <summary>
        /// Only output errors. Generally these are fatal to the functioning of the library.
        /// </summary>
        PLCTAG_DEBUG_ERROR = 1,

        /// <summary>
        /// Outputs warnings such as error found when checking a malformed tag attribute string or when unexpected problems are reported from the PLC.
        /// </summary>
        PLCTAG_DEBUG_WARN = 2,

        /// <summary>
        /// Outputs diagnostic information about the internal calls within the library.
        /// Includes some packet dumps.
        /// </summary>
        PLCTAG_DEBUG_INFO = 3,

        /// <summary>
        /// Outputs detailed diagnostic information about the code executing within the library including packet dumps.
        /// </summary>
        PLCTAG_DEBUG_DETAIL = 4,

        /// <summary>
        /// Outputs extremely detailed information.
        /// Do not use this unless you are trying to debug detailed information about every mutex lock and release.
        /// Will output many lines of output per millisecond.
        /// You have been warned!
        /// </summary>
        PLCTAG_DEBUG_SPEW = 5
    }
}
