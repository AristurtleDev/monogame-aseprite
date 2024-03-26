using System.Net.Mime;
using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Build.GitHubActions.Data;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Xml;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;

namespace MonoGame.Aseprite.Build;

public sealed class BuildContext : FrostingContext
{
    private const string DefaultRepositoryUrl = "https://github.com/AristurtleDev/monogame-aseprite";
    private const string DefaultBuildConfiguration = "Release";


    public string Version { get; }
    public string BuildOutput { get; }
    public string RepositoryUrl { get; }
    public string BuildConfiguration { get; }
    public bool IsPreRelease { get; }

    public DirectoryPath NuGetsDirectory { get; }
    public DotNetMSBuildSettings DotNetMSBuildSettings { get; }
    public DotNetPublishSettings DotNetPublishSettings { get; }
    public MSBuildSettings MSBuildSettings { get; }
    public MSBuildSettings MSPackSettings { get; }

    public BuildContext(ICakeContext context) : base(context)
    {
        RepositoryUrl = context.Argument(nameof(RepositoryUrl), DefaultRepositoryUrl);
        BuildConfiguration = context.Argument(nameof(BuildConfiguration), DefaultBuildConfiguration);
        BuildOutput = context.Argument(nameof(BuildOutput), ".artifacts");
        NuGetsDirectory = $"{BuildOutput}/NuGet/";
        IsPreRelease = context.Argument(nameof(IsPreRelease), false);

        Version = context.XmlPeek("Directory.Build.props", "/Project/PropertyGroup/Version");
        if (context.BuildSystem().IsRunningOnGitHubActions)
        {
            GitHubActionsWorkflowInfo workflow = context.BuildSystem().GitHubActions.Environment.Workflow;
            RepositoryUrl = $"https://github.com/{workflow.Repository}";

            if (!RepositoryUrl.Equals(DefaultRepositoryUrl, StringComparison.OrdinalIgnoreCase))
            {
                Version = $"{Version}-{workflow.RepositoryOwner}";
            }
            else if (workflow.RefType == GitHubActionsRefType.Branch && !workflow.RefName.Equals("refs/head/main", StringComparison.OrdinalIgnoreCase))
            {
                Version = $"{Version}-develop";
            }
            else if (IsPreRelease)
            {
                Version = $"{Version}-prerelease";
            }
            else
            {
                Version = $"{Version}";
            }
        }

        DotNetMSBuildSettings = new DotNetMSBuildSettings();
        DotNetMSBuildSettings.WithProperty(nameof(Version), Version);
        DotNetMSBuildSettings.WithProperty(nameof(RepositoryUrl), RepositoryUrl);

        MSBuildSettings = new MSBuildSettings
        {
            Verbosity = Verbosity.Minimal,
            Configuration = BuildConfiguration
        };
        MSBuildSettings.WithProperty(nameof(Version), Version);
        MSBuildSettings.WithProperty(nameof(RepositoryUrl), RepositoryUrl);

        MSPackSettings = new MSBuildSettings()
        {
            Verbosity = Verbosity.Minimal,
            Configuration = BuildConfiguration,
            Restore = true
        };
        MSPackSettings.WithProperty(nameof(Version), Version);
        MSPackSettings.WithProperty(nameof(RepositoryUrl), RepositoryUrl);
        MSPackSettings.WithProperty("OutputDirectory", NuGetsDirectory.FullPath);
        MSPackSettings.WithTarget("Pack");

        DotNetPublishSettings = new DotNetPublishSettings()
        {
            MSBuildSettings = DotNetMSBuildSettings,
            Verbosity = DotNetVerbosity.Minimal,
            Configuration = BuildConfiguration,
            SelfContained = false
        };

        Console.WriteLine($"{nameof(Version)}: {Version}");
        Console.WriteLine($"{nameof(RepositoryUrl)}: {RepositoryUrl}");
        Console.WriteLine($"{nameof(BuildConfiguration)}: {BuildConfiguration}");

        context.CreateDirectory(BuildOutput);
    }

}
