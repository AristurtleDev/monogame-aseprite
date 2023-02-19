#load "../common/common.cake"

Task("Clean")
.Description("Cleans up the artifact outputs")
.Does(() =>
{
    //  By calling clean on the .sln we can clean all projects in the sln instead of calling clean
    //  on the projects individually.
    DotNetClean(MONOGAME_ASEPRITE_SLN_PATH);
    CleanDirectories(CommonConfiguration.Output);
});