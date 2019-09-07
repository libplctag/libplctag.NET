## Example Usage

```csharp
var myTag = new Tag(IPAddress.Parse("192.168.0.10"), "1,0", CpuType.LGX, DataType.DINT, "MY_DINT");
while (myTag.GetStatus() == StatusCode.PLCTAG_STATUS_PENDING)
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
