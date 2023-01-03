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
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a tileset used by tilemap cels in an Aseprite image.
/// </summary>
public sealed class AsepriteTileset
{
    private Color[] _pixels;

    /// <summary>
    ///     The ID of this tileset.
    /// </summary>
    public int ID { get; }

    /// <summary>
    ///     The total number of tiles in this tileset.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    ///     The width and height extents, in pixels, of each tile in this
    ///     tileset
    /// </summary>
    public Point TileSize { get; }

    /// <summary>
    ///     The width, in pixels, of each tile in this tileset.
    /// </summary>
    public int TileWidth => TileSize.X;

    /// <summary>
    ///     The height, in pixels, of each tile in this tileset.
    /// </summary>
    public int TileHeight => TileSize.Y;

    /// <summary>
    ///     The name of this tileset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     A read-only collection of the pixel data for this tileset. Pixels
    ///     are in order of top-to-bottom, read left-to-right.
    /// </summary>
    public ReadOnlyCollection<Color> Pixels => Array.AsReadOnly<Color>(_pixels);

    /// <summary>
    ///     The total number of pixels in this tileset.
    /// </summary>
    public int PixelCount => _pixels.Length;

    /// <summary>
    ///     The width and height extents, in pixels, of this tileset.
    /// </summary>
    public Point Size { get; }

    /// <summary>
    ///     The width, in pixels, of this tileset.
    /// </summary>
    public int Width => Size.X;

    /// <summary>
    ///     THe height, in pixels, of this tileset.
    /// </summary>
    public int Height => Size.Y;

    /// <summary>
    ///     Returns a new array containing the color data for all pixels that
    ///     make up the image for the tile at the specified index from this
    ///     tileset. Pixel order is from top-to-bottom, read left-to-right
    /// </summary>
    /// <param name="tileId">
    ///     The index of the tile in this tileset.
    /// </param>
    /// <returns>
    ///     A new array containing the color data for all pixels that make up
    ///     the image for the tile at the specified index from this tileset.
    ///     Pixel order is from top-to-bottom, read left-to-right
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tiles in this tileset.
    /// </exception>
    public Color[] this[int tileId]
    {
        get
        {
            if (tileId < 0 || tileId >= TileCount)
            {
                throw new ArgumentOutOfRangeException(nameof(tileId));
            }

            int len = TileWidth * TileHeight;
            return _pixels[(tileId * len)..((tileId * len) + len)];
        }
    }

    internal AsepriteTileset(int id, int count, Point size, string name, Color[] pixels)
    {
        ID = id;
        TileCount = count;
        TileSize = size;
        Name = name;
        _pixels = pixels;

        //  Aseprite stores tileset images as a vertical strip
        Size = new(TileWidth, TileHeight * TileCount);
    }
}
