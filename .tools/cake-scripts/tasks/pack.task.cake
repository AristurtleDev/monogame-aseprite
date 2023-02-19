#load "../common/common.cake"

Task("Pack")
.IsDependentOn("Test")
.Description("Creates the NuGet files to be uploaded.")
.Does(() =>
{
    string outDir = System.IO.Path.Combine(CommonConfiguration.Output, CommonConfiguration.Configuration, "NuGet");
    DotNetPackSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = !CommonConfiguration.Restore;
    settings.Configuration = CommonConfiguration.Configuration;
    settings.OutputDirectory = outDir;

    //  Call pack on the projects themselves, not the solution, otherwise, we'll get a pack of the test project.
    DotNetPack(MONOGAME_ASEPRITE_CSPROJ, settings);
    DotNetPack(MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ, settings);

    //  Clean the local nuget feed directory used by the example projects
    CleanDirectories("../examples/local-nuget-feed");

    string nupkg = System.IO.Path.Combine(outDir, $"MonoGame.Aseprite.{CommonConfiguration.Version}.nupkg");
    ProcessSettings processSettings = new();
    processSettings.WithArguments(builder => 
    {
        builder.Append($"add {nupkg} -Source ../examples/local-nuget-feed");
    });
    StartProcess("nuget", processSettings);
    
});