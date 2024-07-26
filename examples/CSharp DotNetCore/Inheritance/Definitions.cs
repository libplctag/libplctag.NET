// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace CSharpDotNetCore.Inheritance
{

    class TagDint : TagSingle<int>
    {
        public TagDint()
            : base(elementSize:4)
        {
        }

        protected override int Decode(int offset) => tag.GetInt32(offset);
        protected override void Encode(int offset, int value) => tag.SetInt32(offset, value);
    }

    class TagDint1D : Tag1D<int>
    {
        public TagDint1D(int length)
            :base(length, elementSize:4)
        {
        }

        protected override int Decode(int offset) => tag.GetInt32(offset);
        protected override void Encode(int offset, int value) => tag.SetInt32(offset, value);
    }

    class TagDint2D : Tag2D<int>
    {
        public TagDint2D(int dim1, int dim2)
            : base(dim1, dim2, elementSize:4)
        {
        }

        protected override int Decode(int offset) => tag.GetInt32(offset);
        protected override void Encode(int offset, int value) => tag.SetInt32(offset, value);
    }

    class TagDint3D : Tag3D<int>
    {
        public TagDint3D(int dim1, int dim2, int dim3)
            : base(dim1, dim2, dim3, elementSize:4)
        {
        }

        protected override int Decode(int offset) => tag.GetInt32(offset);
        protected override void Encode(int offset, int value) => tag.SetInt32(offset, value);
    }

}
