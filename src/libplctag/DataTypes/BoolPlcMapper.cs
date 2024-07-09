﻿// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag.DataTypes.Extensions;
using System;
using System.Linq;

namespace libplctag.DataTypes
{

    public class BoolPlcMapper : IPlcMapper<bool>, IPlcMapper<bool[]>, IPlcMapper<bool[,]>, IPlcMapper<bool[,,]>
    {
        public int? ElementSize => 1;

        public PlcType PlcType { get; set; }
        public int[] ArrayDimensions { get; set; }

        public int? GetElementCount()
        {
            if (ArrayDimensions == null)
                return null;

            //TODO: Test -> I'm not confident that the overall bool count is packed as a 1D array and not packed by dimension.
            //Multiply dimensions for total elements
            var totalElements = ArrayDimensions.Aggregate(1, (x, y) => x * y);
            return (int)Math.Ceiling((double)totalElements / 32.0);
        }

        public int? SetArrayLength(int? elementCount) => (int)Math.Ceiling((double)elementCount.Value / 32.0);

        virtual protected bool[] DecodeArray(Tag tag)
        {
            if (ElementSize is null)
                throw new ArgumentNullException($"{nameof(ElementSize)} cannot be null for array decoding");

            var buffer = new bool[tag.ElementCount.Value * 32];
            for (int ii = 0; ii < tag.ElementCount.Value * 32; ii++)
            {
                buffer[ii] = tag.GetBit(ii);
            }
            return buffer;
        }

        virtual protected void EncodeArray(Tag tag, bool[] values)
        {
            for (int ii = 0; ii < tag.ElementCount.Value * 32; ii++)
            {
                tag.SetBit(ii, values[ii]);
            }
        }

        bool IPlcMapper<bool>.Decode(Tag tag) => tag.GetUInt8(0) != 0;

        void IPlcMapper<bool>.Encode(Tag tag, bool value) => tag.SetUInt8(0, value == true ? (byte)255 : (byte)0);

        bool[] IPlcMapper<bool[]>.Decode(Tag tag) => DecodeArray(tag);

        void IPlcMapper<bool[]>.Encode(Tag tag, bool[] value) => EncodeArray(tag, value);

        bool[,] IPlcMapper<bool[,]>.Decode(Tag tag) => DecodeArray(tag).To2DArray(ArrayDimensions[0], ArrayDimensions[1]);

        void IPlcMapper<bool[,]>.Encode(Tag tag, bool[,] value) => EncodeArray(tag, value.To1DArray());

        bool[,,] IPlcMapper<bool[,,]>.Decode(Tag tag) => DecodeArray(tag).To3DArray(ArrayDimensions[0], ArrayDimensions[1], ArrayDimensions[2]);

        void IPlcMapper<bool[,,]>.Encode(Tag tag, bool[,,] value) => EncodeArray(tag, value.To1DArray());

    }
}
