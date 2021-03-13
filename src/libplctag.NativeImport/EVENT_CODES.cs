namespace libplctag.NativeImport
{
    public enum EVENT_CODES
    {
        /// <summary>
        /// A read of the tag has been requested.
        /// The callback is called immediately before the underlying protocol implementation is called.
        /// </summary>
        PLCTAG_EVENT_READ_STARTED = 1,

        /// <summary>
        /// The callback is called after a read completes.
        /// The final status of the read is passed to the callback as well.
        /// </summary>
        PLCTAG_EVENT_READ_COMPLETED = 2,

        /// <summary>
        /// As with reads, the callback is called when a write is requested.
        /// The callback can change the data in the tag and the changes will be sent to the PLC.
        /// </summary>
        PLCTAG_EVENT_WRITE_STARTED = 3,

        /// <summary>
        /// The callback is called when the PLC indicates that the write has completed.
        /// The status of the write is passed to the callback.
        /// </summary>
        PLCTAG_EVENT_WRITE_COMPLETED = 4,

        /// <summary>
        /// The callback function is called when something calls <see cref="plctag.plc_tag_abort"/> on the tag.
        /// </summary>
        PLCTAG_EVENT_ABORTED = 5,

        /// <summary>
        /// The callback function is called when the tag is being destroyed.
        /// It is not safe to call any API functions on the tag at this time.
        /// This is purely for the callback to manage any application state.
        /// </summary>
        PLCTAG_EVENT_DESTROYED = 6
    }
}
