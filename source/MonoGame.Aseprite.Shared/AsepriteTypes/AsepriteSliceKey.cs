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
///     Represents a key instance of a slice in an Aseprite image.
/// </summary>
/// <param name="FrameIndex">
///     The index of the <see cref="AsepriteFrame"/> this
///     <see cref="AsepriteSliceKey"/> is valid starting on.
/// </param>
/// <param name="Bounds">
///     The bounds of the <see cref="AsepriteSlice"/> during this
///     <see cref="AsepriteSliceKey"/>, relative to the bounds of the
///     <see cref="AsepriteFrame"/> is on.
/// </param>
/// <param name="CenterBounds">
///     When the <see cref="AsepriteSliceKey.IsNinePatch"/> value for this
///     <see cref="AsepriteSliceKey"/> instance is <see langword="true"/>,
///     contains the bounds of the center area of the nine-path  for the
///     <see cref="AsepriteSlice"/> during this <see cref="AsepriteSliceKey"/>,
///     relative to the bounds; otherwise, <see langword="null"/>.
/// </param>
/// <param name="Pivot">
///     When the <see cref="AsepriteSliceKey.HasPivot"/> value for this
///     <see cref="AsepriteSliceKey"/> instance is <see langword="true"/>,
///     contains the x- and y-coordinate position of the pivot point for the
///     <see cref="AsepriteSlice"/> during this <see cref="AsepriteSliceKey"/>,
///     relative to the upper-left corner of the bounds; otherwise,
///     <see langword="null"/>.
/// </param>
public sealed record AsepriteSliceKey(int FrameIndex, Rectangle Bounds, Rectangle? CenterBounds = default, Point? Pivot = default)
{

    /// <summary>
    ///     <para>
    ///         Indicates whether this <see cref="AsepriteSliceKey"/> has
    ///         nine-patch.
    ///     </para>
    ///     <para>
    ///         When this returns <see langword="true"/> it guarantees that
    ///         the <see cref="AsepriteSliceKey.CenterBounds"/> value of this
    ///         instance  is not <see langword="null"/>.
    ///     </para>
    /// </summary>
    [MemberNotNullWhen(true, nameof(CenterBounds))]
    public bool IsNinePatch => CenterBounds is not null;

    /// <summary>
    ///     <para>
    ///         Indicates whether this <see cref="AsepriteSliceKey"/> has pivot
    ///         data.
    ///     </para>
    ///     <para>
    ///         When this returns <see langword="true"/> it guarantees that
    ///         the <see cref="AsepriteSliceKey.Pivot"/> value of this instance
    ///         is not <see langword="null"/>.
    ///     </para>
    /// </summary>
    [MemberNotNullWhen(true, nameof(Pivot))]
    public bool HasPivot => Pivot is not null;
}

// /// <summary>
// ///     Represents a key instance of a slice in an Aseprite image.
// /// </summary>
// public sealed class AsepriteSliceKey
// {
//     /// <summary>
//     ///     The index of the frame this key is valid starting on.
//     /// </summary>
//     public int FrameIndex { get; }

//     /// <summary>
//     ///     The rectangular bounds of the slice during this key, relative to the
//     ///     bounds of the frame it is on.
//     /// </summary>
//     public Rectangle Bounds { get; }

//     /// <summary>
//     ///     The width and height extents, in pixels, of the slice during this
//     ///     key.
//     /// </summary>
//     public Point Size => Bounds.Size;

//     /// <summary>
//     ///     The width, in pixels, of the slice during this key.
//     /// </summary>
//     public int Width => Bounds.Width;

//     /// <summary>
//     ///     The height, in pixels, of the slice during this key.
//     /// </summary>
//     public int Height => Bounds.Height;

//     /// <summary>
//     ///     The x- and y-coordinate position of the upper-left corner of the
//     ///     slice during this key, relative to the upper-left corner of the
//     ///     frame it is on.
//     /// </summary>
//     public Point Position => Bounds.Location;

//     /// <summary>
//     ///     The x-coordinate position of the upper-left corner of the slice
//     ///     during this key, relative to the upper-left corner of the frame
//     ///     it is on.
//     /// </summary>
//     public int X => Bounds.X;

//     /// <summary>
//     ///     The y-coordinate position of the upper-left corner of the slice
//     ///     during this key, relative to the upper-left corner of the frame
//     ///     it is on.
//     /// </summary>
//     public int Y => Bounds.Y;

//     /// <summary>
//     ///     The y-coordinate position of the upper-left corner of the slice
//     ///     during this key, relative to the upper-left corner of the frame it
//     ///     is on.
//     /// </summary>
//     public int Top => Bounds.Top;

//     /// <summary>
//     ///     The y-coordinate position of the bottom-right corner of the slice
//     ///     during this key, relative to the upper-left corner of the frame it
//     ///     is on.
//     /// </summary>
//     public int Bottom => Bounds.Bottom;

//     /// <summary>
//     ///     The x-coordinate position of the upper-left corner of the slice
//     ///     during this key, relative to the upper-left corner of the frame it
//     ///     is on.
//     /// </summary>
//     public int Left => Bounds.Left;

//     /// <summary>
//     ///     The x-coordinate position of the bottom-right corner of the slice
//     ///     during this key, relative to the upper-left corner of the frame it
//     ///     is on.
//     /// </summary>
//     public int Right => Bounds.Right;

