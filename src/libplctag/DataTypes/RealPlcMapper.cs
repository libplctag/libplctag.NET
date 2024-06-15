// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace libplctag.DataTypes
{
    public class RealPlcMapper : PlcMapperBase<float>
    {

        override public int? ElementSize => 4;

        override public float Decode(Tag tag, int offset) => tag.GetFloat32(offset);

        override public void Encode(Tag tag, int offset, float value) => tag.SetFloat32(offset, value);

    }
}
