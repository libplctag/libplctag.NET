# Developing for systems with immutable application directories

UWP, Xamarin.Forms and some other frameworks produce executables that, when installed, can not modify their own application directory. [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) relies on the ability to extract the native library to this location, so on these platforms, libplctag will not work.

The workaround is to supply the appropriate binary(ies) yourself:
1. Get the native library (i.e. plctag.dll) from [Releases](https://github.com/libplctag/libplctag/releases).
2. Add this file to your project such that it is copied to the output directory.
3. Set `plctag.ForceExtractLibrary = false` before any other calls to libplctag.

Watch out for x64/x86 mismatches between the native library you downloaded and your solution.

This bug is tracked in https://github.com/libplctag/libplctag.NET/issues/137
