#tool "dotnet:?package=GitVersion.Tool&version=5.10.3"
#nullable enable
string target = Argument(nameof(target), "Default");
string configuration = Argument(nameof(configuration), "Release");
string version = string.Empty;
string[] projects = new string[]
{
    "./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj",
    "./source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj"
};

Task("Clean")
.Does(() => 
{
    DotNetClean("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj");
    DotNetClean("./source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj");
    DotNetClean("./tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj");
    CleanDirectory("./.artifacts");
});

Task("Restore")
.IsDependentOn("Clean")
.Does(() => 
{
    DotNetRestore("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj");
    DotNetRestore("./source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj");
    DotNetRestore("./tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj");
});

Task("Version")
.Does(() => 
{
    GitVersionSettings settings = new();
    settings.UpdateAssemblyInfo = true;

    GitVersion gitVersion = GitVersion(settings);
    version = gitVersion.NuGetVersionV2;
    Information($"Version: {version}");
});

Task("Build")
.IsDependentOn("Version")
.Does(() => 
{
    DotNetMSBuildSettings msBuildSettings = new();
    msBuildSettings.WithProperty("Version", version);

    DotNetBuildSettings buildSettings = new();
    buildSettings.Configuration = configuration;
    buildSettings.MSBuildSettings = msBuildSettings;

    DotNetBuild("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", buildSettings);
    DotNetBuild("./source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj", buildSettings);
    DotNetBuild("./tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj", buildSettings);
});

Task("Test")
.IsDependentOn("Build")
.Does(() => 
{
    DotNetTestSettings testSettings = new();
    testSettings.Configuration = configuration;
    testSettings.NoBuild = true;

    DotNetTest("./tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj", testSettings);
});

Task("Pack")
.IsDependentOn("Test")
.Does(() =>
{
    DotNetMSBuildSettings msBuildSettings = new();
    msBuildSettings.WithProperty("PackageVersion", version)
                   .WithProperty("Version", version);

    DotNetPackSettings packSettings = new();
    packSettings.Configuration = configuration;
    packSettings.OutputDirectory = "./.artifacts";
    packSettings.NoBuild = true;
    packSettings.NoRestore = true;
    packSettings.MSBuildSettings = msBuildSettings;

    DotNetPack("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", packSettings);
});

Task("PublishNuGet")
.IsDependentOn("Pack")
.Does(context =>
{
    if(BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    {
        FilePath nupkgPath = $"./.artifacts/MonoGame.Aseprite.{version}.nupkg";

        Information("Publishing {0} to NuGet...", nupkgPath.GetFilename().FullPath);

        DotNetNuGetPushSettings pushSettings = new();
        pushSettings.ApiKey = context.EnvironmentVariable("NUGET_API_KEY");
        pushSettings.Source = "https://api.nuget.org/v3/index.json";

        DotNetNuGetPush(nupkgPath, pushSettings);
        
    }
});

Task("PublishGitHub")
.IsDependentOn("Pack")
.Does(context =>
{
    if(BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    {
        FilePath nupkgPath = $"./.artifacts/MonoGame.Aseprite.{version}.nupkg";

        Information("Publishing {0} to GitHub...", nupkgPath.GetFilename().FullPath);

        DotNetNuGetPushSettings pushSettings = new();
        pushSettings.ApiKey = context.EnvironmentVariable("GITHUB_TOKEN");
        pushSettings.Source = "https://nuget.pkg.github.com/aristurtledev/index.json";

        DotNetNuGetPush(nupkgPath, pushSettings);
        
    }
});

Task("Docs")
.IsDependentOn("Default")
.Does(() =>
{
    DotNetToolSettings toolSettings = new();
    toolSettings.ArgumentCustomization = builder =>
    {
        builder.Append("apireference");
        builder.AppendSwitch("--configurationFilePath", "./mddocs.config.json");
        builder.AppendSwitch("--assemblies", $"./source/MonoGame.Aseprite/bin/{configuration}/net6.0/MonoGame.Aseprite.dll");
        builder.AppendSwitchQuoted("--outdir", "./.artifacts/documentation");
        return builder;
    };

    DotNetTool("mddocs", toolSettings);

    //  Fix links
    string[] mdFiles = System.IO.Directory.GetFiles("./.artifacts/documentation", "*.md", SearchOption.AllDirectories);

    foreach(string file in mdFiles)
    {
        string text = System.IO.File.ReadAllText(file);
        text = text.Replace("index.md", string.Empty);
        System.IO.File.WriteAllText(file, text);
    }

    //  Rename index.md files to [directoryName].md
    string[] indexFiles = System.IO.Directory.GetFiles("./.artifacts/documentation", "index.md", SearchOption.AllDirectories);

    foreach(string file in indexFiles)
    {
        FileInfo fileInfo = new(file);
        if(fileInfo.Name == "index.md")
        {
            if(fileInfo.Directory is null)
            {
                throw new System.Exception($"Unable to get directory name for '{fileInfo.Name}'");
            }

            string newName = fileInfo.Directory.Name + ".md";

            fileInfo.MoveTo(System.IO.Path.Combine(fileInfo.Directory.FullName, newName));
        }
    }

    //  Inject front matter
    string[] files = System.IO.Directory.GetFiles("./.artifacts/documentation", "*.md", SearchOption.AllDirectories);

    foreach(string file in files)
    {
        string? newText = default;

        using(FileStream readStream = System.IO.File.OpenRead(file))
        {
            using(StreamReader streamReader = new(readStream))
            {
                string? firstLine = streamReader.ReadLine();

                if(firstLine is not null && firstLine.StartsWith("#"))
                {
                    firstLine = firstLine.Replace("#", string.Empty).Trim();
                    string title = firstLine;

                    string id = title.ToLower().Replace('.', '-').Replace(' ', '-');
                    
                    string? label = default;

                    if(firstLine.Contains("Namespace"))
                    {
                        FileInfo fileInfo = new(file);
                        label = fileInfo.Directory?.Name;
                    }

                    if(label is null)
                    {
                        label = title.Replace("Namespace", string.Empty)
                                     .Replace("Class", string.Empty)
                                     .Replace("Method", string.Empty)
                                     .Replace("Property", string.Empty)
                                     .Replace("Enum", string.Empty)
                                     .Replace("Field", string.Empty)
                                     .Replace("Event", string.Empty)
                                     .Trim();
                    }

                    string yaml =
                    $"""
                    ---
                    id: {id}
                    title: {title}
                    sidebar_label: {label}
                    ---
                    """;

                    newText = yaml + streamReader.ReadToEnd();
                }
            }
        }

        if(newText is not null)
        {
            System.IO.File.WriteAllText(file, newText);
        }
    }
});

Setup((context) =>
{
    //  Turn on xml document generation for docs task
    if(target is "Docs")
    {
        XmlPoke("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", "//GenerateDocumentationFile", "True");
    }
});

Teardown((context) =>
{
    //  Turn off xml document generation for docs task
    if(target is "Docs")
    {
        XmlPoke("./source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", "//GenerateDocumentationFile", "False");
    }
});


Task("Default")
.IsDependentOn("Clean")
.IsDependentOn("Restore")
.IsDependentOn("Version")
.IsDependentOn("Build")
.IsDependentOn("Test");

Task("Publish")
.IsDependentOn("Default")
.IsDependentOn("PublishNuGet")
.IsDependentOn("PublishGitHub");

RunTarget(target);