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

namespace MonoGame.Aseprite;

public sealed class TilemapLayer
{
    private Tile?[] _tiles;
    private Vector2 _offset = Vector2.Zero;

    /// <summary>
    ///     Gets the name of this tilemap layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the tileset used by the tiles in this tilemap layer.
    /// </summary>
    public Tileset Tileset { get; }

    /// <summary>
    ///     Gets the total number of columns in this tilemap layer.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    ///     Gets the total number of rows in tis tilemap layer.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    ///     Gets the width, in pixels, of this tilemap layer.
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Gets the height, in pixels, of this tilemap layer.
    /// </summary>
    public int Height { get; }

    /// <summary>
    ///     Gets or Sets the opacity of this layer.
    /// </summary>
    public float Opacity { get; set; } = 1.0f;

    /// <summary>
    ///     Gets or Sets a value that indicates whether this tilemap layer is
    ///     visible.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate position offset to draw this
    ///     tilemap layer, relative to the x- and y-coordinate position of
    ///     the tilemap this tilemap layer is in.
    /// </summary>
    public Vector2 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    /// <summary>
    ///     Gets the total number of tiles, including empty tiles, in this
    ///     tilemap layer.
    /// </summary>
    public int TileCount => _tiles.Length;

    /// <summary>
    ///     Gets the tile located at the specified index in this tilemap layer.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile to locate in this tilemap layer.
    /// </param>
    /// <returns>
    ///     The tile located at the specified index in this tilemap layer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than
    ///     or equal to the total number of tiles in this tilemap layer.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no tile has been set at the specified index location
    ///     in this tilemap.
    /// </exception>
    public Tile this[int index] => GetTile(index);

    /// <summary>
    ///     Gets the tile located at the specified column and row in this
    ///     tilemap layer.
    /// </summary>
    /// <param name="column">
    ///     The column of the tile to locate in this tilemap layer.
    /// </param>
    /// <param name="row">
    ///     The row of the tile to locate in this tilemap layer.
    /// </param>
    /// <returns>
    ///     The tile located at the specified column and row in this tilemap
    ///     layer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row specified is less than zero or
    ///     if either is greater than or equal to the total number of columns or
    ///     rows respectively.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no tile has been set at the specified column and row
    ///     in this tilemap.
    /// </exception>
    public Tile? this[int column, int row] => GetTile(column, row);

    /// <summary>
    ///     Gets the tile located at the specified location in this tilemap
    ///     layer.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the tile to locate in this tilemap
    ///     layer.
    /// </param>
    /// <returns>
    ///     The tile located at the specified column and row location in this
    ///     tilemap layer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the location specified is less
    ///     than zero or if either is greater than or equal to the total number
    ///     of columns or rows respectively.
    /// </exception>
    public Tile? this[Point location] => GetTile(location);

    public TilemapLayer(string name, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        Tileset = tileset;
        Name = name;
        ColumnCount = columns;
        RowCount = rows;
        Width = tileset.TileWidth * columns;
        Height = tileset.TileHeight * rows;
        _offset = offset;

        _tiles = new Tile[columns * rows];
    }

    /// <summary>
    ///     Returns a value that indicates whether the specified index in this
    ///     tilemap layer is empty.
    /// </summary>
    /// <param name="index">
    ///     The index in this tilemap layer to check.
    /// </param>
    /// <returns>
    ///     true if hte specified index in this tilemap layer is empty;
    ///     otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than
    ///     or equal to the total number of tile in this tileset.
    /// </exception>
    public bool IsEmpty(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tiles in this tilemap.");
        }

