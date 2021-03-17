using System;
using System.Runtime.InteropServices;
using System.Text;

namespace libplctag.NativeImport
{
    /// <summary>
    /// This class provides low-level (raw) access to the native libplctag library (which is written in C).
    /// The purpose of this package is to expose the API for this native library, and handle platform and configuration issues.
    /// 
    /// <para>See <see href="https://github.com/libplctag/libplctag/wiki/API"/> for documentation.</para>
    /// </summary>
    public static class plctag
    {



        static private bool _forceExtractLibrary = true;

        /// <summary>
        /// During initialization, this package extracts to disk the appropriate native library.
        /// By default, it will overwrite files with the same filename (plctag.dll, libplctag.so, or libplctag.dylib).
        /// If you wish to disable this behaviour and use a different native library (e.g. one that you've compiled yourself, or a pre-release), you can disable the Force Extract feature by setting this value to false.
        /// </summary>
        static public bool ForceExtractLibrary
        {
            get => _forceExtractLibrary;
            set
            {
                if (libraryAlreadyInitialized)
                    throw new InvalidOperationException("Library already initialized");
                _forceExtractLibrary = value;
            }
        }
        
        static private bool libraryAlreadyInitialized = false;
        static object _libraryExtractLocker = new object();
        private static void ExtractLibraryIfRequired()
        {
            // Non-blocking check
            // Except during startup, this will be hit 100% of the time
            if(!libraryAlreadyInitialized)
            {

                // Blocking check
                // This is hit if multiple threads simultaneously try to initialize the library
                lock (_libraryExtractLocker)
                {
                    if (!libraryAlreadyInitialized)
                    {
                        LibraryExtractor.Init(ForceExtractLibrary);
                        libraryAlreadyInitialized = true;
                    }
                }
            }
        }




        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void callback_func(Int32 tag_id, Int32 event_id, Int32 status);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate void log_callback_func(Int32 tag_id, int debug_level, [MarshalAs(UnmanagedType.LPStr)] string message);



        public static int plc_tag_check_lib_version(int req_major, int req_minor, int req_patch)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_check_lib_version(req_major, req_minor, req_patch);
        }

