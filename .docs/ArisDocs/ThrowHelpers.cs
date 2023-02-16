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

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ArisDocs;

internal static class ThrowHelpers
{
    internal static void ThrowIfParameterNull([NotNull] object? param, string paramName)
    {
        if (param is null)
        {
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null");
        }
    }

    internal static void ThrowIfElementTypeNull([NotNull] Type? elementType, string typeName, string paramName)
    {
        if (elementType is null)
        {
            string message = $"{nameof(Type)}.{nameof(Type.HasElementType)} was true but {nameof(Type)}.{nameof(Type.GetElementType)} returned null for type: '{typeName}'";
            throw new ArgumentException(message, paramName);
        }
    }

    internal static void ThrowIfFullNameNull([NotNull] string? fullName, string objName, string paramName)
    {
        if (fullName is null)
        {
            string message = $"{nameof(Type)}.{nameof(Type.FullName)} is null for type: '{objName}'";
            throw new ArgumentException(message, paramName);
        }
    }

    internal static void ThrowIfDeclaringTypeNull([NotNull] Type? declaringType, string typeName, string paramName)
    {
        if (declaringType is null)
        {
            string message = $"{nameof(Type)}.{nameof(Type.DeclaringType)} is null for type: {typeName}";
            throw new ArgumentException(message, paramName);
        }
    }

    internal static void ThrowIfFileDoesntExist(string path, string paramName)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Unable to locate the file '{path}'. Please check the file path you provided for '{paramName}'.");
        }
    }
}