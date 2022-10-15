// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.DataTypes
{
    public class LrealPlcMapper : PlcMapperBase<double>
    {

        override public int? ElementSize => 8;

        override public double Decode(Tag tag, int offset) => tag.GetFloat64(offset);

        override public void Encode(Tag tag, int offset, double value)=> tag.SetFloat64(offset, value);
    }
}
