# libplctag.NET

[libplctag](https://github.com/libplctag/libplctag) is a C library for Linux, Android, Windows and macOS that uses EtherNet/IP or Modbus TCP to read and write tags in PLCs.

[libplctag.NET](https://www.nuget.org/packages/libplctag/) provides wrapper packages for libplctag, with an API naturalised to .NET by adding the following features:

* Values are strongly-typed
* Errors are thrown as Exceptions
* Async/Await
* Native resource cleanup

## How to use 

```csharp
// Example tag configuration for a global DINT tag in an Allen-Bradley CompactLogix/ControlLogix PLC
var myTag = new Tag()
{
    Name = "SomeDINT",
    Gateway = "10.10.10.10",
    Path = "1,0",
    PlcType = PlcType.ControlLogix,
    Protocol = Protocol.ab_eip
};

// Read the value from the PLC and output to console
myTag.Read();
int originalValue = myTag.GetInt32();        
Console.WriteLine($"Original value: {originalValue}");

// Write a new value to the PLC, then read it back, and output to console
int updatedValue = 1234;
myTag.SetInt32(updatedValue);
myTag.Write();    
Console.WriteLine($"Updated value: {updatedValue}");
```

See the examples projects for further detail and usage:

* [C# (.NET)](../examples/CSharp%20DotNetCore/)
* [C# (.NET Framework)](../examples/CSharp%20DotNetFramework/)
* [VB.NET](../examples/VB.NET%20DotNetCore/Program.vb)

## Explanation

A tag is a local reference to a region of PLC memory.
Depending on the PLC type and protocol the region may be named.
For some protocols, the region is simply a type and register number (e.g. Modbus).
For other protocols, it is a name, possible array element, field names etc. (e.g. a CIP-based PLC).

Your program directly controls the resources associated with the connection to the PLC.
It initializes these resources through the `Initialize()` function and frees the resources used with the `Dispose()` function.
The lowest level of access to a tag is via the `Read()` and `Write()` operations.
In most cases you must explicitly call these functions to write to the PLC or read from the PLC.
There are also attributes that can be passed when creating a Tag to make it either automatically write to the PLC when the local copy of the tag is updated or read from the PLC periodically, or both.
Once initialized, the tag memory is exposed as a byte array, and can be manipulated using various [data accessors](https://github.com/libplctag/libplctag/wiki/API#tag-data-accessors).

Libplctag does not expose the concept of a PLC.
Just tags.

Read more on the [libplctag wiki](https://github.com/libplctag/libplctag/wiki/API).

## `libplctag.Tag`

libplctag.NET provides a wrapper for the C API naturalised for .NET.
The `Tag` class is intended to be functionally equivalent to the C API.

For example:

* `Read(..)` ≋ [`plc_tag_read(..)`](https://github.com/libplctag/libplctag/wiki/API#reading-a-tag)
* `Write(..)` ≋ [`plc_tag_write(..)`](https://github.com/libplctag/libplctag/wiki/API#writing-a-tag)
* `GetInt32(..)` ≋ [`plc_tag_get_int32(..)`](https://github.com/libplctag/libplctag/wiki/API#tag-data-accessors)

Some methods are presented slightly differently due to the differences in languages and language idioms.
For example, the counterpart to `Initialize(..)` is [`plc_tag_create(..)`](https://github.com/libplctag/libplctag/wiki/API#creating-a-tag-handle) and the tag attributes are specified as properties (e.g. `Tag.Path`).

### Data Accessors

Mapping the raw tag buffer to some typed value (e.g. `int`) and vice-versa can be achieved using the built-in [Data Accessor methods](https://github.com/libplctag/libplctag/wiki/API#tag-data-accessors).
Alternatively, get a copy of the byte array and do the conversion yourself (e.g. with [`BitConverter`](https://learn.microsoft.com/en-us/dotnet/api/system.bitconverter), [`BinaryPrimities`](https://learn.microsoft.com/en-us/dotnet/api/system.buffers.binary.binaryprimitives), [`Encoding`](https://learn.microsoft.com/en-us/dotnet/api/system.text.encoding), among others).

In general, you will need prior knowedge of the binary format of the tag data, and you may need to reverse-engineer it.
The manuals provided by your device manufacturer are the best source of information on these details.

## `libplctag.LibPlcTag`

This is a static class used to access some utility features of the libplctag base library such as global debug levels and logging.

## libplctag.NativeImport

The libplctag package depends on the core libplctag libraries which are written in C and are released as native binaries.
The delivery of these files, and the interop to the .NET environment is provided by the [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) package.

Information on this package can be found [here](libplctag.NativeImport.md).
