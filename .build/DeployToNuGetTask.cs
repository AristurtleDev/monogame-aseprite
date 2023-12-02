using System;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Frosting;

namespace BuildScripts;

[TaskName(nameof(DeployToNuGetTask))]
public sealed class DeployToNuGetTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) =>
        context.IsRunningOnGitHubActions &&
        context.IsTag &&
        !string.IsNullOrEmpty(context.RepositoryOwner) &&
        context.RepositoryOwner.Equals("AristurtleDev", StringComparison.InvariantCultureIgnoreCase);


    public override void Run(BuildContext context)
    {
        DotNetNuGetPushSettings pushSettings = new DotNetNuGetPushSettings()
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = context.NuGetAccessToken
        };

        context.DotNetNuGetPush($"{context.ArtifactsDirectory}/*.nupkg", pushSettings);
    }
}