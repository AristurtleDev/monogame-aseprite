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

using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
/// Defines a tileset with a source image and named texture regions that represent the tiles.
/// </summary>
/// <remarks>
/// A tileset is similar in function to a texture atlas in that it uses a single source image and has named texture
/// regions for sections within that image.  The difference is that a tileset autogenerates the texture regions into a
/// grid like structure and the accessor for each texture region is by location id or column and row only.
/// </remarks>
public sealed class Tileset
{
    private TextureRegion[] _regions;

    /// <summary>
    /// Gets the name of this tileset.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture image used by this tileset.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    /// Gets the width, in pixels, of each tile in this tileset.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// Gets the height, in pixels of each tile in this tileset.
    /// </summary>
    public int TileHeight { get; }

    /// <summary>
    /// Gets the total number of rows in this tileset.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Gets the total number of columns in this tileset.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Gets the total number of tiles in this tileset.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    /// Gets a read-only span of the texture regions that represent the tiles in this tileset.
    /// </summary>
    public ReadOnlySpan<TextureRegion> Tiles => _regions;

    /// <summary>
    /// Gets the texture region of the tile at the specified index in this tileset.
    /// </summary>
    /// <param name="index">The index of the tile to locate.</param>
    /// <returns>The texture region for the tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tiles in this
    /// tileset.
    /// </exception>
    public TextureRegion this[int index] => GetTile(index);

    /// <summary>
    /// Gets the texture region for the tile at the specified column and row in this tileset.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tileset.</param>
    /// <param name="row">The row of the tile to locate in this tileset.</param>
    /// <returns>The texture region for the tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified are less than zero or if either are greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public TextureRegion this[int column, int row] => GetTile(column, row);

    /// <summary>
    /// Gets the texture region for the tile at the specified column and row in this tileset.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tileset.</param>
    /// <returns>The texture region for the tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row in the specified location are less than zero or if either are greater than or
    /// equal to the total number of columns or rows respectively.
    /// </exception>
    public TextureRegion this[Point location] => GetTile(location);

    /// <summary>
    /// Creates a new tileset.
    /// </summary>
    /// <remarks>
    /// The texture regions for each tile in this tileset are auto-generated based on the tile width and tile height
    /// specified.  Both of these values must be greater than zero and the width of the texture must divide evenly by
    /// the tile width and the height of the texture must divide evenly by the tile height.
    /// </remarks>
    /// <param name="name">The name to give tis tileset.</param>
    /// <param name="texture">The source texture used by this tileset.</param>
    /// <param name="tileWidth">The width, in pixels, of each tile in this tileset.</param>
    /// <param name="tileHeight">The height, in pixels, of each tile in this tilest.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the tile width or tile height values are less than one.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the width of the texture does not divide evenly by the tile width specified or if the height of the
    /// texture does not divide evenly by the tile height specified.
    /// </exception>
    public Tileset(string name, Texture2D texture, int tileWidth, int tileHeight)
    {
        if (tileWidth < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(tileWidth), $"{nameof(tileWidth)} must be greater than zero");
        }

        if (tileHeight < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(tileHeight), $"{nameof(tileHeight)} must be greater than zero");
        }

        if (texture.Width % tileWidth != 0)
        {
            throw new ArgumentException($"The texture width ({texture.Width}) does not divide evenly by the tile width ({tileWidth} given.");
        }

        if (texture.Height % tileHeight != 0)
        {
            throw new ArgumentException($"The texture height ({texture.Height}) does not divide evenly by the tile height ({tileHeight}) given.");
        }


        Name = name;
        Texture = texture;
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        ColumnCount = texture.Width / tileWidth;
        RowCount = texture.Height / tileHeight;
        TileCount = ColumnCount * RowCount;
        CreateTextureRegions();
    }

    [MemberNotNull(nameof(_regions))]
    private void CreateTextureRegions()
    {
        _regions = new TextureRegion[TileCount];

        for (int i = 0; i < TileCount; i++)
        {
            int x = (i % ColumnCount) * TileWidth;
            int y = (i / ColumnCount) * TileHeight;

            Rectangle bounds = new(x, y, TileWidth, TileHeight);
            _regions[i] = new TextureRegion($"{Name}_{i}", Texture, bounds);
        }
    }

    /// <summary>
    /// Gets the texture region of the tile at the specified index in this tileset.
    /// </summary>
    /// <param name="index">The index of the tile to locate.</param>
    /// <returns>The texture region for the tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tiles in this
    /// tileset.
    /// </exception>
    public TextureRegion GetTile(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _regions[index];
    }

    /// <summary>
    /// Gets the texture region for the tile at the specified column and row in this tileset.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tileset.</param>
    /// <returns>The texture region for the tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row in the specified location are less than zero or if either are greater than or
    /// equal to the total number of columns or rows respectively.
    /// </exception>
    public TextureRegion GetTile(Point location) => GetTile(location.X, location.Y);

    /// <summary>
    /// Gets the texture region for the tile at the specified column and row in this tileset.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tileset.</param>
    /// <param name="row">The row of the tile to locate in this tileset.</param>
    /// <returns>The texture region for the tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified are less than zero or if either are greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public TextureRegion GetTile(int column, int row)
    {
        int index = row * ColumnCount + column;
        return GetTile(index);
    }

    /// <summary>
    /// Gets the texture region of the tile at the specified index in this tileset.
    /// </summary>
    /// <param name="index">The index of the tile to locate.</param>
    /// <param name="tile">
    /// When this method returns true, contains the texture region of the tile located; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a tile was located at the specified index; otherwise, false.  This method returns false if the specified
    /// index is less than zero or is greater than or equal to the total number of tiles in this tileset.
    /// </returns>
    public bool TryGetTile(int index, [NotNullWhen(true)] out TextureRegion? tile)
    {
        tile = default;

        if (index < 0 || index >= TileCount)
        {
            return false;
        }

        tile = _regions[index];
        return true;
    }

    /// <summary>
    /// Gets the texture region for the tile at the specified column and row in this tileset.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tileset.</param>
    /// <param name="tile">
    /// When this method returns true, contains the texture region of the tile located; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a tile was located at the specified column and row location; otherwise false.  This method return false
    /// if the column or row in the location specified is less than zero or if either are greater than or equal to the
    /// total number of columns or rows respectively.
    /// </returns>
    public bool TryGetTile(Point location, [NotNullWhen(true)] out TextureRegion? tile) =>
        TryGetTile(location.X, location.Y, out tile);

    /// <summary>
    /// Gets the texture region for the tile at the specified column and row in this tileset.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tileset.</param>
    /// <param name="row">The row of the tile to locate in this tileset.</param>
    /// <param name="tile">
    /// When this method returns true, contains the texture region of the tile located; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a tile was located at the specified column and row; otherwise false.  This method return false if the
    /// column or row in the location specified is less than zero or if either are greater than or equal to the total
    /// number of columns or rows respectively.
    /// </returns>
    public bool TryGetTile(int column, int row, [NotNullWhen(true)] out TextureRegion? tile)
    {
        int index = row * ColumnCount + column;
        return TryGetTile(index, out tile);
    }
}
