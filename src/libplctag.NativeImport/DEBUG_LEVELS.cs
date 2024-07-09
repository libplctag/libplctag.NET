// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.NativeImport
{
    /// <summary>
    /// The library provides debugging output when enabled.
    /// There are three ways to set the debug level (for historical reasons):
    /// <list type="number">
    ///     <item>Adding a debug attribute to the attribute string when creating a tag: "protocol=XXX&amp;...&amp;debug=4".</item>
    ///     <item>Using the <see cref="plctag.plc_tag_set_int_attribute(int, string, int)"/>function to set the debug attribute.</item>
    ///     <item>Using the <see cref="plctag.plc_tag_set_debug_level(int)"/> function.</item>
    /// </list>
    /// The preferred method in code is the last one.
    /// </summary>
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
