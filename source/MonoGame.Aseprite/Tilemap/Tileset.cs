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

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a tileset that represents an image with named texture regions that represent the tiles.
/// </summary>
public sealed class Tileset
{
    private TextureRegion[] _regions;

    /// <summary>
    ///     Gets the name of this <see cref="Tileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="Texture2D"/> represented by this <see cref="Tileset"/>.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the width, in pixels, of each tile in this <see cref="Tileset"/>.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    ///     Gets the height, in pixels, of each tile in this <see cref="Tileset"/>.
    /// </summary>
    public int TileHeight { get; }

    /// <summary>
    ///     Gets the total number of rows (i.e. how many tiles vertically) in this <see cref="Tileset"/>.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    ///     Gets the total number of columns (i.e. how many tiles horizontally) in this <see cref="Tileset"/>.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    ///     Gets the total number of tiles (texture regions) in this <see cref="Tileset"/>.
    /// </summary>
    public int TileCount { get; }

    /// <summary>
    ///     Gets a <see cref="ReadOnlySpan{T}"/> of <see cref="TextureRegion"/> elements for each tile in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    public ReadOnlySpan<TextureRegion> Tiles => _regions;

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified index in this <see cref="Tileset"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> for the tile located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of tiles in
    ///     this <see cref="Tileset"/>.  Use <see cref="Tileset.TileCount"/> to determine the total number of tiles.
    /// </exception>
    public TextureRegion this[int index] => GetTile(index);

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified column and row location in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <param name="row">
    ///     The row of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> for the tile located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the column and row location provided are less than zero or are greater than or equal to the
    ///     total number of columns or rows.  Use <see cref="Tileset.ColumnCount"/> and <see cref="Tileset.RowCount"/>
    ///     to determine the total number of columns or rows.
    /// </exception>
    public TextureRegion this[int column, int row] => GetTile(column, row);

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified column and row location in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="location">
    ///     A <see cref="Point"/> value that represents the column and row location of the tile to locate in this
    ///     <see cref="Tileset"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> for the tile located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the column and row location provided are less than zero or are greater than or equal to the
    ///     total number of columns or rows.  Use <see cref="Tileset.ColumnCount"/> and <see cref="Tileset.RowCount"/>
    ///     to determine the total number of columns or rows.
    /// </exception>
    public TextureRegion this[Point location] => GetTile(location);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tileset"/> class.
    ///     Creates a new tileset.
    /// </summary>
    /// <remarks>
    ///     The <see cref="TextureRegion"/> elements for each tile in this <see cref="Tileset"/> are auto-generated
    ///     based on the <paramref name="tileWidth"/> and <paramref name="tileHeight"/> values provided.  Both values
    ///     must be greater than zero, and the width and height of the <paramref name="texture"/> provided must divide
    ///     evenly by the <paramref name="tileWidth"/> and <paramref name="tileHeight"/>.
    /// </remarks>
    /// <param name="name">
    ///     The name to give this <see cref="Tileset"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Texture2D"/> that is represented by this <see cref="Tileset"/>.
    /// </param>
    /// <param name="tileWidth">
    ///     The width, in pixels, of each tile in this <see cref="Tileset"/>.
    /// </param>
    /// <param name="tileHeight">
    ///     The height, in pixels, of each tile in this <see cref="Tileset"/>.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <paramref name="tileWidth"/> or <paramref name="tileHeight"/> values provided are less one.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the width and height of the <paramref name="texture"/> does not divide evenly by the
    ///     <paramref name="tileWidth"/> and <paramref name="tileHeight"/> values.
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
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified index in this <see cref="Tileset"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> for the tile located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of tiles in
    ///     this <see cref="Tileset"/>.  Use <see cref="Tileset.TileCount"/> to determine the total number of tiles.
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
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified column and row location in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="location">
    ///     A <see cref="Point"/> value that represents the column and row location of the tile to locate in this
    ///     <see cref="Tileset"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> for the tile located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the column and row location provided are less than zero or are greater than or equal to the
    ///     total number of columns or rows.  Use <see cref="Tileset.ColumnCount"/> and <see cref="Tileset.RowCount"/>
    ///     to determine the total number of columns or rows.
    /// </exception>
    public TextureRegion GetTile(Point location) => GetTile(location.X, location.Y);

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified column and row location in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <param name="row">
    ///     The row of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> for the tile located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the column and row location provided are less than zero or are greater than or equal to the
    ///     total number of columns or rows.  Use <see cref="Tileset.ColumnCount"/> and <see cref="Tileset.RowCount"/>
    ///     to determine the total number of columns or rows.
    /// </exception>
    public TextureRegion GetTile(int column, int row)
    {
        int index = row * ColumnCount + column;
        return GetTile(index);
    }

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified index in this <see cref="Tileset"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <param name="tile">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> of the tile
    ///     located; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a tile was located at the specified index; otherwise, <see langword="false"/>.
    ///     This method returns <see langword="false"/> if the index specified is less than zero or is greater than or
    ///     equal to the total number of tiles in this <see cref="Tileset"/>.  Use <see cref="Tileset.TileCount"/> to
    ///     determine the total number of tiles.
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
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified column and row location in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="location">
    ///     A <see cref="Point"/> value that represents the column and row location of the tile to locate in this
    ///     <see cref="Tileset"/>.
    /// </param>
    /// <param name="tile">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> of the tile
    ///     located; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a tile was located at the specified column and row location; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if the column and row specified is
    ///     less than zero or is greater than or equal to the total number of columns and rows.  Use
    ///     <see cref="Tileset.ColumnCount"/> and <see cref="Tileset.RowCount"/> to determine the total number of
    ///     columns and rows.
    /// </returns>
    public bool TryGetTile(Point location, [NotNullWhen(true)] out TextureRegion? tile) =>
        TryGetTile(location.X, location.Y, out tile);

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> for the tile at the specified column and row location in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <param name="row">
    ///     The row of the tile in this <see cref="Tileset"/> to locate.
    /// </param>
    /// <param name="tile">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> of the tile
    ///     located; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a tile was located at the specified column and row location; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if the column and row specified is
    ///     less than zero or is greater than or equal to the total number of columns and rows.  Use
    ///     <see cref="Tileset.ColumnCount"/> and <see cref="Tileset.RowCount"/> to determine the total number of
    ///     columns and rows.
    /// </returns>
    public bool TryGetTile(int column, int row, [NotNullWhen(true)] out TextureRegion? tile)
    {
        int index = row * ColumnCount + column;
        return TryGetTile(index, out tile);
    }
}
