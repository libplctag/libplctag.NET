# libplctag .NET

This is a .NET wrapper around [libplctag](https://github.com/kyle-github/libplctag).

Download from [nuget.org](https://www.nuget.org/packages/libplctag/).

It was inspired by [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Package the libplctag functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard
* Make it easy for package consumers to comply with libplctag's [licensing model](https://github.com/kyle-github/libplctag/blob/master/LICENSE)

## Notes

* Nothing has been tested, it probably doesn't work on your platform.


## How to Build

```
>dotnet build
>dotnet pack -o .
```
