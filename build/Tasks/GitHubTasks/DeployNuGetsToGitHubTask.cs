using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Frosting;


namespace MonoGame.Aseprite.Build;

[TaskName("DeployNuGetsToGithub")]
[IsDependentOn(typeof(DownloadArtifactsTask))]
public sealed class DeployNuGetsToGitHubTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.BuildSystem().IsRunningOnGitHubActions;

    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        string repositoryOwner = context.GitHubActions().Environment.Workflow.RepositoryOwner;
        DotNetNuGetPushSettings settings = new DotNetNuGetPushSettings()
        {
            ApiKey = context.EnvironmentVariable("GITHUB_TOKEN"),
            Source = $"https://nuget.pkg.github.com/{repositoryOwner}/index.json"
        };

        context.DotNetNuGetPush("nugets/*.nupkg", settings);
    }
}
