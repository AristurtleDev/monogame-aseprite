using System.Reflection;
using ArisDocs;
using ArisDocs.Extensions;


string asmPath = Path.GetFullPath("../../Artifacts/Debug/Build/MonoGame.Aseprite.Common.dll");
string xmlPath = Path.GetFullPath("../../Artifacts/Debug/Build/MonoGame.Aseprite.Common.xml");
string outputDir = Path.GetFullPath("../../Artifacts/DevDocs/");

MarkdownDocumentation.WriteDocumentForAssembly(asmPath, xmlPath, outputDir);

// //  Load the assembly
// Assembly asm = Assembly.LoadFrom(asmPath);

// //  Load the XML Documentation that was generated on build
// AssemblyXmlDocumentation xmlDoc = AssemblyXmlDocumentation.LoadFrom(xmlPath);

// //  Getting the Sprite type from the assembly
// Type? spriteType = asm.GetType("MonoGame.Aseprite.AsepriteTypes.AsepriteCel");
// if (spriteType is null) throw new Exception("Type not found in assembly");

// string? summaryDoc = xmlDoc.GetDocumentation(spriteType);

// MethodInfo[] properties = spriteType.GetMethods();
// string?[] propertyDocs = new string[properties.Length];
// for (int i = 0; i < properties.Length; i++)
// {
//     propertyDocs[i] = xmlDoc.GetDocumentation(properties[i]);
// }

// for (int i = 0; i < propertyDocs.Length; i++)
// {
//     Console.WriteLine(propertyDocs[i]);
// }

