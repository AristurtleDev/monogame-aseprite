#tool "dotnet:?package=GitVersion.Tool&version=5.10.3"
#nullable enable
///////////////////////////////////////////////////////////////////////////////
/// Arguments
///////////////////////////////////////////////////////////////////////////////
string target = Argument(nameof(target), "Default");
string configuration = Argument(nameof(configuration), "Release");
bool restore = Argument(nameof(restore), true);
bool clean = Argument(nameof(clean), true);
bool test = Argument(nameof(test), true);
bool generateDocs = Argument(nameof(generateDocs), false);

///////////////////////////////////////////////////////////////////////////////
/// Variables
///////////////////////////////////////////////////////////////////////////////
string version = string.Empty;

///////////////////////////////////////////////////////////////////////////////
/// Setup
///////////////////////////////////////////////////////////////////////////////

Setup((context) =>
{
    //  Turn on xml document generation for docs task
    if(target is "Docs" || generateDocs)
    {
        XmlPoke("../source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", "//GenerateDocumentationFile", "True");
    }
});

///////////////////////////////////////////////////////////////////////////////
/// Teardown
///////////////////////////////////////////////////////////////////////////////

Teardown((context) =>
{
    //  Turn off xml document generation for docs task
    if(target is "Docs" || generateDocs)
    {
        XmlPoke("../source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", "//GenerateDocumentationFile", "False");
    }
});

///////////////////////////////////////////////////////////////////////////////
/// Tasks
///////////////////////////////////////////////////////////////////////////////
Task("Clean")
.WithCriteria(clean)
.Does(() => 
{
    DotNetClean("../source/MonoGame.Aseprite/MonoGame.Aseprite.csproj");
    DotNetClean("../source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj");
    DotNetClean("../tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj");
    CleanDirectory("../.artifacts");
});

Task("Restore")
.IsDependentOn("Clean")
.WithCriteria(restore)
.Does(() => 
{
    DotNetRestore("../source/MonoGame.Aseprite/MonoGame.Aseprite.csproj");
    DotNetRestore("../source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj");
    DotNetRestore("../tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj");
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

    DotNetBuild("../source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", buildSettings);
    DotNetBuild("../source/MonoGame.Aseprite.Content.Pipeline/MonoGame.Aseprite.Content.Pipeline.csproj", buildSettings);
    DotNetBuild("../tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj", buildSettings);
});

Task("Test")
.IsDependentOn("Build")
.WithCriteria(test)
.Does(() => 
{
    DotNetTestSettings testSettings = new();
    testSettings.Configuration = configuration;
    testSettings.NoBuild = true;

    DotNetTest("../tests/MonoGame.Aseprite.Tests/MonoGame.Aseprite.Tests.csproj", testSettings);
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
    packSettings.OutputDirectory = "../.artifacts";
    packSettings.NoBuild = true;
    packSettings.NoRestore = true;
    packSettings.MSBuildSettings = msBuildSettings;

    DotNetPack("../source/MonoGame.Aseprite/MonoGame.Aseprite.csproj", packSettings);
});

