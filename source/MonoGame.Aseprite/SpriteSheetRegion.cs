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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public sealed class SpriteSheetRegion
{
    /// <summary>
    ///     Gets the name of this <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="Texture2D"/> used by this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the x-coordinate location of the upper-left corner of this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public int X { get; }

    /// <summary>
    ///     Gets the y-coordinate location of the upper-left corner of this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public int Y { get; }

    /// <summary>
    ///     Gets the width, in pixels, of this <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Gets the height, in pixels, of this <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public int Height { get; }

    /// <summary>
    ///     Gets the rectangular bounds of this <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public Rectangle Bounds => new(X, Y, Width, Height);

    /// <summary>
    ///     Gets the width and height extents, in pixels of this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </summary>
    public Point Size => new(Width, Height);

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheetRegion"/>
    ///     class with the given name, and bounds that are equal to the bounds
    ///     of the given texture.
    /// </summary>
    /// <param name="name">
    ///     The name of this <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Texture2D"/> to be used by this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </param>
    public SpriteSheetRegion(string name, Texture2D texture)
        : this(name, texture, 0, 0, texture.Width, texture.Height) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheetRegion"/>
    ///     class with the name and bounds provided.
    /// </summary>
    /// <param name="name">
    ///     The name of this <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Texture2D"/> to be used by this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="region">
    ///     The bounds of this <see cref="SpriteSheetRegion"/>.
    /// </param>
    public SpriteSheetRegion(string name, Texture2D texture, Rectangle region)
        : this(name, texture, region.X, region.Y, region.Width, region.Height) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheetRegion"/>
    ///     class with the name and bounds equal to the values provided.
    /// </summary>
    /// <param name="name">
    ///     The name of this <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Texture2D"/> to be used by this
    ///     <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="x">
    ///     The x-coordinate location of the upper-left corner of the bounds
    ///     for this <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="y">
    ///     The x-coordinate location of the upper-left corner of the bounds
    ///     for this <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="width">
    ///     The width, in pixels, of the bounds of this
    ///     <see cref="SpriteSheetRegion"/>
    /// </param>
    /// <param name="height">
    ///     The height, in pixels, of the bounds of this
    ///     <see cref="SpriteSheetRegion"/>
    /// </param>
    public SpriteSheetRegion(string name, Texture2D texture, int x, int y, int width, int height) =>
        (Name, Texture, X, Y, Width, Height) = (name, texture, x, y, width, height);

}
