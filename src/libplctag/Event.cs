using libplctag.NativeImport;

namespace libplctag
{
    /// <summary>
    /// Events returned by the base libplctag library
    /// </summary>
    public enum Event
    {
        /// <inheritdoc cref="EVENT_CODES.PLCTAG_EVENT_READ_STARTED"/>
        ReadStarted =       EVENT_CODES.PLCTAG_EVENT_READ_STARTED,

        /// <inheritdoc cref="EVENT_CODES.PLCTAG_EVENT_READ_COMPLETED"/>
        ReadCompleted =     EVENT_CODES.PLCTAG_EVENT_READ_COMPLETED,

        /// <inheritdoc cref="EVENT_CODES.PLCTAG_EVENT_WRITE_STARTED"/>
        WriteStarted =      EVENT_CODES.PLCTAG_EVENT_WRITE_STARTED,

        /// <inheritdoc cref="EVENT_CODES.PLCTAG_EVENT_WRITE_COMPLETED"/>
        WriteCompleted =    EVENT_CODES.PLCTAG_EVENT_WRITE_COMPLETED,

        /// <inheritdoc cref="EVENT_CODES.PLCTAG_EVENT_ABORTED"/>
        Aborted =           EVENT_CODES.PLCTAG_EVENT_ABORTED,

        /// <inheritdoc cref="EVENT_CODES.PLCTAG_EVENT_DESTROYED"/>
        Destroyed =         EVENT_CODES.PLCTAG_EVENT_DESTROYED
    }
}