//     /// <summary>
//     ///     Indicates whether this key has nine-patch values.
//     /// </summary>
//     [MemberNotNullWhen(true, nameof(CenterBounds))]
//     [MemberNotNullWhen(true, nameof(CenterSize))]
//     [MemberNotNullWhen(true, nameof(CenterWidth))]
//     [MemberNotNullWhen(true, nameof(CenterHeight))]
//     [MemberNotNullWhen(true, nameof(CenterPosition))]
//     [MemberNotNullWhen(true, nameof(CenterX))]
//     [MemberNotNullWhen(true, nameof(CenterY))]
//     [MemberNotNullWhen(true, nameof(CenterTop))]
//     [MemberNotNullWhen(true, nameof(CenterBottom))]
//     [MemberNotNullWhen(true, nameof(CenterLeft))]
//     [MemberNotNullWhen(true, nameof(CenterRight))]
//     public bool IsNinePatch => CenterBounds is not null;

//     /// <summary>
//     ///     The rectangular bounds of hte center of the slice during this key,
//     ///     relative to the bounds of the slice during this key, if this key has
//     ///     nine-patch values; otherwise, null.
//     /// </summary>
//     public Rectangle? CenterBounds { get; }

//     /// <summary>
//     ///     The width and height extents, in pixels, of the center bounds of the
//     ///     slice during this key, if this key has nine-patch values; otherwise,
//     //      null.
//     /// </summary>
//     public Point? CenterSize => CenterBounds?.Size;

//     /// <summary>
//     ///     The width, in pixels, of the center bounds of the slice during this
//     ///     key, if this key has nine-patch values; otherwise, null.
//     /// </summary>
//     public int? CenterWidth => CenterBounds?.Width;

//     /// <summary>
//     ///     The height, in pixels, of the center bounds of the slice during this
//     ///     key, if this key has nine-patch values; otherwise, null.
//     /// </summary>
//     public int? CenterHeight => CenterBounds?.Height;

//     /// <summary>
//     ///     The x and y-coordinate position of the upper-left corner of the
//     ///     center bounds of the slice during this key, relative to the
//     ///     upper-left corner of the slice; if this key has nine-patch values;
//     ///     otherwise, null.
//     /// </summary>
//     public Point? CenterPosition => CenterBounds?.Location;

//     /// <summary>
//     ///     The x-coordinate position of the upper-left corner of the center
//     ///     bounds of the slice during this key, relative to the upper-left
//     ///     corner of the slice; if this key has nine-patch values; otherwise,
//     ///     null.
//     /// </summary>
//     public int? CenterX => CenterBounds?.X;

//     /// <summary>
//     ///     The y-coordinate position of the upper-left corner of the center
//     ///     bounds of the slice during this key, relative to the upper-left
//     ///     corner of the slice; if this key has nine-patch values; otherwise,
//     ///     null.
//     /// </summary>
//     public int? CenterY => CenterBounds?.Y;

//     /// <summary>
//     ///     The y-coordinate position of the upper-left corner of the center
//     ///     bounds of the slice during this key, relative to the upper-left
//     ///     corner of the slice; if this key has nine-patch values; otherwise,
//     ///     null.
//     /// </summary>
//     public int? CenterTop => CenterBounds?.Top;

//     /// <summary>
//     ///     The y-coordinate position of the bottom-right corner of the center
//     ///     bounds of the slice during this key, relative to the upper-left
//     ///     corner of the slice; if this key has nine-patch values; otherwise,
//     ///     null.
//     /// </summary>
//     public int? CenterBottom => CenterBounds?.Bottom;

//     /// <summary>
//     ///     The x-coordinate position of the upper-left corner of the center
//     ///     bounds of the slice during this key, relative to the upper-left
//     ///     corner of the slice; if this key has nine-patch values; otherwise,
//     ///     null.
//     /// </summary>
//     public int? CenterLeft => CenterBounds?.Left;

//     /// <summary>
//     ///     The x-coordinate position of the bottom-right corner of the center
//     ///     bounds of the slice during this key, relative to the upper-left
//     ///     corner of the slice; if this key has nine-patch values; otherwise,
//     ///     null.
//     /// </summary>
//     public int? CenterRight => CenterBounds?.Right;

//     /// <summary>
//     ///     Indicates whether this key has pivot values.
//     /// </summary>
//     [MemberNotNullWhen(true, nameof(Pivot))]
//     [MemberNotNullWhen(true, nameof(PivotX))]
//     [MemberNotNullWhen(true, nameof(PivotY))]
//     public bool HasPivot => Pivot is not null;

//     /// <summary>
//     ///     THe x and y-coordinate position of the pivot point for the slice
//     ///     during this key, relative to the upper-left corner of the slice, if
//     ///     this key has pivot values; otherwise, null.
//     /// </summary>
//     public Point? Pivot { get; }

//     /// <summary>
//     ///     The x-coordinate position of the pivot point for the slice during
//     ///     this key, relative to the upper-left corner of the slice, if this
//     ///     key has pivot values; otherwise, null.
//     /// </summary>
//     public int? PivotX => Pivot?.X;

//     /// <summary>
//     ///     The y-coordinate position of the pivot point for the slice during
//     ///     this key, relative to the upper-left corner of the slice, if this
//     ///     key has pivot values; otherwise, null.
//     /// </summary>
//     public int? PivotY => Pivot?.Y;

//     internal AsepriteSliceKey(int frameIndex, Rectangle bounds, Rectangle? centerBounds, Point? pivot)
//     {
//         FrameIndex = frameIndex;
//         Bounds = bounds;
//         CenterBounds = centerBounds;
//         Pivot = pivot;
//     }
// }
