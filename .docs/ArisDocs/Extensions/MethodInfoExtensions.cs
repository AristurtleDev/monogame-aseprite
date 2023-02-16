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

using System.Reflection;
using System.Text.RegularExpressions;

namespace ArisDocs.Extensions;

internal static class MethodInfoExtensions
{
    internal static string GetXmlName(this MethodBase methodBase)
    {
        Dictionary<string, int> methodGenericMap = new();
        if (methodBase is MethodInfo)
        {
            ThrowHelpers.ThrowIfDeclaringTypeNull(methodBase.DeclaringType, methodBase.Name, nameof(methodBase));
            if (methodBase.DeclaringType.IsGenericType)
            {
                Type genericTypeDefinition = methodBase.DeclaringType.GetGenericTypeDefinition();
                MethodInfo[] methods = genericTypeDefinition.GetMethods(BindingFlags.Static |
                                                                        BindingFlags.Public |
                                                                        BindingFlags.Instance |
                                                                        BindingFlags.NonPublic);

                methodBase = methods.First(m => m.MetadataToken == methodBase.MetadataToken);
            }

            Type[] methodGenericArguments = methodBase.GetGenericArguments();
            for (int i = 0; i < methodGenericArguments.Length; i++)
            {
                Type methodGeneric = methodGenericArguments[i];
                methodGenericMap[methodGeneric.Name] = i;
            }
        }

        ThrowHelpers.ThrowIfDeclaringTypeNull(methodBase.DeclaringType, methodBase.Name, nameof(methodBase));

        Dictionary<string, int> typeGenericMap = new();
        Type[] typeGenericArguments = methodBase.DeclaringType.GetGenericArguments();
        for (int i = 0; i < typeGenericArguments.Length; i++)
        {
            Type typeGeneric = typeGenericArguments[i];
            typeGenericMap[typeGeneric.Name] = i;
        }

        ParameterInfo[] parameterInfos = methodBase.GetParameters();

        string declarationTypeString = GetXmlDocumentationFormattedString(methodBase.DeclaringType, false, typeGenericMap, methodGenericMap);
        string memberNameString = methodBase is ConstructorInfo ? "#ctor" : methodBase.Name;
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

        string key = $"M:{declarationTypeString}.{memberNameString}{methodGenericArgumentsString}{parametersString}";

        if (methodBase is MethodInfo methodInfo && (methodBase.Name is "op_Implicit" || methodBase.Name is "op_Explicit"))
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
            ThrowHelpers.ThrowIfElementTypeNull(elementType, type.Name, nameof(type));
            string elementTypeString = GetXmlDocumentationFormattedString(elementType, isMethodParameter, typeGenericMap, methodGenericMap);

            switch (type)
            {
                case Type when type.IsPointer:
                    return $"{elementTypeString}*";
                case Type when type.IsByRef:
                    return $"{elementTypeString}@";
                case Type when type.IsArray:
                    int arrayRank = type.GetArrayRank();
                    string dimensions = arrayRank > 1
                                        ? $"[{string.Join(",", Enumerable.Repeat("0:", arrayRank))}]"
                                        : "[]";
                    return $"{elementTypeString}{dimensions}";
                default:
                    throw new ArgumentException($"Unknown element type: '{elementType}'");

            }
        }

        string preface = $"{type.Namespace}.";
        if (type.IsNested)
        {
            ThrowHelpers.ThrowIfDeclaringTypeNull(type.DeclaringType, type.Name, nameof(type));
            string declaringTypeString = GetXmlDocumentationFormattedString(type.DeclaringType, isMethodParameter, typeGenericMap, methodGenericMap);
            preface = $"{declaringTypeString}.";
        }

        string typeName = type.Name;
        if (isMethodParameter)
        {
            typeName = Regex.Replace(type.Name, @"`\d+", string.Empty);
        }

        string genericArgument = string.Empty;
        if (type.IsGenericType && isMethodParameter)
        {
            Type[] genericArgumentTypes = type.GetGenericArguments();
            string[] genericArguments = new string[genericArgumentTypes.Length];

            for (int i = 0; i < genericArguments.Length; i++)
            {
                genericArguments[i] = GetXmlDocumentationFormattedString(genericArgumentTypes[i], isMethodParameter, typeGenericMap, methodGenericMap);
            }

            genericArgument = $"{{{string.Join(",", genericArguments)}}}";
        }

        return $"{preface}{typeName}{genericArgument}";
    }

    internal static string GetVisibility(this MethodBase methodBase) => methodBase switch
    {
        { IsPublic: true } => "public",
        { IsAssembly: true } => "internal",
        { IsFamily: true } => "protected",
        { IsFamilyOrAssembly: true } => "protected public",
        { IsFamilyAndAssembly: true } => "private protected",
        _ => throw new ArgumentException($"{nameof(MethodBase)}.{nameof(GetVisibility)} encountered an unknown visibility for {nameof(MethodBase)}: '{methodBase.Name}")
    };
}