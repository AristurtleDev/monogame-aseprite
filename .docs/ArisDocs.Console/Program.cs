using System.Reflection;
using ArisDocs;
using ArisDocs.Extensions;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

// CodeTypeDeclaration d = new();
// d.IsClass = true;
// d.Name = "Heather";

// CodeMemberEvent e = new();
// e.Name = "EventName";
// e.Type = new CodeTypeReference(typeof(int?));

// CodeMemberProperty p = new();
// p.Name = "PropertyName";
// p.HasGet = true;
// p.HasSet = true;
// CodeStatement g = new();
// CodeDirective cd = new();
// g.StartDirectives.Add(cd);)
// p.GetStatements.Add(new CodeStatement().StartDirectives)

// Console.WriteLine(e.ToString());
// StringBuilder sb = new();
// using StringWriter writer = new(sb);
// var options = new CodeGeneratorOptions();
// CodeDomProvider.CreateProvider("c#").GenerateCodeFromMember(p, writer, options);

// Console.WriteLine(sb.ToString());

string asmPath = Path.GetFullPath("../../Artifacts/Debug/Build/MonoGame.Aseprite.Common.dll");
string xmlPath = Path.GetFullPath("../../Artifacts/Debug/Build/MonoGame.Aseprite.Common.xml");
string outputDir = Path.GetFullPath("../../Artifacts/DevDocs/");

// MarkdownDocumentation.WriteDocumentForAssembly(asmPath, xmlPath, outputDir);

//  Load the assembly
Assembly asm = Assembly.LoadFrom(asmPath);
Type? type = asm.GetType("MonoGame.Aseprite.AsepriteFile");
if(type is null) throw new Exception();

PropertyInfo[] props = type.GetProperties();
foreach (PropertyInfo prop in props)
{
    Console.WriteLine(prop.GetSignature());
}
// }
// Type[] types = asm.GetTypes();
// foreach(Type type in types)
// {
//     Console.WriteLine(type.GetSignature());
// }

// //  Load the XML Documentation that was generated on build
// AssemblyXmlDocumentation xmlDoc = AssemblyXmlDocumentation.LoadFrom(xmlPath);

// //  Getting the Sprite type from the assembly
// Type? spriteType = asm.GetType("MonoGame.Aseprite.Sprites.Sprite");
// if (spriteType is null) throw new Exception("Type not found in assembly");

// string? summaryDoc = xmlDoc.GetDocumentation(spriteType);

// Console.WriteLine(summaryDoc);

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

