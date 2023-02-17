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
using System.Reflection;
using System.Text;

namespace ArisDocs.Extensions;

public static class PropertyInfoExtensions
{
    public static string GetXmlName(this PropertyInfo propertyInfo)
    {
        ThrowHelpers.ThrowIfDeclaringTypeNull(propertyInfo.DeclaringType, propertyInfo.Name, nameof(propertyInfo));
        string xmlTypeName = propertyInfo.DeclaringType.GetXmlName();

        //  Use [2..] to remove the "T:" from the type name string
        return $"P:{xmlTypeName[2..]}.{propertyInfo.Name}";
    }

public static string GetSignature(this PropertyInfo propertyInfo)
{
    CodeMemberProperty memberProperty = new();
    memberProperty.Name = propertyInfo.Name;
    memberProperty.Type = new CodeTypeReference(propertyInfo.PropertyType);

    bool isPublic = false;
    bool isVirtual = false;
    if (propertyInfo.GetGetMethod() is MethodInfo getMethod)
    {
        memberProperty.HasGet = true;
        isPublic = getMethod.IsPublic;
        isVirtual = getMethod.IsVirtual;
    }

    if (propertyInfo.GetSetMethod() is MethodInfo setMethod)
    {
        memberProperty.HasSet = true;
        if (!isPublic) isPublic = setMethod.IsPublic;
        if (!isVirtual) isVirtual = setMethod.IsVirtual;
    }

    //  Set initial attributes this way so that the public modifier appears correctly.
    memberProperty.Attributes = ~MemberAttributes.AccessMask & ~MemberAttributes.ScopeMask;

    memberProperty.Attributes |= isPublic ? MemberAttributes.Public : MemberAttributes.Private;
    memberProperty.Attributes |= !isVirtual ? MemberAttributes.Final : 0;

    StringBuilder sb = new();
    using StringWriter writer = new(sb);
    CodeGeneratorOptions options = new();
    options.BracingStyle = "C";
    options.IndentString = "";
    CodeDomProvider.CreateProvider("c#").GenerateCodeFromMember(memberProperty, writer, options);

    string signature = sb.ToString();
    signature = signature.Replace(Environment.NewLine, " ")
                            .Replace(" get { }", " get;")
                            .Replace(" set { }", " set;");
    return signature;
}
}