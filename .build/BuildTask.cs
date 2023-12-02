using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Frosting;

namespace BuildScripts;

[TaskName(nameof(BuildTask))]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        DotNetMSBuildSettings msBuildSettings = new DotNetMSBuildSettings();
        msBuildSettings.WithProperty("Version", context.Version);

        DotNetBuildSettings buildSettings = new DotNetBuildSettings()
        {
            MSBuildSettings = msBuildSettings,
            Configuration = "Release",
            Verbosity = DotNetVerbosity.Minimal,
            NoLogo = true
        };

        context.DotNetBuild(context.MonoGameAsepritePath, buildSettings);
        context.DotNetBuild(context.MonoGameAsepriteContentPipelinePath, buildSettings);
        context.DotNetBuild(context.MonoGameAsepriteTestsPath, buildSettings);
    }
}