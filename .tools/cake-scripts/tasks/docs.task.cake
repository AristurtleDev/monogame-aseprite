#load "../common/common.cake"

Task("Docs")
.IsDependentOn("Rebuild")
.Description("Builds the documentation markdown to be used for the website API pages")
.Does(() =>
{
    MdDocs();
    RenameIndexMDFiles();
    FixLinks();
    InjectFrontMatter();

});

private void MdDocs()
{
    DotNetToolSettings settings = new();

    string[] assembliesToDocument = new string[]
    {
        $"{CommonConfiguration.Output}{CommonConfiguration.Configuration}/Build/MonoGame.Aseprite.dll",
    };
    
    settings.ArgumentCustomization = builder =>
    {
        builder.Append("apireference");
        builder.AppendSwitch("--configurationFilePath", "./mddocs.config.json");
        builder.AppendSwitch("--assemblies", string.Join(" ", assembliesToDocument));
        builder.AppendSwitchQuoted("--outdir", $"{CommonConfiguration.Output}Documentation");
        return builder;
    };

    DotNetTool("mddocs", settings);
}

private void FixLinks()
{
    string[] files = System.IO.Directory.GetFiles($"{CommonConfiguration.Output}Documentation", "*.md", SearchOption.AllDirectories);

    foreach(string file in files)
    {
        string text = System.IO.File.ReadAllText(file);
        text = text.Replace("index.md", string.Empty);
        System.IO.File.WriteAllText(file, text);
    }
}

private void RenameIndexMDFiles()
{
    string[] files = System.IO.Directory.GetFiles($"{CommonConfiguration.Output}Documentation", "index.md", SearchOption.AllDirectories);

    foreach(string file in files)
    {
        FileInfo fileInfo = new FileInfo(file);

        if(fileInfo.Name == "index.md")
        {
            if(fileInfo.Directory is null)
            {
                throw new Exception("Unable to get directory name for file");
            }

            string newName = fileInfo.Directory.Name + ".md";

            fileInfo.MoveTo(System.IO.Path.Combine(fileInfo.Directory.FullName, newName ));
        }
    }
}

private void InjectFrontMatter()
{
    string[] files = System.IO.Directory.GetFiles($"{CommonConfiguration.Output}Documentation", "*.md", SearchOption.AllDirectories);


    foreach(string file in files)
    {
        string? newText = default;

        using (FileStream readStream = System.IO.File.OpenRead(file))
        {
            using (StreamReader streamReader = new(readStream))
            {
                string? firstLine = streamReader.ReadLine();

                if(firstLine is not null && firstLine.StartsWith("#"))
                {
                    firstLine = firstLine.Replace("#", string.Empty).Trim();
                    string title = firstLine;

                    string id = title.ToLower().Replace('.', '-')
                                               .Replace(' ', '-');

                    string? label = default;

                    if(firstLine.Contains("Namespace"))
                    {
                        FileInfo fileInfo = new FileInfo(file);
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
                };
            }
        }

        if(newText is not null)
        {
            System.IO.File.WriteAllText(file, newText);
        }
    }

}