#nullable enable

//////////////////////////////////////////////////////////////////////
/// Arguments
//////////////////////////////////////////////////////////////////////
private string _target = Argument("target", "All");
private bool _incrementBuild = HasArgument("increment-build");
private string _outputDirectory = Argument("output-directory", "./Artifacts");
private string _configuration = Argument("configuration", "Debug");
private bool _noRestore = HasArgument("no-restore") ? true : false;
private bool _noClean = HasArgument("no-clean") ? true : false;

//////////////////////////////////////////////////////////////////////
/// Setup
//////////////////////////////////////////////////////////////////////
private readonly string _monogameAsepriteProjectPath = "./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj";
private readonly string _monogameAsepriteCommonProjectPath = "./source/MonoGame.Aseprite.Common/MonoGame.Aseprite.Common.csproj";
private readonly string _monogameAsepriteContentPipelineProjectPath = "./source/Monogame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj";
private readonly string _testProjectPath = "./tests/MonoGame.Aseprite.Common.Tests/MonoGame.Aseprite.Common.Tests.csproj";
private string _version;
private string _githubRepositoryUrl;
DotNetMSBuildSettings _dotnetMsBuildSettings;

//////////////////////////////////////////////////////////////////////
// Helpers
//////////////////////////////////////////////////////////////////////
private void ParseVersion()
{
    string xmlValue = XmlPeek("./source/Directory.Build.props", "//Version");
    Version assemblyVersion = new(xmlValue);

    if(_incrementBuild)
    {
        assemblyVersion = new(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build + 1);
        XmlPoke("./source/Directory.Build.props", "//Version", assemblyVersion.ToString());
    }

    string? suffix = ParseSuffix();

    _version = $"{assemblyVersion}{suffix}";
    Console.WriteLine(_version);
}

private string? ParseSuffix()
{
    if(HasEnvironmentVariable("GITHUB_ACTIONS"))
    {
        string? repo = EnvironmentVariable("GITHUB_REPOSITORY");

        if(repo != "AristurtleDev/monogame-aseprite")
        {
            string? repoOwner = EnvironmentVariable("GITHUB_REPOSITORY_OWNER");
            return $"-{repoOwner}";
        }

        string? ghRefType = EnvironmentVariable("GITHUB_REF_TYPE");
        string? ghRef = EnvironmentVariable("GITHUB_REF");

        if(ghRefType == "branch" && ghRef != "refs/head/stable")
        {
            return "-develop";
        }
    }

    if(EnvironmentVariable("BRANCH_NAME", string.Empty) != "stable")
    {
        return "-develop";
    }

    return null;
}

private void InitializeBuildSettings()
{
    _dotnetMsBuildSettings = new();
    _dotnetMsBuildSettings.WithProperty("Version", _version);
}

private void ParseRepositoryUrl()
{
    _githubRepositoryUrl = "https://github.com/{0}";

    string repositoryPath = "AristurtleDev/monogame-aseprite";

    if(HasEnvironmentVariable("GITHUB_ACTIONS"))
    {
        repositoryPath = EnvironmentVariable("GITHUB_REPOSITORY", repositoryPath);
    }

    _githubRepositoryUrl = string.Format(_githubRepositoryUrl, repositoryPath);
    Console.WriteLine($"Repository URL: {_githubRepositoryUrl}");
}


private void CleanTask()
{
    DotNetClean(_monogameAsepriteProjectPath);
    CleanDirectories(_outputDirectory);
}

private void RestoreTask()
{
    DotNetRestore(_monogameAsepriteProjectPath);
    DotNetRestore(_testProjectPath);
    _noRestore = true;
}

private void BuildTask()
{
    DotNetBuildSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = _noRestore;
    settings.Configuration = _configuration;
    settings.OutputDirectory = System.IO.Path.Combine(_outputDirectory, _configuration, "Build");
        
    DotNetBuild(_monogameAsepriteProjectPath, settings);
}

private void TestTask()
{
    DotNetTestSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = _noRestore;
    settings.Configuration = _configuration;    

    DotNetTest(_testProjectPath, settings);
}

private void PackTask()
{
    DotNetPackSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = _noRestore;
    settings.Configuration = _configuration;
    settings.OutputDirectory = System.IO.Path.Combine(_outputDirectory, _configuration, "NuGet");

    DotNetPack(_monogameAsepriteProjectPath, settings);
}

