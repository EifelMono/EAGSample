var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");


Task("DeleteProLogGlobalPackages")
.Does(() => {
   var globalPackageDir= $"C:/users/{System.Environment.UserName}/.nuget/packages/";
   if (!DirectoryExists(globalPackageDir))
        Error($"{globalPackageDir} does not exist!");
   Information($"DeleteProLogGlobalPackages:{globalPackageDir}");
   foreach(var dir in GetDirectories(globalPackageDir+ "*.*"))
   {
       if (dir.GetDirectoryName().ToLower().StartsWith("prolog")
         || dir.GetDirectoryName().ToLower().StartsWith("rowalog"))
         {
            try {
                DeleteDirectory(dir, new DeleteDirectorySettings { Recursive= true});
                Information($"Deleted: {dir}");
            }
            catch {
                Error($"Not Deleted: {dir}");
            }
         }
   }
});

Task("DeleteBinAndObj")
.Does(() => {
    foreach(var name in new string[] {"bin", "obj"} )
    {
        var rootDir= MakeAbsolute(Directory("./"));
        Information($"Check for folders \"{name}\" in {rootDir}");
        foreach(var dir in GetDirectories($"{rootDir}/src/**/{name}"))
        {
            try {
                DeleteDirectory(dir, new DeleteDirectorySettings { Recursive= true});
                Information($"Deleted: {dir}");
            }
            catch {
                Error($"Not Deleted: {dir}");
            }
        }
    }
});

Task("NuGetCleanUp")
.IsDependentOn("DeleteProLogGlobalPackages")
.IsDependentOn("DeleteBinAndObj")
.Does(() => {
});

Task("RestoreSolution")
.Does(() => {
    DotNetCoreRestore("EagSample.sln");
});

Task("BuildSolution")
.Does(() => {
    DotNetCoreRestore("EagSample.sln");
});

Task("Default")
.IsDependentOn("NuGetCleanUp")
.IsDependentOn("RestoreSolution")
.IsDependentOn("BuildSolution")
.Does(() => {
   Information("Hello Cake!");
});

try {
RunTarget(target);
}
catch(Exception ex)
{
    Error(ex);
}
