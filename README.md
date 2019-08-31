# libplctag

This repository is a wrapper around [libplctag](https://github.com/timyhac/libplctagWrapper).

It does nothing more than expose the libplctag API in a way idiomatic to .NET

It was forked from on [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Make the libplctag API available in a way that is idomatic to .NET (Exceptions, IDisposable, Strong-Typing, Object-Oriented, etc)
* Package this functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard
