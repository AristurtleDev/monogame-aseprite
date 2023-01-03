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
///     Represents a cel that contains tile data in an Aseprite image.
/// </summary>
public sealed class AsepriteTilemapCel : AsepriteCel
{
    private List<AsepriteTile> _tiles = new();

    /// <summary>
    ///     The width and heigh extents, in number of tiles, of this tilemap
    ///     cel.
    /// </summary>
    /// <remarks>
    ///     Note: as stated in the description above, this is the width and
    ///     height in number of tiles, not in pixels.
    /// </remarks>
    public Point Size { get; }

    /// <summary>
    ///     The width, in number of tiles, of this tilemap cel.
    /// </summary>
    /// <remarks>
    ///     Note: as stated in the description above, this is the width in
    ///     number of tiles, not in pixels.
    /// </remarks>
    public int Width => Size.X;

    /// <summary>
    ///     The height, in number of tiles, of this tilemap cel.
    /// </summary>
    /// <remarks>
    ///     Note: as stated in the description above, this is the height in
    ///     number of tiles, not in pixels.
    /// </remarks>
    public int Height => Size.Y;

    /// <summary>
    ///     A read-only collection of the tile elements of this tilemap cel.
    /// </summary>
    public ReadOnlyCollection<AsepriteTile> Tiles => _tiles.AsReadOnly();

    /// <summary>
    ///     The total number of tile elements in this tilemap cel.
    /// </summary>
    public int TileCount => _tiles.Count;

    /// <summary>
    ///     Returns the tile element at the specified index from this tilemap
    ///     cel.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile element.
    /// </param>
    /// <returns>
    ///     The tile element at the specified index from this tilemap cel.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tile elements in this tilemap cel.
    /// </exception>
    public AsepriteTile this[int index]
    {
        get
        {
            if (index < 0 || index >= TileCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return Tiles[index];
        }
    }

    /// <summary>
    ///     The tileset used by this tilemap cel.
    /// </summary>
    public AsepriteTileset Tileset
    {
        get
        {
            if (Layer is AsepriteTilemapLayer tilemapLayer)
            {
                return tilemapLayer.Tileset;
            }

            //  This should theoretically never happen
            throw new InvalidOperationException($"The layer of this cel is not a tilemap layer");
        }
    }

    internal AsepriteTilemapCel(Point size, AsepriteLayer layer, Point position, int opacity)
        : base(layer, position, opacity)
    {
        Size = size;
    }

    internal void AddTile(AsepriteTile tile) => _tiles.Add(tile);
}
