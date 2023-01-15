using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.IO;
using MonoGame.Aseprite.Content.Pipeline.Processors;

string path = Path.Combine(Environment.CurrentDirectory, "townmap.aseprite");


AsepriteFile file = AsepriteFileReader.ReadFile(path);
TilemapProcessor processor = new();
var result = processor.Process(file, null);

Console.WriteLine(file.Name);
