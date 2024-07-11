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

## Introduction

Before describing the concrete parts of the API a brief explanation of the model is needed.
The library models each tag, or field within a tag, as an individual handle that your program can use.
There is no exposed concept of a PLC.
Just tags.

A tag is a local reference to a region of PLC memory.
Depending on the PLC type and protocol the region may be named.
For some protocols, the region is simply a type and register number (e.g. Modbus).
For other protocols, it is a name, possible array element, field names etc. (e.g. a CIP-based PLC).

The library provides functions to retrieve and set the values in the PLC's memory as well as many functions to extract and set specific data elements within a tag.
Note that a tag may not correspond directly to a named area in a PLC.
For instance, MyNiftyArray[14] with a length of 2 elements will get elements MyNiftyTag[14] and MyNiftyTag[15] from the tag MyNiftyTag. 

Tags are designed to be very long lived.
They can live weeks or months.
In order to use the library to its full potential, it is important to note this.
Setting up and tearing down connections to a PLC are the heaviest operations possible.
They take significant time and multiple packets to handshake the connection set up.
The library automatically closes PLC connections after periods of no use (typically five seconds) and automatically reopens them when you try to do something with a tag.

Performance Tip: Avoid creating and destroying tags often.

Your program directly controls the lifetime of a handle.
It opens a handle to a PLC tag through the `Initialize()` function and frees the resources used with the `Dispose()` function.
All other resources are handled internally within the library.
You will likely run into system constraints such as available memory, network bandwidth and CPU long before exhausting the internal limits of the library.

The lowest level of access to a tag is via the `Read()` and `Write()` operations.
In most cases you must explicitly call these functions to write to the PLC or read from the PLC.
There are also attributes that can be passed when creating a tag handle to make it either automatically write to the PLC when the local copy of the tag is updated or read from the PLC periodically, or both.
See the wiki page on auto sync tag string attributes for more information.

## Mappers and `Tag<M,T>`

In your .NET application, you will usually need to convert the raw bytes into a .NET type.
It is possible to use `GetInt32()` and `SetInt32()` (and [others](https://github.com/libplctag/libplctag/wiki/API#tag-data-accessors)) provided by the `Tag` class to perform this conversion.
Most of the time however, there will only be one sensible way to interpret the bytes for a given tag.

For example, a `DINT` tag defined in a PLC is a 32bit signed integer, whose natural counterpart in C# would be an `int` type.
This data is exposed as a 4 byte array that represents a little-endian encoded signed integer.
It would be rare to want to interpret this data instead as a `float`, or a 4-byte ASCII string.

To this end, libplctag.NET offers a typed tag class `Tag<M,T>` that exposes the tag value as a C# type instead of the complete set of Getter/Setter functions.
It pairs with a "Mapper", which encapsulates the mapping between a .NET type (e.g. `int`, `float`) and the PLC type (e.g. `DINT`, `REAL`) by calling the appropriate functions on a Tag (as well as providing other information that libplctag needs for this mapping).

Mappers are implemented as an [`IPlcMapper`](src/libplctag/DataTypes/IPlcMapper.cs) and are used in a `Tag<M,T>` to provide an interface that is convenient for application developers to to work with.

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

Because the structure of the data depends on many factors (PLC Make/model, Protocol, even Tag Name), libplctag.NET does not provide built-in Mappers for all types.
The manuals provided by your device manufacturer are the best source of information on these details.

## `libplctag` namepsace
* `Tag` - A wrapper around the core libplctag library tag by providing an interface naturalised to .NET.
* `Tag<M,T>` - A tag type that exposes a .NET type (generic parameter `T`) by delegating data access logic to an `IPlcMapper` (generic parameter `M`).
* `Libplctag` - A static class used to access some additional features of the libplctag base library such as global debug levels and logging.
* `ITag`

## `libplctag.DataTypes` namespace
* [`IPlcMapper`](src/libplctag/DataTypes/IPlcMapper.cs)
* [`DintPlcMapper`](src/libplctag/DataTypes/DintPlcMapper.cs)
* [`LrealPlcMapper`](src/libplctag/DataTypes/LrealPlcMapper.cs)
* ... and so on

Of note are [TagInfoPlcMapper](src/libplctag/DataTypes/TagInfoPlcMapper.cs) and [UdtInfoMapper](src/libplctag/DataTypes/UdtInfoPlcMapper.cs), which can be used to [list the tags in a ControlLogix PLC](src/Examples/CSharp%20DotNetCore/ListUdtDefinitions.cs).

## `libplctag.DataTypes.Simple` namespace
* [`TagDint`](src/libplctag/DataTypes/Simple/Definitions.cs#L21)
* [`TagLreal2D`](src/libplctag/DataTypes/Simple/Definitions.cs#L41)
* ... and so on

## Example

For more detail and further usage, see the examples in the example projects:

* [C#](src/Examples/CSharp%20DotNetCore)
* [VB.NET](src/Examples/VB.NET%20DotNetCore/Program.vb)
