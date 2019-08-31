# libplctag

This repository is a wrapper around [libplctag](https://github.com/timyhac/libplctagWrapper).

It does nothing more than expose the libplctag API in a way idiomatic to .NET

It was forked from on [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Expose the API available in libplctag
* Package this functionality in a way that is convenient to consume for .NET projects (i.e. publish on nuget.org).
* Use idiomatic .NET concepts (Exceptions, IDisposable, Strongly-Typed variables, etc) so long as it does not prevent downstream libraries from creating advanced functionality based on the original libplctag
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard
