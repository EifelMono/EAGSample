var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var rootDir= MakeAbsolute(Directory("./"));

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
    DotNetCoreBuild("EagSample.sln", new DotNetCoreBuildSettings{
        Configuration= configuration
    });
});

Task("ExecClient")
.Does(() => {
    if (GetFiles($"{rootDir}/src/*client/**/*.exe").FirstOrDefault() is var exe && exe is object)
             System.Diagnostics.Process.Start(exe.FullPath);
});

Task("ExecServer")
.Does(() => {
    if (GetFiles($"{rootDir}/src/*server/**/*.exe").FirstOrDefault() is var exe && exe is object)
        System.Diagnostics.Process.Start(exe.FullPath);
});

Task("ExecPrograms")
.IsDependentOn("ExecServer")
.IsDependentOn("ExecClient")
.Does(() => {
});

Task("Default")
.IsDependentOn("NuGetCleanUp")
.IsDependentOn("RestoreSolution")
.IsDependentOn("BuildSolution")
.Does(() => {
   Information("Hello EagSample!");
});

try {
RunTarget(target);
}
catch(Exception ex)
{
    Error(ex);
    Console.ReadLine();
}
