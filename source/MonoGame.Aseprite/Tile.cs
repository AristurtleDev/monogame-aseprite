// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Aseprite;

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
