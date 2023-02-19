#load "clean.task.cake"
#load "restore.task.cake"
#load "build.task.cake"
#load "test.task.cake"
#load "pack.task.cake"
#load "docs.task.cake"

Task("Rebuild")
.Description("Performs a Clean, Restore, and Build of each project.")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build");

Task("All")
.Description("Rebuilds each project, tests them, packs them for NuGet, and generates the documentation")
.IsDependentOn("Rebuild")
.IsDependentOn("Test")
.IsDependentOn("Pack")
.IsDependentOn("Docs");