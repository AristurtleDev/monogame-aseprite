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
///     Defines a class that represents the raw values of a tilemap tile.
/// </summary>
public sealed class RawTilemapTile : IEquatable<RawTilemapTile>
{
    /// <summary>
    ///     Gets the ID of the source tile in the tileset that represents the texture region used by the tilemap tile.
    /// </summary>
    public int TilesetTileID { get; }

    /// <summary>
    ///     Gets a value that indicates if the tilemap tile should be flipped horizontally.
    /// </summary>
    public bool FlipHorizontally { get; }

    /// <summary>
    ///     Gets a value that indicates if the tilemap tile should be flipped vertically.
    /// </summary>
    public bool FlipVertically { get; }

    /// <summary>
    ///     Gets the rotation, in radians, of the tilemap tile.
    /// </summary>
    public float Rotation { get; }

    internal RawTilemapTile(int tilesetTileID, bool flipHorizontally, bool flipVertically, float rotation) =>
        (TilesetTileID, FlipHorizontally, FlipVertically, Rotation) = (tilesetTileID, flipHorizontally, flipVertically, rotation);

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawTilemapTile"/> is equal to this
    ///     <see cref="RawTilemapTile"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawTilemapTile"/> to check for equality with this <see cref="RawTilemapTile"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawTilemapTile"/> is equal to this 
    ///     <see cref="RawTilemapTile"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawTilemapTile? other) => other is not null
                                                 && TilesetTileID == other.TilesetTileID
                                                 && FlipHorizontally == other.FlipHorizontally
                                                 && FlipVertically == other.FlipVertically;
}
