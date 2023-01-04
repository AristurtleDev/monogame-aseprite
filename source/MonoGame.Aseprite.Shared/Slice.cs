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

namespace MonoGame.Aseprite;

/// <summary>
///     Represents a name region of a frame.
/// </summary>
public sealed class Slice
{
    /// <summary>
    ///     The name of this slice.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The color of this slice.
    /// </summary>
    public Color Color { get; }

    /// <summary>
    ///     The index of the frame this slice is valid on.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    ///     The rectangular bounds of this slice relative to the bounds of the
    ///     frame.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     The width and height extents, in pixels, of this slice.
    /// </summary>
    public Point Size => Bounds.Size;

    /// <summary>
    ///     The width, in pixels, of this slice.
    /// </summary>
    public int Width => Bounds.Width;

    /// <summary>
    ///     The height, in pixels, of this slice.
    /// </summary>
    public int Height => Bounds.Height;

    /// <summary>
    ///     The x- and y-coordinate position of the upper-left corner of this
    ///     slice relative to the upper-left corner of the frame.
    /// </summary>
    public Point Position => Bounds.Location;

    /// <summary>
    ///     The x-coordinate position of the upper-left corner of this slice
    ///     relative to the upper-left corner of the frame.
    /// </summary>
    public int X => Bounds.X;

    /// <summary>
    ///     The y-coordinate position of the upper-left corner of this slice
    ///     relative to the upper-left corner of the frame.
    /// </summary>
    public int Y => Bounds.Y;

    /// <summary>
    ///     The y-coordinate position of the upper-left corner of this slice
    ///     relative to the upper-left corner of the frame.
    /// </summary>
    public int Top => Bounds.Top;

    /// <summary>
    ///     The y-coordinate position of the bottom-right corner of this slice
    ///     relative to the upper-left corner of the frame.
    /// </summary>
    public int Bottom => Bounds.Bottom;

    /// <summary>
    ///     The x-coordinate position of the upper-left corner of this slice
    ///     relative to the upper-left corner of the frame.
    /// </summary>
    public int Left => Bounds.Left;

    /// <summary>
    ///     The x-coordinate position of the bottom-right corner of this slice
    ///     relative to the upper-left corner of the frame.
    /// </summary>
    public int Right => Bounds.Right;

    /// <summary>
    ///     Indicates whether this slice is a nine-patch slice.
    /// </summary>
    [MemberNotNullWhen(true, nameof(CenterBounds))]
    [MemberNotNullWhen(true, nameof(CenterSize))]
    [MemberNotNullWhen(true, nameof(CenterWidth))]
    [MemberNotNullWhen(true, nameof(CenterHeight))]
    [MemberNotNullWhen(true, nameof(CenterPosition))]
    [MemberNotNullWhen(true, nameof(CenterX))]
    [MemberNotNullWhen(true, nameof(CenterY))]
    [MemberNotNullWhen(true, nameof(CenterTop))]
    [MemberNotNullWhen(true, nameof(CenterBottom))]
    [MemberNotNullWhen(true, nameof(CenterLeft))]
    [MemberNotNullWhen(true, nameof(CenterRight))]
    public bool IsNinePatch => CenterBounds is not null;

    /// <summary>
    ///     The rectangular bounds of the center of this slice relative to the
    ///     bounds of this slice, if this slice is a nine-patch slice;
    ///     otherwise, null.
    /// </summary>
    public Rectangle? CenterBounds { get; }

    /// <summary>
    ///     The width and height extents, in pixels, of the center bounds of
    ///     this slice, if this slice is a nine-patch slice; otherwise, null.
    /// </summary>
    public Point? CenterSize => CenterBounds?.Size;

    /// <summary>
    ///     The width, in pixels, of the center bounds of this slice, if this
    ///     slice is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterWidth => CenterBounds?.Width;

    /// <summary>
    ///     The height, in pixels, of the center bounds of this slice, if this
    ///     slice is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterHeight => CenterBounds?.Height;

    /// <summary>
    ///     The x- and y-coordinate position of the upper-left corner of the
    ///     center bounds of this slice, relative to the upper-left corner of
    ///     this slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public Point? CenterPosition => CenterBounds?.Location;

    /// <summary>
    ///     The x-coordinate position of the upper-left corner of the center
    ///     bonds of this slice, relative to the upper-left corner of this
    ///     slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterX => CenterBounds?.X;

    /// <summary>
    ///     The y-coordinate position of the upper-left corner of the center
    ///     bonds of this slice, relative to the upper-left corner of this
    ///     slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterY => CenterBounds?.Y;


    /// <summary>
    ///     The y-coordinate position of the upper-left corner of the center
    ///     bonds of this slice, relative to the upper-left corner of this
    ///     slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterTop => CenterBounds?.Top;

    /// <summary>
    ///     The y-coordinate position of the bottom-right corner of the center
    ///     bonds of this slice, relative to the upper-left corner of this
    ///     slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterBottom => CenterBounds?.Bottom;

    /// <summary>
    ///     The x-coordinate position of the upper-left corner of the center
    ///     bonds of this slice, relative to the upper-left corner of this
    ///     slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterLeft => CenterBounds?.Left;

    /// <summary>
    ///     The x-coordinate position of the bottom-right corner of the center
    ///     bonds of this slice, relative to the upper-left corner of this
    ///     slice, if this is a nine-patch slice; otherwise, null.
    /// </summary>
    public int? CenterRight => CenterBounds?.Right;

    /// <summary>
    ///     Indicates whether this slice has pivot data.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Pivot))]
    [MemberNotNullWhen(true, nameof(PivotX))]
    [MemberNotNullWhen(true, nameof(PivotY))]
    public bool HasPivot => Pivot is not null;

    /// <summary>
    ///     The x- and y-coordinate position of the pivot point for this slice,
    ///     relative to the upper-left corner of this slice, if this slice has
    ///     pivot data; otherwise, null.
    /// </summary>
    public Point? Pivot { get; }

    /// <summary>
    ///     The x-coordinate position of the pivot point for this slice,
    ///     relative to the upper-left corner of this slice, if this slice has
    ///     pivot data; otherwise, null.
    /// </summary>
    public int? PivotX => Pivot?.X;

    /// <summary>
    ///     The y-coordinate position of the pivot point for this slice,
    ///     relative to the upper-left corner of this slice, if this slice has
    ///     pivot data; otherwise, null.
    /// </summary>
    public int? PivotY => Pivot?.Y;

    internal Slice(string name, Color color, int frame, Rectangle bounds, Rectangle? center, Point? pivot)
    {
        Name = name;
        Color = color;
        FrameIndex = frame;
        Bounds = bounds;
        CenterBounds = center;
        Pivot = pivot;
    }
}
