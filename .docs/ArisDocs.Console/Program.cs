using System.Reflection;
using ArisDocs;


string path = Path.GetFullPath("../../Artifacts/Debug/Build/MonoGame.Aseprite.Common.dll");
Assembly asm = Assembly.LoadFrom(path);
AssemblyToXmlMapper.LoadAssembly(asm);

// Type? spriteType = asm.GetType("MonoGame.Aseprite.Sprites.Sprite");
// if (spriteType is null)
// {
//     throw new Exception("Unable to get type from assembly");
// }
// // Console.WriteLine(AssemblyDocument.GetDocumentation(spriteType));
// Console.WriteLine(AssemblyDocument.ConvertToCSharpSource(typeof(List<(int, int, int)>), true));
// Console.WriteLine(AssemblyDocument.MyConvertToCSharpSource(typeof(List<(int, int, int)>), true));

Type? t = asm.GetType("MonoGame.Aseprite.AsepriteTypes.AsepriteCel");

if(t is null)
{

}

MethodInfo[] infos = t.GetMethods();

foreach(MethodInfo info in infos)
{
    Console.WriteLine(info.ToString());
}
