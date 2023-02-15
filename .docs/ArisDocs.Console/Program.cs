using System.Reflection;
using ArisDocs;


string path = Path.GetFullPath("../../Artifacts/Debug/Build/MonoGame.Aseprite.dll");
Assembly asm = Assembly.LoadFrom(path);
AssemblyDocument.LoadXmlDocumentation(asm);

Type? spriteType = asm.GetType("MonoGame.Aseprite.Sprites.Sprite");
if (spriteType is null)
{
    throw new Exception("Unable to get type from assembly");
}
Console.WriteLine(spriteType.GetDocumentation());