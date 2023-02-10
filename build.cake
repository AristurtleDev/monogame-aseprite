
#nullable enable

private record CustomVersion(Version AssemblyVersion, string? Meta = default)
{
    public override string ToString()
    {
        string value = AssemblyVersion.ToString();

        if(!string.IsNullOrEmpty(Meta))
        {
            value += $"-{Meta}";
        }

        return value;
    }
}


//  -----------------------------------------------------------------
//  Arguments
//  -----------------------------------------------------------------
private bool _incrementVersion = Argument("increment-version", true);
private string _target = Argument("target", "Build");
private string _configuration = Argument("configuration", "Debug");
private string _repositoryUrl = Argument("repository-url", "https://github.com/AristurtleDev/monogame-aseprite");


//  -----------------------------------------------------------------
//  Prep
//  -----------------------------------------------------------------

private CustomVersion _version;

DotNetMSBuildSettings dnMsBuildSettings;
DotNetBuildSettings dnBuildSettings;
DotNetPackSettings dnPackSettings;

//  -----------------------------------------------------------------
//  Helpers
//  -----------------------------------------------------------------
private void ParseVersion()
{
    string versionValue = XmlPeek("./source/Directory.Build.props", "//Version");
    Version assemblyVersion = new(versionValue);

    if(_incrementVersion)
    {
        assemblyVersion = new(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build + 1);
        XmlPoke("./source/Directory.Build.props", "//Version", assemblyVersion.ToString());
    }

    _version = new(assemblyVersion);

    //  Are we in the GitHub Actions environment?
    if(!string.IsNullOrEmpty(EnvironmentVariable("GITHUB_ACTIONS")))
    {
        string ghRepo = EnvironmentVariable("GITHUB_REPOSITORY").ToLower();

        //  Are we in a forked repo or the source repo
        if(ghRepo == "aristurtledev/monogame-aseprite")
        {
            //  Source repo.
            string ghRefType = EnvironmentVariable("GITHUB_REF_TYPE");
            string ghRef = EnvironmentVariable("GITHUB_REF");

            //  If we're not on the stable branch, add develop meta tag to package version.
            if(ghRefType == "branch" && ghRef != "refs/head/stable")
            {
                _version = _version with { Meta = "develop" };
            }
        }
        else
        {
            //  Forked repo, so add their name as the meta tag of the package version
            string repoOwner = EnvironmentVariable("GITHUB_REPOSITORY_OWNER");
            _version = _version with { Meta = repoOwner };
        }

        _repositoryUrl = $"https://github.com/{ghRepo}";
    }

    Console.WriteLine($"Assembly Version: {assemblyVersion}");
    Console.WriteLine($"Package Version: {_version}");

}

//  -----------------------------------------------------------------
//  Tasks
//  -----------------------------------------------------------------

Task("Prep").Does(() =>
{

    ParseVersion();

    dnMsBuildSettings = new DotNetMSBuildSettings();
    dnMsBuildSettings.WithProperty("Version", _version.ToString());

    dnBuildSettings = new DotNetBuildSettings();
    dnBuildSettings.MSBuildSettings = dnMsBuildSettings;
    dnBuildSettings.Configuration = _configuration;
    dnBuildSettings.OutputDirectory = "Artifacts/Build";

    dnPackSettings = new DotNetPackSettings();
    dnPackSettings.MSBuildSettings = dnMsBuildSettings;
    dnPackSettings.Configuration = _configuration;
    dnPackSettings.OutputDirectory = "Artifacts/NuGet";
});


Task("Build").IsDependentOn("Prep").Does(() =>
{
    DotNetRestore("./source/MonoGame.Aseprite/Monogame.Aseprite.csproj");
    DotNetBuild("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", dnBuildSettings);
});

Task("Pack").IsDependentOn("Prep").Does(() =>
{
    DotNetPack("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", dnPackSettings);
});


Task("All").IsDependentOn("Prep")
           .IsDependentOn("Build")
           .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(_target);
