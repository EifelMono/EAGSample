# Documentation

Es ist noch nicht alles .........

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

```csharp
using Position3D = ProLog3.Communication.ImageProcessing.Position3D;
using Size3D = ProLog3.Communication.ImageProcessing.Size3D;
using Table = ProLog3.Communication.ImageProcessing.Table;
using Ocr = ProLog3.Communication.ImageProcessing.Ocr;
  ```

### Communication

// Host leer => Server
```csharp
var Position3DCommunication = new Position3D.ImageProcessingCommunication(Port, Host optional)
var Size3DCommunication = new Size3D.ImageProcessingCommunication(Port, Host optional)
var TableCommunication = new Table.ImageProcessingCommunication(Port, Host])
var OcrCommunication = new Ocr.ImageProcessingCommunication(Port, Host])
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
          *  Unknown, Ok, SystemNotReady, Error, RequestWrongProperties,
        * Messages => List of string 
* **Event**

### Senden

```csharp
Communication.Send(message)
```

### Empfangen

#### über event
```csharp
var eventHandler = Communication.OnReceived.Add(
    (communication, message) => { /* todo */  });

Communication.OnReceived.Remove(eventHandler);
```
#### über Wait

```csharp
await Communication.WaitAsync<message>();

// mt timeout oder CancellationToken
await Communication.WaitAsync<message>(TimeSpan.FromSeconds(....));


Communication.Send(messageRequest);
// hier verbraten wir Zeit
await Communication.WaitAsync<messageResonse>();
// Jetzt kann es sein das die Nachricht vor dem Aufruf
// von WaitAsync schon da ist !!!! Also geht die verloren

// abhilfe mit einer Marke!
var mark= Communication.Send(messageRequest);
// hier verbraten wir Zeit
await mark.WaitAsync<messageResonse>();
// Jetzt geht die Naricht nicht verloren
```







