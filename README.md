# libplctag .NET

## Packages
This repository contains two .NET packages for [libplctag](https://github.com/libplctag/libplctag) that are published to Nuget.org:

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

### Example

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

This package depends on libplctag.NativeImport to gain access to the underlying libplctag native library.

For further usage, see the examples in the example projects:

* [C#](https://github.com/libplctag/libplctag.NET/tree/master/src/Examples/CSharp%20DotNetCore)
* [VB.NET](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/VB.NET%20DotNetCore/Program.vb)


## libplctag.NativeImport

The libplctag.NativeImport library automatically extracts platform-specific version of the base libplctag library needed for the .NET wrapper. If you wish to override this behavior you can do so: [Using a non packaged version of the native libplctag library](https://github.com/libplctag/libplctag.NET/wiki/Using-a-non-packaged-version-of-the-native-libplctag-library)


Documentation for the base library API can be found [here](https://github.com/libplctag/libplctag/wiki/API). Further examples of its usage can be found [here](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/NativeImportExample.cs).

The libplctag native library can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper. If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).


## Getting Started

Both packages are available from nuget.org and can be added using your preferred installation tool.

`dotnet add package libplctag` or `dotnet add package libplctag.NativeImport`


## Developing for systems with immutable application directories

UWP, Xamarin.Forms and some other frameworks produce executables that, when installed, can not modify their own application directory. [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) relies on the ability extract the native library to this location, so on these platforms, libplctag will not work.

The workaround is to supply the appropriate binary(ies) yourself:
1. Get the native library (i.e. plctag.dll) from [Releases](https://github.com/libplctag/libplctag/releases).
2. Add this file to your project such that it is copied to the output directory.
3. Set `plctag.ForceExtractLibrary = false` before any other calls to libplctag.

Watch out for x64/x86 mismatches between the native library you downloaded and your solution.

This bug is tracked in https://github.com/libplctag/libplctag.NET/issues/137


## Project Goals

* Package the libplctag functionality in a way that is convenient to use in .NET applications.
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0
