// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag.NativeImport;

namespace libplctag
{
    public enum Status
    {
        /// <inheritdoc cref="STATUS.PLCTAG_STATUS_PENDING"/>
        Pending =               STATUS.PLCTAG_STATUS_PENDING,

        /// <inheritdoc cref="STATUS.PLCTAG_STATUS_OK"/>
        Ok =                    STATUS.PLCTAG_STATUS_OK,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_ABORT"/>
        ErrorAbort =            STATUS.PLCTAG_ERR_ABORT,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_CONFIG"/>
        ErrorBadConfig =        STATUS.PLCTAG_ERR_BAD_CONFIG,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_CONNECTION"/>
        ErrorBadConnection =    STATUS.PLCTAG_ERR_BAD_CONNECTION,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_DATA"/>
        ErrorBadData =          STATUS.PLCTAG_ERR_BAD_DATA,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_DEVICE"/>
        ErrorBadDevice =        STATUS.PLCTAG_ERR_BAD_DEVICE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_GATEWAY"/>
        ErrorBadGateway =       STATUS.PLCTAG_ERR_BAD_GATEWAY,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_PARAM"/>
        ErrorBadParam =         STATUS.PLCTAG_ERR_BAD_PARAM,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_REPLY"/>
        ErrorBadReply =         STATUS.PLCTAG_ERR_BAD_REPLY,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BAD_STATUS"/>
        ErrorBadStatus =        STATUS.PLCTAG_ERR_BAD_STATUS,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_CLOSE"/>
        ErrorClose =            STATUS.PLCTAG_ERR_CLOSE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_CREATE"/>
        ErrorCreate =           STATUS.PLCTAG_ERR_CREATE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_DUPLICATE"/>
        ErrorDuplicate =        STATUS.PLCTAG_ERR_DUPLICATE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_ENCODE"/>
        ErrorEncode =           STATUS.PLCTAG_ERR_ENCODE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_MUTEX_DESTROY"/>
        ErrorMutexDestroy =     STATUS.PLCTAG_ERR_MUTEX_DESTROY,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_MUTEX_INIT"/>
        ErrorMutexInit =        STATUS.PLCTAG_ERR_MUTEX_INIT,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_MUTEX_LOCK"/>
        ErrorMutexLock =        STATUS.PLCTAG_ERR_MUTEX_LOCK,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_MUTEX_UNLOCK"/>
        ErrorMutexUnlock =      STATUS.PLCTAG_ERR_MUTEX_UNLOCK,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NOT_ALLOWED"/>
        ErrorNotAllowed =       STATUS.PLCTAG_ERR_NOT_ALLOWED,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NOT_FOUND"/>
        ErrorNotFound =         STATUS.PLCTAG_ERR_NOT_FOUND,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NOT_IMPLEMENTED"/>
        ErrorNotImplemented =   STATUS.PLCTAG_ERR_NOT_IMPLEMENTED,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NO_DATA"/>
        ErrorNoData =           STATUS.PLCTAG_ERR_NO_DATA,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NO_MATCH"/>
        ErrorNoMatch =          STATUS.PLCTAG_ERR_NO_MATCH,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NO_MEM"/>
        ErrorNoMem =            STATUS.PLCTAG_ERR_NO_MEM,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NO_RESOURCES"/>
        ErrorNoResources =      STATUS.PLCTAG_ERR_NO_RESOURCES,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_NULL_PTR"/>
        ErrorNullPtr =          STATUS.PLCTAG_ERR_NULL_PTR,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_OPEN"/>
        ErrorOpen =             STATUS.PLCTAG_ERR_OPEN,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_OUT_OF_BOUNDS"/>
        ErrorOutOfBounds =      STATUS.PLCTAG_ERR_OUT_OF_BOUNDS,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_READ"/>
        ErrorRead =             STATUS.PLCTAG_ERR_READ,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_REMOTE_ERR"/>
        ErrorRemoteErr =        STATUS.PLCTAG_ERR_REMOTE_ERR,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_THREAD_CREATE"/>
        ErrorThreadCreate =     STATUS.PLCTAG_ERR_THREAD_CREATE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_THREAD_JOIN"/>
        ErrorThreadJoin =       STATUS.PLCTAG_ERR_THREAD_JOIN,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_TIMEOUT"/>
        ErrorTimeout =          STATUS.PLCTAG_ERR_TIMEOUT,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_TOO_LARGE"/>
        ErrorTooLarge =         STATUS.PLCTAG_ERR_TOO_LARGE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_TOO_SMALL"/>
        ErrorTooSmall =         STATUS.PLCTAG_ERR_TOO_SMALL,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_UNSUPPORTED"/>
        ErrorUnsupported =      STATUS.PLCTAG_ERR_UNSUPPORTED,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_WINSOCK"/>
        ErrorWinsock =          STATUS.PLCTAG_ERR_WINSOCK,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_WRITE"/>
        ErrorWrite =            STATUS.PLCTAG_ERR_WRITE,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_PARTIAL"/>
        ErrorPartial =          STATUS.PLCTAG_ERR_PARTIAL,

        /// <inheritdoc cref="STATUS.PLCTAG_ERR_BUSY"/>
        ErrorBusy =             STATUS.PLCTAG_ERR_BUSY
    }
}