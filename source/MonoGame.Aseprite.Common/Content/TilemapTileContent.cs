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

namespace MonoGame.Aseprite.RawTypes;

/// <summary>
/// Defines a class that represents the raw values of a tilemap tile.
/// </summary>
public sealed class TilemapTileContent : IEquatable<TilemapTileContent>
{
    /// <summary>
    /// Gets the ID of the source tile in the tileset that represents the texture region used by the tilemap tile that
    /// is represented by this raw tilemap tile.
    /// </summary>
    public int TilesetTileID { get; }

    /// <summary>
    /// Gets a value that indicates if the tilemap tile represented by this raw tilemap tile should be flipped
    /// horizontally along its x-axis.
    /// </summary>
    public bool FlipHorizontally { get; }

    /// <summary>
    /// Gets a value that indicates if the tilemap tile represented by this raw tilemap tile should be flipped
    /// vertically along its y-axis.
    /// </summary>
    public bool FlipVertically { get; }

    /// <summary>
    /// Gets the rotation, in radians, of the tilemap tile represented by this raw tilemap tile.
    /// </summary>
    public float Rotation { get; }

    internal TilemapTileContent(int tilesetTileID, bool flipHorizontally, bool flipVertically, float rotation) =>
        (TilesetTileID, FlipHorizontally, FlipVertically, Rotation) = (tilesetTileID, flipHorizontally, flipVertically, rotation);

    public bool Equals(TilemapTileContent? other) => other is not null
                                                 && TilesetTileID == other.TilesetTileID
                                                 && FlipHorizontally == other.FlipHorizontally
                                                 && FlipVertically == other.FlipVertically;
}
