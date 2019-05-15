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

``` cs --region setlights --source-file .\src\EagTry\client.cs --project .\src\EagTry\EagTry.csproj
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
if (result.Ok) // No Timeout
{
    if (result.Value.IsOk())
        Console.WriteLine("Client SetLight Ok".LogInfo());
    else
        Console.WriteLine("Client SetLight Server Error".LogError());
}
else
    Console.WriteLine("Client SetLight Client TimeOut".LogError());
```

### TurnTable

``` cs --region turntable --source-file .\src\EagTry\client.cs --project .\src\EagTry\EagTry.csproj
using var TableCommunication = new Table.ImageProcessingCommunication(4711, "127.0.0.1").DoRun();
if (!await TableCommunication.WaitConnectedAsync(TimeSpan.FromSeconds(2)))
    return 0;

var mark = TableCommunication.Send(new Table.Messages.TurnRelativeRequest
{
    Angle = 0.0,
    Velocity = 100,
});

var result = await mark.WaitAsync<Table.Messages.TurnRelativeResponse>(TimeSpan.FromSeconds(5));
if (result.Ok) // No Timeout
{
    if (result.Value.IsOk())
        Console.WriteLine("TurnRelative Ok".LogInfo());
    else
        Console.WriteLine("TurnRelative Server Error".LogError());
}
else
    Console.WriteLine("TurnRelative Client TimeOut".LogError()); 
```
