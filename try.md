# Test mit Try.Dot Net

[Scot Hanselmans blog](https://www.hanselman.com/blog/IntroducingTheTryNETGlobalToolInteractiveInbrowserDocumentationAndWorkshopCreator.aspx)

## Install 

```  
dotnet tool install --global dotnet-try
```

Start in the folder EagSample
```  
dotnet try 
```

### SetLights
#### long
``` cs --region setlights1 --source-file .\src\EagTry\client.cs --project .\src\EagTry\EagTry.csproj
using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
    return 0;
var result = await TableCommunication.Send(new Table.Messages.SetLightsRequest
{
    Top = true,
    Bottom = true,
    SideLeft = false,
    SideMiddle = false,
    SideRight = false
}).WaitAsync<Table.Messages.SetLightsResponse>();
if (result.IsOk()) // No Timeout
{
    if (result.Message.IsOk())
        Console.WriteLine("Client SetLight Ok".LogInfo());
    else
        Console.WriteLine("Client SetLight Server Error".LogError());
}
else
    Console.WriteLine("Client SetLight Client TimeOut".LogError());
```
#### short
``` cs --region setlights2 --source-file .\src\EagTry\client.cs --project .\src\EagTry\EagTry.csproj
using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
    return 0;
if (await TableCommunication.SendAndWaitAsync<Table.Messages.SetLightsResponse>
    (new Table.Messages.SetLightsRequest
    {
        Top = true,
        Bottom = true,
        SideLeft = false,
        SideMiddle = false,
        SideRight = false
    }) is var result && result.IsMessageOk())
    Console.WriteLine("message Ok");
else
    Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EagClient\EagClient");
```

### TurnTable
#### long
``` cs --region turntable1 --source-file .\src\EagTry\client.cs --project .\src\EagTry\EagTry.csproj
using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
    return 0;

var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest
{
    Angle = 0.0,
    Velocity = 100,
});

var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(5));
if (result.IsOk()) // No Timeout
{
    if (result.Message.IsOk())
        Console.WriteLine("TurnRelative Ok".LogInfo());
    else
        Console.WriteLine("TurnRelative Server Error".LogError());
}
else
    Console.WriteLine("TurnRelative Client TimeOut".LogError());
```
#### short
``` cs --region turntable2 --source-file .\src\EagTry\client.cs --project .\src\EagTry\EagTry.csproj
using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
    return 0;

if (await TableCommunication.SendAndWaitAsync<Table.Messages.TurnRelativeResponse>(
    new Table.Messages.TurnRelativeRequest
    {
        Angle = 0.0,
        Velocity = 0,
    }) is var result && result.IsMessageOk())
    Console.WriteLine("message Ok");
else
    Console.WriteLine(@"error infos in the logs C:\ProgramData\Rowa\Protocol\EagClient\EagClient");
```
