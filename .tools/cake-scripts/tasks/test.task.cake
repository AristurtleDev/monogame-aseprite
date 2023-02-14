#load "../common/common.cake"

Task("Test")
.IsDependentOn("Build")
.Description("Calls `dotnet test` on the Monogame.Aseprite.sln solution file")
.Does(() =>
{
    DotNetTestSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = !CommonConfiguration.Restore;
    settings.Configuration = CommonConfiguration.Configuration;

    //  By calling restore on the .sln we can test all projects in the sln instead of calling test
    //  on the projects individually.
    DotNetTest(MONOGAME_ASEPRITE_SLN_PATH, settings);    
});