Task("PublishNuGet")
.IsDependentOn("Pack")
.Does(context =>
{
    if(BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    {
        FilePath nupkgPath = $"../.artifacts/MonoGame.Aseprite.{version}.nupkg";

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
        FilePath nupkgPath = $"../.artifacts/MonoGame.Aseprite.{version}.nupkg";

        Information("Publishing {0} to GitHub...", nupkgPath.GetFilename().FullPath);

        DotNetNuGetPushSettings pushSettings = new();
        pushSettings.ApiKey = context.EnvironmentVariable("GITHUB_TOKEN");
        pushSettings.Source = "https://nuget.pkg.github.com/AristurtleDev/index.json";

        DotNetNuGetPush(nupkgPath, pushSettings);
        
    }
});

Task("Docs")
.IsDependentOn("Default")
.Does(() =>
{
    // ---------------------------------------------------
    //  Use mddcos dotnet tool to create the initial
    //  documentation files in the .artifacts directory
    // ---------------------------------------------------
    DotNetToolSettings toolSettings = new();
    toolSettings.ArgumentCustomization = builder =>
    {
        builder.Append("apireference");
        builder.AppendSwitch("--configurationFilePath", "./mddocs.config.json");
        builder.AppendSwitch("--assemblies", $"../source/MonoGame.Aseprite/bin/{configuration}/net6.0/MonoGame.Aseprite.dll");
        builder.AppendSwitchQuoted("--outdir", "../.artifacts/documentation");
        return builder;
    };

    DotNetTool("mddocs", toolSettings);

    // ---------------------------------------------------
    //  Go through all documentation directories and change
    //  the lowercase directory names title case
    // ---------------------------------------------------
    Dictionary<string, string> renamedDirMap = new();
    foreach(string dir in (string[])System.IO.Directory.GetDirectories("../.artifacts/documentation", "*", SearchOption.AllDirectories))
    {
        System.IO.DirectoryInfo dirInfo = new(dir);

        if(char.IsLower(dirInfo.Name[0]))
        {
            string? containingDirectoryPath = System.IO.Path.GetDirectoryName(dir);
            if(containingDirectoryPath is null)
            {
                throw new Exception($"System.IO.Path.GetDirectoryname for directory '{dir}' returned null!");
            }
            string newDirName = $"{char.ToUpper(dirInfo.Name[0])}{dirInfo.Name[1..]}";
            string newPath = System.IO.Path.Combine(containingDirectoryPath, newDirName);
            renamedDirMap.TryAdd(dirInfo.Name, newDirName);
            dirInfo.MoveTo(newPath);
        }
    }


    // ---------------------------------------------------
    //  Go through all documentation files and perform the following
    //      - Rename all "index.md" files to "[directoryName].md"
    //      - Read each file line-by-line and
    //          - Replace the first line with generated yaml frontmatter
    //          - Fix links to index.md from the previous name change
    //          - Fix links to directories that names changed from the
    //            lowercase to uppercase change prior
    // ---------------------------------------------------
     //  Rename index.md files to [directoryName].md
    foreach(string file in (string[])System.IO.Directory.GetFiles("../.artifacts/documentation", "*.md", SearchOption.AllDirectories))
    {
        string? newName = default;

        FileInfo fileInfo = new(file);
        if(fileInfo.Name == "index.md")
        {
            if(fileInfo.Directory is null)
            {
                throw new System.Exception($"Unable to get directory name for '{fileInfo.Name}'");
            }

            newName = fileInfo.Directory.Name + ".md";
            string newPath = System.IO.Path.Combine(fileInfo.Directory.FullName , newName);
            fileInfo.MoveTo(newPath);

            //  Don't forget to update the fileInfo reference after moving to new path
            fileInfo = new(newPath);

        }

        StringBuilder sb = new();

        using(System.IO.FileStream fileStream = new(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using(System.IO.StreamReader streamReader = new(fileStream))
            {
                bool isFirstLine = true;
                string? line = default;
                bool previousLineStartsCodeBlock = false;
                bool inCommentChain = false;
                while((line = streamReader.ReadLine()) is not null)
                {
                    if(isFirstLine && line.StartsWith("#"))
                    {
                        string title = line.Replace("#", string.Empty).Trim();
                        string id = title.ToLower().Replace('.', '-').Replace(' ', '-');
                        string? label = default;

                        if(line.Contains("Namespace"))
                        {
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

                        
                        bool isMainNamespace = false;
                        if(label is "Aseprite")
                        {
                            isMainNamespace = true;
                            //  This should only occur for the MonoGame.Aseprite namespace document
                            //  but we want the label for that to be "MonoGame.Aseprite" and not "Aseprite"
                            label = "MonoGame.Aseprite Namespace";
                        }
                        else
                        {
                            //  Otherwise, we want to remove the first part of the label up to the '.' character
                            //  if the label has a '.' character.
                            //  This effectibly changes the Property/Field/Method/Indexor from being "Type.Property"
                            //  to just "Property".  
                            //  This is to remove clutter in the sidbar labeling so instead of this
                            //
                            //  MonoGame.Aseprite
                            //      ↳ AsepriteFile
                            //          ↳ Methods
                            //              ↳ AsepriteFile.GetFrame
                            //
                            //  We get this
                            //
                            //  Monogame.Aseprite
                            //      ↳ AsepriteFile
                            //          ↳ Methods
                            //              ↳ GetFrame

                            int dotIndex = label.IndexOf('.');
                            if(dotIndex >= 0) label = label[(dotIndex + 1)..];
                        }



                        string yaml = 
                        $"""
                        ---
                        title: {title}
                        sidebar_label: {label}
                        {(isMainNamespace ? "sidebar_position: 0" : string.Empty)}
                        ---
                        """;
                        sb.AppendLine(yaml);
                    }
                    else
                    {
                        //  Replace all calls to an index.md with an empty string
                        string sanatized = line.Replace("index.md", string.Empty);

                        //  Replace all calls to the lowercase folder names to uppercase since we changed them
                        //  in the previous loop
                        foreach(KeyValuePair<string, string> entry in renamedDirMap)
                        {
                            sanatized = sanatized.Replace($"({entry.Key}/", $"({entry.Value}/")
                                                 .Replace($"../{entry.Key}/", $"../{entry.Value}/");
                        }


                        //  Fix issue where mddocs removes intentional new lines in <code> blocks by appending
                        //  a new line anytime comment lines are discovered
                        if(line.StartsWith("//") && !inCommentChain && !previousLineStartsCodeBlock)
                        {
                            inCommentChain = true;
                            sb.AppendLine();
                        }
                        else
                        {
                            inCommentChain = false;
                        }

                        //  Fix issue where sometimes the space between words and links are removed because of a 
                        //  newline in the XML comment, resulting in something like this
                        //
                        //  word[Link](link)
                        //  
                        //  The regex will match any word character that is directly connected to a '[', for instance
                        //  the 'd[' in the above example.
                        //  If a match is found, it then replaces it with a space between it like 'd ['
                        string pattern = @"\w\[";
                        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(sanatized, pattern);

                        if(match.Success)
                        {
                            sanatized = System.Text.RegularExpressions.Regex.Replace(sanatized, pattern, match.Value.Insert(1, " "));
                        }

                        //  In one document so far, I'm using '### Header' in the <remarks> section.  mddocs is escaping
                        //  the "###" as "\#\#\#".  Find this and fix it
                        if(sanatized.Contains(@"\#\#\#"))
                        {
                            sanatized = sanatized.Replace(@"\#\#\#", "\n\n###");
                        }

                        //  Fix external links in the see also secitons
                        if(sanatized.StartsWith(@"- [https:\/\/docs.monogame.net\/api\/"))
                        {
                            sanatized = sanatized.Replace(@"https:\/\/docs.monogame.net\/api\/", string.Empty)
                                                 .Replace(".html]", "]");
                        }
                        
                        sb.AppendLine(sanatized);
                    }
                    isFirstLine = false;
                    previousLineStartsCodeBlock = line.StartsWith("```cs");
                }
            }
        }

        System.IO.File.WriteAllText(fileInfo.FullName, sb.ToString());
    }


    // ---------------------------------------------------
    //  Move the "Aseprite" directory out of the MonoGame" 
    //  directory and rename it to "MonoGame.Aseprite"
    //  then delete the "MonoGame" directory
    // ---------------------------------------------------
    new System.IO.DirectoryInfo("../.artifacts/documentation/MonoGame/Aseprite").MoveTo("../.artifacts/documentation/MonoGame.Aseprite");
    System.IO.Directory.Delete("../.artifacts/documentation/MonoGame", true);

    // ---------------------------------------------------
    //  Rename the "Aseprite.md" file to "MonoGame.Aseprite.md"
    // ---------------------------------------------------
    new System.IO.FileInfo("../.artifacts/documentation/MonoGame.Aseprite/Aseprite.md").MoveTo("../.artifacts/documentation/MonoGame.Aseprite/MonoGame.Aseprite.md");
    
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

///////////////////////////////////////////////////////////////////////////////
/// Execution
///////////////////////////////////////////////////////////////////////////////
RunTarget(target);