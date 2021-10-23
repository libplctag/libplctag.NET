using libplctag.NativeImport;
using Microsoft.Extensions.Logging;
using System;

namespace libplctag
{
    /// <summary>
    /// A static class used to access some additional features of the libplctag base library
    /// </summary>
    public static class LibPlcTag
    {

        static INativeTag _native = new NativeTag();

        private const int LIB_ATTRIBUTE_POINTER = 0;

        static public int VersionMajor => _native.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "version_major", int.MinValue);
        static public int VersionMinor => _native.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "version_minor", int.MinValue);
        static public int VersionPatch => _native.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "version_patch", int.MinValue);

        /// <summary>
        /// Check if base library meets version requirements
        /// </summary>
        /// <param name="requiredMajor">Major</param>
        /// <param name="requiredMinor">Minor</param>
        /// <param name="requiredPatch">Patch</param>
        /// <returns></returns>
        static public bool IsRequiredVersion(int requiredMajor, int requiredMinor, int requiredPatch)
        {
            var result = (Status)_native.plc_tag_check_lib_version(requiredMajor, requiredMinor, requiredPatch);

            if (result == Status.Ok)
                return true;
            else if (result == Status.ErrorUnsupported)
                return false;
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Sets a debug level for the underlying libplctag library
        /// </summary>
        static public LogLevel LogLevel
        {
            get => (LogLevel)_native.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "debug", int.MinValue);
            set => _native.plc_tag_set_debug_level((int)value);
        }


        static bool alreadySubscribedToEvents = false;
        static plctag.log_callback_func loggerDelegate;

        static private void ensureSubscribeToEvents()
        {
            if (alreadySubscribedToEvents)
                return;

            loggerDelegate = new plctag.log_callback_func(invokeLogEvent);
            var statusAfterRegistration = (Status)_native.plc_tag_register_logger(loggerDelegate);
            if (statusAfterRegistration != Status.Ok)
                throw new LibPlcTagException(statusAfterRegistration);

            alreadySubscribedToEvents = true;
        }

        static void invokeLogEvent(int tag_id, int debug_level, string message)
        {
            logger?.Log((LogLevel)debug_level, message.TrimEnd('\n'));
        }

        private static ILogger logger;
        static public ILogger Logger
        {
            get => logger;
            set
            {
                ensureSubscribeToEvents();
                logger = value;
            }
        }

        /// <summary>
        /// After this function returns, the library will have cleaned up all internal threads and resources.
        /// You can immediately turn around and call plc_tag_create() again and the library will start up again.
        /// Note: you must dispose of all Tags before calling <see cref="Shutdown"/>
        /// </summary>
        /// <remarks>
        /// Some wrappers and systems are not able to trigger the standard POSIX or Windows functions when the library is 
        /// being unloaded or the program is shutting down. In those cases, you can call this function.
        /// </remarks>
        static public void Shutdown()
        {
            _native.plc_tag_shutdown();
        }

    }
}
