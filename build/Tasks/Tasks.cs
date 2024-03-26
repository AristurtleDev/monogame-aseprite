using Cake.Frosting;

namespace MonoGame.Aseprite.Build;

[TaskName("Default")]
[IsDependentOn(typeof(BuildTask))]
[IsDependentOn(typeof(PackTask))]
public sealed class DefaultTask : FrostingTask<BuildContext> { }
