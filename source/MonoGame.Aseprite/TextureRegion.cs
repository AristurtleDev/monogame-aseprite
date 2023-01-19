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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a named rectangular region that represents the location and extents of a region within a
///     <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/>.
/// </summary>
public class TextureRegion
{
    /// <summary>
    ///     Gets the name of this <see cref="TextureRegion"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that this <see cref="TextureRegion"/>
    ///     represents.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the rectangular bounds that define the location and the width and height extents, in pixels, of the
    ///     region within the <see cref="TextureRegion.Texture"/> that is represented by this
    ///     <see cref="TextureRegion"/>.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextureRegion"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to give this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that this <see cref="TextureRegion"/> will
    ///     represent.
    /// </param>
    /// <param name="x">
    ///     The x-coordinate location of the upper-left corner of the bounds for this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="y">
    ///     The y-coordinate location of the upper-left corner of the bounds for this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="width">
    ///     The width, in pixels, of the bounds for this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="height">
    ///     The height, in pixels, of the bounds for this <see cref="TextureRegion"/>.
    /// </param>
    public TextureRegion(string name, Texture2D texture, int x, int y, int width, int height)
        : this(name, texture, new Rectangle(x, y, width, height)) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextureRegion"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to give this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that this <see cref="TextureRegion"/> will
    ///     represent.
    /// </param>
    /// <param name="location">
    ///     The x- and y-coordinate location of the upper-left corner of the bounds for this
    ///     <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="size">
    ///     The width and height extents, in pixels, of the bounds for this <see cref="TextureRegion"/>.
    /// </param>
    public TextureRegion(string name, Texture2D texture, Point location, Point size)
        : this(name, texture, new Rectangle(location, size)) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextureRegion"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to give this <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that this <see cref="TextureRegion"/> will
    ///     represent.
    /// </param>
    /// <param name="bounds">
    ///     The rectangular bounds that define the location and the width and height extents, in pixels, of the region
    ///     region within the <paramref name="texture"/> that is represented by this <see cref="TextureRegion"/>.
    /// </param>
    public TextureRegion(string name, Texture2D texture, Rectangle bounds) =>
        (Name, Texture, Bounds) = (name, texture, bounds);
}
