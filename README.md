# libplctag .NET

This is a .NET wrapper around [libplctag](https://github.com/libplctag/libplctag).
Download from [nuget.org](https://www.nuget.org/packages/libplctag/).
It was inspired by [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

This wrapper simply embeds all the runtimes for each possible platform.
On startup of an application, it checks if the runtime is present on the filesystem, and if it isn't it extracts the appropriate one.

## Goals

* Package the libplctag functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0
* A minimal layer that bridges the gap between libplctag's raw tag handles, and a strongly-typed .NET object.

## Notes

* This wrapper is still in alpha. Please let us know if you use this library and whether it works for you or not (including the platform you are using).

## Example Usage

See the examples in the example project

* [C#](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/CSharp%20DotNetCore/Example.cs)
* [VB.NET](https://github.com/libplctag/libplctag.NET/blob/master/src/Examples/VB.NET%20DotNetCore/Example.cs)