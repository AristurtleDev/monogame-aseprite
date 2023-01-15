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

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a tile with a texture region in a tilemap layer.
/// </summary>
public class Tile
{
    /// <summary>
    ///     Gets the texture region that represents this tile.
    /// </summary>
    public TextureRegion TextureRegion { get; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether this tile should be
    ///     flipped on its vertical axis when rendered.
    /// </summary>
    public bool FlipVertically { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether this tile should be
    ///     flipped on its horizontal axis when rendered.
    /// </summary>
    public bool FlipHorizontally { get; set; }

    /// <summary>
    ///     Gets or Sets the amount of rotation, in radians, to apply to this
    ///     tile when rendered.
    /// </summary>
    public float Rotation { get; set; }

    internal Tile(TextureRegion textureRegion, bool flipVertically, bool flipHorizontally, float rotation) =>
        (TextureRegion, FlipVertically, FlipHorizontally, Rotation) = (textureRegion, flipVertically, flipHorizontally, rotation);
}
