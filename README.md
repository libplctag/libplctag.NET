# libplctag .NET

This repository contains two .NET wrappers for [libplctag](https://github.com/libplctag/libplctag) that are published to Nuget.org
* [libplctag](https://www.nuget.org/packages/libplctag/)
* [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/)

## libplctag

This is the package intended for use by application developers.

### Usage

See the examples in the example project

* [C#](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/Example.cs)
* [VB.NET](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/VB.NET%20DotNetCore/Example.cs)


## libplctag.NativeImport

libplctag.NativeImport provides low-level (raw) access to the native libplctag library. This package does not provide an API which is friendly to app developers. Rather, it provides an API which handles platform and configuration issues, upon which a friendlier API can be built.

This wrapper embeds the native runtimes for each platform.
On startup of an application, it checks if the runtime is present on the filesystem, and if it isn't it extracts the appropriate one.

## Goals

* Package the libplctag functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0
* A layer that bridges the gap between libplctag's raw tag handles, and a strongly-typed .NET object.

## Notes

* This wrapper is still in alpha. Please let us know if you use this library and whether it works for you or not (including the platform you are using).
