# libplctag.NET

[libplctag](https://github.com/libplctag/libplctag) is a C library for Linux, Android, Windows and macOS that uses EtherNet/IP or Modbus TCP to read and write tags in PLCs.

[libplctag.NET](https://www.nuget.org/packages/libplctag/) provides wrapper packages for libplctag, with an API naturalised to .NET by adding the following features:

* Values are strongly-typed (both Atomic types and User-Defined Types).
* Errors are thrown as Exceptions
* Async/Await
* Native resource cleanup

## How to use 

```csharp
// Example tag configuration for a global DINT tag in an Allen-Bradley CompactLogix/ControlLogix PLC
var myTag = new TagDint()
{
    Name = "SomeDINT",
    Gateway = "10.10.10.10",
    Path = "1,0",
    PlcType = PlcType.ControlLogix,
    Protocol = Protocol.ab_eip
};

// Read the value from the PLC and output to console
int output = myTag.Read();        
Console.WriteLine($"Original value: {output}");

// Write a new value to the PLC, then read it back, and output to console
myTag.Write(37);    
output = myTag.Read();
Console.WriteLine($"Updated value: {output}");
```

See the examples projects for further detail and usage:

* [C# (.NET)](src/Examples/CSharp%20DotNetCore)
* [C# (.NET Framework)](src/Examples/CSharp%20DotNetFramework)
* [VB.NET](src/Examples/VB.NET%20DotNetCore/Program.vb)

## Introduction

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

## `Tag`

libplctag.NET provides a wrapper for the C API naturalised for .NET.
The `Tag` class is intended to be functionally equivalent to the C API.

For example:

* `Read(..)` ≋ [`plc_tag_read(..)`](https://github.com/libplctag/libplctag/wiki/API#reading-a-tag)
* `Write(..)` ≋ [`plc_tag_write(..)`](https://github.com/libplctag/libplctag/wiki/API#writing-a-tag)
* `GetInt32(..)` ≋ [`plc_tag_get_int32(..)`](https://github.com/libplctag/libplctag/wiki/API#tag-data-accessors)

Some methods are presented slightly differently due to the differences in languages and language idioms.
For example, the counterpart to `Initialize(..)` is [`plc_tag_create(..)`](https://github.com/libplctag/libplctag/wiki/API#creating-a-tag-handle) and the tag attributes are specified as properties (e.g. `Tag.Path`).


## `Tag<M,T>` and Mappers

In your .NET application, you will usually need to convert the raw bytes into a .NET type.
It is possible to use `GetInt32()` and `SetInt32()` (and [others](https://github.com/libplctag/libplctag/wiki/API#tag-data-accessors)) provided by the `Tag` class to perform this conversion, and most of the time, there will only be one sensible way to interpret these bytes for a given tag.

For example, a `DINT` tag defined in a PLC is a 32bit signed integer, and this would be exposed as a little-endian encoded 4-byte array.
The natural choice for a C# type would be `int` - it would be rare to want to work with this data as a `float` or a 4-byte ASCII string for example.

To this end, libplctag.NET offers a typed tag class `Tag<M,T>` that exposes the tag value as a C# type instead of the complete set of Getter/Setter functions.
This class pairs with an [`IPlcMapper`](src/libplctag/DataTypes/IPlcMapper.cs), which encapsulates the mapping between a .NET type (e.g. `int`, `float`) and the PLC type (e.g. `DINT`, `REAL`) by calling the appropriate functions on a Tag (as well as providing other information that libplctag needs for this mapping).

```csharp
class DintPlcMapper : IPlcMapper<int>
{
    public PlcType PlcType { get; set; }
    public int? ElementSize => 4;
    public int[] ArrayDimensions { get; set; }
    public int? GetElementCount() => 1;
    public int Decode(Tag tag, int offset) => tag.GetInt32(offset);
    public void Encode(Tag tag, int offset, int value) => tag.SetInt32(offset, value);
}

var myTag = new Tag<DintPlcMapper, int>(){...configuration...};
myTag.Initialize();
myTag.Value = 1234;
myTag.Write();
```

In general, you will need prior knowedge of the structure of the tag data, and you may need to reverse-engineer it.
An example for reverse engineering a UDT can be found [here](src/Examples/CSharp%20DotNetCore/SequencePlcMapper.cs).

Because the structure of the data depends on many factors (PLC Make/model, Protocol, and even the tag Name), libplctag.NET does not provide built-in Mappers for all types.
The manuals provided by your device manufacturer are the best source of information on these details.

## Types

### `libplctag` namepsace
* `Tag` - A wrapper around the core libplctag library tag with an interface naturalised to .NET.
* `Tag<M,T>` - A wrapper that exposes a .NET type (generic parameter `T`) instead of Data Accessors. The data access logic is delegated to an `IPlcMapper` (generic parameter `M`).
* `ITag` - an interface that is implemented by `Tag<M,T>`.
* `Libplctag` - A static class used to access some additional features of the libplctag base library such as global debug levels and logging.
* Enum types such as `DebugLevel`.
* Supporting types such as `TagEventArgs`.

All types are shipped with XML documentation, so the full API is discoverable in your IDE.

### `libplctag.DataTypes` namespace

* [`IPlcMapper`](src/libplctag/DataTypes/IPlcMapper.cs)
* [`DintPlcMapper`](src/libplctag/DataTypes/DintPlcMapper.cs)
* [`LrealPlcMapper`](src/libplctag/DataTypes/LrealPlcMapper.cs)
* ... and so on

Of note are [TagInfoPlcMapper](src/libplctag/DataTypes/TagInfoPlcMapper.cs) and [UdtInfoMapper](src/libplctag/DataTypes/UdtInfoPlcMapper.cs), which can be used to [list the tags in a ControlLogix PLC](src/Examples/CSharp%20DotNetCore/ListUdtDefinitions.cs).

### `libplctag.DataTypes.Simple` namespace

In simple cases such as atomic tags (e.g.`DINT`) or arrays of atomic tags (e.g. `LREAL[x,y]`), we provide classes that pair a built-in `IPlcMapper` with the natural .NET type:

* [`TagDint`](src/libplctag/DataTypes/Simple/Definitions.cs#L21)
* [`TagLreal2D`](src/libplctag/DataTypes/Simple/Definitions.cs#L41)
* ... and so on

## libplctag.NativeImport

The libplctag package depends on the core libplctag libraries which are written in C and are released as native binaries.
The delivery of these files, and the interop to the .NET environment is provided by the [libplctag.NativeImport](https://www.nuget.org/packages/libplctag.NativeImport/) package.

Information on this package can be found [here](libplctag.NativeImport.md).