        public static Int32 plc_tag_create([MarshalAs(UnmanagedType.LPStr)] string lpString, int timeout)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_create(lpString, timeout);
        }

        public static int plc_tag_destroy(Int32 tag)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_destroy(tag);
        }

        public static int plc_tag_shutdown()
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_shutdown();
        }

        public static int plc_tag_register_callback(Int32 tag_id, callback_func func)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_register_callback(tag_id, func);
        }

        public static int plc_tag_unregister_callback(Int32 tag_id)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_unregister_callback(tag_id);
        }

        public static int plc_tag_register_logger(log_callback_func func)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_register_logger(func);
        }
        public static int plc_tag_unregister_logger(Int32 tag_id)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_unregister_logger(tag_id);
        }

        public static int plc_tag_lock(Int32 tag)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_lock(tag);
        }

        public static int plc_tag_unlock(Int32 tag)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_unlock(tag);
        }
        public static int plc_tag_status(Int32 tag)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_status(tag);
        }

        public static string plc_tag_decode_error(int err)
        {
            ExtractLibraryIfRequired();
            return Marshal.PtrToStringAnsi(NativeMethods.plc_tag_decode_error(err));
        }

        public static int plc_tag_read(Int32 tag, int timeout)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_read(tag, timeout);
        }

        public static int plc_tag_write(Int32 tag, int timeout)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_write(tag, timeout);
        }

        public static int plc_tag_get_size(Int32 tag)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_size(tag);
        }

        public static int plc_tag_abort(Int32 tag)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_abort(tag);
        }

        public static int plc_tag_get_int_attribute(Int32 tag, string attrib_name, int default_value)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_int_attribute(tag, attrib_name, default_value);
        }

        public static int plc_tag_set_int_attribute(Int32 tag, string attrib_name, int new_value)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_int_attribute(tag, attrib_name, new_value);
        }

        public static UInt64 plc_tag_get_uint64(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_uint64(tag, offset);
        }

        public static Int64 plc_tag_get_int64(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_int64(tag, offset);
        }

        public static int plc_tag_set_uint64(Int32 tag, int offset, UInt64 val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_uint64(tag, offset, val);
        }

        public static int plc_tag_set_int64(Int32 tag, int offset, Int64 val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_int64(tag, offset, val);
        }

        public static double plc_tag_get_float64(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_float64(tag, offset);
        }

        public static int plc_tag_set_float64(Int32 tag, int offset, double val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_float64(tag, offset, val);
        }
        public static UInt32 plc_tag_get_uint32(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_uint32(tag, offset);
        }

        public static Int32 plc_tag_get_int32(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_int32(tag, offset);
        }

        public static int plc_tag_set_uint32(Int32 tag, int offset, UInt32 val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_uint32(tag, offset, val);
        }

        public static int plc_tag_set_int32(Int32 tag, int offset, Int32 val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_int32(tag, offset, val);
        }

        public static float plc_tag_get_float32(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_float32(tag, offset);
        }

        public static int plc_tag_set_float32(Int32 tag, int offset, float val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_float32(tag, offset, val);
        }

        public static UInt16 plc_tag_get_uint16(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_uint16(tag, offset);
        }

        public static Int16 plc_tag_get_int16(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_int16(tag, offset);
        }

        public static int plc_tag_set_uint16(Int32 tag, int offset, UInt16 val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_uint16(tag, offset, val);
        }

        public static int plc_tag_set_int16(Int32 tag, int offset, Int16 val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_int16(tag, offset, val);
        }

        public static byte plc_tag_get_uint8(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_uint8(tag, offset);
        }

        public static sbyte plc_tag_get_int8(Int32 tag, int offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_int8(tag, offset);
        }

        public static int plc_tag_set_uint8(Int32 tag, int offset, byte val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_uint8(tag, offset, val);
        }

        public static int plc_tag_set_int8(Int32 tag, int offset, sbyte val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_int8(tag, offset, val);
        }

        public static int plc_tag_get_bit(Int32 tag, int offset_bit)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_bit(tag, offset_bit);
        }

        public static int plc_tag_set_bit(Int32 tag, int offset_bit, int val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_bit(tag, offset_bit, val);
        }
        public static void plc_tag_set_debug_level(int debug_level)
        {
            ExtractLibraryIfRequired();
            plc_tag_set_debug_level(debug_level);
        }



        public static int plc_tag_get_string(Int32 tag_id, int string_start_offset, StringBuilder buffer, int buffer_length)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_string(tag_id, string_start_offset, buffer, buffer_length);
        }

        public static int plc_tag_set_string(Int32 tag_id, int string_start_offset, string string_val)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_string(tag_id, string_start_offset, string_val);
        }

        public static int plc_tag_get_string_length(Int32 tag_id, int string_start_offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_string_length(tag_id, string_start_offset);
        }

        public static int plc_tag_get_string_capacity(Int32 tag_id, int string_start_offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_string_capacity(tag_id, string_start_offset);
        }

        public static int plc_tag_get_string_total_length(Int32 tag_id, int string_start_offset)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_string_total_length(tag_id, string_start_offset);
        }


        public static int plc_tag_get_raw_bytes(Int32 tag_id, int start_offset, byte[] buffer, int buffer_length)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_get_raw_bytes(tag_id, start_offset, buffer, buffer_length);
        }

        public static int plc_tag_set_raw_bytes(Int32 tag_id, int start_offset, byte[] buffer, int buffer_length)
        {
            ExtractLibraryIfRequired();
            return NativeMethods.plc_tag_set_raw_bytes(tag_id, start_offset, buffer, buffer_length);
        }


    }
}