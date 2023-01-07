/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents an animation tag in an Aseprite image.
/// </summary>
/// <param name="From">
///     The starting index of the <see cref="AsepriteFile"/> for the animation
///     defined by this <see cref="AsepriteTag"/>.
/// </param>
/// <param name="To">
///     The end index of the <see cref="AsepriteFile"/> for the animation
///     defined by this <see cref="AsepriteTag"/>.
/// </param>
/// <param name="Direction">
///     A <see cref="LoopDirection"/> value that defines the direction of the
///     animation defined by this <see cref="AsepriteTag"/>.
/// </param>
/// <param name="Color">
///     <para>
///         A <see cref="Microsoft.Xna.Framework.Color"/> value that represents
///         the color of this <see cref="AsepriteTag"/>.
///     </para>
///     <para>
///         Per Aseprite File Specs for version 1.3+, the userdata color of a
///         tag should be used instead of this value.  If the Aseprite file
///         was made with version 1.2 or lower, then the userdata will have
///         no color value, in which case, this value should be used.
///     </para>
/// </param>
/// <param name="Name">
///     The name of this <see cref="AsepriteTag"/>.
/// </param>
/// <param name="UserData">
///     The custom user data set for this <see cref="AsepriteLayer"/> in
///     Aseprite, if any was set; otherwise, <see langword="null"/>.
/// </param>
public sealed record AsepriteTag(int From, int To, LoopDirection Direction, Color Color, string Name, AsepriteUserData? UserData = default)
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
// ///     Represents an animation tag in an Aseprite image.
// /// </summary>
// public sealed class AsepriteTag
// {
//     private Color _tagColor;

//     /// <summary>
//     ///     The starting index of the frame for the animation of this tag.
//     /// </summary>
//     public int From { get; }

//     /// <summary>
//     ///     The ending index of the frame for the animation of this tag.
//     /// </summary>
//     public int To { get; }

//     /// <summary>
//     ///     The animation loop direction of the animation of this tag.
//     /// </summary>
//     public LoopDirection Direction { get; }

//     /// <summary>
//     ///     The name of this tag.
//     /// </summary>
//     public string Name { get; }

//     /// <summary>
//     ///     The custom user data that was set for this tag in Aseprite.
//     /// </summary>
//     public AsepriteUserData UserData { get; } = new();

//     /// <summary>
//     ///     The color value of this tag.
//     /// </summary>
//     public Color Color => UserData.Color ?? _tagColor;

//     internal AsepriteTag(ushort from, ushort to, LoopDirection direction, Color color, string name)
//     {
//         From = from;
//         To = to;
//         Direction = direction;
//         _tagColor = color;
//         Name = name;
//     }
// }
