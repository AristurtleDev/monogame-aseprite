#load "./common/common.config.cake"
#load "./common/common.cake"

Setup((context) =>
{
    if(CommonConfiguration.GenerateDocs || CommonConfiguration.Target == "Docs")
    {
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_CSPROJ, true);
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_COMMON_CSPROJ, true);
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ, true);
    }
});
