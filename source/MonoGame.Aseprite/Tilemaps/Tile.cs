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
///     Defines a tile value in a <see cref="TilemapLayer"/>.
/// </summary>
public struct Tile
{
    /// <summary>
    ///     Represents a <see cref="Tile"/> with its properties left uninitialized.
    /// </summary>
    public static readonly Tile Empty;

    /// <summary>
    ///     The ID (or index) of the source tile in the <see cref="Tileset"/> that represents the 
    ///     <see cref="TextureRegion"/> to render for this <see cref="Tile"/>.
    /// </summary>
    public int TilesetTileID = 0;

    /// <summary>
    ///     Indicates whether this <see cref="Tile"/> should be flipped horizontally rendered.
    /// </summary>
    public bool FlipHorizontally = false;

    /// <summary>
    ///     Indicates whether this <see cref="Tile"/> should be flipped vertically rendered.
    /// </summary>
    public bool FlipVertically = false;

    /// <summary>
    ///     Indicates whether this <see cref="Tile"/> should be flipped diagonally when rendered.
    /// </summary>
    public bool FlipDiagonally = false;

    /// <summary>
    ///     Gets a value that indicates if this is an empty <see cref="Tile"/>. 
    /// </summary>
    /// <remarks>
    ///     Empty tiles have a <see cref="TilesetTileID"/> equal to zero.
    /// </remarks>
    public bool IsEmpty => TilesetTileID == 0;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tile"/> class.
    /// </summary>
    public Tile() { }

    /// <summary>
    ///     Initializes a new <see cref="Tile"/> value.
    /// </summary>
    /// <param name="tilesetTileID">
    ///     The ID (or index) of the source tile in the <see cref="Tileset"/> that represents the 
    ///     <see cref="TextureRegion"/> to assign for this <see cref="Tile"/>.
    /// </param>
    public Tile(int tilesetTileID) => TilesetTileID = tilesetTileID;

    /// <summary>
    ///     Initializes a new <see cref="Tile"/> value.
    /// </summary>
    /// <param name="tilesetTileID">
    ///     The ID (or index) of the source tile in the <see cref="Tileset"/> that represents the 
    ///     <see cref="TextureRegion"/> to assign for this <see cref="Tile"/>.
    /// </param>
    /// <param name="flipHorizontally">
    ///     Indicates whether the <see cref="Tile"/> should be flipped horizontally when rendered.
    /// </param>
    /// <param name="flipVertically">
    ///     Indicates whether the <see cref="Tile"/> should be flipped vertically when rendered.
    /// </param>
    /// <param name="flipDiagonally">
    ///     Indicates whether the <see cref="Tile"/> should be flipped diagonally when rendered.
    /// </param>
    public Tile(int tilesetTileID, bool flipHorizontally, bool flipVertically, bool flipDiagonally) =>
        (TilesetTileID, FlipHorizontally, FlipVertically, FlipDiagonally) = (tilesetTileID, flipHorizontally, flipVertically, flipDiagonally);
}
