Imports System.Net
Imports System.Threading
Imports libplctag

Module Module1

    Sub Main()

        Dim myTag = New Tag() With
        {
            .Name = "PROGRAM:SomeProgram.SomeDINT",
            .Gateway = "10.10.10.10",
            .PlcType = PlcType.ControlLogix,
            .ElementSize = 4,
            .Path = "1,0"
        }
        myTag.Initialize(5000)

        While (myTag.GetStatus() = Status.Pending)
            Thread.Sleep(100)
        End While
        If myTag.GetStatus() <> Status.Ok Then
            Throw New LibPlcTagException(myTag.GetStatus())
        End If

        myTag.SetInt32(0, 3737)
        myTag.Write(0)

        While (myTag.GetStatus() = Status.Pending)
            Thread.Sleep(100)
        End While
        If myTag.GetStatus() <> Status.Ok Then
            Throw New LibPlcTagException(myTag.GetStatus())
        End If

        myTag.Read(0)

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