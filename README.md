# libplctag .NET

This is a .NET wrapper around [libplctag](https://github.com/kyle-github/libplctag).

It was forked from [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Make the libplctag API available in a way that is idomatic to .NET (Exceptions, IDisposable, Strong-Typing, Object-Oriented, etc)
* Package this functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard

## Example Usage

```csharp
var myTag = Tag.Create(IPAddress.Parse("192.168.1.42"), "1,0", CpuType.LGX, DataType.DINT, 1, "MY_DINT", 0, TimeSpan.Zero)

while(myTag.GetStatus() == StatusCode.PLCTAG_STATUS_PENDING)
{
    Thread.Sleep(100);
}
if (myTag.GetStatus() != StatusCode.PLCTAG_STATUS_OK)
{
    Console.WriteLine("Something went wrong");
}

myTag.Read(TimeSpan.Zero);
while (myTag.GetStatus() == StatusCode.PLCTAG_STATUS_PENDING)
{
    Thread.Sleep(100);
}
if (myTag.GetStatus() != StatusCode.PLCTAG_STATUS_OK)
{
    Console.WriteLine("Something else went wrong");
}

int myDint = myTag.GetInt32(0);

Console.WriteLine(myDint);
```

## Notes

* Nothing has been tested
* Currently you need to supply a copy of the library yourself. It's filename should be plctag.dll / plctag.so / plctag.dylib
