#load "../common/common.cake"

Task("Pack")
.IsDependentOn("Build")
.Description("Calls `dotnet pack` on the Monogame.Aseprite.csproj project file")
.Does(() =>
{
    DotNetPackSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = !CommonConfiguration.Restore;
    settings.Configuration = CommonConfiguration.Configuration;
    settings.OutputDirectory = System.IO.Path.Combine(CommonConfiguration.Output, CommonConfiguration.Configuration, "NuGet");

    //  Call pack on the project itself, not the solution.
    DotNetPack(MONOGAME_ASEPRITE_CSPROJ, settings);
});