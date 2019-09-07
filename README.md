﻿# libplctag .NET

This is a .NET wrapper around [libplctag](https://github.com/kyle-github/libplctag).

Download from [nuget.org](https://www.nuget.org/packages/libplctag/).

It was inspired by [libplctag-csharp](https://github.com/mesta1/libplctag-csharp).

## Goals

* Package the libplctag functionality in a way that is convenient to use for .NET projects (i.e. publish on nuget.org).
* Be cross-platform: It should support any platform that libplctag can be built for, and supports .NET Standard 2.0
* Make it easy for package consumers to comply with libplctag's [licensing model](https://github.com/kyle-github/libplctag/blob/master/LICENSE)

## Notes

* Nothing has been tested, it probably doesn't work on your platform.

## Example Usage

The below example connects to a CompactLogix PLC with the IP Address 192.168.0.10 with a DINT tag named "MY_DINT" and display it's value.

```csharp
using libplctag.Dll;
using System;
using System.Threading;

namespace ExampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var handle = plctag.create("protocol=ab_eip&gateway=192.168.0.10&path=1,0&cpu=LGX&elem_size=4&elem_count=1&name=MY_DINT", 1000);

            while (plctag.status(handle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusBeforeRead = plctag.status(handle);
            if (statusBeforeRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusBeforeRead}");
            }

            plctag.read(handle, 1000);
            while (plctag.status(handle) == 1)
            {
                Thread.Sleep(100);
            }
            var statusAfterRead = plctag.status(handle);
            if (statusAfterRead != 0)
            {
                Console.WriteLine($"Something went wrong {statusAfterRead}");
            }

            var theValue = plctag.get_uint32(handle, 0);

            Console.WriteLine(theValue);
            Console.Read();
        }
    }
}
```


## How to Build

```
>dotnet build
>dotnet pack -o .
```
