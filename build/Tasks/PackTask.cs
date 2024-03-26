using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Pack;
using Cake.Frosting;

namespace MonoGame.Aseprite.Build;

[TaskName("Pack")]
public sealed class PackTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        DotNetPackSettings settings = new DotNetPackSettings()
        {
            MSBuildSettings = context.DotNetMSBuildSettings,
            Verbosity = DotNetVerbosity.Minimal,
            OutputDirectory = context.NuGetsDirectory,
            Configuration = context.BuildConfiguration
        };

        context.DotNetPack("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", settings);
    }
}
