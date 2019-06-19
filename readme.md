# EagSamples

#### doc.md [c#](./doc.md)
#### try.md [Try.Dot.Net local](./try.md)
#### more [confluence](https://bdrowa.atlassian.net/wiki/spaces/RP/pages/626590139/Software+Dokumente)

## Voraussetzung

* [dotnet core](http://dot.net)
  * dotnet preview mindestens [dotnet v3.0.0-preview6](https://dotnet.microsoft.com/download/dotnet-core/3.0)
    * Warum 3.0 
      * kann als Kompilat eine exe erzeugen
        * [Making a tiny .NET Core 3.0 entirely self-contained single executable](https://www.hanselman.com/blog/MakingATinyNETCore30EntirelySelfcontainedSingleExecutable.aspx)
      * Kein unötiges ausführen mit "dotnet name.dll"  
  * Die Version ist hier auch in der global.json eingetragen
    * dotnet --version 
    * Zeigt die zum kompilieren verwendete dotnet core version an, und diese wird durch die global.json bestimmt.
* [Visual Studio 2019](https://visualstudio.microsoft.com/de/vs/)

## Infos

* nuget Pakete befinden sich auf [myget](https://www.myget.org/F/rowa/auth/28de5fa5-9592-451d-a8e9-e769859e2061/api/v3/index.json) 
* Die source ist in nuget.config eingetagen 
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

weitere [Documentation](https://github.com/EifelMono/EAGSample/blob/master/doc.md) hier!
