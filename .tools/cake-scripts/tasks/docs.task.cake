#load "../common/common.cake"

Task("Docs")
.IsDependentOn("Build")
.Description("Builds the documentation markdown to be used for the website API pages")
.Does(() =>
{
    //  Ensure that the DefaultDocument.Plugin has been build
    string pluginPath = BuildDefaultDocumentPlugin();

    GenerateDocs("MonoGame.Aseprite", MONOGAME_ASEPRITE_CSPROJ, pluginPath);
    GenerateDocs("MonoGame.Aseprite.Common", MONOGAME_ASEPRITE_COMMON_CSPROJ, pluginPath);
    GenerateDocs("MonoGame.Aseprite.Content.Pipeline", MONOGAME_ASEPRITE_CONTENT_PIPELINE_CSPROJ, pluginPath);
});

private string BuildDefaultDocumentPlugin()
{
    DotNetBuildSettings settings = new();
    settings.MSBuildSettings = CommonConfiguration.DotNetMsBuildSettings;
    settings.NoRestore = false;
    settings.Configuration = CommonConfiguration.Configuration;
    settings.OutputDirectory = System.IO.Path.Combine(CommonConfiguration.Output, CommonConfiguration.Configuration, "DefaultDocument.Plugin");

    DotNetBuild(DEFAULT_DOC_PLUGIN_SLN_PATH, settings);
    return settings.OutputDirectory.FullPath;
}

private void GenerateDocs(string assemblyName, string projPath, string pluginPath)
{
    DotNetToolSettings settings = new();

    string assemblyFilePath = $"{CommonConfiguration.Output}{CommonConfiguration.Configuration}/Build/{assemblyName}.dll";
    string documentationFilePath = $"{CommonConfiguration.Output}{CommonConfiguration.Configuration}/Build/{assemblyName}.xml";
    string outputPath = $"{CommonConfiguration.Output}Documentation/{assemblyName}";

    settings.ArgumentCustomization = builder =>
    {
        builder.AppendSwitchQuoted("--AssemblyFilePath", assemblyFilePath);
        builder.AppendSwitchQuoted("--DocumentationFilePath", documentationFilePath);
        builder.AppendSwitchQuoted("--ProjectDirectoryPath", projPath);
        builder.AppendSwitchQuoted("--OutputDirectoryPath", outputPath);
        builder.AppendSwitchQuoted("--ConfigurationFilePath", "./DefaultDocumentation.json");
        builder.AppendSwitchQuoted("--AssemblyPageName", assemblyName);
        builder.AppendSwitchQuoted("--Plugins", $"{pluginPath}/DefaultDocumentation.Plugin.dll");
        return builder;
    };

    DotNetTool("defaultdocumentation", settings);
}