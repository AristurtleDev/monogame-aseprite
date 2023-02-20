#tool "dotnet:?package=GitVersion.Tool&version=5.10.3"

string target = Argument(nameof(target), "Default");
string configuration = Argument(nameof(configuration), "Release");
string version = string.Empty;
string[] projects = new string[]
{
    "./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj",
    "./source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj"
};

Task("Clean")
.Does(() => 
{
    DotNetClean("./source/MonoGame.Aseprite.sln");
    DotNetClean("./tests/MonoGame.Aseprite.Tests.sln");
    CleanDirectory("./.artifacts");
});

Task("Restore")
.IsDependentOn("Clean")
.Does(() => 
{
    DotNetRestore("./source/MonoGame.Aseprite.sln");
    DotNetRestore("./tests/MonoGame.Aseprite.Tests.sln");
});

Task("Version")
.Does(() => 
{
    GitVersionSettings settings = new();
    settings.UpdateAssemblyInfo = true;

    GitVersion gitVersion = GitVersion(settings);
    version = gitVersion.NuGetVersionV2;
    Information($"Version: {version}");
});

Task("Build")
.IsDependentOn("Version")
.Does(() => 
{
    DotNetMSBuildSettings msBuildSettings = new();
    msBuildSettings.WithProperty("Version", version);

    DotNetBuildSettings buildSettings = new();
    buildSettings.Configuration = configuration;
    buildSettings.MSBuildSettings = msBuildSettings;

    
    DotNetBuild("./source/MonoGame.Aseprite.sln", buildSettings);
    DotNetBuild("./tests/MonoGame.Aseprite.Tests.sln", buildSettings);
});

Task("Test")
.IsDependentOn("Build")
.Does(() => 
{
    DotNetTestSettings testSettings = new();
    testSettings.Configuration = configuration;
    testSettings.NoBuild = true;

    DotNetTest("./tests/MonoGame.Aseprite.Tests.sln", testSettings);
});

Task("Pack")
.IsDependentOn("Test")
.Does(() =>
{
    DotNetMSBuildSettings msBuildSettings = new();
    msBuildSettings.WithProperty("PackageVersion", version)
                   .WithProperty("Version", version);

    DotNetPackSettings packSettings = new();
    packSettings.Configuration = configuration;
    packSettings.OutputDirectory = "./.artifacts";
    packSettings.NoBuild = true;
    packSettings.NoRestore = true;
    packSettings.MSBuildSettings = msBuildSettings;

    DotNetPack("./source/MonoGame.Aseprite.sln", packSettings);
});

Task("PublishNuGet")
.IsDependentOn("Pack")
.Does(context =>
{
    if(BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    {
        FilePath nupkgPath = $"./.artifacts/MonoGame.Aseprite.{version}.nupkg";

        Information("Publishing {0} to NuGet...", nupkgPath.GetFilename().FullPath);

        DotNetNuGetPushSettings pushSettings = new();
        pushSettings.ApiKey = context.EnvironmentVariable("NUGET_API_KEY");
        pushSettings.Source = "https://api.nuget.org/v3/index.json";

        DotNetNuGetPush(nupkgPath, pushSettings);
        
    }
});

Task("PublishGitHub")
.IsDependentOn("Pack")
.Does(context =>
{
    if(BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    {
        FilePath nupkgPath = $"./.artifacts/MonoGame.Aseprite.{version}.nupkg";

        Information("Publishing {0} to GitHub...", nupkgPath.GetFilename().FullPath);

        DotNetNuGetPushSettings pushSettings = new();
        pushSettings.ApiKey = context.EnvironmentVariable("GITHUB_TOKEN");
        pushSettings.Source = "https://nuget.pkg.github.com/aristurtledev/index.json";

        DotNetNuGetPush(nupkgPath, pushSettings);
        
    }
});

Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Version")
.IsDependentOn("Build")
.IsDependentOn("Test");

Task("Publish")
.IsDependentOn("PublishNuGet")
.IsDependentOn("PublishGitHub");

RunTarget(target);