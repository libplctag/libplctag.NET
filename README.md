# libplctag .NET

This repository is a .NET wrapper around [libplctag](https://github.com/kyle-github/libplctag).

It was forked from [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Package the libplctag functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard

## Notes

* Nothing has been tested
* Currently you need to supply a copy of the library yourself. It's filename should be plctag.dll / plctag.so / plctag.dylib
