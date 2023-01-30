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

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
/// Defines a tile in a tilemap layer with a source texture region.
/// </summary>
public struct Tile
{
    /// <summary>
    /// Represents a tile with its properties left uninitialized.
    /// </summary>
    public static readonly Tile Empty;

    /// <summary>
    /// The ID of the source tile in the tileset used by this tile that represents the texture region to draw for this
    /// tile.
    /// </summary>
    public int TilesetTileID = 0;

    /// <summary>
    /// Indicates whether this tile should be flipped horizontally along it's x-axis when rendered.
    /// </summary>
    public bool FlipHorizontally = false;

    /// <summary>
    /// Indicates whether this tile should be flipped vertically along it's y-axis when rendered.
    /// </summary>
    public bool FlipVertically = false;

    /// <summary>
    /// The amount of rotation, in radians, to apply when rendering this tile.
    /// </summary>
    public float Rotation = 0.0f;

    /// <summary>
    /// Gets a value that indicates if this is an empty tile.  Empty tiles use tileset id 0.
    /// </summary>
    public bool IsEmpty => TilesetTileID == 0;

    /// <summary>
    /// Creates a new tile.
    /// </summary>
    public Tile() { }
}
