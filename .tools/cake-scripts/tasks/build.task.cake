#load "../common/common.cake"


Task("Build")
.Description("Performs builds of the library projects.")
.Does(() =>
{
    DotNetBuildSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = CommonConfiguration.Restore;
    settings.Configuration = CommonConfiguration.Configuration;
    settings.OutputDirectory = System.IO.Path.Combine(CommonConfiguration.Output, CommonConfiguration.Configuration, "Build");

    DotNetBuild(MONOGAME_ASEPRITE_SLN_PATH, settings);
});