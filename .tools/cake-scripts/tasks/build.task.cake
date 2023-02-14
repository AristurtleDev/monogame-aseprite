#load "../common/common.cake"


Task("Build")
.Description("Calls `dotnet build` on the Monogame.Aseprite.sln solution file")
.Does(() =>
{
    DotNetBuildSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = CommonConfiguration.Restore;
    settings.Configuration = CommonConfiguration.Configuration;
    settings.OutputDirectory = System.IO.Path.Combine(CommonConfiguration.Output, CommonConfiguration.Configuration, "Build");

    //  By calling build on the .sln we can clean all projects in the sln instead of calling build
    //  on the projects individually.
    DotNetBuild(MONOGAME_ASEPRITE_SLN_PATH, settings);
});