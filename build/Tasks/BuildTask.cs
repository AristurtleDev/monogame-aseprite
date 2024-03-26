using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Frosting;

namespace MonoGame.Aseprite.Build;

[TaskName("Build")]
public sealed class BuildTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        DotNetBuildSettings settings = new DotNetBuildSettings()
        {
            MSBuildSettings = context.DotNetMSBuildSettings,
            Verbosity = DotNetVerbosity.Minimal,
            Configuration = context.BuildConfiguration
        };

        context.DotNetBuild("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", settings);
        context.DotNetBuild("./source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj", settings);
    }
}