        return _tiles[index] is not null;
    }

    /// <summary>
    ///     Creates and sets a new tile at the specified column and row in this
    ///     tilemap layer.
    /// </summary>
    /// <param name="column">
    ///     The column in this tilemap layer to set the tile created by this
    ///     method to.
    /// </param>
    /// <param name="row">
    ///     The row in this tilemap layer to set the tile created by this
    ///     method to.
    /// </param>
    /// <param name="tilesetIndex">
    ///     The index of the texture region in the tileset used by this tilemap
    ///     layer that represents the image to draw for the tile created by
    ///     this method.
    /// </param>
    /// <param name="flipVertically">
    ///     A value that indicates whether the tile created by this method
    ///     should be flipped vertically when rendered.
    ///     A value that indicates if the tile being created by this method
    ///     should be flipped vertically when rendered.
    /// </param>
    /// <param name="flipHorizontally">
    ///     A value that indicates whether the tile created by this method
    ///     should be flipped horizontally when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radian, to apply when rendering the tile
    ///     created by this method.
    ///     tile being set.
    /// </param>
    /// <returns>
    ///     The tile that is create by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row specified is less than zero or
    ///     if either is greater than or equal to the total number of columns or
    ///     rows respectively.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified tileset index is less than zero or is
    ///     greater than or equal to the total number of tiles in the tileset
    ///     used by this layer.
    /// </exception>
    public Tile SetTile(int column, int row, int tilesetIndex, bool flipVertically = false, bool flipHorizontally = false, float rotation = 0.0f)
    {
        if (row < 0 || row >= RowCount)
        {
            throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} cannot be less than zero or greater than or equal to the total number of rows.");
        }

        if (column < 0 || column >= ColumnCount)
        {
            throw new ArgumentOutOfRangeException(nameof(column), $"{nameof(column)} cannot be less than zero or greater than or equal to the total number o columns.");
        }

        int index = row * ColumnCount + column;
        return SetTile(index, tilesetIndex, flipVertically, flipHorizontally, rotation);
    }

    /// <summary>
    ///     Creates and adds a new tile at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">
    ///     The index in this tilemap to set the tile created by this method to.
    ///     The index of te tile to set in this tilemap.
    /// </param>
    /// <param name="tilesetIndex">
    ///     The index of the texture region in the tileset used by this tilemap
    ///     layer that represents the image to draw for the tile created by
    ///     this method.
    /// </param>
    /// <param name="flipVertically">
    ///     A value that indicates whether the tile created by this method
    ///     should be flipped vertically when rendered.
    ///     A value that indicates if the tile being created by this method
    ///     should be flipped vertically when rendered.
    /// </param>
    /// <param name="flipHorizontally">
    ///     A value that indicates whether the tile created by this method
    ///     should be flipped horizontally when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radian, to apply when rendering the tile
    ///     created by this method.
    ///     tile being set.
    /// </param>
    /// <returns>
    ///     The tile that is create by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tiles in this tilemap layer.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified tileset index is less than zero or is
    ///     greater than or equal to the total number of tiles in the tileset
    ///     used by this layer.
    /// </exception>
    public Tile SetTile(int index, int tilesetIndex, bool flipVertically = false, bool flipHorizontally = false, float rotation = 0.0f)
    {
        if (index < 0 || index >= TileCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} cannot be less than zero or greater than or equal the number of tiles in this layer.");
        }

        if (!Tileset.TryGetTile(tilesetIndex, out TextureRegion? textureRegion))
        {
            throw new ArgumentException($"The tileset used by this layer does not contain a tile at the index '{tilesetIndex}'", nameof(tilesetIndex));
        }

        Tile tile = new(textureRegion, flipVertically, flipHorizontally, rotation);
        _tiles[index] = tile;
        return tile;
    }

    /// <summary>
    ///     Creates and sets a new tile at the specified column and row location
    ///     in this tilemap.
    /// </summary>
    /// <param name="location">
    ///     The column and row location in this tilemap to set the tile created
    ///     by this method to.
    /// </param>
    /// <param name="tilesetIndex">
    ///     The index of the texture region in the tileset used by this tilemap
    ///     layer that represents the image to draw for the tile created by
    ///     this method.
    /// </param>
    /// <param name="flipVertically">
    ///     A value that indicates whether the tile created by this method
    ///     should be flipped vertically when rendered.
    ///     A value that indicates if the tile being created by this method
    ///     should be flipped vertically when rendered.
    /// </param>
    /// <param name="flipHorizontally">
    ///     A value that indicates whether the tile created by this method
    ///     should be flipped horizontally when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radian, to apply when rendering the tile
    ///     created by this method.
    ///     tile being set.
    /// </param>
    /// <returns>
    ///     The tile that is create by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the specified location is less than zero or
    ///     if either is greater than or equal to the total number of columns or
    ///     rows respectively.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the location specified is less
    ///     than zero or if either is greater than or equal to the total number
    ///     of columns or rows respectively.
    /// </exception>
    public Tile SetTile(Point location, int tilesetIndex, bool flipVertically = false, bool flipHorizontally = false, float rotation = 0.0f) =>
        SetTile(location.X, location.Y, tilesetIndex, flipVertically, flipHorizontally, rotation);

    /// <summary>
    ///     Gets the tile located at the specified index in this tilemap layer.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile to locate in this tilemap layer.
    /// </param>
    /// <returns>
    ///     The tile located at the specified index in this tilemap layer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than
    ///     or equal to the total number of tiles in this tilemap layer.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no tile has been set at the specified index location
    ///     in this tilemap.
    /// </exception>
    public Tile GetTile(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the number of tiles in this {nameof(TilemapLayer)}.");
        }

        Tile? tile = _tiles[index];

        if (tile is null)
        {
            throw new InvalidOperationException($"No tile has been set for index '{index}' in this {nameof(TilemapLayer)}.");
        }

        return tile;
    }

    /// <summary>
    ///     Gets the tile located at the specified column and row in this
    ///     tilemap layer.
    /// </summary>
    /// <param name="column">
    ///     The column of the tile to locate in this tilemap layer.
    /// </param>
    /// <param name="row">
    ///     The row of the tile to locate in this tilemap layer.
    /// </param>
    /// <returns>
    ///     The tile located at the specified column and row in this tilemap
    ///     layer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row specified is less than zero or
    ///     if either is greater than or equal to the total number of columns or
    ///     rows respectively.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no tile has been set at the specified column and row
    ///     in this tilemap.
    /// </exception>
    public Tile GetTile(int column, int row)
    {
        if (row < 0 || row >= RowCount)
        {
            throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} cannot be less than zero or greater than or equal to the total number of rows in this {nameof(TilemapLayer)}.");
        }

        if (column < 0 || column >= ColumnCount)
        {
            throw new ArgumentOutOfRangeException(nameof(column), $"{nameof(column)} cannot be less than zero or greater than or equal to the total number of columns in this {nameof(TilemapLayer)}.");
        }

        int index = row * ColumnCount + column;

        Tile? tile = _tiles[index];

        if (tile is null)
        {
            throw new InvalidOperationException($"No tile has been set for the location [{column}, {row}] in this {nameof(TilemapLayer)}.");
        }

        return tile;
    }

    /// <summary>
    ///     Gets the tile located at the specified location in this tilemap
    ///     layer.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the tile to locate in this tilemap
    ///     layer.
    /// </param>
    /// <returns>
    ///     The tile located at the specified column and row location in this
    ///     tilemap layer.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the location specified is less
    ///     than zero or if either is greater than or equal to the total number
    ///     of columns or rows respectively.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no tile has been set at the specified column and row
    ///     in this tilemap.
    /// </exception>
    public Tile GetTile(Point location) => GetTile(location.X, location.Y);

    /// <summary>
    ///     Returns a new array containing all of the tiles in this tilemap.
    ///     Order of tiles in array is from top-to-bottom read left-to-right.
    /// </summary>
    /// <returns>
    ///     A new array containing all of the tiles this tilemap.
    /// </returns>
    public Tile?[] GetTiles() => new List<Tile?>(_tiles).ToArray();

    /// <summary>
    ///     Gets the tile located at the specified index in this tilemap layer.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile to locate in this tilemap layer.
    /// </param>
    /// <param name="tile">
    ///     When this method returns, contains the tile that was located, if the
    ///     specified index is valid and a tile has been set for the specified
    ///     index; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if the specified index is valid and a tile has been set at
    ///     the specified index in this tilemap; otherwise, false.  This method
    ///     returns false if the index specified is less than zero or is greater
    ///     than or equal to the total number of tiles in this tilemap layer, or
    ///     if not tile has been set at the specified index in this tilemap
    ///     layer.
    /// </returns>
    public bool TryGetTile(int index, [NotNullWhen(true)] out Tile? tile)
    {
        tile = default;

        if (index < 0 || index >= TileCount)
        {
            return false;
        }

        tile = _tiles[index];
        return tile is not null;
    }

    /// <summary>
    ///     Gets the tile located at the specified column and row in this
    ///     tilemap layer.
    /// </summary>
    /// <param name="column">
    ///     The column of the tile to locate in this tilemap layer.
    /// </param>
    /// <param name="row">
    ///     The row of the tile to locate in this tilemap layer.
    /// </param>
    /// <param name="tile">
    ///     When this method returns, contains the tile that was located, if the
    ///     specified column and row is valid and a tile has been set for the
    ///     specified column and row; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if the specified column and row are valid and a tile has been
    ///     set at the specified column and row in this tilemap layer;
    ///     otherwise, false.  This method returns false if either the column
    ///     or row specified is less than zero or if either is greater than or
    ///     equal to the total number of columns or rows respectively, or if
    ///     no tile has been set at the specified column and row in this tilemap
    ///     layer.
    /// </returns>
    public bool TryGetTile(int column, int row, [NotNullWhen(true)] out Tile? tile)
    {
        tile = default;

        if (column < 0 || column >= ColumnCount || row < 0 || row >= RowCount)
        {
            return false;
        }

        int index = row * ColumnCount + column;
        tile = _tiles[index];
        return tile is not null;
    }

    /// <summary>
    ///     Gets the tile located at the specified location in this tilemap
    ///     layer.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the tile to locate in this tilemap
    ///     layer.
    /// </param>
    /// <param name="tile">
    ///     When this method returns, contains the tile that was located, if the
    ///     specified column and row location is valid and a tile has been set
    ///     for the specified column and row location.; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if the specified column and row location are valid and a tile
    ///     has been set at the specified column and row location in this
    ///     tilemap layer; otherwise, false.  This method returns false if
    ///     either the column or row location specified is less than zero or if
    ///     either is greater than or equal to the total number of columns or
    ///     rows respectively, or if no tile has been set at the specified
    ///     column and row in this tilemap layer.
    /// </returns>
    public bool TryGetTile(Point location, [NotNullWhen(true)] out Tile? tile) =>
        TryGetTile(location.X, location.Y, out tile);

    /// <summary>
    ///     Removes the tile located at the specified index in this tilemap
    ///     layer.
    /// </summary>
    /// <param name="index">
    ///     The index of the tile to remove from this tilemap layer.
    /// </param>
    /// <returns>
    ///     true if the tile located at the specified index was removed
    ///     successfully from this tilemap layer; otherwise, false.
    /// </returns>
    public bool RemoveTile(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            return false;
        }

        _tiles[index] = default;
        return true;
    }

    /// <summary>
    ///     Removes the tile located at the specified column and row from this
    ///     tilemap layer.
    /// </summary>
    /// <param name="column">
    ///     The column of tile to remove in this tilemap layer.
    /// </param>
    /// <param name="row">
    ///     The row of the tile to remove in this tilemap layer.
    /// </param>
    /// <returns>
    ///     true if the tile located at the specified column and row location
    ///     was removed successfully from this tilemap layer; otherwise, false.
    /// </returns>
    public bool RemoveTile(int column, int row)
    {
        if (row < 0 || row >= RowCount)
        {
            throw new ArgumentOutOfRangeException(nameof(row), $"{nameof(row)} cannot be less than zero or greater than or equal to the total number of rows.");
        }

        if (column < 0 || column >= ColumnCount)
        {
            throw new ArgumentOutOfRangeException(nameof(column), $"{nameof(column)} cannot be less than zero or greater than or equal to the total number o columns.");
        }

        int index = row * ColumnCount + column;
        return RemoveTile(index);
    }

    /// <summary>
    ///     Removes the tile located at the specified column and row location
    ///     from this tilemap layer.
    /// </summary>
    /// <param name="column">
    ///     The column of tile to remove in this tilemap layer.
    /// </param>
    /// <param name="row">
    ///     The row of the tile to remove in this tilemap layer.
    /// </param>
    /// <returns>
    ///     true if the tile located at the specified column and row location
    ///     was removed successfully from this tilemap layer; otherwise, false.
    /// </returns>
    public bool RemoveTile(Point location) => RemoveTile(location.X, location.Y);

    /// <summary>
    ///     Removes all tiles from this tilemap layer.
    /// </summary>
    public void Clear() => _tiles = new Tile?[_tiles.Length];
}
