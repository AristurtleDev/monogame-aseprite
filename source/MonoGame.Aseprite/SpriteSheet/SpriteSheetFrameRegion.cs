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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

/// <summary>
///     Represents a name rectangular region of a
///     <see cref="TextureRegion"/>.
/// </summary>
/// <param name="Name">
///     The name of this <see cref="SpriteSheetFrameRegion"/>.
/// </param>
/// <param name="Bounds">
///     The rectangular bounds of this <see cref="SpriteSheetFrameRegion"/>
///     relative to the bounds of the <see cref="TextureRegion"/> it is in.
/// </param>
/// <param name="Color">
///     The color of this <see cref="SpriteSheetFrameRegion"/>.
/// </param>
/// <param name="CenterBounds">
///     When the <see cref="SpriteSheetFrameRegion.IsNinePatch"/> value of this
///     instance is <see langword="true"/>, contains the rectangular bounds of
///     the center area of this <see cref="SpriteSheetFrameRegion"/> relative to
///     the <see cref="SpriteSheetFrameRegion.Bounds"/>; otherwise,
///     <see langword="null"/>.
/// </param>
/// <param name="Pivot">
///     When the <see cref="SpriteSheetFrameRegion.HasPivot"/> value of this
///     instance is <see langword="true"/>, contains the x- and y-coordinate
///     position of the pivot point of this <see cref="SpriteSheetFrameRegion"/>
///     relative ot the upper-left corner of the
///     <see cref="SpriteSheetFrameRegion.Bounds"/>; otherwise,
///     <see langword="null"/>.
/// </param>
public sealed record SpriteSheetFrameRegion(string Name, Rectangle Bounds, Color Color, Rectangle? CenterBounds = default, Point? Pivot = default)
{
    /// <summary>
    ///     <para>
    ///         Indicates whether this <see cref="SpriteSheetFrameRegion"/>
    ///         has as <see cref="SpriteSheetFrameRegion.CenterBounds"/> value.
    ///     </para>
    ///     <para>
    ///         When this returns <see langword="true"/> it guarantees that
    ///         the <see cref="SpriteSheetFrameRegion.CenterBounds"/> value of
    ///         this instance is not <see langword="null"/>.
    ///     </para>
    /// </summary>
    [MemberNotNullWhen(true, nameof(CenterBounds))]
    public bool IsNinePatch => CenterBounds is not null;

    /// <summary>
    ///     <para>
    ///         Indicates whether this <see cref="SpriteSheetFrameRegion"/>
    ///         has as <see cref="SpriteSheetFrameRegion.Pivot"/> value.
    ///     </para>
    ///     <para>
    ///         When this returns <see langword="true"/> it guarantees that
    ///         the <see cref="SpriteSheetFrameRegion.Pivot"/> value of this
    ///         instance is not <see langword="null"/>.
    ///     </para>
    /// </summary>
    [MemberNotNullWhen(true, nameof(Pivot))]
    public bool HasPivot => Pivot is not null;
}
