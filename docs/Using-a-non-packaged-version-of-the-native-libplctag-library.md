
## Default Behavior
libplctag.NativeImport provides low-level (raw) access to the [native libplctag library](https://github.com/libplctag/libplctag) (which is written in C).
The purpose of this package is to expose the [API for this native library](https://github.com/libplctag/libplctag/wiki/API), and handle platform and configuration issues.

During initialization, this package extracts to disk the appropriate native library.
By default, it will overwrite files with the same filename (plctag.dll, libplctag.so, or libplctag.dylib).

## Using a self-supplied version of the base libplctag library
If you wish to disable this behaviour and use a different native library (e.g. one that you've compiled yourself, or a pre-release), you can disable the Force Extract feature.

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