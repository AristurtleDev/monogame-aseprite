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

    ProcessDocuments();
    SanitizeDocs();
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

private void SanitizeDocs()
{
    //  Default Documentation seems to use a "rightwards san-serif arrow" (U+1F852) character in the generated output,
    //  but this is not rendering for some reason in Docusaurus correctly.  It could be that I'm developing on Mac and
    //  my Mac isn't rendering it, I dunno.  Either way, scan each document for this character and replace it with
    //  something else.
    //
    //  The markdown files generated are not very large, so using a simple ReadALlText -> Replace -> WriteAllText is
    //  "ok"... but if anyone looking at this has large file generation, you might want to consider instead doing chunk
    //  reads and replacements using streams.
    string[] files = System.IO.Directory.GetFiles($"{CommonConfiguration.Output}Documentation", "*.md", System.IO.SearchOption.AllDirectories);
    foreach(string file in files)
    {
        string text = System.IO.File.ReadAllText(file);
        text = text.Replace("&#129106;", "→", ignoreCase: true, culture: default);
        System.IO.File.WriteAllText(file, text);
    }
}

private void ProcessDocument()
{
    //  Now that the documents have been created, additional processing needs to occur to make them ready for use in
    //  Docusaurus.  We'll need to do the following
    //      -   Sanitize the documents for invalid characters and replace them
    //          -   DefaultDocumentation uses a "rightwards san-serif arrow" (U+1F825) character in the generated
    //              output.  This isn't rendering property for me, either something with Docusaurus, or more likely,
    //              it's something on my Mac since I develop on mac.  Either way, scan for this character and replace
    //              it with the '→' character
    //          -   Methods that use generics (e.g. Get<T>()) will output the <T> in the file. The filenames are already
    //              sanitized to make it "Get_T_", but inside the file, this is a no-go. This is because Docusaurus
    //              will parse all markdown files using JSX and JSX will assume the <T> is a JSX tag and throw an
    //              error because it doesn't know what that is. 
    //      -   Generate the yaml frontmatter
    //          -
}