# EagSamples

## Voraussetzung

* [dotnet core](http://dot.net)
  * dotnet preview mindestens [dotnet v3.0.0-preview5](https://dotnet.microsoft.com/download/dotnet-core/3.0)
    * Warum 3.0 
      * kann exe erzeugen
        * später auch alles in eine Exe Datei
      * kein unötiges ausführen mit "dotnet name.dll"  
  * die Version wird hier auch in die global.json eingetragen
    * dotnet --version 
    * Zeigt die zum kompilieren verwendet dotnet core version an, und diese wird durch die global.json bestimmt.
* [Visual Studio 2019](https://visualstudio.microsoft.com/de/vs/)

## Infos

* globale nuget Pakete liegen jetzt unter [C:\users\\{name}\\.nuget\packages]() 
* csproj struktur ist anders

## Übersetzen

* Wenn alles installiert ist
  * mit Visual Studio 2019
  * oder aber mit [CakeBuild](https://cakebuild.net/) über Powershell 
      * ./build.ps ausführen
        * Hier werden die alten Pakete gelöscht
        * die neuen wieder hergestellt
        * und gebaut
      * Powershell benötigt folgende berechtigungen
        * ```Set-ExecutionPolicy -ExecutionPolicy RemoteSigned```
  * build.cmd ausführen (ruft ./build.ps auf)
    * run.cmd starten, startet client und server Programme




