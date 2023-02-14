#load "../common/common.cake"

Task("Restore")
.Description("Calls `dotnet restore` on the Monogame.Aseprite.sln solution file and development dependencies")
.Does(() =>
{
    //  By calling restore on the .sln we can restore all projects in the sln instead of calling restore
    //  on the projects individually.
    DotNetRestore(MONOGAME_ASEPRITE_SLN_PATH);
    DotNetRestore(DEFAULT_DOC_PLUGIN_SLN_PATH);
    
    //  Set that we restored so this task isn't called again by anything that might depend on it
    CommonConfiguration.RestoreWasPerformed();
});