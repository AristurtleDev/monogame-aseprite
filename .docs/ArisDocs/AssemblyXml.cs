/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2018-2023 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
---------------------------------------------------------------------------- */

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Xml;
using ArisDocs.Extensions;

namespace ArisDocs;

public sealed class AssemblyXmlDocumentation
{
    private Dictionary<string, string> _lookup = new();

    public string AssemblyName { get; }

    public AssemblyXmlDocumentation(string xmlDocPath)
    {
        if (!File.Exists(xmlDocPath))
        {
            throw new FileNotFoundException($"Unable to locate Assembly XML Document at the path '{xmlDocPath}'");
        }

        using Stream stream = File.OpenRead(xmlDocPath);
        using XmlReader reader = XmlReader.Create(stream);

        while (reader.Read())
        {
            if (reader.NodeType is XmlNodeType.Element)
            {
                if (reader.Name == "assembly")
                {
                    AssemblyName = reader.ReadInnerXml();
                }

                if (reader.Name == "member")
                {
                    string? memberName = reader["name"];
                    if (!string.IsNullOrWhiteSpace(memberName))
                    {
                        _lookup.Add(memberName, reader.ReadInnerXml());
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(AssemblyName))
        {
            string message =
            $"""
            The XML Document '{xmlDocPath}' is missing the <assembly> element.
            This does not appear to be a valid C# Assembly XML Document
            """;
            throw new InvalidOperationException(message);
        }
    }

    private string? GetDocumentation(string key)
    {
        string? result = default;
        _lookup.TryGetValue(key, out result);
        return result;
    }

    public string? GetDocumentation(Type type)
    {
        ThrowIfNull(type, nameof(type));
        string typeXmlName = type.GetXmlName();
        return GetDocumentation(typeXmlName);
    }

    public string? GetDocumentation(MethodInfo methodInfo)
    {
        ThrowIfNull(methodInfo, nameof(methodInfo));
        ThrowIfDeclaringTypeNull(methodInfo.DeclaringType, methodInfo.Name, nameof(methodInfo));
        string xmlName = methodInfo.GetXmlName();
        return GetDocumentation(xmlName);
    }

    private static void ThrowIfNull([NotNull] object? obj, string paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }


}