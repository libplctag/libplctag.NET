// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.NativeImport;

    /// <summary>
    /// The library supports setting debug levels for individual modules.
    /// This allows for fine-grained control over which subsystems produce debug output.
    /// Used with <see cref="plctag.plc_tag_set_debug_module_level"/> and <see cref="plctag.plc_tag_get_debug_module_level"/>.
    /// </summary>
    public enum DEBUG_MODULE
    {
        PLCTAG_MODULE_LIB = 0,
        PLCTAG_MODULE_INIT = 1,
        PLCTAG_MODULE_VERSION = 2,
        PLCTAG_MODULE_UTILS = 3,
        PLCTAG_MODULE_AB_SESSION = 4,
        PLCTAG_MODULE_AB_PCCC = 5,
        PLCTAG_MODULE_AB_CIP = 6,
        PLCTAG_MODULE_AB_COMMON = 7,
        PLCTAG_MODULE_AB_EIP_CIP = 8,
        PLCTAG_MODULE_AB_EIP_CIP_SPECIAL = 9,
        PLCTAG_MODULE_AB_EIP_LGX_PCCC = 10,
        PLCTAG_MODULE_AB_EIP_PLC5_PCCC = 11,
        PLCTAG_MODULE_AB_EIP_PLC5_DHP = 12,
        PLCTAG_MODULE_AB_EIP_SLC_PCCC = 13,
        PLCTAG_MODULE_AB_EIP_SLC_DHP = 14,
        PLCTAG_MODULE_AB_ERROR = 15,
        PLCTAG_MODULE_OMRON_CONN = 16,
        PLCTAG_MODULE_OMRON_CIP = 17,
        PLCTAG_MODULE_OMRON_COMMON = 18,
        PLCTAG_MODULE_OMRON_STANDARD_TAG = 19,
        PLCTAG_MODULE_OMRON_RAW_TAG = 20,
        PLCTAG_MODULE_MODBUS = 21,
        PLCTAG_MODULE_SYSTEM = 22,
        PLCTAG_MODULE_PLATFORM = 23,
        PLCTAG_MODULE_AB_CONNECTION = 24,
        PLCTAG_MODULE_MB_CONNECTION = 25,
        PLCTAG_MODULE_OMRON_CONNECTION = 26,
    }