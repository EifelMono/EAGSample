# Documentation
Es ist noch nicht alles .........

Demos immer anhand von Table Communication!

## Source
* alles als Nuget Packete
* alles netstandard 2.0 format

## Loggen
``` cs --region usinglog --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
using ProLog.RowaLog;
using ProLog.Core.Log;
``` 
``` cs --region voidlog --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
// RowaLogProxy("Product", "Component")
// C:\ProgramData\Rowa\Protocol\Product\Component
var rowaLogProxy = new RowaLogProxy(Globals.App.Name);

try
{
    "Info".LogInfo();
    "Error".LogError();
    "Debug".LogDebug();
    "Audit".LogAudit();
}
catch (Exception ex)
{
    ex.LogException();
}

rowaLogProxy.Dispose(); // Ist Notwendig sonst wird ein Thread in RowaLog nicht beendet
```

## Communication 

## Interface

### using
``` cs --region usingcommunication --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
using Position3D = ProLog3.Communication.ImageProcessing.Position3D;
using Size3D = ProLog3.Communication.ImageProcessing.Size3D;
using Table = ProLog3.Communication.ImageProcessing.Table;
using Ocr = ProLog3.Communication.ImageProcessing.Ocr;
```

### Communication
``` cs --region createcommunication --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
var port = 4711;
var host = "127.0.0.1";
// host is optional, 
// if host is null or empty the communication is running as a server
var Position3DCommunication = new Position3D.ImageProcessingCommunication(port, host);
var Size3DCommunication = new Size3D.ImageProcessingCommunication(port, host);
var TableCommunication = new Table.ImageProcessingCommunication(port, host);
var OcrCommunication = new Ocr.ImageProcessingCommunication(port, host);
```

### Messages

#### Namenspace Messages
  
Position3D.Messages
Size3D.Messages
Table.Messages
OcrCommunication.Messages

### Typen

* **Request**
    * **Response**
        * mit Interface ICommunicationMessageResult (CommunicationMessageResult)
            * Result
                * State => CommunicationMessageResultState
                * Unknown, Ok, SystemNotReady, Error, RequestWrongProperties,
            * Messages => List of string 
* **Event**

### Senden

``` cs --region sendmessage --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
// Communication.Send(message)
TableCommunication.Send(new Table.Messages.TurnRelativeRequest(0, 100.0));
```

### Empfangen

#### Event
``` cs --region receiveevent --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
var eventHandler = TableCommunication.OnReceived.Add(
    (communication, message) =>
    {
        switch (message)
        {
            case Table.Messages.TurnRelativeRequest turnRelative:
                communication.Send(new Table.Messages.TurnRelativeResponse().SetStateOk());
                break;
            default:
                Console.WriteLine($"unhandled message {message?.GetType().Name ?? "unkown"}".LogError());
                break;
        }
    });


// wenn der Handler nicht mehr benötigt wird entfernen
TableCommunication.OnReceived.Remove(eventHandler);
```
#### Wait
``` cs --region receivemessage --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
 await TableCommunication.WaitAsync<Table.Messages.TurnRelativeResponse>();
// mit timeout oder CancellationToken
var result = await TableCommunication.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(1));

// Prüfung der Antwort
if (result.Ok)
{
    "Es ist eine Antwort vorhanden".LogInfo();
    // in value steht die response message
    if (result.Value.IsOk()) 
    {
        "Die Antwort vom Server is auch Ok".LogInfo();
    }
    // Hier die Langform und .....
    if (result.Value.Result.State== CommunicationMessageResultState.Ok)
    {
        "Die Antwort vom Server is auch Ok".LogInfo();
        // IsOk() is eine Extension methode für das 
        // interface ICommunicationMessageResult das alle Response haben sollten
        // public static bool IsOk(this ICommunicationMessageResult thisValue)
        //  => thisValue.Result?.State.IsOk() ?? false;
    }
}
else
    "Timeout".LogInfo();
``` 
#### Wait wenn zwischen Senden und Empfangen Zeit vergeht (**fehlerhaft**)
``` cs --region receivemessageproblem --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
TableCommunication.Send(new Table.Messages.TurnRelativeRequest(0, 100.0));
// hier verbraten wir Zeit
var result = await TableCommunication.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(1));
// Jetzt kann es sein das die Nachricht vor dem Aufruf
// von WaitAsync schon da ist und WaitAsync gibt result.Ok mit false zurück
```
#### Wait wenn zwischen Senden und Empfangen Zeit vergeht mit Marker
``` cs --region receivemessagemark --source-file .\src\EagTry\doc.cs --project .\src\EagTry\EagTry.csproj
var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest(0, 100.0));
// hier verbraten wir Zeit
var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(1));
if (result.Ok)
{
    if (result.Value.IsOk())
    {
    }
    else
    {
    }
}
else
{
}
```






