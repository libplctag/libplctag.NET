namespace libplctag
{
    public class StatusCode
    {

        /* library internal status. */
        public const int PLCTAG_STATUS_PENDING = 1; // Operation in progress. Not an error.
        public const int PLCTAG_STATUS_OK = 0; // No error.

        /* for reference only: use DecodeError to get the string associated to the error code*/
        public const int PLCTAG_ERR_ABORT = -1; // The operation was aborted.
        public const int PLCTAG_ERR_BAD_CONFIG = -2; // the operation failed due to incorrect configuration. Usually returned from a remote system.
        public const int PLCTAG_ERR_BAD_CONNECTION = -3; // the connection failed for some reason. This can mean that the remote PLC was power cycled, for instance.
        public const int PLCTAG_ERR_BAD_DATA = -4; // the data received from the remote PLC was undecipherable or otherwise not able to be processed. Can also be returned from a remote system that cannot process the data sent to it.
        public const int PLCTAG_ERR_BAD_DEVICE = -5; // usually returned from a remote system when something addressed does not exist.
        public const int PLCTAG_ERR_BAD_GATEWAY = -6; // usually returned when the library is unable to connect to a remote system.
        public const int PLCTAG_ERR_BAD_PARAM = -7; // a common error return when something is not correct with the tag creation attribute string.
        public const int PLCTAG_ERR_BAD_REPLY = -8; // usually returned when the remote system returned an unexpected response.
        public const int PLCTAG_ERR_BAD_STATUS = -9; // usually returned by a remote system when something is not in a good state.
        public const int PLCTAG_ERR_CLOSE = -10; // an error occurred trying to close some resource.
        public const int PLCTAG_ERR_CREATE = -11; // an error occurred trying to create some internal resource.
        public const int PLCTAG_ERR_DUPLICATE = -12; // an error returned by a remote system when something is incorrectly duplicated = i.e. a duplicate connection ID).
        public const int PLCTAG_ERR_ENCODE = -13; // an error was returned when trying to encode some data such as a tag name.
        public const int PLCTAG_ERR_MUTEX_DESTROY = -14; // an internal library error. It would be very unusual to see this.
        public const int PLCTAG_ERR_MUTEX_INIT = -15; // as above.
        public const int PLCTAG_ERR_MUTEX_LOCK = -16; // as above.
        public const int PLCTAG_ERR_MUTEX_UNLOCK = -17; // as above.
        public const int PLCTAG_ERR_NOT_ALLOWED = -18; // often returned from the remote system when an operation is not permitted.
        public const int PLCTAG_ERR_NOT_FOUND = -19; // often returned from the remote system when something is not found.
        public const int PLCTAG_ERR_NOT_IMPLEMENTED = -20; // returned when a valid operation is not implemented.
        public const int PLCTAG_ERR_NO_DATA = -21; // returned when expected data is not present.
        public const int PLCTAG_ERR_NO_MATCH = -22; // similar to NOT_FOUND.
        public const int PLCTAG_ERR_NO_MEM = -23; // returned by the library when memory allocation fails.
        public const int PLCTAG_ERR_NO_RESOURCES = -24; // returned by the remote system when some resource allocation fails.
        public const int PLCTAG_ERR_NULL_PTR = -25; // usually an internal error, but can be returned when an invalid handle is used with an API call.
        public const int PLCTAG_ERR_OPEN = -26; // returned when an error occurs opening a resource such as a socket.
        public const int PLCTAG_ERR_OUT_OF_BOUNDS = -27; // usually returned when trying to write a value into a tag outside of the tag data bounds.
        public const int PLCTAG_ERR_READ = -28; // returned when an error occurs during a read operation. Usually related to socket problems.
        public const int PLCTAG_ERR_REMOTE_ERR = -29; // an unspecified or untranslatable remote error causes this.
        public const int PLCTAG_ERR_THREAD_CREATE = -30; // an internal library error. If you see this, it is likely that everything is about to crash.
        public const int PLCTAG_ERR_THREAD_JOIN = -31; // another internal library error. It is very unlikely that you will see this.
        public const int PLCTAG_ERR_TIMEOUT = -32; // an operation took too long and timed out.
        public const int PLCTAG_ERR_TOO_LARGE = -33; // more data was returned than was expected.
        public const int PLCTAG_ERR_TOO_SMALL = -34; // insufficient data was returned from the remote system.
        public const int PLCTAG_ERR_UNSUPPORTED = -35; // the operation is not supported on the remote system.
        public const int PLCTAG_ERR_WINSOCK = -36; // a Winsock-specific error occurred = only on Windows).
        public const int PLCTAG_ERR_WRITE = -37; // an error occurred trying to write, usually to a socket.

    }
}
