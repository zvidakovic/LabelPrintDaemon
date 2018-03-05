Imports System.Net
Imports System.IO

Module Program
    Dim logPath As String
    Dim logWriter As StreamWriter

    Sub Main(args As String())
        logPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\LabelPrintDaemon-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log"
        logWriter = New StreamWriter(logPath)

        Log("Program started.")
        If args.Count < 1 Then
            Log("Missing argument. Please, provide absolute path to LBX file. Exiting.")
            Return
        End If

        Dim templatePath As String = args(0).Trim()

        If File.Exists(templatePath) = False Then
            Log("File " + templatePath + " does not exists. Exiting.")
            Return
        End If

        Log("LBX file path is " + args(0))
        Log("Log file path is " + logPath)

        Dim dataUrl As String = "https://jsonplaceholder.typicode.com/posts"
        Dim doc = New bpac.Document
        Dim webRequest As WebRequest
        Dim stream As Stream

        Log("Doing GET request on " + dataUrl)
        webRequest = WebRequest.Create(dataUrl)
        stream = webRequest.GetResponse.GetResponseStream()

        Dim reader As New StreamReader(stream)
        Dim sLine As String = ""
        Dim i As Integer = 0
        Do While Not sLine Is Nothing
            i += 1
            sLine = reader.ReadLine
            If Not sLine Is Nothing Then
                'Console.WriteLine("{0}:{1}", i, sLine)
            End If
        Loop

        Log("Starting to print...")
        doc.Open(templatePath)
        doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault)
        doc.PrintOut(2, bpac.PrintOptionConstants.bpoDefault)
        doc.EndPrint()
        doc.Close()
        Log("Printing done.")

        Log("Program is done. Exiting.")
    End Sub

    Function Log(ByVal message As String) As Boolean
        If logWriter Is Nothing Then
            Return False
        End If
        logWriter.WriteLine(message)
        logWriter.Flush()
        Console.WriteLine(message)
        Return True
    End Function

End Module
