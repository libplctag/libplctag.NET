<img src="https://raw.githubusercontent.com/libplctag/libplctag.NET/master/assets/libplctag-final.svg?sanitize=true" alt="libplctag" width="180"/>
<p>

# libplctag.NET

[libplctag](https://github.com/libplctag/libplctag) is an open source C library for Linux, Windows and macOS using EtherNet/IP or Modbus TCP to read and write tags in PLCs.

libplctag.NET provides a collection of .NET wrapper packages for libplctag.

## Packages

This repository contains two .NET packages that are published to Nuget.org:

| Package | Downloads | Stable | Preview |
|-|-|-|-|
| [libplctag](https://www.nuget.org/packages/libplctag/) | ![Nuget](https://img.shields.io/nuget/dt/libplctag) | ![Nuget version](https://img.shields.io/nuget/v/libplctag) | ![Nuget version](https://img.shields.io/nuget/vpre/libplctag) |
| [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) | ![Nuget](https://img.shields.io/nuget/dt/libplctag.NativeImport) | ![Nuget version](https://img.shields.io/nuget/v/libplctag.NativeImport) | ![Nuget version](https://img.shields.io/nuget/vpre/libplctag.NativeImport) |


## libplctag

This is the package intended for use in .NET applications.
It provides an API for libplctag that should feel natural to .NET developers by supporting the following features:
* Values are strongly-typed (both Atomic types and User-Defined Types).
* Errors are thrown as Exceptions
* Async/Await
* Native resource cleanup

This package depends on libplctag.NativeImport to gain access to the underlying libplctag native library.

## libplctag.NativeImport

See [here](docs/libplctag.NativeImport.md) for information on this package.

## Getting Started

In most cases only the  libplctag package will be needed. It can be added in Visual Studio through the package manager or via the commandline:

`> dotnet add package libplctag`

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

## Project Goals

* Package the libplctag functionality in a way that is convenient to use in .NET applications.
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0

## Getting Help

Please review the [Contributions guidance](CONTRIBUTING.md).

* [libplctag.NET docs](docs)
* [libplctag Wiki](https://github.com/libplctag/libplctag/wiki)

libplctag.NET is part of the libplctag organization, so the [same policies apply](https://github.com/libplctag/libplctag#contact-and-support).
