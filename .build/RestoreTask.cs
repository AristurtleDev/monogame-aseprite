using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Restore;
using Cake.Frosting;

namespace BuildScripts;

[TaskName(nameof(RestoreTask))]
public sealed class RestoreTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        DotNetRestoreSettings restoreSettings = new DotNetRestoreSettings()
        {
            Verbosity = DotNetVerbosity.Quiet
        };
        context.DotNetRestore(context.MonoGameAsepritePath, restoreSettings);
        context.DotNetRestore(context.MonoGameAsepriteContentPipelinePath, restoreSettings);
        context.DotNetRestore(context.MonoGameAsepriteTestsPath, restoreSettings);
    }
}