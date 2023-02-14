#load "common.cake"


Task("Build")
.Description("Calls `dotnet build` on the Monogame.Aseprite.sln solution file")
.Does(() =>
{
    Console.WriteLine($"Target Task: {_target}");
});