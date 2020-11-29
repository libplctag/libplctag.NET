# libplctag .NET

This repository contains two .NET packages for [libplctag](https://github.com/libplctag/libplctag) that are published to Nuget.org:
* [libplctag](https://www.nuget.org/packages/libplctag/) ![Nuget version](https://img.shields.io/nuget/vpre/libplctag) ![Nuget](https://img.shields.io/nuget/dt/libplctag)
* [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) ![Nuget version](https://img.shields.io/nuget/vpre/libplctag.NativeImport) ![Nuget](https://img.shields.io/nuget/dt/libplctag.NativeImport)

## Goals

* Package the libplctag functionality in a way that is convenient to use in .NET applications (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0

## libplctag

This is the package intended for use by application developers. It provides strongly-typed and convenient access to Tags.

For usage, see the examples in the example projects:

* [C#](https://github.com/libplctag/libplctag.NET/tree/master/src/Examples/CSharp%20DotNetCore)
* [VB.NET](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/VB.NET%20DotNetCore/Program.vb)


## libplctag.NativeImport

libplctag.NativeImport provides low-level (raw) access to the native libplctag library.
The purpose of this package is to expose the native library API (which is written in C), and handle platform and configuration issues.

Documentation for the native API can be found [here](https://github.com/libplctag/libplctag/wiki/API). An example of its usage can be found [here](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/NativeImportExample.cs).

During initialization, this package extracts to disk the appropriate native runtime. By default, it will overwrite any runtime that is already on disk. If you wish to disable this behaviour and use a different runtime (e.g. one that you've compiled yourself, or a pre-release), you can disable the Force Extract feature.

```csharp
// Before any calls to any libplctag methods
plctag.ForceExtractLibrary = false;
```

The libplctag native runtime can be compiled for [many platforms](https://github.com/libplctag/libplctag#platform-support), and not all supported platforms are shipped with this wrapper. If you get a `TypeLoadException`, chances are that you can still use this wrapper but you will need to [supply the runtime yourself](https://github.com/libplctag/libplctag/blob/master/BUILD.md).

