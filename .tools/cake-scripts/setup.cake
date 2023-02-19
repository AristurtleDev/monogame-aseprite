#load "./common/common.config.cake"
#load "./common/common.cake"

Setup((context) =>
{
    if(CommonConfiguration.GenerateDocs || CommonConfiguration.Target == "Docs")
    {
        ToggleGenerateDocuments(context, MONOGAME_ASEPRITE_CSPROJ, true);
    }

    string config =
    $"""
    ----- Arguments ------
    Clean: {CommonConfiguration.Clean}
    Configuration: {CommonConfiguration.Configuration}
    GenerateDocs: {CommonConfiguration.GenerateDocs}
    Output: {CommonConfiguration.Output}
    Restore: {CommonConfiguration.Restore}
    Target: {CommonConfiguration.Target}

    ----- Generated Values -----
    IncrementBuild: {CommonConfiguration.IncrementBuild}
    RepositoryUrl: {CommonConfiguration.RepositoryUrl}
    Version: {CommonConfiguration.Version}
    VersionSuffix: {CommonConfiguration.VersionSuffix}
    """;

    Console.WriteLine(config);
});
