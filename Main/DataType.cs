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
    }
}