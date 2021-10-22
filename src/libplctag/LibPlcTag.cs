using libplctag.NativeImport;
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
        static public DebugLevel DebugLevel
        {
            get => (DebugLevel)_native.plc_tag_get_int_attribute(LIB_ATTRIBUTE_POINTER, "debug", int.MinValue);
            set => _native.plc_tag_set_debug_level((int)value);
        }


        static readonly object logEventSubscriptionLock = new object();
        static private event EventHandler<LogEventArgs> logEvent;
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
        }

        static void invokeLogEvent(int tag_id, int debug_level, string message)
        {
            logEvent?.Invoke(null, new LogEventArgs() { DebugLevel = (DebugLevel)debug_level, Message = message.TrimEnd('\n') });
        }

        /// <summary>
        /// You can redirect all logging from the library to your own delegate.
        /// </summary>
        /// <remarks>
        /// WARNING There are some important restrictions on logging delegates:
        /// The delegate will be called from multiple threads, sometimes simultaneously! Your code must be thread aware and thread-safe.
        /// The delegate will be called with one or more mutexes held in almost all cases.You must not call any tag API functions other than plc_tag_decode_error(). If you do there is a large chance that the library will hang.
        /// Logging messages come from deep within the library's core routines. Many of these are very delay sensitive. Do not do anything that would block or delay the return of the logging callback to the library!
        /// The message string passed to your callback function will be managed by the library. Do not attempt to free its memory or modify the string. If you need to do modifications, make your own copy and return.
        /// </remarks>
        static public event EventHandler<LogEventArgs> LogEvent
        {
            add
            {
                lock (logEventSubscriptionLock)
                {
                    ensureSubscribeToEvents();
                    logEvent += value;
                }
            }
            remove
            {
                lock (logEventSubscriptionLock)
                {
                    logEvent -= value;
                }
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
