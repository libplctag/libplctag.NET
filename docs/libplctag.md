# libplctag

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

## Concepts

### Tag

A tag is the key concept exposed by libplctag.
It provides an interface to read and write to PLC memory.
PLC memory is exposed as a byte array, which can be read from the PLC with `Read(..)`, manipulated through the getter/setter functions, and then written back to the PLC with `Write(..)`.

### Mapper
A mapper (an implementation of [`IPlcMapper`](https://github.com/libplctag/libplctag.NET/blob/master/src/libplctag/DataTypes/IPlcMapper.cs) handles the translation between a .NET type and the Plc type.

### Network Protocol (Ethernet/IP Explicit Messaging and Modbus)

Ethernet/IP explicit messaging is the protocol used by libplctag to communicate
It is important to note that using libplctag does not preclude you from understanding the Ethernet/IP API offered by your PLC.

One common example of this is accessing String tags.
String tags are a special case of a STRUCT that vary significantly across devices and so it is necessary to know how the String is encoded in memory.

When working with any UDTs you will need to reverse-engineer the structure.
An example can be found [here](src/Examples/CSharp%20DotNetCore/SequencePlcMapper.cs).

Another example of this is how the Tag name influences the memory layout.
For example, a BOOL Array Tag on Omron PLCs will present different memory layout when the name is `MyBoolArray` versus `MyBoolArray[0]`.

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

## Further Examples

For more detail and further usage, see the examples in the example projects:

* [C#](src/Examples/CSharp%20DotNetCore)
* [VB.NET](src/Examples/VB.NET%20DotNetCore/Program.vb)
