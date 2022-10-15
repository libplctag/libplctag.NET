// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.DataTypes
{
    public class SintPlcMapper : PlcMapperBase<sbyte>
    {

        override public int? ElementSize => 1;

        override public sbyte Decode(Tag tag, int offset) => tag.GetInt8(offset);

        override public void Encode(Tag tag, int offset, sbyte value) => tag.SetInt8(offset, value);

    }
}
