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

``` cs --region setlights --source-file .\src\EAgTry\client.cs --project .\src\EAgTry\EAgTry.csproj 
```

### TurnTable

``` cs --region turntable --source-file .\src\EAgTry\client.cs --project .\src\EAgTry\EAgTry.csproj 
```
