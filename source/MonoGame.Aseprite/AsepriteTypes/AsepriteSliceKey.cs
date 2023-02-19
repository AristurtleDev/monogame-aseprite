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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Defines the values of a key for a <see cref="AsepriteSlice"/>.
/// </summary>
public sealed class AsepriteSliceKey
{
    /// <summary>
    ///     Gets the index of the <see cref="AsepriteFrame"/> this <see cref="AsepriteSliceKey"/> is valid starting on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    ///     Gets the rectangular bounds of the <see cref="AsepriteSlice"/> during this <see cref="AsepriteSliceKey"/>.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Gets the rectangular bounds of the center rectangle of the <see cref="AsepriteSlice"/>, relative to the 
    ///     <see cref="Bounds"/>, during this <see cref="AsepriteSliceKey"/>, if the <see cref="AsepriteSlice"/> is a 
    ///     nine-patch slice, otherwise, <see langword="null"/>.
    /// </summary>
    public Rectangle? CenterBounds { get; }

    /// <summary>
    ///     Gets the x- and y-coordinate location of the pivot point of the <see cref="AsepriteSlice"/>, relative to the
    ///     <see cref="Bounds"/>, during this <see cref="AsepriteSliceKey"/>, if the <see cref="AsepriteSlice"/> contains 
    ///     pivot values; otherwise, <see langword="null"/>.
    /// </summary>
    public Point? Pivot { get; }

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AsepriteSliceKey"/> is for a nine-patch 
    ///     <see cref="AsepriteSlice"/>.  When <see langword="true"/>, guarantees that the <see cref="CenterBounds"/>
    ///     value is not <see langword="null"/>.
    /// </summary>
    [MemberNotNullWhen(true, nameof(CenterBounds))]
    public bool IsNinePatch => CenterBounds is not null;

    /// <summary>
    ///     Gets a value that indicates if this <see cref="AsepriteSliceKey"/> contains pivot values.  When 
    ///     <see langword="true"/>, guarantees that the <see cref="Pivot"/> value is not <see langword="null"/>.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Pivot))]
    public bool HasPivot => Pivot is not null;

    internal AsepriteSliceKey(int frameIndex, Rectangle bounds, Rectangle? centerBounds, Point? pivot) =>
        (FrameIndex, Bounds, CenterBounds, Pivot) = (frameIndex, bounds, centerBounds, pivot);
}
