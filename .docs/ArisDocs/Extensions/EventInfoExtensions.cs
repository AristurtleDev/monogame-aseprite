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

namespace ArisDocs.Extensions;

internal static class EventInfoExtensions
{
    public static string GetXmlName(this EventInfo eventInfo)
    {
        ThrowHelpers.ThrowIfDeclaringTypeNull(eventInfo.DeclaringType, eventInfo.Name, nameof(eventInfo));
        string xmlTypeName = eventInfo.DeclaringType.GetXmlName();

        //  Use [2..] to remove the "T:" from the type name string
        return $"P:{xmlTypeName[2..]}.{eventInfo.Name}";
    }

    // public static string GetSignature(this EventInfo eventInfo)
    // {
    //     List<string> modifiers = new();

    //     //  Since modifiers cannot be placed on the event accessor declarations (CS1609)
    //     //  Both the add and remove methods will contain the same information for visibility,
    //     //  and static/non-static, etc.  So we only need to use one, in this case, the add method
    //     if (!eventInfo.TryGetAddMethod(out MethodInfo? addMethod))
    //     {
    //         throw new ArgumentException($"{nameof(EventInfo)}.{nameof(GetSignature)} was unable to get the add method;");
    //     }

    //     //  "public" | "private" | "internal" | "protected" | "protected public" | "private protected"
    //     modifiers.Add(addMethod.GetVisibility());

    //     //  "static"
    //     if(addMethod.IsStatic) modifiers.Add("static");

    //     if(addMethod.IsAbstract)
    //     {
    //         if(addMethod.IsVirtual) modifiers.Add("virtual");
    //         else modifiers.Add("abstract");
    //     }



    //     Type? type = eventInfo.EventHandlerType;
    //     bool isNullable = Nullable.GetUnderlyingType(type) != null;
    //     return $"{modifiers} {eventInfo.EventHandlerType?.ToString()}{(isNullable ? "?" : default)} {eventInfo.Name}";

    //     // string visibility = addMethod.GetVisibility();
    //     // string? isStatic = addMethod.IsStatic ? "static" : default;
    //     // string? isVirtual = addMethod.IsVirtual ? "virtual" : default;
    //     // string? isAbstract = addMethod.IsAbstract ? "abstract" : default;
    //     // string? typeName = eventInfo.EventHandlerType?.ToString();



    //     // return $"{visibility}{isStatic}{isVirtual}{isAbstract}{typeName}{eventInfo.Name}";

    // }

    public static bool IsPublic(this EventInfo eventInfo)
    {
        if (eventInfo.TryGetAddMethod(out MethodInfo? addMethod))
        {
            return addMethod.IsPublic;
        }
        return false;
    }

    private static bool TryGetAddMethod(this EventInfo eventInfo, [NotNullWhen(true)] out MethodInfo? methodInfo)
    {
        try
        {
            methodInfo = eventInfo.GetAddMethod(nonPublic: true);
            return methodInfo is not null;
        }
        catch (MethodAccessException)
        {
            methodInfo = default;
            return false;
        }
    }
}