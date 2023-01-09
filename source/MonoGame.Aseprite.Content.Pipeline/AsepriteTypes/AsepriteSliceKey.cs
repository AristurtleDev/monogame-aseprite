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

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteSliceKey
{
    internal int FrameIndex { get; set; }
    internal Rectangle Bounds { get; set; }
    internal Rectangle? CenterBounds { get; set; }
    internal Point? Pivot { get; set; }

    [MemberNotNullWhen(true, nameof(CenterBounds))]
    internal bool IsNinePatch => CenterBounds is not null;

    [MemberNotNullWhen(true, nameof(Pivot))]
    internal bool HasPivot => Pivot is not null;

    internal AsepriteSliceKey(int frameIndex, Rectangle bounds, Rectangle? centerBounds, Point? pivot) =>
        (FrameIndex, Bounds, CenterBounds, Pivot) = (frameIndex, bounds, centerBounds, pivot);
}

// /// <summary>
// ///     Represents a key instance of a slice in an Aseprite image.
// /// </summary>
// /// <param name="FrameIndex">
// ///     The index of the <see cref="AsepriteFrame"/> this
// ///     <see cref="AsepriteSliceKey"/> is valid starting on.
// /// </param>
// /// <param name="Bounds">
// ///     The bounds of the <see cref="AsepriteSlice"/> during this
// ///     <see cref="AsepriteSliceKey"/>, relative to the bounds of the
// ///     <see cref="AsepriteFrame"/> is on.
// /// </param>
// /// <param name="CenterBounds">
// ///     When the <see cref="AsepriteSliceKey.IsNinePatch"/> value for this
// ///     <see cref="AsepriteSliceKey"/> instance is <see langword="true"/>,
// ///     contains the bounds of the center area of the nine-path  for the
// ///     <see cref="AsepriteSlice"/> during this <see cref="AsepriteSliceKey"/>,
// ///     relative to the bounds; otherwise, <see langword="null"/>.
// /// </param>
// /// <param name="Pivot">
// ///     When the <see cref="AsepriteSliceKey.HasPivot"/> value for this
// ///     <see cref="AsepriteSliceKey"/> instance is <see langword="true"/>,
// ///     contains the x- and y-coordinate position of the pivot point for the
// ///     <see cref="AsepriteSlice"/> during this <see cref="AsepriteSliceKey"/>,
// ///     relative to the upper-left corner of the bounds; otherwise,
// ///     <see langword="null"/>.
// /// </param>
// public sealed record AsepriteSliceKey(int FrameIndex, Rectangle Bounds, Rectangle? CenterBounds = default, Point? Pivot = default)
// {

//     /// <summary>
//     ///     <para>
//     ///         Indicates whether this <see cref="AsepriteSliceKey"/> has
//     ///         nine-patch.
//     ///     </para>
//     ///     <para>
//     ///         When this returns <see langword="true"/> it guarantees that
//     ///         the <see cref="AsepriteSliceKey.CenterBounds"/> value of this
//     ///         instance  is not <see langword="null"/>.
//     ///     </para>
//     /// </summary>
//     [MemberNotNullWhen(true, nameof(CenterBounds))]
//     public bool IsNinePatch => CenterBounds is not null;

//     /// <summary>
//     ///     <para>
//     ///         Indicates whether this <see cref="AsepriteSliceKey"/> has pivot
//     ///         data.
//     ///     </para>
//     ///     <para>
//     ///         When this returns <see langword="true"/> it guarantees that
//     ///         the <see cref="AsepriteSliceKey.Pivot"/> value of this instance
//     ///         is not <see langword="null"/>.
//     ///     </para>
//     /// </summary>
//     [MemberNotNullWhen(true, nameof(Pivot))]
//     public bool HasPivot => Pivot is not null;
// }
