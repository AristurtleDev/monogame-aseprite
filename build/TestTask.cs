using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Frosting;

namespace BuildScripts;

[TaskName(nameof(TestTask))]
public sealed class TestTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        DotNetTestSettings testSettings = new DotNetTestSettings()
        {
            Configuration = "Release",
        };
        context.DotNetTest(context.MonoGameAsepriteTestsPath, testSettings);
    }
}