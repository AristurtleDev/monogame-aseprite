using Cake.Common.Build;
using Cake.Common.IO;
using Cake.Frosting;

namespace MonoGame.Aseprite.Build;

[TaskName("DownloadArtifacts")]
public sealed class DownloadArtifactsTask : AsyncFrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.BuildSystem().IsRunningOnGitHubActions;

    public override async Task RunAsync(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        string path = "nugets";
        context.CreateDirectory(path);
        await context.GitHubActions()
                     .Commands
                     .DownloadArtifact(path, path)
                     .ConfigureAwait(true);
    }
}
