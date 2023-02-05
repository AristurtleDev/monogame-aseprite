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
///     Defines a named rectangular region that represents the location and extents of a region within a source texture.
/// </summary>
public class TextureRegion
{
    /// <summary>
    ///     Gets the name assigned to this <see cref="TextureRegion"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the source texture used by this <see cref="TextureRegion"/>.
    /// </summary>
    public Texture2D Texture { get; }

    public bool IsDisposed { get; private set; }

    /// <summary>
    ///     Gets the rectangular bounds that define the location and width and height extents, in pixels, of the region
    ///     within the source texture that is represented by this <see cref="TextureRegion"/>.
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TextureRegion"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="TextureRegion"/>.
    /// </param>
    /// <param name="texture">
    ///     The source texture image this region is from.
    /// </param>
    /// <param name="bounds">
    ///     The rectangular bounds of this region within the source texture.
    /// </param>
    public TextureRegion(string name, Texture2D texture, Rectangle bounds) =>
        (Name, Texture, Bounds) = (name, texture, bounds);
}
