#load "./common/common.cake"
#load "./common/common.config.cake"

Teardown((context) =>
{
    if(CommonConfiguration.GenerateDocs || CommonConfiguration.Target == "Docs")
    {
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_CSPROJ, false);
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_COMMON_CSPROJ, false);
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ, false);
    }
});
