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
    /// Debug levels available in the base libplctag library
    /// </summary>
    public enum DebugLevel
    {
        /// <inheritdoc cref="DEBUG_LEVEL.PLCTAG_DEBUG_NONE"/>
        None =      DEBUG_LEVEL.PLCTAG_DEBUG_NONE,

        /// <inheritdoc cref="DEBUG_LEVEL.PLCTAG_DEBUG_ERROR"/>
        Error =     DEBUG_LEVEL.PLCTAG_DEBUG_ERROR,

        /// <inheritdoc cref="DEBUG_LEVEL.PLCTAG_DEBUG_WARN"/>
        Warn =      DEBUG_LEVEL.PLCTAG_DEBUG_WARN,

        /// <inheritdoc cref="DEBUG_LEVEL.PLCTAG_DEBUG_INFO"/>
        Info =      DEBUG_LEVEL.PLCTAG_DEBUG_INFO,

        /// <inheritdoc cref="DEBUG_LEVEL.PLCTAG_DEBUG_DETAIL"/>
        Detail =    DEBUG_LEVEL.PLCTAG_DEBUG_DETAIL,

        /// <inheritdoc cref="DEBUG_LEVEL.PLCTAG_DEBUG_SPEW"/>
        Spew =      DEBUG_LEVEL.PLCTAG_DEBUG_SPEW
    }
}