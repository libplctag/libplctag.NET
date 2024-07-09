# libplctag.NativeImport

NOTE: Most application developers will not need to directly reference the Native Import library, it is primarily for use by other wrapper libraries.

This library automatically extracts platform-specific version of the libplctag core library needed for the libplctag .NET wrapper. 

If you wish to override this behavior you can do so: [Using a non packaged version of the native libplctag library](docs/libplctag.NativeImport.md#Using-a-non-packaged-version-of-the-libplctag-core-library.md)

Documentation for the core library can be found [here](https://github.com/libplctag/libplctag/wiki/API).
Further examples of its usage can be found [here](src/Examples/CSharp%20DotNetCore/NativeImportExample.cs).

The libplctag core library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper. If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

## Using a non-packaged version of libplctag core library

libplctag.NativeImport provides low-level (raw) access to the [native libplctag library](https://github.com/libplctag/libplctag) (which is written in C).
The purpose of this package is to expose the [API for this native library](https://github.com/libplctag/libplctag/wiki/API), and handle platform and configuration issues.

During initialization, this package extracts to disk the appropriate native library.
By default, it will overwrite files with the same filename (plctag.dll, libplctag.so, or libplctag.dylib).

## Using a copy of the base libplctag library you supply yourself

If you wish to disable this behaviour and use a different core library (e.g. one that you've compiled yourself, or a pre-release), you can disable the Force Extract feature.

```csharp
// Before any calls to any libplctag methods
plctag.ForceExtractLibrary = false;
```

If the above example were to be implemented using this API, it would look like something this:

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


## Developing for systems with immutable application directories
