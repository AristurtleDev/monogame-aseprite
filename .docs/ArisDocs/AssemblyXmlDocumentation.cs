/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2023 Christopher Whitley

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

using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using ArisDocs.Extensions;

namespace ArisDocs;

public sealed class AssemblyXmlDocumentation
{
    private Dictionary<string, string> _lookup;
    public string AssemblyName { get; }

    private AssemblyXmlDocumentation(string assemblyName, Dictionary<string, string> lookup) =>
        (AssemblyName, _lookup) = (assemblyName, lookup);

    private string? GetDocumentation(string key)
    {
        string? result = default;
        _lookup.TryGetValue(key, out result);
        return result;
    }

    public string? GetDocumentation(Type type)
    {
        ThrowHelpers.ThrowIfParameterNull(type, nameof(type));
        string typeXmlName = type.GetXmlName();
        return GetDocumentation(typeXmlName);
    }

    public string? GetDocumentation(MethodInfo methodInfo)
    {
        ThrowHelpers.ThrowIfParameterNull(methodInfo, nameof(methodInfo));
        string xmlName = methodInfo.GetXmlName();
        return GetDocumentation(xmlName);
    }

    public string? GetDocumentation(ConstructorInfo constructorInfo)
    {
        ThrowHelpers.ThrowIfParameterNull(constructorInfo, nameof(constructorInfo));
        string xmlName = constructorInfo.GetXmlName();
        return GetDocumentation(xmlName);
    }

    public string? GetDocumentation(PropertyInfo propertyInfo)
    {
        ThrowHelpers.ThrowIfParameterNull(propertyInfo, nameof(propertyInfo));
        string xmlName = propertyInfo.GetXmlName();
        return GetDocumentation(xmlName);
    }

    public string? GetDocumentation(FieldInfo fieldInfo)
    {
        ThrowHelpers.ThrowIfParameterNull(fieldInfo, nameof(fieldInfo));
        string xmlName = fieldInfo.GetXmlName();
        return GetDocumentation(xmlName);
    }

    public string? GetDocumentation(EventInfo eventInfo)
    {
        ThrowHelpers.ThrowIfParameterNull(eventInfo, nameof(eventInfo));
        string xmlName = eventInfo.GetXmlName();
        return GetDocumentation(xmlName);
    }

    public string? GetDocumentation(MemberInfo memberInfo) => memberInfo switch
    {
        FieldInfo fieldInfo => GetDocumentation(fieldInfo),
        PropertyInfo propertyInfo => GetDocumentation(propertyInfo),
        EventInfo eventInfo => GetDocumentation(eventInfo),
        ConstructorInfo constructorInfo => GetDocumentation(constructorInfo),
        MethodInfo methodInfo => GetDocumentation(methodInfo),
        Type type => GetDocumentation(type),
        null => throw new ArgumentNullException(nameof(memberInfo)),
        _ => throw new NotImplementedException($"{nameof(GetDocumentation)} encountered an unhandled {nameof(MemberInfo)} type: {memberInfo}")
    };

    public string? GetDocumentation(ParameterInfo parameterInfo)
    {
        string? memberDoc = GetDocumentation(parameterInfo.Member);

        if (memberDoc is not null)
        {
            string paramTagPrefix = Regex.Escape($@"<param name=""{parameterInfo.Name}"">");
            string paramTagSuffix = Regex.Escape("</param>");
            string pattern = $"{paramTagPrefix}.*?{paramTagSuffix}";

            Match match = Regex.Match(memberDoc, pattern);
            if (match.Success)
            {
                return match.Value;
            }
        }

        return null;
    }

    public static AssemblyXmlDocumentation LoadFrom(string xmlDocPath)
    {
        string? assemblyName = default;
        Dictionary<string, string> lookup = new();

        ThrowHelpers.ThrowIfFileDoesntExist(xmlDocPath, nameof(xmlDocPath));

        using Stream stream = File.OpenRead(xmlDocPath);
        using XmlReader reader = XmlReader.Create(stream);

        while (reader.Read())
        {
            if (reader.NodeType is XmlNodeType.Element)
            {
                if (reader.Name == "assembly")
                {
                    assemblyName = reader.ReadInnerXml();
                }

                if (reader.Name == "member")
                {
                    string? memberName = reader["name"];
                    if (!string.IsNullOrWhiteSpace(memberName))
                    {
                        lookup.Add(memberName, reader.ReadInnerXml());
                    }
                }
            }
        }

        if (string.IsNullOrEmpty(assemblyName))
        {
            string message =
            $"""
            The XML Document '{xmlDocPath}' is missing the <assembly> element.
            This does not appear to be a valid C# Assembly XML Document
            """;
            throw new InvalidOperationException(message);
        }

        return new(assemblyName, lookup);
    }
}