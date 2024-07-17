// Copyright (c) libplctag.NET contributors
// https://github.com/libplctag/libplctag.NET
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using libplctag;
using System.Linq;
using System;
using System.Collections.Generic;

namespace CSharpDotNetCore.PlcMapper
{

    public abstract class PlcMapperBase<T> : IPlcMapper<T>, IPlcMapper<T[]>, IPlcMapper<T[,]>, IPlcMapper<T[,,]>
    {
        public PlcType PlcType { get; set; }

        abstract public int? ElementSize { get; }

        public int[] ArrayDimensions { get; set; }

        //Multiply all the dimensions to get total elements
        virtual public int? GetElementCount() => ArrayDimensions?.Aggregate(1, (x, y) => x * y);

        virtual protected T[] DecodeArray(Tag tag)
        {
            if (ElementSize is null)
                throw new ArgumentNullException($"{nameof(ElementSize)} cannot be null for array decoding");


            var buffer = new List<T>();

            var tagSize = tag.GetSize();

            int offset = 0;
            while (offset < tagSize)
            {
                buffer.Add(Decode(tag, offset));
                offset += ElementSize.Value;
            }

            return buffer.ToArray();

        }

        virtual protected void EncodeArray(Tag tag, T[] values)
        {
            if (ElementSize is null)
            {
                throw new ArgumentNullException($"{nameof(ElementSize)} cannot be null for array encoding");
            }

            int offset = 0;
            foreach (var item in values)
            {
                Encode(tag, offset, item);
                offset += ElementSize.Value;
            }
        }

        virtual public T Decode(Tag tag) => Decode(tag, 0);
        public abstract T Decode(Tag tag, int offset);


        virtual public void Encode(Tag tag, T value) => Encode(tag, 0, value);
        public abstract void Encode(Tag tag, int offset, T value);

        virtual public void Encode(Tag tag, T[] value) => EncodeArray(tag, value);

        T[] IPlcMapper<T[]>.Decode(Tag tag) => DecodeArray(tag);


        T[,] IPlcMapper<T[,]>.Decode(Tag tag) => DecodeArray(tag).To2DArray<T>(ArrayDimensions[0], ArrayDimensions[1]);

        void IPlcMapper<T[,]>.Encode(Tag tag, T[,] value) => EncodeArray(tag, value.To1DArray());

        T[,,] IPlcMapper<T[,,]>.Decode(Tag tag) => DecodeArray(tag).To3DArray<T>(ArrayDimensions[0], ArrayDimensions[1], ArrayDimensions[2]);

        void IPlcMapper<T[,,]>.Encode(Tag tag, T[,,] value) => EncodeArray(tag, value.To1DArray());
    }

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

    public class DintPlcMapper : PlcMapperBase<int>
    {
        public override int? ElementSize => 4;

        override public int Decode(Tag tag, int offset) => tag.GetInt32(offset);

        override public void Encode(Tag tag, int offset, int value) => tag.SetInt32(offset, value);

    }

    public class IntPlcMapper : PlcMapperBase<short>
    {
        public override int? ElementSize => 2;

        override public short Decode(Tag tag, int offset) => tag.GetInt16(offset);

        override public void Encode(Tag tag, int offset, short value) => tag.SetInt16(offset, value);

    }

    public class LintPlcMapper : PlcMapperBase<long>
    {
        public override int? ElementSize => 8;

        override public long Decode(Tag tag, int offset) => tag.GetInt64(offset);

        override public void Encode(Tag tag, int offset, long value) => tag.SetInt64(offset, value);

    }

    public class LrealPlcMapper : PlcMapperBase<double>
    {

        override public int? ElementSize => 8;

        override public double Decode(Tag tag, int offset) => tag.GetFloat64(offset);

        override public void Encode(Tag tag, int offset, double value) => tag.SetFloat64(offset, value);
    }

    public class RealPlcMapper : PlcMapperBase<float>
    {

        override public int? ElementSize => 4;

        override public float Decode(Tag tag, int offset) => tag.GetFloat32(offset);

        override public void Encode(Tag tag, int offset, float value) => tag.SetFloat32(offset, value);

    }

    public class SintPlcMapper : PlcMapperBase<sbyte>
    {

        override public int? ElementSize => 1;

        override public sbyte Decode(Tag tag, int offset) => tag.GetInt8(offset);

        override public void Encode(Tag tag, int offset, sbyte value) => tag.SetInt8(offset, value);

    }

    public class StringPlcMapper : PlcMapperBase<string>
    {

        override public int? ElementSize
        {
            get
            {
                switch (PlcType)
                {
                    case PlcType.ControlLogix: return 88;
                    case PlcType.Plc5: return 84;
                    case PlcType.Slc500: return 84;
                    case PlcType.LogixPccc: return 84;
                    case PlcType.Micro800: return 256; //To be Confirmed
                    case PlcType.MicroLogix: return 84;
                    default: throw new NotImplementedException();
                }
            }
        }


        override public string Decode(Tag tag, int offset) => tag.GetString(offset);
        override public void Encode(Tag tag, int offset, string value) => tag.SetString(offset, value);

    }

