// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.NativeImport;

    /// <summary>
    /// You can query the connection status of a tag using the <c>connection_status</c> attribute
    /// with <see cref="plctag.plc_tag_get_int_attribute"/>:
    /// <code>
    /// int status = plc_tag_get_int_attribute(tag, "connection_status", PLCTAG_CONN_STATUS_DOWN);
    /// </code>
    /// <para>
    /// This attribute is read-only and can be queried at any time after tag creation.
    /// It is useful for determining the current state of the connection to the PLC
    /// and can be used in conjunction with callbacks to monitor connection lifecycle events.
    /// </para>
    /// </summary>
    public enum CONN_STATUS
    {
        /// <summary>
        /// Connected and ready for communication.
        /// </summary>
        PLCTAG_CONN_STATUS_UP = 0,

        /// <summary>
        /// Not connected.
        /// </summary>
        PLCTAG_CONN_STATUS_DOWN = 1,

        /// <summary>
        /// Disconnection in progress.
        /// </summary>
        PLCTAG_CONN_STATUS_DISCONNECTING = 2,

        /// <summary>
        /// Connection in progress.
        /// </summary>
        PLCTAG_CONN_STATUS_CONNECTING = 3,

        /// <summary>
        /// Waiting to reconnect after disconnect.
        /// </summary>
        PLCTAG_CONN_STATUS_IDLE_WAIT = 4,

        /// <summary>
        /// Waiting to retry after error.
        /// </summary>
        PLCTAG_CONN_STATUS_ERR_WAIT = 5
    }