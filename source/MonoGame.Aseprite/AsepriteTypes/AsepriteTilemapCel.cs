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
/// Defines a cel in a frame that contains tile data for a tilemap.
/// </summary>
public sealed class AsepriteTilemapCel : AsepriteCel
{
    private AsepriteTile[] _tiles;

    /// <summary>
    /// Gets a read-only span of the tile data for this tilemap cel.
    /// </summary>
    public ReadOnlySpan<AsepriteTile> Tiles => _tiles;

    /// <summary>
    /// Gets the total number of columns in this tilemap cel.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the total number of rows in this tilemap cel.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the total number of tiles in this tilemap cel.\
    /// </summary>
    public int TileCount => _tiles.Length;

    /// <summary>
    /// Gets a reference to the tileset used by this tiles in this tilemap cel.
    /// </summary>
    public AsepriteTileset Tileset => LayerAs<AsepriteTilemapLayer>().Tileset;

    internal AsepriteTilemapCel(int columns, int rows, AsepriteTile[] tiles, AsepriteLayer layer, Point position, int opacity)
        : base(layer, position, opacity) => (Columns, Rows, _tiles) = (columns, rows, tiles);
}
