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

The libplctag core library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper.
If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the core libplctag library yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

