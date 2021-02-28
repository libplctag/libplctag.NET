# libplctag .NET

This repository contains two .NET packages for [libplctag](https://github.com/libplctag/libplctag) that are published to Nuget.org:
* [libplctag](https://www.nuget.org/packages/libplctag/) ![Nuget version](https://img.shields.io/nuget/vpre/libplctag) ![Nuget](https://img.shields.io/nuget/dt/libplctag)
* [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) ![Nuget version](https://img.shields.io/nuget/vpre/libplctag.NativeImport) ![Nuget](https://img.shields.io/nuget/dt/libplctag.NativeImport)


### libplctag

This is the package intended for use by application developers. It attempts to provide an API that should feel natural to .NET developers by providing:
* Values are strongly-typed (both ATOMIC types and User-Defined-Types).
* Errors are thrown as Exceptions
* Async/Await
* Native memory handle cleanup via IDisposable and finalizers

```csharp
// Instantiate the tag with the appropriate mapper and datatype
var myTag = new Tag<DintPlcMapper, int>()
{
    Name = "PROGRAM:SomeProgram.SomeDINT",
    Gateway = "10.10.10.10",
    Path = "1,0",
    PlcType = PlcType.ControlLogix,
    Protocol = Protocol.ab_eip,
    Timeout = TimeSpan.FromSeconds(5)
};

// Read the value from the PLC
myTag.Read();

// Output to Console
Console.WriteLine(myTag.Value);
```

For further usage, see the examples in the example projects:

* [C#](https://github.com/libplctag/libplctag.NET/tree/master/src/Examples/CSharp%20DotNetCore)
* [VB.NET](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/VB.NET%20DotNetCore/Program.vb)


### libplctag.NativeImport

libplctag.NativeImport provides low-level (raw) access to the native libplctag library.
The purpose of this package is to expose the native library API (which is written in C), and handle platform and configuration issues.

During initialization, this package extracts to disk the appropriate native runtime. By default, it will overwrite any runtime that is already on disk. If you wish to disable this behaviour and use a different runtime (e.g. one that you've compiled yourself, or a pre-release), you can disable the Force Extract feature.

```csharp
// Before any calls to any libplctag methods
plctag.ForceExtractLibrary = false;
```

If the above example were to be implemented using this API, it would look like something this:

```csharp
var tagHandle = plctag.plc_tag_create("protocol=ab_eip&gateway=10.10.10.10&path=1,0&plc=LGX&elem_size=4&elem_count=1&name=PROGRAM:SomeProgram.SomeDINT", 5000);

var statusBeforeRead = (STATUS_CODES)plctag.plc_tag_status(tagHandle);
if (statusBeforeRead != STATUS_CODES.PLCTAG_STATUS_OK)
{
    throw new Exception($"Something went wrong: {statusBeforeRead}");
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

Documentation for this API can be found [here](https://github.com/libplctag/libplctag/wiki/API). Further examples of its usage can be found [here](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/NativeImportExample.cs).

The libplctag native library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper. If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

## Project Goals

* Package the libplctag functionality in a way that is convenient to use in .NET applications.
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0
