using libplctag.NativeImport;

namespace libplctag
{
    public enum Status
    {
        /// <inheritdoc cref="STATUS_CODES.PLCTAG_STATUS_PENDING"/>
        Pending =               STATUS_CODES.PLCTAG_STATUS_PENDING,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_STATUS_OK"/>
        Ok =                    STATUS_CODES.PLCTAG_STATUS_OK,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_ABORT"/>
        ErrorAbort =            STATUS_CODES.PLCTAG_ERR_ABORT,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_CONFIG"/>
        ErrorBadConfig =        STATUS_CODES.PLCTAG_ERR_BAD_CONFIG,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_CONNECTION"/>
        ErrorBadConnection =    STATUS_CODES.PLCTAG_ERR_BAD_CONNECTION,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_DATA"/>
        ErrorBadData =          STATUS_CODES.PLCTAG_ERR_BAD_DATA,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_DEVICE"/>
        ErrorBadDevice =        STATUS_CODES.PLCTAG_ERR_BAD_DEVICE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_GATEWAY"/>
        ErrorBadGateway =       STATUS_CODES.PLCTAG_ERR_BAD_GATEWAY,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_PARAM"/>
        ErrorBadParam =         STATUS_CODES.PLCTAG_ERR_BAD_PARAM,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_REPLY"/>
        ErrorBadReply =         STATUS_CODES.PLCTAG_ERR_BAD_REPLY,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BAD_STATUS"/>
        ErrorBadStatus =        STATUS_CODES.PLCTAG_ERR_BAD_STATUS,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_CLOSE"/>
        ErrorClose =            STATUS_CODES.PLCTAG_ERR_CLOSE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_CREATE"/>
        ErrorCreate =           STATUS_CODES.PLCTAG_ERR_CREATE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_DUPLICATE"/>
        ErrorDuplicate =        STATUS_CODES.PLCTAG_ERR_DUPLICATE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_ENCODE"/>
        ErrorEncode =           STATUS_CODES.PLCTAG_ERR_ENCODE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_MUTEX_DESTROY"/>
        ErrorMutexDestroy =     STATUS_CODES.PLCTAG_ERR_MUTEX_DESTROY,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_MUTEX_INIT"/>
        ErrorMutexInit =        STATUS_CODES.PLCTAG_ERR_MUTEX_INIT,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_MUTEX_LOCK"/>
        ErrorMutexLock =        STATUS_CODES.PLCTAG_ERR_MUTEX_LOCK,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_MUTEX_UNLOCK"/>
        ErrorMutexUnlock =      STATUS_CODES.PLCTAG_ERR_MUTEX_UNLOCK,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NOT_ALLOWED"/>
        ErrorNotAllowed =       STATUS_CODES.PLCTAG_ERR_NOT_ALLOWED,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NOT_FOUND"/>
        ErrorNotFound =         STATUS_CODES.PLCTAG_ERR_NOT_FOUND,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NOT_IMPLEMENTED"/>
        ErrorNotImplemented =   STATUS_CODES.PLCTAG_ERR_NOT_IMPLEMENTED,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NO_DATA"/>
        ErrorNoData =           STATUS_CODES.PLCTAG_ERR_NO_DATA,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NO_MATCH"/>
        ErrorNoMatch =          STATUS_CODES.PLCTAG_ERR_NO_MATCH,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NO_MEM"/>
        ErrorNoMem =            STATUS_CODES.PLCTAG_ERR_NO_MEM,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NO_RESOURCES"/>
        ErrorNoResources =      STATUS_CODES.PLCTAG_ERR_NO_RESOURCES,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_NULL_PTR"/>
        ErrorNullPtr =          STATUS_CODES.PLCTAG_ERR_NULL_PTR,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_OPEN"/>
        ErrorOpen =             STATUS_CODES.PLCTAG_ERR_OPEN,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_OUT_OF_BOUNDS"/>
        ErrorOutOfBounds =      STATUS_CODES.PLCTAG_ERR_OUT_OF_BOUNDS,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_READ"/>
        ErrorRead =             STATUS_CODES.PLCTAG_ERR_READ,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_REMOTE_ERR"/>
        ErrorRemoteErr =        STATUS_CODES.PLCTAG_ERR_REMOTE_ERR,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_THREAD_CREATE"/>
        ErrorThreadCreate =     STATUS_CODES.PLCTAG_ERR_THREAD_CREATE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_THREAD_JOIN"/>
        ErrorThreadJoin =       STATUS_CODES.PLCTAG_ERR_THREAD_JOIN,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_TIMEOUT"/>
        ErrorTimeout =          STATUS_CODES.PLCTAG_ERR_TIMEOUT,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_TOO_LARGE"/>
        ErrorTooLarge =         STATUS_CODES.PLCTAG_ERR_TOO_LARGE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_TOO_SMALL"/>
        ErrorTooSmall =         STATUS_CODES.PLCTAG_ERR_TOO_SMALL,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_UNSUPPORTED"/>
        ErrorUnsupported =      STATUS_CODES.PLCTAG_ERR_UNSUPPORTED,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_WINSOCK"/>
        ErrorWinsock =          STATUS_CODES.PLCTAG_ERR_WINSOCK,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_WRITE"/>
        ErrorWrite =            STATUS_CODES.PLCTAG_ERR_WRITE,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_PARTIAL"/>
        ErrorPartial =          STATUS_CODES.PLCTAG_ERR_PARTIAL,

        /// <inheritdoc cref="STATUS_CODES.PLCTAG_ERR_BUSY"/>
        ErrorBusy =             STATUS_CODES.PLCTAG_ERR_BUSY
    }
}