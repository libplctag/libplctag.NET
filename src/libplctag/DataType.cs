using System;

namespace libplctag
{

    public static class DataType
    {
        /* 1-byte / 8-bit types */
        public const int Int8 = 1;
        public const int SINT = 1;

        /* 2-byte / 16-bit types */
        public const int Int16 = 2;
        public const int INT = 2;

        /* 4-byte / 32-bit types */
        public const int Int32 = 4;
        public const int DINT = 4;
        public const int Float32 = 4;
        public const int REAL = 4;

        /* 8-byte / 64-bit types */
        public const int Int64 = 8;
        public const int LINT = 8;
        public const int Float64 = 8;
        // Not sure what AB calls 64-bit floats

        public const int String = 88;

        public static int Parse(string type)
        {
            switch (type)
            {
                case nameof(Int8):  return Int8;
                case nameof(SINT):  return SINT;

                case nameof(Int16): return Int16;
                case nameof(INT):   return INT;

                case nameof(Int32): return Int32;
                case nameof(DINT): return DINT;
                case nameof(Float32): return Float32;
                case nameof(REAL): return REAL;

                case nameof(Int64): return Int64;
                case nameof(LINT): return LINT;
                case nameof(Float64): return Float64;

                case nameof(String): return String;

                default: break;
            }

            throw new ArgumentException(nameof(type));
        }
    }
}