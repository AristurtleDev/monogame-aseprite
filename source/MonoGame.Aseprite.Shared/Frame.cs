/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

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
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite;

/// <summary>
///     Represents a frame within a spritesheet.
/// </summary>
public class Frame
{
    /// <summary>
    ///     The rectangular bounds that define x- and y-coordinate location and
    ///     the width and height extents of this <see cref="Frame"/>.
    /// </summary>
    public Rectangle SourceRectangle { get; }

    /// <summary>
    ///     The width and height extents, in pixels, of this
    ///     <see cref="Frame"/>.
    /// </summary>
    public Point Size => SourceRectangle.Size;

    /// <summary>
    ///     The width, in pixels, of this <see cref="Frame"/>.
    /// </summary>
    public int Width => SourceRectangle.Width;

    /// <summary>
    ///     The height, in pixels, of this <see cref="Frame"/>.
    /// </summary>
    public int Height => SourceRectangle.Height;

    /// <summary>
    ///     The x- and y-coordinate location of the upper-left corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public Point Location => SourceRectangle.Location;

    /// <summary>
    ///     The x-coordinate location of the upper-left corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public int X => SourceRectangle.X;

    /// <summary>
    ///     The y-coordinate location of the upper-left corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public int Y => SourceRectangle.Y;

    /// <summary>
    ///     The y-coordinate location of the upper-left corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public int Top => SourceRectangle.Top;

    /// <summary>
    ///     The y-coordinate location of the bottom-right corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public int Bottom => SourceRectangle.Bottom;

    /// <summary>
    ///     The x-coordinate location of the upper-left corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public int Left => SourceRectangle.Left;

    /// <summary>
    ///     The x-coordinate location of the bottom-right corner of this
    ///     <see cref="Frame"/> within the spritesheet it is in.
    /// </summary>
    public int Right => SourceRectangle.Right;

    /// <summary>
    ///     The duration of this <see cref="Frame"/> when used in an animation.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Frame"/> class.
    /// </summary>
    /// <param name="sourceRectangle">
    ///     The rectangular bounds of this <see cref="Frame"/> within the
    ///     spritesheet.
    /// </param>
    /// <param name="duration">
    ///     The duration of this <see cref="Frame"/> when used in an animation.
    /// </param>
    public Frame(Rectangle sourceRectangle, TimeSpan duration)
    {
        SourceRectangle = sourceRectangle;
        Duration = duration;
    }
}
