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

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Defines a tileset in an aseprite file.
/// </summary>
public sealed class AsepriteTileset
{
    private Color[] _pixels;

    /// <summary>
    ///     Gets a read-only span of the pixel data for this <see cref="AsepriteTileset"/>.  Pixel order is from 
    ///     top-to-bottom, read left-to-right.
    /// </summary>
    public ReadOnlySpan<Color> Pixels => _pixels;

    /// <summary>
    ///     Gets the id assigned to this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int ID { get; }

    /// <summary>
    ///     Gets the total number of tiles in this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    ///     Gets the width, in pixels, of each tile in this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    ///     Gets the height, in pixels, of each tile in this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int TileHeight { get; }

    /// <summary>
    ///     Gets the name assigned to this <see cref="AsepriteTileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the width, in pixels, of the source image for this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Gets the height, in pixels, of the source image for this <see cref="AsepriteTileset"/>.
    /// </summary>
    public int Height { get; }

    /// <summary>
    ///     Gets the <see cref="AsepriteUserData"/> set for this <see cref="AsepriteSlice"/> in aseprite.
    /// </summary>
    /// <remarks>
    ///     User Data for tilesets was added in Aseprite 1.3-rc1.  It currently doesn't appear that you can set user
    ///     data for a tileset within the Aseprite UI itself, but can set it through the LUA Scripting API.
    /// </remarks>
    public AsepriteUserData UserData { get; } = new();

    /// <summary>
    ///     Gets a read-only span of the color values that represent the pixel data for a the tile at the specified 
    ///     index in this <see cref="AsepriteTileset"/>.  Pixel order is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <param name="tileIndex">
    ///     The index of the tile in this <see cref="AsepriteTileset"/> to get the pixel data for.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of 
    ///     tiles in this <see cref="AsepriteTileset"/>.
    /// </exception>
    public ReadOnlySpan<Color> this[int tileIndex] => GetTile(tileIndex);

    internal AsepriteTileset(int id, int count, int tileWidth, int tileHeight, string name, Color[] pixels)
    {
        ID = id;
        TileCount = count;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Name = name;
        _pixels = pixels;
        Width = tileWidth;
        Height = tileHeight * TileCount;
    }

    /// <summary>
    ///     Gets a read-only span of the color values that represent the pixel data for a the tile at the specified 
    ///     index in this <see cref="AsepriteTileset"/>.  Pixel order is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <param name="tileIndex">
    ///     The index of the tile in this <see cref="AsepriteTileset"/> to get the pixel data for.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of 
    ///     tiles in this <see cref="AsepriteTileset"/>.
    /// </exception>
    public ReadOnlySpan<Color> GetTile(int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= TileCount)
        {
            throw new ArgumentOutOfRangeException(nameof(tileIndex));
        }

        int len = TileWidth * TileHeight;
        return Pixels.Slice(tileIndex * len, len);
    }
}
