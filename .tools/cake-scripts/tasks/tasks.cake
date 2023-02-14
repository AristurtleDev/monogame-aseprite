#load "clean.task.cake"
#load "restore.task.cake"
#load "build.task.cake"
#load "test.task.cake"
#load "pack.task.cake"
#load "docs.task.cake"

Task("All")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Build")
.IsDependentOn("Test")
.IsDependentOn("Pack")
.IsDependentOn("Docs");