using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;
using MonoGame.Aseprite.Content.Pipeline.IO;

string path = Path.Combine(Environment.CurrentDirectory, "adventurer.aseprite");


AsepriteFile file = AsepriteFileReader.ReadFile(path);

Console.WriteLine(file.Name);
