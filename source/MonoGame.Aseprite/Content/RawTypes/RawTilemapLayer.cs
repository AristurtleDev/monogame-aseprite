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

namespace MonoGame.Aseprite.Content.RawTypes;

/// <summary>
/// Defines a class that represents the raw values of a tilemap layer.
/// </summary>
public sealed class RawTilemapLayer : IEquatable<RawTilemapLayer>
{
    private RawTilemapTile[] _rawTilemapTiles;

    /// <summary>
    /// Gets the name assigned to the tilemap layer represented by this raw tilemap layer record.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the id of the source tileset used by the tilemap layer represented by this raw tilemap layer record.
    /// </summary>
    public int TilesetID { get; }

    /// <summary>
    /// Gets the total number of columns in tilemap layer represented by this raw tilemap layer record.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the total number of rows in tilemap layer represented by this raw tilemap layer record.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets a read-only span of the raw tilemap tile records that represent the tiles for the tilemap layer represented
    /// by this raw tilemap layer record.
    /// </summary>
    public ReadOnlySpan<RawTilemapTile> RawTilemapTiles => _rawTilemapTiles;

    /// <summary>
    /// Gets the offset of the tilemap layer represented by this raw tilemap layer.
    /// </summary>
    public Point Offset { get; }

    internal RawTilemapLayer(string name, int tilesetID, int columns, int rows, RawTilemapTile[] rawTilemapTiles, Point offset) =>
        (Name, TilesetID, Columns, Rows, _rawTilemapTiles, Offset) = (name, tilesetID, columns, rows, rawTilemapTiles, offset);

    public bool Equals(RawTilemapLayer? other) => other is not null
                                                  && Name == other.Name
                                                  && TilesetID == other.TilesetID
                                                  && Columns == other.Columns
                                                  && Rows == other.Rows
                                                  && RawTilemapTiles.SequenceEqual(other.RawTilemapTiles)
                                                  && Offset == other.Offset;
}
