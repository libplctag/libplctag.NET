# libplctag

This repository is a .NET wrapper around [libplctag](https://github.com/kyle-github/libplctag).

It was forked from [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Make the libplctag API available in a way that is idomatic to .NET (Exceptions, IDisposable, Strong-Typing, Object-Oriented, etc)
* Package this functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard

## Example

```csharp
var myTag = Tag.Create(IPAddress.Parse("192.168.1.42"), "1,0", CpuType.LGX, DataType.DINT, 1, "MY_DINT", 0, TimeSpan.Zero)

myTag.Read(TimeSpan.Zero);

int myDint = GetInt32(0);

Console.WriteLine(myDint);
```

## Notes

* Nothing has been tested
* Only Windows currently supported
* Currently you need to supply a copy of plctag.dll yourself.