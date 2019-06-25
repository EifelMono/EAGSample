# Interactive Client Sample EAGTry

## eagtrytest in Program.cs

```cs --source-file ./Program.cs --project ./EAGTry.csproj --region eagtrytest
```
The first run usually does not work! (Internal Server Error)

## runrowalogzip in Client.cs

RowaLog zips the data from the previous day's, but writes messages to the console.<br>
These messages interfere.<br>
Therefore, with this section of code, the program should be given the opportunity to do that.<br>
The other sections of the code do not run so long.

```cs --source-file ./Client.cs --project ./EAGTry.csproj --region rowalogzipcleanup
```



## setlights in Client.cs

```cs --source-file ./Client.cs --project ./EAGTry.csproj --region setlights
```
## turntable in Client.cs

```cs --source-file ./Client.cs --project ./EAGTry.csproj --region turntable
```

## turntable error with more details in Client.cs

```cs --source-file ./Client.cs --project ./EAGTry.csproj --region turntableerror
```

