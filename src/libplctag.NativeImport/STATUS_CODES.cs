namespace libplctag.NativeImport
{
    public enum STATUS_CODES
    {
        /// <summary>
        /// Operation in progress. Not an error.
        /// </summary>
        PLCTAG_STATUS_PENDING = 1,

        /// <summary>
        /// No error.
        /// </summary>
        PLCTAG_STATUS_OK = 0,

        /// <summary>
        /// The operation was aborted.
        /// </summary>
        PLCTAG_ERR_ABORT = -1,

        /// <summary>
        /// The operation failed due to incorrect configuration. Usually returned from a remote system.
        /// </summary>
        PLCTAG_ERR_BAD_CONFIG = -2,

        /// <summary>
        /// The connection failed for some reason. This can mean that the remote PLC was power cycled, for instance.
        /// </summary>
        PLCTAG_ERR_BAD_CONNECTION = -3,

        /// <summary>
        /// The data received from the remote PLC was undecipherable or otherwise not able to be processed.
        /// Can also be returned from a remote system that cannot process the data sent to it.
        /// </summary>
        PLCTAG_ERR_BAD_DATA = -4,

        /// <summary>
        /// Usually returned from a remote system when something addressed does not exist.
        /// </summary>
        PLCTAG_ERR_BAD_DEVICE = -5,

        /// <summary>
        /// Usually returned when the library is unable to connect to a remote system.
        /// </summary>
        PLCTAG_ERR_BAD_GATEWAY = -6,

        /// <summary>
        /// A common error return when something is not correct with the tag creation attribute string.
        /// </summary>
        PLCTAG_ERR_BAD_PARAM = -7,

        /// <summary>
        /// Usually returned when the remote system returned an unexpected response.
        /// </summary>
        PLCTAG_ERR_BAD_REPLY = -8,

        /// <summary>
        /// Usually returned by a remote system when something is not in a good state.
        /// </summary>
        PLCTAG_ERR_BAD_STATUS = -9,

        /// <summary>
        /// An error occurred trying to close some resource.
        /// </summary>
        PLCTAG_ERR_CLOSE = -10,

        /// <summary>
        /// An error occurred trying to create some internal resource.
        /// </summary>
        PLCTAG_ERR_CREATE = -11,

        /// <summary>
        /// An error returned by a remote system when something is incorrectly duplicated (i.e. a duplicate connection ID).
        /// </summary>
        PLCTAG_ERR_DUPLICATE = -12,

        /// <summary>
        /// An error was returned when trying to encode some data such as a tag name.
        /// </summary>
        PLCTAG_ERR_ENCODE = -13,

        /// <summary>
        /// An internal library error. It would be very unusual to see this.
        /// </summary>
        PLCTAG_ERR_MUTEX_DESTROY = -14,

        /// <summary>
        /// An internal library error. It would be very unusual to see this.
        /// </summary>
        PLCTAG_ERR_MUTEX_INIT = -15,

        /// <summary>
        /// An internal library error. It would be very unusual to see this.
        /// </summary>
        PLCTAG_ERR_MUTEX_LOCK = -16,

        /// <summary>
        /// An internal library error. It would be very unusual to see this.
        /// </summary>
        PLCTAG_ERR_MUTEX_UNLOCK = -17,

        /// <summary>
        /// Often returned from the remote system when an operation is not permitted.
        /// </summary>
        PLCTAG_ERR_NOT_ALLOWED = -18,

        /// <summary>
        /// Often returned from the remote system when something is not found.
        /// </summary>
        PLCTAG_ERR_NOT_FOUND = -19,

        /// <summary>
        /// returned when a valid operation is not implemented.
        /// </summary>
        PLCTAG_ERR_NOT_IMPLEMENTED = -20,

        /// <summary>
        /// Returned when expected data is not present.
        /// </summary>
        PLCTAG_ERR_NO_DATA = -21,

        /// <summary>
        /// Similar to <see cref="PLCTAG_ERR_NOT_FOUND"/>
        /// </summary>
        PLCTAG_ERR_NO_MATCH = -22,

        /// <summary>
        /// Returned by the library when memory allocation fails.
        /// </summary>
        PLCTAG_ERR_NO_MEM = -23,

        /// <summary>
        /// Returned by the remote system when some resource allocation fails.
        /// </summary>
        PLCTAG_ERR_NO_RESOURCES = -24,

        /// <summary>
        /// Usually an internal error, but can be returned when an invalid handle is used with an API call.
        /// </summary>
        PLCTAG_ERR_NULL_PTR = -25,

        /// <summary>
        /// Returned when an error occurs opening a resource such as a socket.
        /// </summary>
        PLCTAG_ERR_OPEN = -26,

        /// <summary>
        /// Usually returned when trying to write a value into a tag outside of the tag data bounds.
        /// </summary>
        PLCTAG_ERR_OUT_OF_BOUNDS = -27,

        /// <summary>
        /// Returned when an error occurs during a read operation. Usually related to socket problems.
        /// </summary>
        PLCTAG_ERR_READ = -28,

        /// <summary>
        /// An unspecified or untranslatable remote error causes this.
        /// </summary>
        PLCTAG_ERR_REMOTE_ERR = -29,

        /// <summary>
        /// An internal library error. If you see this, it is likely that everything is about to crash.
        /// </summary>
        PLCTAG_ERR_THREAD_CREATE = -30,

        /// <summary>
        /// Another internal library error. It is very unlikely that you will see this.
        /// </summary>
        PLCTAG_ERR_THREAD_JOIN = -31,

        /// <summary>
        /// An operation took too long and timed out.
        /// </summary>
        PLCTAG_ERR_TIMEOUT = -32,

        /// <summary>
        /// More data was returned than was expected.
        /// </summary>
        PLCTAG_ERR_TOO_LARGE = -33,

        /// <summary>
        /// Insufficient data was returned from the remote system.
        /// </summary>
        PLCTAG_ERR_TOO_SMALL = -34,

        /// <summary>
        /// The operation is not supported on the remote system.
        /// </summary>
        PLCTAG_ERR_UNSUPPORTED = -35,

        /// <summary>
        /// A Winsock-specific error occurred (only on Windows).
        /// </summary>
        PLCTAG_ERR_WINSOCK = -36,

        /// <summary>
        /// An error occurred trying to write, usually to a socket.
        /// </summary>
        PLCTAG_ERR_WRITE = -37,

        /// <summary>
        /// Partial data was received or something was unexpectedly incomplete.
        /// </summary>
        PLCTAG_ERR_PARTIAL = -38,

        /// <summary>
        /// The operation cannot be performed as some other operation is taking place.
        /// </summary>
        PLCTAG_ERR_BUSY = -39
    }
}
