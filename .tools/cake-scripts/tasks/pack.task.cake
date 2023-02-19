#load "../common/common.cake"

Task("Pack")
.IsDependentOn("Test")
.Description("Creates the NuGet files to be uploaded.")
.Does(() =>
{
    DotNetPackSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = !CommonConfiguration.Restore;
    settings.Configuration = CommonConfiguration.Configuration;
    settings.OutputDirectory = System.IO.Path.Combine(CommonConfiguration.Output, CommonConfiguration.Configuration, "NuGet");

    //  Call pack on the projects themselves, not the solution, otherwise, we'll get a pack of the test project.
    DotNetPack(MONOGAME_ASEPRITE_CSPROJ, settings);
    DotNetPack(MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ, settings);
});