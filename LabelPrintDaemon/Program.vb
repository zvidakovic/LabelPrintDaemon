Imports System.Net
Imports System.IO

Module Program
    Sub Main(args As String())
        Console.WriteLine("PROGRAM STARTED")

        Console.WriteLine("INITIALIZING VARIABLES...")
        Dim dataUrl As String = "https://jsonplaceholder.typicode.com/posts"
        Dim templatePath As String = "C:\LabelPrintDaemon.lbx"
        Dim doc = New bpac.Document
        Dim webRequest As WebRequest
        Dim stream As Stream
        Console.WriteLine("INITIALIZING VARIABLES COMPLETED")

        webRequest = WebRequest.Create(dataUrl)

        Console.WriteLine("DOING GET REQUEST ON URL: " + dataUrl)

        stream = webRequest.GetResponse.GetResponseStream()

        Dim reader As New StreamReader(stream)
        Dim sLine As String = ""
        Dim i As Integer = 0

        Do While Not sLine Is Nothing
            i += 1
            sLine = reader.ReadLine
            If Not sLine Is Nothing Then
                Console.WriteLine("{0}:{1}", i, sLine)
            End If
        Loop

        Console.ReadLine()

        Console.WriteLine("STARTING TO PRINT")
        doc.Open(templatePath)
        doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault)
        doc.PrintOut(2, bpac.PrintOptionConstants.bpoDefault)
        doc.EndPrint()
        doc.Close()
        Console.WriteLine("PRINT DONE")

        Console.WriteLine("PROGRAM IS DONE. EXITING.")
    End Sub
End Module
