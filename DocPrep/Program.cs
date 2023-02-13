using System.Text;
using System.Text.Json;

string prepPath = "../Artifacts/Documentation/Prep";
string outputPath = "../Artifacts/Documentation";

string[] files = Directory.GetFiles(prepPath, "*.md", SearchOption.AllDirectories);

Dictionary<string, List<string>> sidebarMap = new();

foreach (string file in files)
{
    FileInfo fileInfo = new(file);
    string dir = fileInfo?.Directory?.Name ?? throw new InvalidOperationException();

    if (!sidebarMap.ContainsKey(dir))
    {
        sidebarMap.Add(dir, new());
    }

    string fileName = Path.GetFileNameWithoutExtension(file);
    sidebarMap[dir].Add($"api/{dir}/{fileName}");
}

Stream stream = File.Create(Path.Combine(outputPath, "sidebar.json"));

using Utf8JsonWriter writer = new(stream, new JsonWriterOptions() { Indented = true });


writer.WriteStartObject();
{
    writer.WriteStartArray("apiSidebar");
    {
        foreach (var entry in sidebarMap)
        {
            writer.WriteStartObject();
            {
                writer.WriteString("type", "category");
                writer.WriteString("label", entry.Key);
                writer.WriteStartArray("items");
                {
                    foreach(string docId in entry.Value)
                    {
                        writer.WriteStringValue(docId);
                    }
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
    }
    writer.WriteEndArray();
}
writer.WriteEndObject();

writer.Flush();
writer.Dispose();

