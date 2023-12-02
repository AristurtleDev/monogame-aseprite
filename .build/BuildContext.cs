using System.IO;
using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Xml;
using Cake.Core;
using Cake.Frosting;

namespace BuildScripts;

public sealed class BuildContext : FrostingContext
{
    public readonly string ArtifactsDirectory;
    public readonly string Version;
    public readonly string? RepositoryOwner;
    public readonly string? RepositoryUrl;
    public readonly bool IsTag;
    public readonly bool IsRunningOnGitHubActions;
    public readonly string? GitHubToken;
    public readonly string? NuGetAccessToken;
    public readonly string MonoGameAsepritePath;
    public readonly string MonoGameAsepriteContentPipelinePath;
    public readonly string MonoGameAsepriteTestsPath;

    public BuildContext(ICakeContext context) : base(context)
    {
        ArtifactsDirectory = context.Argument(nameof(ArtifactsDirectory), ".artifacts");
        MonoGameAsepritePath = context.Argument(nameof(MonoGameAsepritePath), "source/MonoGame.Aseprite/MonoGame.Aseprite.csproj");
        MonoGameAsepriteContentPipelinePath = context.Argument(nameof(MonoGameAsepriteContentPipelinePath), "source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj");
        MonoGameAsepriteTestsPath = context.Argument(nameof(MonoGameAsepriteTestsPath), "tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj");
        Version = context.XmlPeek("source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", "/Project/PropertyGroup/Version");

        IsRunningOnGitHubActions = context.BuildSystem().IsRunningOnGitHubActions;
        if (IsRunningOnGitHubActions)
        {
            RepositoryOwner = context.EnvironmentVariable("GITHUB_REPOSITORY_OWNER");
            RepositoryUrl = $"https://github.com/{context.EnvironmentVariable("GITHUB_REPOSITORY")}";
            GitHubToken = context.EnvironmentVariable("GITHUB_TOKEN");
            IsTag = context.EnvironmentVariable("GITHUB_REF_TYPE") == "tag";

            if (IsTag)
            {
                NuGetAccessToken = context.EnvironmentVariable("NUGET_ACCESS_TOKEN");
            }
        }
    }


}