private void DocsTask()
{
    //  Remove all existing documentation files
    CleanDirectory("./Artifacts/Documentation");

    //  Turn document generation on for each project
    XmlPoke(_monogameAsepriteProjectPath, "//GenerateDocumentationFile", "true");
    XmlPoke(_monogameAsepriteCommonProjectPath, "//GenerateDocumentationFile", "true");
    XmlPoke(_monogameAsepriteContentPipelineProjectPath, "//GenerateDocumentationFile", "true");

    //  Restore each project
    DotNetRestoreSettings restoreSettings = new();
    restoreSettings.MSBuildSettings = _dotnetMsBuildSettings;
    DotNetRestore(_monogameAsepriteProjectPath, restoreSettings);
    DotNetRestore(_monogameAsepriteCommonProjectPath, restoreSettings);
    DotNetRestore(_monogameAsepriteContentPipelineProjectPath, restoreSettings);

    //  Build each project.  Since documentation generation was enabled, each project should create the xml 
    //  documentation, then the DefaultDocumentation NuGet Package in each project should transform them into XML and 
    //  output them in the Artifacts/Documentation directory. Each project has it's own DefaultDocumentation.json file 
    //  that specific, among other things, the output directory, so if it's generating somewhere other than 
    //  Artifacts/Documentation, check those json configs first.
    DotNetBuildSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = true;
    settings.Configuration = _configuration;

    DotNetBuild(_monogameAsepriteProjectPath, settings);
    DotNetBuild(_monogameAsepriteCommonProjectPath, settings);
    DotNetBuild(_monogameAsepriteContentPipelineProjectPath, settings);

    //  Default Documentation seems to use a "rightwards san-serif arrow" (U+1F852) character in the generated output,
    //  but this is not rendering for some reason in Docusaurus correctly.  It could be that I'm developing on Mac and
    //  my Mac isn't rendering it, I dunno.  Either way, scan each document for this character and replace it with
    //  something else.
    //  The markdown files generated are not very large, so using a simple ReadAllText -> Replace -> WriteAllText
    //  is fine.  
    string[] files = System.IO.Directory.GetFiles("./Artifacts/Documentation", "*.md", System.IO.SearchOption.AllDirectories);
    foreach(string file in files)
    {
        string text = System.IO.File.ReadAllText(file);
        text = text.Replace("&#129106;", "➡️", ignoreCase: true, culture: default);
        System.IO.File.WriteAllText(file, text);
    }

    //  Turn document generation off for each project now that we're finished
    XmlPoke(_monogameAsepriteProjectPath, "//GenerateDocumentationFile", "false");
    XmlPoke(_monogameAsepriteCommonProjectPath, "//GenerateDocumentationFile", "false");
    XmlPoke(_monogameAsepriteContentPipelineProjectPath, "//GenerateDocumentationFile", "false");
}

//////////////////////////////////////////////////////////////////////
// Tasks
//////////////////////////////////////////////////////////////////////
Setup((context) =>
{
    ParseVersion();
    ParseRepositoryUrl();
    InitializeBuildSettings();
});

Task("Clean")
.Description("Cleans up existing artifacts for both the MonoGame.Aseprite project and the output from these cake scripts")
.WithCriteria(!_noClean)
.Does(CleanTask);

Task("Restore")
.Description("Performs a dotnet restore on the MonoGame.Aseprite project. Does not run when --no-restore specified")
.WithCriteria(!_noRestore)
.Does(RestoreTask);


Task("Build")
.Description("Builds the MonoGame.Aseprite project")
.Does(BuildTask);

Task("Test")
.Description("Executes the tests for the MonoGame.Aseprite project")
.IsDependentOn("Build")
.Does(TestTask);

Task("Pack")
.Description("Performs the dotnet pack command for the MonoGame.Aseprite project")
.IsDependentOn("Build").
Does(PackTask);

Task("Docs")
.Description("Generates the Markdown documentation for each projects")
.Does(DocsTask);


Task("All").IsDependentOn("Clean")
           .IsDependentOn("Restore")
           .IsDependentOn("Build")
           .IsDependentOn("Test")
           .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(_target);