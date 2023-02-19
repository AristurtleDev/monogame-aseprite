#load "../common/common.cake"

Task("Restore")
.Description("Restores the dependencies for each project.")
.Does(() =>
{
    //  By calling restore on the .sln we can restore all projects in the sln instead of calling restore
    //  on the projects individually.
    DotNetRestore(MONOGAME_ASEPRITE_SLN_PATH);
    
    //  Set that we restored so this task isn't called again by anything that might depend on it
    CommonConfiguration.RestoreWasPerformed();
});