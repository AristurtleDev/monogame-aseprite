/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using Cake.Common;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Xml;
using Cake.Core;
using Cake.Frosting;

namespace MonoGame.Aseprite.Cake;

public class BuildContext : FrostingContext
{
    public string DotNetBuildConfiguration { get; }
    public string OutputDirectory { get; }
    public string GitHubRepositoryUrl { get; }
    public DotNetMSBuildSettings DotNetMsBuildSettings { get; }

    public BuildContext(ICakeContext context)
        : base(context)
    {
        string version = ParseVersion(context);

        DotNetMsBuildSettings = new();

        string repositoryPath = "AristurtleDev/monogame-aseprite";
        bool isGitHubActionsEnvironment = context.EnvironmentVariable("GITHUB_ACTIONS", false);

        if (isGitHubActionsEnvironment)
        {
            if (ParseVersionSuffix(context, repositoryPath) is string suffix)
            {
                version += $"-{suffix}";
            }
            repositoryPath = context.EnvironmentVariable("GITHUB_REPOSITORY", repositoryPath);
        }
        else if (context.EnvironmentVariable("BRANCH_NAME", string.Empty) != "stable")
        {
            version += $"-develop";
        }


        Console.WriteLine("Version: " + version);

        DotNetMsBuildSettings.WithProperty("Version", version);
        GitHubRepositoryUrl = $"https://github.com/{repositoryPath}";
        DotNetBuildConfiguration = context.Argument("configuration", "Debug");
        OutputDirectory = context.Argument("output", "../Artifacts");
    }

    private string ParseVersion(ICakeContext context)
    {
        string xmlValue = context.XmlPeek("../source/Directory.Build.props", "//Version");
        Version assemblyVersion = new(xmlValue);

        bool incrementBuild = context.HasArgument("increment-build");

        if (incrementBuild)
        {
            assemblyVersion = new(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Build + 1);
            context.XmlPoke("../source/Directory.Build.props", "//Version", assemblyVersion.ToString());
        }

        string? meta = default;


        return $"{assemblyVersion}{meta}";
    }

    private string? ParseVersionSuffix(ICakeContext context, string sourceRepo)
    {
        string? repo = context.EnvironmentVariable("GITHUB_REPOSITORY");

        if (repo != sourceRepo)
        {
            return context.EnvironmentVariable("GITHUB_REPOSITORY_OWNER");
        }

        string? ghRefType = context.EnvironmentVariable("GITHUB_REF_TYPE");
        string? ghRef = context.EnvironmentVariable("GITHUB_REF");

        if (ghRefType == "branch" && ghRef != "refs/head/stable")
        {
            return "develop";
        }

        return null;
    }
}