#nullable enable

public static class CommonConfiguration
{
    //  Default values for additional configs
    private static string _version = "4.0.0";
    private static string? _versionSuffix = default;
    private static string _repositoryUrl = "https://github.com/AristurtleDev/monogame-aseprite";
    private static DotNetMSBuildSettings _dnMsBuildSettings = new();

    //  Default values for configs passed in through cli arguments
    private static string _target = "All";
    private static bool _generateDocs = false;
    private static bool _incrementBuild = false;
    private static string _output = "../Artifacts/";
    private static bool _restore = true;
    private static bool _clean = true;
    private static string _configuration = "Debug";

    //  Public accessors to configs passed in through cli arguments
    public static string Target => _target;
    public static bool GenerateDocs => _generateDocs;
    public static bool IncrementBuild => _incrementBuild;
    public static string Output => _output;
    public static bool Restore => _restore;
    public static bool Clean => _clean;
    public static string Configuration => _configuration;

    //  Additional configs
    public static string Version => _version;
    public static string? VersionSuffix => _versionSuffix;
    public static string RepositoryUrl => _repositoryUrl;
    public static DotNetMSBuildSettings DotNetMsBuildSettings => _dnMsBuildSettings;


    public static void ParseArguments(ICakeContext context)
    {
        _target = context.Argument(nameof(Target), _target);
        _generateDocs = context.Argument(nameof(GenerateDocs), _generateDocs);
        _incrementBuild = context.Argument(nameof(IncrementBuild), _incrementBuild);
        _output = context.Argument(nameof(Output), _output);
        _restore = context.Argument(nameof(Restore), _restore);
        _clean = context.Argument(nameof(Clean), _clean);
        _configuration = context.Argument(nameof(Configuration), _configuration);
    }

    public static void ParseVersion(ICakeContext context)
    {
        const string PROPS_PATH = "../source/Directory.Build.props";
        string xmlValue = context.XmlPeek(PROPS_PATH, "//Version");
        Version assemblyVersion = new(xmlValue);

        if(IncrementBuild)
        {
            assemblyVersion = new(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build + 1);
            context.XmlPoke(PROPS_PATH, "//Version", assemblyVersion.ToString());
        }

        _version = assemblyVersion.ToString();

        if(context.HasEnvironmentVariable("GITHUB_ACTIONS"))
        {
            string? repo = context.EnvironmentVariable("GITHUB_REPOSITORY");

            if(repo != "AristurtleDev/monogame-aseprite")
            {
                string? repoOwner = context.EnvironmentVariable("GITHUB_REPOSITORY_OWNER");
                _versionSuffix = $"-{repoOwner}";
            }
            else
            {
                string? ghRefType = context.EnvironmentVariable("GITHUB_REF_TYPE");
                string? ghRef = context.EnvironmentVariable("GITHUB_REF");

                if(ghRefType == "branch" && ghRef != "refs/head/stable")
                {
                    _versionSuffix = "-develop";
                }
            }
        }
        else if(context.EnvironmentVariable("BRANCH_NAME", string.Empty) != "stable")
        {
            _versionSuffix = "-develop";
        }

        Console.WriteLine($"Version: {_version}{_versionSuffix}");
    }

    public static void ParseRepositoryUrl(ICakeContext context)
    {
        string urlFormat = "https://github.com/{0}";

        string path = "AristurtleDev/monogame-aseprite";

        if(context.HasEnvironmentVariable("GITHUB_ACTIONS"))
        {
            path = context.EnvironmentVariable("GITHUB_REPOSITORY", path);
        }

        _repositoryUrl = string.Format(urlFormat, path);
        Console.WriteLine($"Repository: {_repositoryUrl}");
    }

    public static void InitializeBuildSettings()
    {
        _dnMsBuildSettings.WithProperty("Version", _version);
    }

    public static void RestoreWasPerformed() => _restore = false;
}