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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a layer in an Aseprite image.
/// </summary>
/// <param name="IsVisible">
///     Indicates whether the <see cref="AsepriteCel"/> elements that are on
///     this <see cref="AsepriteLayer"/> are visible.
/// </param>
/// <param name="IsBackground">
///     Indicates whether this <see cref="AsepriteLayer"/> was marked as the
///     background layer in the Aseprite UI.
/// </param>
/// <param name="IsReference">
///     Indicates whether this <see cref="AsepriteLayer"/> was marked as a
///     reference layer in the Aseprite UI.
/// </param>
/// <param name="BlendMode">
///     A <see cref="MonoGame.Aseprite.BlendMode"/> value that defines the type
///     of pixel blending to use when the <see cref="AsepriteCel"/> elements on
///     this <see cref="AsepriteLayer"/> are blended with those below them.
/// </param>
/// <param name="Opacity">
///     The opacity level of this <see cref="AsepriteLayer"/>.
/// </param>
/// <param name="Name">
///     The name of this <see cref="AsepriteLayer"/>.
/// </param>
/// <param name="UserData">
///     The custom user data set for this <see cref="AsepriteLayer"/> in
///     Aseprite, if any was set; otherwise, <see langword="null"/>.
/// </param>
public abstract record AsepriteLayer(bool IsVisible, bool IsBackground, bool IsReference, BlendMode BlendMode, int Opacity, string Name, AsepriteUserData? UserData = default)
{
    /// <summary>
    ///     <para>
    ///         Indicates whether this <see cref="AsepriteLayer"/> has user
    ///         data.
    ///     </para>
    ///     <para>
    ///         When this returns <see langword="true"/> it guarantees that
    ///         the <see cref="AsepriteLayer.UserData"/> value of this instance
    ///         is not <see langword="null"/>.
    ///     </para>
    /// </summary>
    [MemberNotNullWhen(true, nameof(UserData))]
    public bool HasUserData => UserData is not null;
}

// /// <summary>
// ///     Represents a layer in an Aseprite image.
// /// </summary>
// public abstract class AsepriteLayer
// {
//     /// <summary>
//     ///     Gets a value that indicates whether the <see cref="AsepriteCel"/>
//     ///     elements that are on this <see cref="AsepriteLayer"/> are visible.
//     /// </summary>
//     public bool IsVisible { get; }

//     /// <summary>
//     ///     Gets a value that indicates whether this <see cref="AsepriteLayer"/>
//     ///     was marked as the background in Aseprite.
//     /// </summary>
//     public bool IsBackground { get; }

//     /// <summary>
//     ///     Gets a value that indicates whether this <see cref="AsepriteLayer"/>
//     ///     is a reference layer.
//     /// </summary>
//     public bool IsReference { get; }

//     /// <summary>
//     ///     Gets a <see cref="BlendMode"/> value that defines the type of
//     ///     pixel blending to use when the <see cref="AsepriteCel"/> elements
//     ///     on this <see cref="AsepriteLayer"/> are blended with those below
//     ///     them.
//     /// </summary>
//     public BlendMode BlendMode { get; }

//     /// <summary>
//     ///     Gets the opacity level of this <see cref="AsepriteLayer"/>.
//     /// </summary>
//     public int Opacity { get; }

//     /// <summary>
//     ///     Gets the name of this <see cref="AsepriteLayer"/>.
//     /// </summary>
//     public string Name { get; }

//     /// <summary>
//     ///     Gets the <see cref="UserData"/> that was set for this
//     ///     <see cref="AsepriteLayer"/> in Aseprite.
//     /// </summary>
//     public AsepriteUserData UserData { get; } = new();

//     internal AsepriteLayer(bool isVisible, bool isBackground, bool isReference, BlendMode blend, int opacity, string name) =>
//         (IsVisible, IsBackground, IsReference, BlendMode, Opacity, Name) = (isVisible, isBackground, isReference, blend, opacity, name);
// }
