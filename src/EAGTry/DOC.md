# Documentation
Demos immer anhand von Table Communication!

## Source
* alles netstandard 2.0 format
* alles aus Nuget Packeten
  * RowaLog.*.nupkg
  * ProLog.Core.*.nupkg
  * ProLog.RowaLog.*.nupkg
  * ProLog3.Types.*.nupkg
  * ProLog.Communication.Core.*.nupkg
  * ProLog3.Communication.ImageProcessing.Position3D.*.nupkg
  * ProLog3.Communication.ImageProcessing.Size3D.*.nupkg
  * ProLog3.Communication.ImageProcessing.Table3D.*.nupkg
  * ProLog3.Communication.ImageProcessing.Ocr3D.*.nupkg
* Nuget.Config bestimmt wo die Packete herkommen 
* https://www.myget.org/F/rowa/auth/28de5fa5-9592-451d-a8e9-e769859e2061/api/v3/index.json


## Loggen
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region usinglog
using ProLog.RowaLog;
using ProLog.Core.Log;
``` 
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region voidlog
// RowaLogProxy("Product", "Component")
// C:\ProgramData\Rowa\Protocol\Product\Component
var rowaLogProxy = new RowaLogProxy(Globals.App.Name);

try
{
    "Debug".LogDebug();
    "UserIn".LogUserIn();
    "ExtIf".LogExtIf();
    "Info".LogInfo();
    "Warning".LogWarning();
    "Error".LogError();
    "Fatal".LogFatal();
    "Audit".LogAudit();
}
catch (Exception ex)
{
    ex.LogException(); // LogFatal
}

rowaLogProxy.Dispose(); // Ist Notwendig sonst wird ein Thread in RowaLog nicht beendet
```

## Communication Benutzung

### using
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region usingcommunication
using Position3D = ProLog3.Communication.ImageProcessing.Position3D;
using Size3D = ProLog3.Communication.ImageProcessing.Size3D;
using Table = ProLog3.Communication.ImageProcessing.Table;
using Ocr = ProLog3.Communication.ImageProcessing.Ocr;
```

### Communication
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region createcommunication
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
  
* Position3D.Messages
* Size3D.Messages
* Table.Messages
* OcrCommunication.Messages

### Typen

* **Request**
    * **Response**
        * mit Interface **ICommunicationMessageResult** (CommunicationMessageResult)
            * Result
                * State => CommunicationMessageResultState
                * Unknown, Ok, SystemNotReady, Error, RequestWrongProperties,
            * Messages => List of string 
* **Event**

### Senden
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region sendmessage
// Communication.Send(message)
TableCommunication.Send(new Table.Messages.TurnRelativeRequest(0, 100.0));
```

### Empfangen

#### Event
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region receiveevent
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
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region receivemessage
await TableCommunication.WaitAsync<Table.Messages.TurnRelativeResponse>();

// mit timeout oder CancellationToken
var result = await TableCommunication.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(1));

// A. Prüfung der Antwort
if (result.IsOk())
{
    "Es ist eine Antwort vorhanden".LogInfo();
    // in message steht die response message
    if (result.Message.IsOk())
        "Die Antwort vom Server is auch Ok".LogInfo();
    // Hier die Langform und .....
    if (result.Message.Result.State == CommunicationMessageResultState.Ok)
        "Die Antwort vom Server is auch Ok".LogInfo();
}
else
    "Timeout".LogInfo();

// B. oder kurz und die Fehler stehen im log
if (result.IsMessageOk())
{
    "Die Antwort vom Server is auch Ok".LogInfo();
}
``` 
#### Send and Wait
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region receivemessagewithsend
var result1 = await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>
    (new Table.Messages.TurnRelativeRequest(0, 100.0), TimeSpan.FromSeconds(1));
if (result1.IsMessageOk())
{
    // everthing is ok
};

if (await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>
    (new Table.Messages.TurnRelativeRequest(0, 100.0), TimeSpan.FromSeconds(1))
    is var result2 && result2.IsMessageOk())
{
    // everthing is ok
};
``` 
#### Wait wenn zwischen Senden und Empfangen Zeit vergeht (**fehlerhaft**)
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region receivemessageproblem
TableCommunication.Send(new Table.Messages.TurnRelativeRequest(0, 100.0));
// hier verbraten wir Zeit
var result = await TableCommunication.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(1));
// Jetzt kann es sein das die Nachricht vor dem Aufruf
// von WaitAsync schon da ist und WaitAsync gibt result.Ok mit false zurück
```
#### Wait wenn zwischen Senden und Empfangen Zeit vergeht mit Marker
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region receivemessagemark
var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest(0, 100.0));
// hier verbraten wir Zeit
var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(1));
if (result.IsOk())
{
    if (result.Message.IsOk())
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

#### ICommunicationMessageResult in  MessageResponse

##### beim Senden
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region icommunicationmessageresultsend
// Message erzeugen
var message = new Table.Messages.TurnRelativeResponse { /* und füllen */ };

// Result setzen 
// ok Meldungen
message.Result.State = CommunicationMessageResultState.Ok;
message.Result.Messages.Add("Mit Message");
message.SetStateOk();
message.SetStateOk("Mit Message");

// FehlerMeldungen
message.Result.State = CommunicationMessageResultState.Error;
message.SetStateError();
message.SetStateError("Mit Message");
message.SetStateError(new Exception());
message.SetStateSystemNotReady();
message.SetStateSystemNotReady("Mit Message");

// Oder durch boolschen Wert
message.SetState(true); // => message.SetStateOk();
message.SetState(false); // => message.SetStateError();
```
##### beim Empfangen
``` cs --source-file .\doc.cs --project .\EagTry.csproj --region icommunicationmessageresultreceive
// Message erzeugen
var message = new Table.Messages.TurnRelativeResponse { /* und füllen */ };

// Message abfragen
if (message.Result.State == CommunicationMessageResultState.Ok)
{ }
if (message.IsOk())
{ }

if (message.Result.State == CommunicationMessageResultState.Error)
{ }
if (message.IsError())
{ }

var messages = message.Result.Messages; // Hier stehen die Fehlermeldungen drin.
```





