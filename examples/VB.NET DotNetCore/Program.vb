' Copyright(c) libplctag.NET contributors
' https://github.com/libplctag/libplctag.NET

' This Source Code Form Is subject To the terms Of the Mozilla Public
' License, v. 2.0. If a copy of the MPL was Not distributed with this
' file, You can obtain one at https: //mozilla.org/MPL/2.0/.

Imports System.Net
Imports System.Threading
Imports libplctag
Imports libplctag.DataTypes

Module Module1

    Sub Main()

        Dim myTag = New Tag(Of DintPlcMapper, Integer)() With
        {
            .Name = "PROGRAM:SomeProgram.SomeDINT",
            .Gateway = "10.10.10.10",
            .PlcType = PlcType.ControlLogix,
            .Protocol = Protocol.ab_eip,
            .Path = "1,0",
            .Timeout = TimeSpan.FromMilliseconds(5000)
        }
        myTag.Initialize()

        myTag.Value = 3737
        myTag.Write()

        myTag.Read()
        Dim myDint = myTag.Value

        Console.WriteLine(myDint)
        Console.ReadKey()

    End Sub

End Module