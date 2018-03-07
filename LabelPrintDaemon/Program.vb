Imports System.Net
Imports System.IO

Module Program
    Dim templatePath As String
    Dim logPath As String
    Dim logWriter As StreamWriter
    Dim doc As bpac.Document
    Dim printerName As String
    Dim fieldNamePrefix As String = "field"
    Dim numOfCopies As Integer = 1
    Dim dataUrl As String = "https://jsonplaceholder.typicode.com/posts"
    Dim data As ArrayList

    Sub Main(args As String())
        Log("Program started.")
        If args.Count < 1 Then
            Log("Missing argument. Please, provide absolute path to LBX file. Exiting.")
            Return
        End If

        logPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\LabelPrintDaemon-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log"
        logWriter = New StreamWriter(logPath)
        doc = New bpac.Document
        templatePath = args(0).Trim()

        If File.Exists(templatePath) = False Then
            Log("File " + templatePath + " does not exists. Exiting.")
            Return
        End If

        Log("Getting printer...")
        printerName = GetPrinterName()
        If printerName = "" Then
            Log("No printer found. Exiting.")
            Return
        End If
        Log("Printer found: " + printerName)

        Log("LBX file path is " + args(0))
        Log("Log file path is " + logPath)

        Log("Getting data...")
        GetData()
        If data.Count < 1 Then
            Log("Could not get data. Exiting.")
            Return
        End If
        Log("Got data.")

        Log("Starting to print...")
        If (doc.Open(templatePath) <> False) Then
            Dim f As Integer = 0
            For Each field As String In data
                Log("Setting " + fieldNamePrefix + f.ToString + " to " + field)
                doc.GetObject(fieldNamePrefix + f.ToString).Text = field
                f += 1
            Next
            doc.StartPrint(printerName, bpac.PrintOptionConstants.bpoDefault)
            doc.PrintOut(numOfCopies, bpac.PrintOptionConstants.bpoDefault)
            doc.EndPrint()
            doc.Close()
        End If
        Log("Printing done.")
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

    Function GetData()
        Dim webClient As New WebClient
        'Dim xElement As XElement = XElement.Parse(webClient.DownloadString(dataUrl))
        Dim xElement As XElement = XElement.Parse("<Fields><Field>TEST1</Field><Field>TEST2</Field><Field>TEST3</Field></Fields>")
        Dim xElementFields As IEnumerable(Of XElement) = xElement.Descendants("Field")
        data = New ArrayList()
        For Each xElementField As XElement In xElementFields
            data.Add(xElementField.Value)
        Next
    End Function

    Function GetPrinterName() As String
        Try
            Dim printers As Object() = doc.Printer.GetInstalledPrinters()
            If printers.Count > 0 Then
                Dim printerName As String = printers(0).ToString
                If doc.Printer.IsPrinterOnline(printerName) Then
                    Return printerName
                End If
            End If
        Catch e As NullReferenceException
            'Do nothing.
        End Try
        Return ""
    End Function

End Module
