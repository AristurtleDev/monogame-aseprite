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

namespace ArisDocs.Extensions;

internal static class FieldInfoExtensions
{
    public static string GetXmlName(this FieldInfo fieldInfo)
    {
        ThrowHelpers.ThrowIfDeclaringTypeNull(fieldInfo.DeclaringType, fieldInfo.Name, nameof(fieldInfo));
        string xmlTypeName = fieldInfo.DeclaringType.GetXmlName();

        //  Use [2..] to remove the "T:" from the type name string
        return $"P:{xmlTypeName[2..]}.{fieldInfo.Name}";
    }

    public static string GetSignature(this FieldInfo fieldInfo)
    {
        string visibility = fieldInfo switch
        {
            { IsPublic: true, IsAssembly: false, IsFamily: false, IsFamilyOrAssembly: false, IsFamilyAndAssembly: false } => "public",
            { IsPublic: false, IsAssembly: true, IsFamily: false, IsFamilyOrAssembly: false, IsFamilyAndAssembly: false } => "internal",
            { IsPublic: false, IsAssembly: false, IsFamily: true, IsFamilyOrAssembly: false, IsFamilyAndAssembly: false } => "protected",
            { IsPublic: false, IsAssembly: false, IsFamily: false, IsFamilyOrAssembly: true, IsFamilyAndAssembly: false } => "protected public",
            { IsPublic: false, IsAssembly: false, IsFamily: false, IsFamilyOrAssembly: false, IsFamilyAndAssembly: true } => "private protected",
            _ => throw new ArgumentException($"{nameof(FieldInfo)}.{nameof(GetSignature)} encountered an unknown visibility for '{fieldInfo.Name}", nameof(fieldInfo))
        };

        string? isStatic = fieldInfo.IsStatic ? "static" : default;
        string? isReadOnly = fieldInfo.IsInitOnly ? "readonly" : default;
        string fieldType = fieldInfo.FieldType.ToString();

        return $"{visibility}{isStatic}{isReadOnly}{fieldType}{fieldInfo.Name}";

    }
}