    public static class ArrayExtensions
    {
        /// <summary>
        /// Extension method to flatten a 2D array to a 1D array
        /// </summary>
        /// <typeparam name="T">Array Type</typeparam>
        /// <param name="input">2D array to be flattened</param>
        /// <returns>1D array</returns>
        public static T[] To1DArray<T>(this T[,] input)
        {
            // Step 1: get total size of 2D array, and allocate 1D array.
            int size = input.Length;
            T[] result = new T[size];

            // Step 2: copy 2D array elements into a 1D array.
            int write = 0;
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                for (int z = 0; z <= input.GetUpperBound(1); z++)
                {
                    result[write++] = input[i, z];
                }
            }
            // Step 3: return the new array.
            return result;
        }

        /// <summary>
        /// Extension method to flatten a 3D array to a 1D array
        /// </summary>
        /// <typeparam name="T">Array Type</typeparam>
        /// <param name="input">3D array to be flattened</param>
        /// <returns>1D array</returns>
        public static T[] To1DArray<T>(this T[,,] input)
        {
            // Step 1: get total size of 3D array, and allocate 1D array.
            int size = input.Length;
            T[] result = new T[size];

            // Step 2: copy 3D array elements into a 1D array.
            int write = 0;
            for (int i = 0; i <= input.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= input.GetUpperBound(1); j++)
                {
                    for (int k = 0; k < input.GetUpperBound(2); k++)
                    {
                        result[write++] = input[i, j, k];
                    }
                }
            }
            // Step 3: return the new array.
            return result;
        }

        /// <summary>
        /// Extension method to reshape a 1D array into a 2D array
        /// </summary>
        /// <typeparam name="T">Array Type</typeparam>
        /// <param name="input">1D array to be reshaped</param>
        /// <param name="height">Desired height (first index) of 2D array</param>
        /// <param name="width">Desired width (second index) of 2D array</param>
        /// <returns>2D array</returns>
        public static T[,] To2DArray<T>(this T[] input, int height, int width)
        {
            T[,] output = new T[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output[i, j] = input[i * width + j];
                }
            }
            return output;
        }

        /// <summary>
        /// Extension method to reshape a 1D array into a 3D array
        /// </summary>
        /// <typeparam name="T">Array Type</typeparam>
        /// <param name="input">1D array to be reshaped</param>
        /// <param name="height">Desired height (first index) of 3D array</param>
        /// <param name="width">Desired width (second index) of 3D array</param>
        /// <param name="length">Desired length (third index) of 3D array</param>
        /// <returns>#D array</returns>
        public static T[,,] To3DArray<T>(this T[] input, int height, int width, int length)
        {
            T[,,] output = new T[height, width, length];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        output[i, j, k] = input[i * height * width + j * width + k];
                    }
                }
            }
            return output;
        }

    }

    public class TagBool : Tag<BoolPlcMapper, bool> { }
    public class TagBool1D : Tag<BoolPlcMapper, bool[]> { }
    public class TagBool2D : Tag<BoolPlcMapper, bool[,]> { }
    public class TagBool3D : Tag<BoolPlcMapper, bool[,,]> { }


    public class TagDint : Tag<DintPlcMapper, int> { }
    public class TagDint1D : Tag<DintPlcMapper, int[]> { }
    public class TagDint2D : Tag<DintPlcMapper, int[,]> { }
    public class TagDint3D : Tag<DintPlcMapper, int[,,]> { }


    public class TagInt : Tag<IntPlcMapper, short> { }
    public class TagInt1D : Tag<IntPlcMapper, short[]> { }
    public class TagInt2D : Tag<IntPlcMapper, short[,]> { }
    public class TagInt3D : Tag<IntPlcMapper, short[,,]> { }


    public class TagLint : Tag<LintPlcMapper, long> { }
    public class TagLint1D : Tag<LintPlcMapper, long[]> { }
    public class TagLint2D : Tag<LintPlcMapper, long[,]> { }
    public class TagLint3D : Tag<LintPlcMapper, long[,,]> { }


    public class TagLreal : Tag<LrealPlcMapper, double> { }
    public class TagLreal1D : Tag<LrealPlcMapper, double[]> { }
    public class TagLreal2D : Tag<LrealPlcMapper, double[,]> { }
    public class TagLreal3D : Tag<LrealPlcMapper, double[,,]> { }


    public class TagReal : Tag<RealPlcMapper, float> { }
    public class TagReal1D : Tag<RealPlcMapper, float[]> { }
    public class TagReal2D : Tag<RealPlcMapper, float[,]> { }
    public class TagReal3D : Tag<RealPlcMapper, float[,,]> { }


    public class TagSint : Tag<SintPlcMapper, sbyte> { }
    public class TagSint1D : Tag<SintPlcMapper, sbyte[]> { }
    public class TagSint2D : Tag<SintPlcMapper, sbyte[,]> { }
    public class TagSint3D : Tag<SintPlcMapper, sbyte[,,]> { }


    public class TagString : Tag<StringPlcMapper, string> { }
    public class TagString1D : Tag<StringPlcMapper, string[]> { }
    public class TagString2D : Tag<StringPlcMapper, string[,]> { }
    public class TagString3D : Tag<StringPlcMapper, string[,,]> { }


    public class TagTagInfo : Tag<TagInfoPlcMapper, TagInfo[]> { }

    public class TagTimer : Tag<TimerPlcMapper, AbTimer> { }

    public class TagUdtInfo : Tag<UdtInfoPlcMapper, UdtInfo> { }
}
