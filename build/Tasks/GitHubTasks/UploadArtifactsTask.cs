using Cake.Common.Build;
using Cake.Core.IO;
using Cake.Frosting;

namespace MonoGame.Aseprite.Build;

[TaskName("UploadArtifacts")]
public sealed class UploadArtifactsTask : AsyncFrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context) => context.BuildSystem().IsRunningOnGitHubActions;

    public override async Task RunAsync(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        DirectoryPath path = context.NuGetsDirectory.FullPath;
        string artifactName = "nugets";
        await context.GitHubActions()
                     .Commands
                     .UploadArtifact(path, artifactName)
                     .ConfigureAwait(true);
    }
}
