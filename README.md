<img src="https://raw.githubusercontent.com/libplctag/libplctag.NET/master/assets/libplctag-final.svg?sanitize=true" alt="libplctag" width="180"/>
<p>

# libplctag .NET

## Packages
This repository contains two .NET packages for [libplctag](https://github.com/libplctag/libplctag) that are published to Nuget.org:

| Package | Downloads | Stable | Preview |
|-|-|-|-|
| [libplctag](https://www.nuget.org/packages/libplctag/) | ![Nuget](https://img.shields.io/nuget/dt/libplctag) | ![Nuget version](https://img.shields.io/nuget/v/libplctag) | ![Nuget version](https://img.shields.io/nuget/vpre/libplctag) |
| [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) | ![Nuget](https://img.shields.io/nuget/dt/libplctag.NativeImport) | ![Nuget version](https://img.shields.io/nuget/v/libplctag.NativeImport) | ![Nuget version](https://img.shields.io/nuget/vpre/libplctag.NativeImport) |


## libplctag

This is the package intended for use in .NET applications. It depends on libplctag.NativeImport to gain access to the underlying libplctag native library.

It provides an API for libplctag that should feel natural to .NET developers by supporting the following features:
* Values are strongly-typed (both Atomic types and User-Defined Types).
* Errors are thrown as Exceptions
* Async/Await
* Native resource cleanup


## Getting Started

In most cases only the  libplctag package will be needed. It can be added in Visual Studio through the package manager or via the commandline:

`dotnet add package libplctag`

### Simple Example Code for an Allen-Bradley CompactLogix/ControlLogix PLC

```csharp
// Instantiate the tag with the appropriate mapper and datatype
var myTag = new TagDint()
{
    //Name is the full path to tag. 
    Name = "PROGRAM:SomeProgram.SomeDINT",
    //Gateway is the IP Address of the PLC or communication module.
    Gateway = "10.10.10.10", 
    //Path is the location in the control plane of the CPU. Almost always "1,0".
    Path = "1,0", 
    PlcType = PlcType.ControlLogix,
    Protocol = Protocol.ab_eip,
    Timeout = TimeSpan.FromSeconds(5)
};

// Read the value from the PLC
int output = myTag.Read();

// Output to Console
Console.WriteLine($"Original value: SomeProgram.SomeDINT = {output}");

// Write a new value to the PLC then read it back
myTag.Write(37);
output = myTag.Read();

// Output to Console
Console.WriteLine($"Updated value: SomeProgram.SomeDINT = {output}");
```
In advanced scenarios, tags can be instantiated using generics (ex. `Tag<DintPlcMapper, int>`, `Tag<BoolPlcMapper, bool>`) and can be referenced via an `ITag` interface.

For more detail and further usage, see the examples in the example projects:

* [C#](src/Examples/CSharp%20DotNetCore)
* [VB.NET](src/Examples/VB.NET%20DotNetCore/Program.vb)



## libplctag.NativeImport

libplctag.NativeImport provides low-level (raw) access to the [native libplctag library](https://github.com/libplctag/libplctag) (which is written in C).
The purpose of this package is to expose the [API for this native library](https://github.com/libplctag/libplctag/wiki/API), and handle platform and configuration issues.

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

Most developers will not need to directly reference the Native Import library.

Documentation for the base library API can be found [here](https://github.com/libplctag/libplctag/wiki/API). Further examples of its usage can be found [here](src/Examples/CSharp%20DotNetCore/NativeImportExample.cs).

The libplctag native library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper. If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

## Project Goals

* Package the libplctag functionality in a way that is convenient to use in .NET applications.
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0
