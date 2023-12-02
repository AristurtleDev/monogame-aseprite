using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Frosting;

namespace BuildScripts;

[TaskName(nameof(PackageTask))]
public sealed class PackageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.CleanDirectory(context.ArtifactsDirectory);
        context.CreateDirectory(context.ArtifactsDirectory);

        DotNetMSBuildSettings msBuildSettings = new DotNetMSBuildSettings();
        msBuildSettings.WithProperty("Version", context.Version);
        msBuildSettings.WithProperty("PackageVersion", context.Version);

        DotNetPackSettings packSettings = new DotNetPackSettings()
        {
            Configuration = "Release",
            OutputDirectory = context.ArtifactsDirectory,
            MSBuildSettings = msBuildSettings
        };

        context.DotNetPack(context.MonoGameAsepritePath, packSettings);
    }
}