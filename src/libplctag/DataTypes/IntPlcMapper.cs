// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.DataTypes
{
    public class IntPlcMapper : PlcMapperBase<short>
    {
        public override int? ElementSize => 2;

        override public short Decode(Tag tag, int offset) => tag.GetInt16(offset);

        override public void Encode(Tag tag, int offset, short value) => tag.SetInt16(offset, value);

    }
}
