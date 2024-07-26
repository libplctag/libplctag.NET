// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag.NativeImport;

namespace libplctag
{
    /// <summary>
    /// Events returned by the base libplctag library
    /// </summary>
    public enum Event
    {
        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_READ_STARTED"/>
        ReadStarted =       EVENT.PLCTAG_EVENT_READ_STARTED,

        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_READ_COMPLETED"/>
        ReadCompleted =     EVENT.PLCTAG_EVENT_READ_COMPLETED,

        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_WRITE_STARTED"/>
        WriteStarted =      EVENT.PLCTAG_EVENT_WRITE_STARTED,

        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_WRITE_COMPLETED"/>
        WriteCompleted =    EVENT.PLCTAG_EVENT_WRITE_COMPLETED,

        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_ABORTED"/>
        Aborted =           EVENT.PLCTAG_EVENT_ABORTED,

        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_DESTROYED"/>
        Destroyed =         EVENT.PLCTAG_EVENT_DESTROYED,

        /// <inheritdoc cref="EVENT.PLCTAG_EVENT_CREATED"/>
        Created =           EVENT.PLCTAG_EVENT_CREATED
    }
}