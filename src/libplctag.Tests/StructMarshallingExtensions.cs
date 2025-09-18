using System;
using System.Runtime.InteropServices;

namespace libplctag.Tests
{
    public static class StructMarshallingExtensions
    {
        /// <summary>
        /// Converts a blittable struct to a byte array.
        /// </summary>
        public static byte[] ToByteArray<T>(this T value) where T : struct
        {
            byte[] bytes = new byte[Marshal.SizeOf<T>()];
            MemoryMarshal.Write(bytes, ref value); // zero-allocation, fast
            return bytes;
        }

        /// <summary>
        /// Reads a blittable struct from a byte array.
        /// </summary>
        public static T ToStruct<T>(this byte[] bytes) where T : struct
        {
            if (bytes.Length < Marshal.SizeOf<T>())
                throw new ArgumentException($"Byte array too small for struct {typeof(T)}");

            return MemoryMarshal.Read<T>(bytes);
        }
    }
}