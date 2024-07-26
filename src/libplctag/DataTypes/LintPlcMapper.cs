// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;

namespace libplctag.DataTypes
{
    [Obsolete("see - https://github.com/libplctag/libplctag.NET/issues/406")]
    public class LintPlcMapper : PlcMapperBase<long>
    {
        public override int? ElementSize => 8;

        override public long Decode(Tag tag, int offset) => tag.GetInt64(offset);

        override public void Encode(Tag tag, int offset, long value) => tag.SetInt64(offset, value);

    }
}
