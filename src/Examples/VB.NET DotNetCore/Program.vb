Imports System.Net
Imports System.Threading
Imports libplctag

Module Module1

    Sub Main()

        Dim myTag = New Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, DataType.DINT, "PROGRAM:SomeProgram.SomeDINT")

        While (myTag.GetStatus() = Status.Pending)
            Thread.Sleep(100)
        End While
        If myTag.GetStatus() <> Status.Ok Then
            Throw New LibPlcTagException(myTag.GetStatus())
        End If

        myTag.SetInt32(0, 3737)
        myTag.Write(TimeSpan.Zero)

        While (myTag.GetStatus() = Status.Pending)
            Thread.Sleep(100)
        End While
        If myTag.GetStatus() <> Status.Ok Then
            Throw New LibPlcTagException(myTag.GetStatus())
        End If

        myTag.Read(TimeSpan.Zero)

        While (myTag.GetStatus() = Status.Pending)
            Thread.Sleep(100)
        End While
        If myTag.GetStatus() <> Status.Ok Then
            Throw New LibPlcTagException(myTag.GetStatus())
        End If

        Dim myDint = myTag.GetInt32(0)

        Console.WriteLine(myDint)
        Console.ReadKey()

    End Sub

End Module