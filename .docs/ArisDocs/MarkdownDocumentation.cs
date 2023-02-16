/* -----------------------------------------------------------------------------
Copyright 2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */

using System.Reflection;
using ArisDocs.Extensions;

namespace ArisDocs;

public static class MarkdownDocumentation
{
    public static void WriteDocumentForAssembly(string asmPath, string? xmlPath, string outputDir)
    {
        ThrowHelpers.ThrowIfFileDoesntExist(asmPath, nameof(asmPath));

        if (string.IsNullOrEmpty(xmlPath))
        {
            //  Remove .dll from end and replace with .xml
            xmlPath = $"{asmPath[..^4]}.xml";
        }

        ThrowHelpers.ThrowIfFileDoesntExist(xmlPath, nameof(xmlPath));

        Assembly asm = Assembly.LoadFrom(asmPath);
        AssemblyXmlDocumentation xmlDoc = AssemblyXmlDocumentation.LoadFrom(xmlPath);

        Directory.CreateDirectory(outputDir);

        foreach (Type type in (Type[])asm.GetTypes())
        {
            WriteDocumentationForType(type, xmlDoc);
        }
    }

    public static void WriteDocumentationForType(Type type, AssemblyXmlDocumentation xmlDoc)
    {
        Console.WriteLine(type.GetYamlID());
    }

    private static string GetFrontMatter(Type type)
    {
        return
        $"""
        ---
        id: 
        ---
        """;
    }

    private static string GetYamlID(MemberInfo memberInfo)
    {
        string id = memberInfo switch
        {
            FieldInfo fieldInfo => fieldInfo.GetXmlName(),
            PropertyInfo propertyInfo => propertyInfo.GetXmlName(),
            EventInfo eventInfo => eventInfo.GetXmlName(),
            ConstructorInfo constructorInfo => constructorInfo.GetXmlName(),
            MethodInfo methodInfo => methodInfo.GetXmlName(),
            Type type => type.GetXmlName(),
            null => throw new ArgumentNullException(nameof(memberInfo)),
            _ => throw new NotImplementedException($"{nameof(GetYamlID)} encountered an unhandled {nameof(MemberInfo)} type: {memberInfo}")
        };

        id = YamlHelper.SanitizeString(id);
        return id.ToLower();
    }

}