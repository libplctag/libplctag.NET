# libplctag.NativeImport

libplctag.NativeImport provides low-level (raw) access to the [libplctag core library](https://github.com/libplctag/libplctag) which is written in C.
The purpose of this package is to expose the [API for this library](https://github.com/libplctag/libplctag/wiki/API) to .NET applications, and handle platform and configuration issues.

NOTE: Most application developers will not need to directly reference the Native Import library, it is primarily for use by other wrapper libraries.

## Example

```csharp
var tagHandle = plctag.plc_tag_create("protocol=ab_eip&gateway=10.10.10.10&path=1,0&plc=LGX&elem_size=4&elem_count=1&name=PROGRAM:SomeProgram.SomeDINT", 5000);

var statusAfterCreate = (STATUS_CODES)plctag.plc_tag_status(tagHandle);
if (statusAfterCreate != STATUS_CODES.PLCTAG_STATUS_OK)
{
    throw new Exception($"Something went wrong: {statusAfterCreate}");
}

var statusAfterRead = (STATUS_CODES)plctag.plc_tag_read(tagHandle, 5000);
if (statusAfterRead != STATUS_CODES.PLCTAG_STATUS_OK)
{
    throw new Exception($"Something went wrong: {statusAfterRead}");
}

var theValue = plctag.plc_tag_get_uint32(tagHandle, 0);

Console.WriteLine(theValue);

plctag.plc_tag_destroy(tagHandle);
```

Further examples can be found [here](../examples/CSharp%20NativeImport/).

## How it works

During initialization, this package extracts to disk the appropriate (platform-specific) core library.
By default, it will overwrite files with the same filename (`plctag.dll`, `libplctag.so`, or `libplctag.dylib`).

If you wish to disable this behaviour and use a different core library (e.g. one that you've compiled yourself, or a pre-release), you can disable the Force Extract feature.

```csharp
// Before any calls to any libplctag methods
plctag.ForceExtractLibrary = false;
```

The libplctag core library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper.
If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

## Developing for systems with immutable application directories

UWP, Xamarin.Forms and some other frameworks produce executables that, when installed, can not modify their own application directory.
[libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) relies on the ability to extract the native library to this location, so on these platforms, libplctag will not work.

The workaround is to supply the appropriate binary(ies) yourself:
1. Get the core library (e.g. plctag.dll) from [Releases](https://github.com/libplctag/libplctag/releases).
2. Add this file to your project such that it is copied to the output directory.
3. Set `plctag.ForceExtractLibrary = false` before any other calls to libplctag.

Watch out for x64/x86 mismatches between the native library you downloaded and your solution.

This bug is tracked in https://github.com/libplctag/libplctag.NET/issues/137
