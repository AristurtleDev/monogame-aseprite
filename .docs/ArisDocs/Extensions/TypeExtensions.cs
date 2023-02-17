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

using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text;
using System.Text.RegularExpressions;

namespace ArisDocs.Extensions;


public static class TypeExtensions
{
    public static string GetXmlName(this Type type)
    {
        ThrowHelpers.ThrowIfFullNameNull(type.FullName, type.Name, nameof(type));
        string xmlName = Regex.Replace(type.FullName, @"\[.*\]", string.Empty).Replace('+', '.');
        return $"T:{xmlName}";
    }

    public static string GetSignature(this Type type)
    {
        CodeTypeDeclaration typeDeclaration = new(type.Name);
        typeDeclaration.TypeAttributes = type.Attributes;
        typeDeclaration.IsClass = type.IsClass;
        typeDeclaration.IsEnum = type.IsEnum;
        typeDeclaration.IsInterface = type.IsInterface;
        typeDeclaration.IsStruct = type.IsValueType;


        StringBuilder sb = new();
        using StringWriter writer = new(sb);
        CodeGeneratorOptions options = new();
        options.BracingStyle = "C";
        options.IndentString = "    ";
        CodeDomProvider.CreateProvider("c#").GenerateCodeFromType(typeDeclaration, writer, options);

        string signature = sb.ToString();
        signature = signature.Replace("sealed abstract", "static");
        return signature;
    }

    // public static string GetSignature(this Type type)
    // {

    //     string visibility = type switch
    //     {
    //         { IsPublic: true, IsAssembly: false, IsFamily: false, IsFamilyOrAssembly: false, IsFamilyAndAssembly: false } => "public",
    //         { IsPublic: false, IsAssembly: true, IsFamily: false, IsFamilyOrAssembly: false, IsFamilyAndAssembly: false } => "internal",
    //         { IsPublic: false, IsAssembly: false, IsFamily: true, IsFamilyOrAssembly: false, IsFamilyAndAssembly: false } => "protected",
    //         { IsPublic: false, IsAssembly: false, IsFamily: false, IsFamilyOrAssembly: true, IsFamilyAndAssembly: false } => "protected public",
    //         { IsPublic: false, IsAssembly: false, IsFamily: false, IsFamilyOrAssembly: false, IsFamilyAndAssembly: true } => "private protected",
    //         _ => throw new ArgumentException($"{nameof(FieldInfo)}.{nameof(GetSignature)} encountered an unknown visibility for '{fieldInfo.Name}", nameof(fieldInfo))
    //     };

    // }
}