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

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace ArisDocs;

public static class AssemblyToXmlMapper
{
    private const string TYPE_PREFIX = "T:";
    private const string METHOD_INFO_PREFIX = "M:";
    private const string PROPERTY_INFO_PREFIX = "P:";
    private const string FIELD_INFO_PREFIX = "F:";
    private const string EVENT_INFO_PREFIX = "E:";

    private static object s_xmlCacheLock = new();
    private static HashSet<Assembly> s_assemblies = new();
    private static Dictionary<string, string> s_xmlDocumentation = new();

    public static bool LoadAssembly(Assembly assembly)
    {
        if (s_assemblies.Contains(assembly))
        {
            return false;
        }

        bool isNew = false;
        string? dirPath = Path.GetDirectoryName(assembly.Location);
        if (dirPath is not null)
        {
            string xmlPath = Path.Combine(dirPath, assembly.GetName().Name + ".xml");
            if (File.Exists(xmlPath))
            {
                using StreamReader reader = new(xmlPath);
                LoadXmlNoLock(reader);
                isNew = true;
            }
        }
        s_assemblies.Add(assembly);
        return isNew;
    }

    public static void LoadXml(string xml)
    {
        using StringReader stringReader = new(xml);
        LoadXml(stringReader);
    }

    public static void LoadXml(TextReader textReader)
    {
        lock (s_xmlCacheLock)
        {
            LoadXmlNoLock(textReader);
        }
    }

    private static void LoadXmlNoLock(TextReader textReader)
    {
        using XmlReader xmlReader = XmlReader.Create(textReader);
        while (xmlReader.Read())
        {
            if (xmlReader.NodeType is XmlNodeType.Element && xmlReader.Name is "member")
            {
                string? name = xmlReader[nameof(name)];
                if (!string.IsNullOrWhiteSpace(name))
                {
                    s_xmlDocumentation.Add(name, xmlReader.ReadInnerXml());
                }
            }
        }
    }

    public static void ClearXmlDocumentation()
    {
        lock (s_xmlCacheLock)
        {
            s_assemblies.Clear();
            s_xmlDocumentation.Clear();
        }
    }

    private static string? GetDocumentation(string key, Assembly assembly)
    {
        lock (s_xmlCacheLock)
        {
            string? result = default;

            s_xmlDocumentation.TryGetValue(key, out result);

            if (result is null)
            {
                LoadAssembly(assembly);
                s_xmlDocumentation.TryGetValue(key, out result);
            }

            return result;
        }
    }

    public static string? GetDocumentation([DisallowNull] Type type)
    {
        if (type.FullName is null) throw TypeFullNameIsNull(type, nameof(type));
        string xmlName = GetXmlName(type);
        return GetDocumentation(xmlName, type.Assembly);
    }

