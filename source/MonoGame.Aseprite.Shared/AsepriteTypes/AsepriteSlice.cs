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
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a named region slice in an Aseprite image.
/// </summary>
/// <param name="IsNinePatch">
///     Indicates whether the <see cref="AsepriteSliceKey"/> elements of this
///     <see cref="AsepriteSlice"/> have nine-patch data.
/// </param>
/// <param name="HasPivot">
///     Indicates whether the <see cref="AsepriteSliceKey"/> elements of this
///     <see cref="AsepriteSlice"/> have pivot data.
/// </param>
/// <param name="Name">
///     The name of this <see cref="AsepriteSlice"/>.
/// </param>
/// <param name="Keys">
///     An <see cref="ImmutableArray{T}"/> of <see cref="AsepriteSliceKey"/>
///     elements for this <see cref="AsepriteSlice"/>.
/// </param>
/// <param name="UserData">
///     The custom user data set for this <see cref="AsepriteSlice"/> in
///     Aseprite, if any was set; otherwise, <see langword="null"/>.
/// </param>
public sealed record AsepriteSlice(bool IsNinePatch, bool HasPivot, string Name, ImmutableArray<AsepriteSliceKey> Keys, AsepriteUserData? UserData = default)
{
    /// <summary>
    ///     <para>
    ///         Indicates whether this <see cref="AsepriteSlice"/> has user
    ///         data.
    ///     </para>
    ///     <para>
    ///         When this returns <see langword="true"/> it guarantees that
    ///         the <see cref="AsepriteSlice.UserData"/> value of this instance
    ///         is not <see langword="null"/>.
    ///     </para>
    /// </summary>
    [MemberNotNullWhen(true, nameof(UserData))]
    public bool HasUserData => UserData is not null;
}

// /// <summary>
// ///     Represents a named region slice in an Aseprite image.
// /// </summary>
// public sealed class AsepriteSlice
// {
//     /// <summary>
//     ///     Gets a value that indicates whether the
//     ///     <see cref="AsepriteSliceKey"/> elements of this
//     ///     <see cref="AsepriteSlice"/> have nine-patch values.
//     /// </summary>
//     public bool IsNinePatch { get; }

//     /// <summary>
//     ///     Gets a value that indicates whether the
//     ///     <see cref="AsepriteSliceKey"/> elements of this
//     ///     <see cref="AsepriteSlice"/> have pivot values.
//     /// </summary>
//     public bool HasPivot { get; }

//     /// <summary>
//     ///     Gets the name of this <see cref="AsepriteSlice"/>.
//     /// </summary>
//     public string Name { get; }

//     /// <summary>
//     ///     A read-only collection of all slice keys for this slice.
//     /// </summary>
//     public ReadOnlyCollection<AsepriteSliceKey> Keys { get; }

//     /// <summary>
//     ///     Gets the <see cref="UserData"/> that was set for this
//     ///     <see cref="AsepriteSlice"/> in Aseprite.
//     /// </summary>
//     public AsepriteUserData UserData { get; } = new();


//     internal AsepriteSlice(string name, bool isNine, bool hasPivot, List<AsepriteSliceKey> keys)
//     {
//         Name = name;
//         IsNinePatch = isNine;
//         HasPivot = hasPivot;
//         Keys = keys.AsReadOnly();
//     }
// }
