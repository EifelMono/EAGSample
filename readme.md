# EagSamples

## Voraussetzung

* [dotnet core](http://dot.net)
  * dotnet preview mindestens [dotnet v3.0.0-preview5](https://dotnet.microsoft.com/download/dotnet-core/3.0)
    * Warum 3.0 
      * kann als Kompilat eine exe erzeugen
        * später auch alles in eine Exe Datei
      * Kein unötiges ausführen mit "dotnet name.dll"  
  * Die Version ist hier auch in der global.json eingetragen
    * dotnet --version 
    * Zeigt die zum kompilieren verwendete dotnet core version an, und diese wird durch die global.json bestimmt.
* [Visual Studio 2019](https://visualstudio.microsoft.com/de/vs/)

## Infos

* globale nuget Pakete liegen jetzt unter [C:\users\\{name}\\.nuget\packages]() 
* rowa nuget packete liegen in .\nuget
* nuget.config steuer bzw. erweitern wo die nugets herkommen 
* csproj sind im neuen Format

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

## weitere Infos

### Es gibt 4 Schnittstellen

* Position3D
* Size3D
* Table
* Ocr

In dem Beispiel ist nur die Table Schnittstelle implementiert mit folgenden 2 Funktionen

* SetLightsRequest
  * => SetLightsResponse

* TurnRelativeRequest
  * => TurnRelativeResponse (Wartzeit bis die antwort gesendet wird 2 Sekunden)