    public static string? GetDocumentation([DisallowNull] MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null) throw DeclaringTypeIsNull(methodInfo, nameof(methodInfo));
        string xmlName = GetXmlName(methodInfo);
        return GetDocumentation(xmlName, methodInfo.DeclaringType.Assembly);
    }

    public static string? GetDocumentation([DisallowNull] ConstructorInfo constructorInfo)
    {
        if (constructorInfo.DeclaringType is null) throw DeclaringTypeIsNull(constructorInfo, nameof(constructorInfo));
        string xmlName = GetXmlName(constructorInfo);
        return GetDocumentation(xmlName, constructorInfo.DeclaringType.Assembly);
    }

    public static string? GetDocumentation([DisallowNull] PropertyInfo propertyInfo)
    {
        if (propertyInfo.DeclaringType is null) throw DeclaringTypeIsNull(propertyInfo, nameof(propertyInfo));
        if (propertyInfo.DeclaringType.FullName is null) throw TypeFullNameIsNull(propertyInfo.DeclaringType, $"{nameof(propertyInfo)}.{nameof(propertyInfo.DeclaringType)}");
        string xmlName = GetXmlName(propertyInfo);
        return GetDocumentation(xmlName, propertyInfo.DeclaringType.Assembly);
    }

    public static string? GetDocumentation([DisallowNull] FieldInfo fieldInfo)
    {
        if (fieldInfo.DeclaringType is null) throw DeclaringTypeIsNull(fieldInfo, nameof(fieldInfo));
        if (fieldInfo.DeclaringType.FullName is null) throw TypeFullNameIsNull(fieldInfo.DeclaringType, $"{nameof(fieldInfo)}.{nameof(fieldInfo.DeclaringType)}");
        string xmlName = GetXmlName(fieldInfo);
        return GetDocumentation(xmlName, fieldInfo.DeclaringType.Assembly);
    }

    public static string? GetDocumentation([DisallowNull] EventInfo eventInfo)
    {
        if (eventInfo.DeclaringType is null) throw DeclaringTypeIsNull(eventInfo, nameof(eventInfo));
        if (eventInfo.DeclaringType.FullName is null) throw TypeFullNameIsNull(eventInfo.DeclaringType, $"{nameof(eventInfo)}.{nameof(eventInfo.DeclaringType)}");
        string xmlName = GetXmlName(eventInfo);
        return GetDocumentation(xmlName, eventInfo.DeclaringType.Assembly);
    }

    public static string? GetDocumentation(MemberInfo memberInfo)
    {
        return memberInfo switch
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
    }

    public static string? GetDocumentation([DisallowNull] ParameterInfo parameterInfo)
    {
        string? memberDocumentation = GetDocumentation(parameterInfo.Member);

        if (memberDocumentation is not null)
        {
            string paramTagPrefix = Regex.Escape($@"<param name=""{parameterInfo.Name}"">");
            string paramTagSuffix = Regex.Escape($@"</param>");
            string pattern = $"{paramTagPrefix}.*?{paramTagSuffix}";

            Match match = Regex.Match(memberDocumentation, pattern);
            if (match.Success)
            {
                return match.Value;
            }
        }

        return null;
    }


    /// <summary>
    ///     Gets the name of the specified <see cref="Type"/> as it would be shown in the name attribute of the
    ///     generated XML documentation.
    /// </summary>
    /// <param name="type">
    ///     the <see cref="Type"/> to get the name of.
    /// </param>
    /// <returns>
    ///     The name of <paramref name="type"/> as it would be shown in the generated XML documentation.
    /// </returns>
    public static string GetXmlName([DisallowNull] Type type)
    {
        if (type.FullName is null) throw TypeFullNameIsNull(type, nameof(type));
        LoadAssembly(type.Assembly);
        string xmlTypeName = GetXmlTypeName(type.FullName);
        return $"{TYPE_PREFIX}{xmlTypeName}";
    }

    public static string GetXmlName([DisallowNull] MethodInfo methodInfo)
    {
        if (methodInfo.DeclaringType is null) throw DeclaringTypeIsNull(methodInfo, nameof(methodInfo));
        return GetXmlMethodBaseName(methodInfo: methodInfo);
    }

    public static string GetXmlName([DisallowNull] ConstructorInfo constructorInfo)
    {
        if (constructorInfo.DeclaringType is null) throw DeclaringTypeIsNull(constructorInfo, nameof(constructorInfo));
        return GetXmlMethodBaseName(constructorInfo: constructorInfo);
    }

    public static string GetXmlName([DisallowNull] PropertyInfo propertyInfo)
    {
        if (propertyInfo.DeclaringType is null) throw DeclaringTypeIsNull(propertyInfo, nameof(propertyInfo));
        if (propertyInfo.DeclaringType.FullName is null) throw TypeFullNameIsNull(propertyInfo.DeclaringType, $"{nameof(propertyInfo)}.{nameof(propertyInfo.DeclaringType)}");
        string xmlTypeName = GetXmlTypeName(propertyInfo.DeclaringType.FullName);
        return $"{PROPERTY_INFO_PREFIX}{xmlTypeName}.{propertyInfo.Name}";
    }

    public static string GetXmlName([DisallowNull] FieldInfo fieldInfo)
    {
        if (fieldInfo.DeclaringType is null) throw DeclaringTypeIsNull(fieldInfo, nameof(fieldInfo));
        if (fieldInfo.DeclaringType.FullName is null) throw TypeFullNameIsNull(fieldInfo.DeclaringType, $"{nameof(fieldInfo)}.{nameof(fieldInfo.DeclaringType)}");
        string xmlTypeName = GetXmlTypeName(fieldInfo.DeclaringType.FullName);
        return $"{FIELD_INFO_PREFIX}{xmlTypeName}.{fieldInfo.Name}";
    }

    public static string GetXmlName([DisallowNull] EventInfo eventInfo)
    {
        if (eventInfo.DeclaringType is null) throw DeclaringTypeIsNull(eventInfo, nameof(eventInfo));
        if (eventInfo.DeclaringType.FullName is null) throw TypeFullNameIsNull(eventInfo.DeclaringType, $"{nameof(eventInfo)}.{nameof(eventInfo.DeclaringType)}");
        string xmlTypeName = GetXmlTypeName(eventInfo.DeclaringType.FullName);
        return $"{FIELD_INFO_PREFIX}{xmlTypeName}.{eventInfo.Name}";
    }

    private static string GetXmlMethodBaseName(MethodInfo? methodInfo = default, ConstructorInfo? constructorInfo = default)
    {
        if (methodInfo is not null && constructorInfo is not null)
        {
            throw new Exception($"{nameof(methodInfo)} is not null and {nameof(constructorInfo)} is not null");
        }

        if (methodInfo is not null)
        {
            if (methodInfo.DeclaringType is null) throw DeclaringTypeIsNull(methodInfo, nameof(methodInfo));

            if (methodInfo.DeclaringType.IsGenericType)
            {
                Type genericTypeDefinition = methodInfo.DeclaringType.GetGenericTypeDefinition();
                MethodInfo[] methods = genericTypeDefinition.GetMethods(BindingFlags.Static |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.NonPublic);

                methodInfo = methods.First(m => m.MetadataToken == methodInfo.MetadataToken);
            }
        }

        MethodBase? methodBase = methodInfo ?? (MethodBase?)constructorInfo;
        if (methodBase is null) throw new Exception($"{nameof(methodBase)} is null");
        if (methodBase.DeclaringType is null) throw DeclaringTypeIsNull(methodBase, nameof(methodBase));

        LoadAssembly(methodBase.DeclaringType.Assembly);

        Dictionary<string, int> typeGenericMap = new();
        Type[] typeGenericArguments = methodBase.DeclaringType.GetGenericArguments();
        for (int i = 0; i < typeGenericArguments.Length; i++)
        {
            Type typeGeneric = typeGenericArguments[i];
            typeGenericMap[typeGeneric.Name] = i;
        }

        Dictionary<string, int> methodGenericMap = new();
        if (constructorInfo is null)
        {
            Type[] methodGenericArguments = methodBase.GetGenericArguments();
            for (int i = 0; i < methodGenericArguments.Length; i++)
            {
                Type methodGeneric = methodGenericArguments[i];
                methodGenericMap[methodGeneric.Name] = i;
            }
        }

        ParameterInfo[] parameterInfos = methodBase.GetParameters();

        string declarationTypeString = GetXmlDocumentationFormattedString(methodBase.DeclaringType, false, typeGenericMap, methodGenericMap);
        string memberNameString = constructorInfo is not null ? "#ctor" : methodBase.Name;
        string methodGenericArgumentsString = methodGenericMap.Count > 0 ? $"``{methodGenericMap.Count}" : string.Empty;

        string parametersString = string.Empty;
        if (parameterInfos.Length > 0)
        {
            string[] parameters = new string[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                parameters[i] = GetXmlDocumentationFormattedString(parameterInfos[i].ParameterType, true, typeGenericMap, methodGenericMap);
            }
            parametersString = $"({string.Join(',', parameters)})";
        }

        string key = $"{METHOD_INFO_PREFIX}{declarationTypeString}.{memberNameString}{methodGenericArgumentsString}{parametersString}";

        if (methodInfo is not null)
        {
            string returnTypeString = GetXmlDocumentationFormattedString(methodInfo.ReturnType, true, typeGenericMap, methodGenericMap);
            key += $"~{returnTypeString}";
        }

        return key;
    }

    private static string GetXmlDocumentationFormattedString(Type type, bool isMethodParameter, Dictionary<string, int> typeGenericMap, Dictionary<string, int> methodGenericMap)
    {
        if (type.IsGenericParameter)
        {
            if (methodGenericMap.TryGetValue(type.Name, out int index))
            {
                return $"``{index}";
            }

            return $"`{typeGenericMap[type.Name]}";
        }

        if (type.HasElementType)
        {
            Type? elementType = type.GetElementType();
            if (elementType is null) throw ElementTypeIsNull(type, nameof(type));
            string elementTypeString = GetXmlDocumentationFormattedString(elementType, isMethodParameter, typeGenericMap, methodGenericMap);

            switch (type)
            {
                case Type when type.IsPointer:
                    return $"{elementTypeString}*";
                case Type when type.IsByRef:
                    return $"{elementTypeString}@";
                case Type when type.IsArray:
                    int arrayRank = type.GetArrayRank();
                    string arrayDimensionsString = arrayRank > 1
                                                   ? $"[{string.Join(",", Enumerable.Repeat("0:", arrayRank))}]"
                                                   : "[]";
                    return $"{elementTypeString}{arrayDimensionsString}";
                default:
                    throw UnknownElementType(elementType);
            }
        }

        string prefaceString;
        if (type.IsNested)
        {
            if (type.DeclaringType is null) throw DeclaringTypeIsNull(type, nameof(type));
            string declaringTypeString = GetXmlDocumentationFormattedString(type.DeclaringType, isMethodParameter, typeGenericMap, methodGenericMap);
            prefaceString = $"{declaringTypeString}.";
        }
        else
        {
            prefaceString = $"{type.Namespace}.";
        }

        string typeNameString;
        if (isMethodParameter)
        {
            typeNameString = Regex.Replace(type.Name, @"`\d+", string.Empty);
        }
        else
        {
            typeNameString = type.Name;
        }

        string genericArgumentsString = string.Empty;
        if (type.IsGenericType && isMethodParameter)
        {
            Type[] genericArguments = type.GetGenericArguments();
            string[] genericArgumentsStrings = new string[genericArguments.Length];

            for (int i = 0; i < genericArguments.Length; i++)
            {
                genericArgumentsStrings[i] = GetXmlDocumentationFormattedString(genericArguments[i], isMethodParameter, typeGenericMap, methodGenericMap);
            }

            genericArgumentsString = $"{{{string.Join(",", genericArgumentsStrings)}}}";
        }

        return $"{prefaceString}{typeNameString}{genericArgumentsString}";
    }

    private static string GetXmlTypeName(string typeFullName) =>
        Regex.Replace(typeFullName, @"\[.*\]", string.Empty).Replace('+', '.');


    public static string ConvertToCSharpSource(this Type type, bool showGenericParameters = false)
    {
        Queue<Type> genericParameters = new Queue<Type>(type.GetGenericArguments());
        return convertToSource(type);

        string convertToSource(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            string result = type.IsNested
                ? type.DeclaringType is not null ? convertToSource(type.DeclaringType) + "." : throw new ArgumentException()
                // ? convertToSource(sourceof(type.DeclaringType, out string c1) ?? throw new ArgumentException(c1)) + "."
                : type.Namespace + ".";
            result += Regex.Replace(type.Name, "`.*", string.Empty);
            if (type.IsGenericType)
            {
                result += "<";
                bool firstIteration = true;
                foreach (Type generic in type.GetGenericArguments())
                {
                    if (genericParameters.Count <= 0)
                    {
                        break;
                    }
                    Type correctGeneric = genericParameters.Dequeue();
                    result += (firstIteration ? string.Empty : ",") +
                        (correctGeneric.IsGenericParameter
                            ? (showGenericParameters ? (firstIteration ? string.Empty : " ") + correctGeneric.Name : string.Empty)
                            : (firstIteration ? string.Empty : " ") + ConvertToCSharpSource(correctGeneric, showGenericParameters));
                    firstIteration = false;
                }
                result += ">";
            }
            return result;
        }
    }

    public static string MyConvertToCSharpSource([DisallowNull] Type type, bool showGenericParameters = false)
    {
        Queue<Type> genericParameters = new(type.GetGenericArguments());
        return convertToSource(type);

        string convertToSource([DisallowNull] Type type)
        {
            string result = type.Namespace + ".";
            if (type.IsNested)
            {
                if (type.DeclaringType is null) throw DeclaringTypeIsNull(type, nameof(type));
                result = convertToSource(type.DeclaringType) + ".";
            }

            result += Regex.Replace(type.Name, "`.*", string.Empty);

            if (!type.IsGenericType)
            {
                return result;
            }

            result += "<";
            bool firstIteration = true;
            foreach (Type generic in (Type[])type.GetGenericArguments())
            {
                if (genericParameters.Count <= 0)
                {
                    break;
                }

                Type correctGeneric = genericParameters.Dequeue();

                result += (firstIteration ? string.Empty : ",") +
                        (correctGeneric.IsGenericParameter
                            ? (showGenericParameters ? (firstIteration ? string.Empty : " ") + correctGeneric.Name : string.Empty)
                            : (firstIteration ? string.Empty : " ") + ConvertToCSharpSource(correctGeneric, showGenericParameters));
                firstIteration = false;
            }
            result += ">";
            return result;
        }
    }















    private static ArgumentException DeclaringTypeIsNull([DisallowNull] MemberInfo memberInfo, string paramName)
    {
        string message = $"{memberInfo.GetType()}.{nameof(memberInfo.DeclaringType)} for {memberInfo.GetType()} '{memberInfo.Name}' is null";
        ArgumentException ex = new(message, paramName);
        ex.Data.Add(memberInfo.GetType().Name, memberInfo);
        ex.Data.Add(nameof(paramName), paramName);
        return ex;
    }

    private static ArgumentException TypeFullNameIsNull([DisallowNull] Type type, string paramName)
    {
        string message = $"{nameof(Type)}.{nameof(Type.FullName)} for {nameof(Type)} '{type.Name}' is null";
        ArgumentException ex = new(message, paramName);
        ex.Data.Add(nameof(Type), type);
        ex.Data.Add(nameof(paramName), paramName);
        return ex;
    }

    private static ArgumentException ElementTypeIsNull(Type type, string paramName)
    {
        string message = $"{type.Name}.{nameof(type.HasElementType)} was true but {nameof(type)}.{nameof(type.GetElementType)} returned null";
        ArgumentException ex = new(message, paramName);
        ex.Data.Add(nameof(Type), type);
        ex.Data.Add(nameof(paramName), paramName);
        return ex;
    }

    private static Exception UnknownElementType(Type type)
    {
        string message = $"Unknown element type '{type}'";
        Exception ex = new(message);
        ex.Data.Add(nameof(Type), type);
        return ex;
    }
}