#nullable enable

//////////////////////////////////////////////////////////////////////
// Arguments
//////////////////////////////////////////////////////////////////////
private string _target = Argument("target", "All");
private bool _incrementBuild = HasArgument("increment-build");
private string _outputDirectory = Argument("output-directory", "./Artifacts");
private string _configuration = Argument("configuration", "Debug");
private bool _noRestore = HasArgument("no-restore") ? true : false;
private bool _noClean = HasArgument("no-clean") ? true : false;

//////////////////////////////////////////////////////////////////////
// Setup
//////////////////////////////////////////////////////////////////////
private readonly string _projectPath = "./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj";
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
.Does(() =>
{
    DotNetClean(_projectPath);
    CleanDirectories(_outputDirectory);
});

Task("Restore")
.Description("Performs a dotnet restore on the MonoGame.Aseprite project. Does not run when --no-restore specified")
.WithCriteria(!_noRestore)
.Does(() =>
{
    DotNetRestore(_projectPath);
    DotNetRestore(_testProjectPath);
    _noRestore = true;
});


Task("Build")
.Description("Builds the MonoGame.Aseprite project")
.Does(() =>
{
    DotNetBuildSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = _noRestore;
    settings.Configuration = _configuration;
    settings.OutputDirectory = System.IO.Path.Combine(_outputDirectory, _configuration, "Build");
        
    DotNetBuild(_projectPath, settings);
});

Task("Test")
.Description("Executes the tests for the MonoGame.Aseprite project")
.IsDependentOn("Build")
.Does(() =>
{
    DotNetTestSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = _noRestore;
    settings.Configuration = _configuration;    

    DotNetTest(_testProjectPath, settings);
});

Task("Pack")
.Description("Performs the dotnet pack command for the MonoGame.Aseprite project")
.IsDependentOn("Build").Does(() =>
{
    DotNetPackSettings settings = new();
    settings.MSBuildSettings = _dotnetMsBuildSettings;
    settings.NoRestore = _noRestore;
    settings.Configuration = _configuration;
    settings.OutputDirectory = System.IO.Path.Combine(_outputDirectory, _configuration, "NuGet");

    DotNetPack(_projectPath, settings);
});


Task("All").IsDependentOn("Clean")
           .IsDependentOn("Restore")
           .IsDependentOn("Build")
           .IsDependentOn("Test")
           .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(_target);