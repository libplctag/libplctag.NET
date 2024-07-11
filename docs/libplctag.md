# libplctag.NET

[libplctag](https://github.com/libplctag/libplctag) is a library provides a portable and simple API for accessing Allen-Bradley and Modbus PLC data over Ethernet.

[libplctag.NET](https://www.nuget.org/packages/libplctag/) provides wrapper packages for libplctag, with an API naturalised to .NET by supporting the following features:

* Values are strongly-typed (both Atomic types and User-Defined Types).
* Errors are thrown as Exceptions
* Async/Await
* Native resource cleanup

Except for the above, all of the libplctag functionality is provided by the core libplctag library.


## Getting Started

Add the libplctag package in the Visual Studio Nuget Package Manager UI or via the commandline:

`> dotnet add package libplctag`

Then add the following in a C# application:

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

Libplctag does a great job at abstracting many of the details of Ethernet/IP and Modbus, there are still some aspects of these network protocols that library users must understand.

The manuals provided by your device manufacturer are the best source of information on these details.

### Gateway and Path
Gateway refers to the IP Address or hostname 
Usually this is the IP Address of the PLC CPU or network module.
Ethernet/IP communication is routed through a series of devices, so the `path` configures that routing.

### Name
libplctag converts the `name` string into a format appropriate for the network protocol.

Some examples where 
* Program tags
* Array elements
* Unions
* Struct members

### Memory layout
For 





### Network Protocol (Ethernet/IP Explicit Messaging and Modbus)

Ethernet/IP explicit messaging is the protocol used by libplctag to communicate
It is important to note that using libplctag does not preclude you from understanding the Ethernet/IP API offered by your PLC system.

The most obvious example of this is how to connect to the target device; i.e. `Gateway` and `Path`.


One common example of this is accessing String tags.
String tags are a special case of a STRUCT that vary significantly across devices and so it is necessary to know how the String is encoded in memory.

In general, when working with any UDTs you will need to reverse-engineer the structure.
An example can be found [here](src/Examples/CSharp%20DotNetCore/SequencePlcMapper.cs).

Another example of this is how the Tag name influences the memory layout.
For example, a BOOL Array Tag on Omron PLCs will present different memory layout when the name is `MyBoolArray` versus `MyBoolArray[0]`.

## Concepts

### Tag

A tag is the primary concept in libplctag.
It provides a way to read and write to tags in a PLC.

The Tag value is exposed as a byte array, which can be
* Read from the PLC to the client with `Read()`.
* Manipulated on the client through the getter/setter functions such as `GetInt32(int offset)` / `SetInt32(int offset, int value)`.
* Written back to the PLC with `Write()`.

### Mapper
A mapper handles the mapping between a .NET type (e.g. `int`, `float`) and the PLC type (e.g. `DINT`, `REAL`) by calling the appropriate functions on a Tag.
For example, in a PLC a tag could be defined of type `DINT` - a 32bit signed integer, and the natural counterpart in C# would be an `int` type.
The Mapper would encapsulate the calls to `GetInt32(int offset)` and `SetInt32(int offset, int value)` as well as providing other information that libplctag needs for this mapping.

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
