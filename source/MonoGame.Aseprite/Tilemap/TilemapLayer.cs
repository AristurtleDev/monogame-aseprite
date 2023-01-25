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

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a layer in a tilemap that contains a collection of tiles.
/// </summary>
public sealed class TilemapLayer
{
    private Tile?[] _tiles;
    private Vector2 _offset = Vector2.Zero;

    /// <summary>
    /// Gets a read-only span of all tiles in this tilemap layer.
    /// </summary>
    public ReadOnlySpan<Tile?> Tiles => _tiles;

    /// <summary>
    /// Gets the name of this tilemap layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source tileset used by the tiles in this tilemap layer.
    /// </summary>
    public Tileset Tileset { get; }

    /// <summary>
    /// Gets the total number of columns in this tilemap layer.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Gets the total number of rows in this tilemap layer.
    /// </summary>
    public int RowCount { get; }

    /// <summary>
    /// Gets the width, in pixels, of this tilemap layer.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height, in pixels, of this tilemap layer.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets or Sets the transparency of this tilemap layer.
    /// </summary>
    public float Transparency { get; set; } = 1.0f;

    /// <summary>
    /// Gets or Sets a value that indicates whether this tilemap layer is visible and should be rendered.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or Sets the x- and y-coordinate position offset to render this tilemap layer at relative to the position
    /// the tilemap it is in is rendered at.
    /// </summary>
    public Vector2 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    /// <summary>
    /// Gets or Sets the x-coordinate position offset to render this tilemap layer at relative to the position of the
    /// tilemap it is in is rendered at.
    /// </summary>
    public float OffsetX
    {
        get => _offset.X;
        set => _offset.X = value;
    }

    /// <summary>
    /// Gets or Sets the y-coordinate position offset to render this tilemap layer at relative to the position of the
    /// tilemap it is in is rendered at.
    /// </summary>
    public float OffsetY
    {
        get => _offset.Y;
        set => _offset.Y = value;
    }

    /// <summary>
    /// Gets the total number of tile locations in this tilemap layer.
    /// </summary>
    public int TileLocationCount => _tiles.Length;

    /// <summary>
    /// Gets the tile from this tilemap layer at the specified index.
    /// </summary>
    /// <param name="tileIndex">The index of the tile in this tilemap layer to locate.</param>
    /// <returns>
    /// The tile at the specified index in this tilemap layer, if a tile has been set at that location; otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tile indexes
    /// in this tilemap layer.
    /// </exception>
    public Tile? this[int tileIndex] => GetTile(tileIndex);

    /// <summary>
    /// Gets the tile located at the specified column and row in this tilemap layer.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tilemap layer.</param>
    /// <param name="row">The row of the tile to locate in this tilemap layer.</param>
    /// <returns>
    /// The tile at the specified column and row in this tilemap layer, if a tile has been set at that location;
    /// otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public Tile? this[int column, int row] => GetTile(column, row);

    /// <summary>
    /// Gets the tile located at the specified column and row location in this tilemap layer.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tilemap layer.</param>
    /// <returns>
    /// The tile at the specified colum nad row location in this tilemap layer, if a tile has been set at that location;
    /// otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified in the location is less than zero or if either is greater than or
    /// equal to the total number of columns or rows respectively.
    /// </exception>
    public Tile? this[Point location] => GetTile(location);

    /// <summary>
    /// Creates a new tilemap layer class.
    /// </summary>
    /// <param name="name">The name to give this tilemap layer.</param>
    /// <param name="tileset">The source tileset used by the tiles in this tilemap layer.</param>
    /// <param name="columns">The total number of columns in this tilemap layer.</param>
    /// <param name="rows">The total number of rows in this tilemap layer.</param>
    /// <param name="offset">
    /// The x- and y-coordinate position offset to render this tilemap layer at relative to the position of the tilemap
    /// it is in is rendered at.
    /// </param>
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
    /// Returns a value that indicates whether the specified index in this tilemap layer is empty, meaning no tile has
    /// been set there.
    /// </summary>
    /// <param name="index">The index of the tile location in this tilemap layer to check.</param>
    /// <returns>
    /// true if the specified index in this tilemap layer is empty; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tile
    /// locations in this tilemap layer.
    /// </exception>
    public bool IsEmpty(int index) => GetTile(index) is null;

    /// <summary>
    /// Returns a value that indicates whether the specified column and row in this tilemap layer is empty, meaning no
    /// tile has been set there.
    /// </summary>
    /// <param name="column">The column of the tile location in this tilemap layer to check.</param>
    /// <param name="row">The row of the tile location in this tilemap layer to check.</param>
    /// <returns>
    /// true if the specified column and row in this tilemap layer is empty; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public bool IsEmpty(int column, int row) => GetTile(column, row) is null;

    /// <summary>
    /// Returns a value tha indicates whether the specified column and row location in this tilemap layer is empty,
    /// meaning no tile has been set there.
    /// </summary>
    /// <param name="location">The column and row location in this tilemap layer to check.</param>
    /// <returns>
    /// true fi the specified column and row location in this tilemap layer is empty; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row in the location specified is less than zero or if either is greater than or
    /// equal to the total number of columns or rows respectively.
    /// </exception>
    public bool IsEmpty(Point location) => GetTile(location) is null;

    private Tile CreateTile(int tilesetIndex, bool flipVertically, bool flipHorizontally, float rotation)
    {
        TextureRegion textureRegion = Tileset[tilesetIndex];
        return new(textureRegion, flipVertically, flipHorizontally, rotation);
    }

    private void SetTile(int index, Tile tile)
    {

        RemoveTile(index);
        _tiles[index] = tile;
    }

    /// <summary>
    /// Creates and sets a new tile at the specified index in this tilemap layer.
    /// </summary>
    /// <param name="index">The index in this tilemap layer to set the tile.</param>
    /// <param name="tilesetIndex">
    /// The index of the source tile in the tileset used by this tilemap layer that represents the tile being set.
    /// </param>
    /// <param name="flipVertically">Indicates whether the tile being set should be flipped vertically.</param>
    /// <param name="flipHorizontally">Indicates whether the tile being set should be flipped horizontally.</param>
    /// <param name="rotation">The amount of rotation, in radians, to give the tile being set.</param>
    /// <returns>The tile that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tile
    /// locations in this tilemap layer.  This can also be thrown if the tilesetIndex specified is less than zero or is
    /// greater than or equal to the total number of texture regions in the tileset.
    /// </exception>
    public Tile SetTile(int index, int tilesetIndex, bool flipVertically = false, bool flipHorizontally = false, float rotation = 0.0f)
    {
        if (index < 0 || index >= TileLocationCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} cannot be less than zero or greater than or equal the number of tiles in this layer.");
        }

        Tile tile = CreateTile(tilesetIndex, flipVertically, flipHorizontally, rotation);
        SetTile(index, tile);
        return tile;
    }


    /// <summary>
    /// Creates and sets a new tile a the specified column and row in this tilemap layer.
    /// </summary>
    /// <param name="column">The column in this tilemap layer to set the tile.</param>
    /// <param name="row">The row in this tilemap layer to set the tile.</param>
    /// <param name="tilesetIndex">
    /// The index of the source tile in the tileset used by this tilemap layer that represents the tile being set.
    /// </param>
    /// <param name="flipVertically">Indicates whether the tile being set should be flipped vertically.</param>
    /// <param name="flipHorizontally">Indicates whether the tile being set should be flipped horizontally.</param>
    /// <param name="rotation">The amount of rotation, in radians, to give the tile being set.</param>
    /// <returns>The tile that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.  This can also be thrown if the tilesetIndex specified is less
    /// than zero or is greater than or equal to the total number of texture regions in the tileset.
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

        Tile tile = CreateTile(tilesetIndex, flipVertically, flipHorizontally, rotation);
        SetTile(index, tile);
        return tile;
    }

    /// <summary>
    /// Creates and sets a new tile a the specified column and row location in this tilemap layer.
    /// </summary>
    /// <param name="location">The column and row location in this tilemap layer to set the tile.</param>
    /// <param name="tilesetIndex">
    /// The index of the source tile in the tileset used by this tilemap layer that represents the tile being set.
    /// </param>
    /// <param name="flipVertically">Indicates whether the tile being set should be flipped vertically.</param>
    /// <param name="flipHorizontally">Indicates whether the tile being set should be flipped horizontally.</param>
    /// <param name="rotation">The amount of rotation, in radians, to give the tile being set.</param>
    /// <returns>The tile that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.  This can also be thrown if the tilesetIndex specified is less
    /// than zero or is greater than or equal to the total number of texture regions in the tileset.
    /// </exception>
    public Tile SetTile(Point location, int tilesetIndex, bool flipVertically = false, bool flipHorizontally = false, float rotation = 0.0f) =>
        SetTile(location.X, location.Y, tilesetIndex, flipVertically, flipHorizontally, rotation);

    /// <summary>
    /// Gets the tile from this tilemap layer at the specified index.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap layer to locate.</param>
    /// <returns>
    /// The tile at the specified index in this tilemap layer, if a tile has been set at that location; otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tile indexes
    /// in this tilemap layer.
    /// </exception>
    public Tile? GetTile(int index)
    {
        if (index < 0 || index >= TileLocationCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the number of tiles in this {nameof(TilemapLayer)}.");
        }

        return _tiles[index];
    }

    /// <summary>
    /// Gets the tile located at the specified column and row in this tilemap layer.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tilemap layer.</param>
    /// <param name="row">The row of the tile to locate in this tilemap layer.</param>
    /// <returns>
    /// The tile at the specified column and row in this tilemap layer, if a tile has been set at that location;
    /// otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public Tile? GetTile(int column, int row)
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
        return _tiles[index];
    }

    /// <summary>
    /// Gets the tile located at the specified column and row location in this tilemap layer.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tilemap layer.</param>
    /// <returns>
    /// The tile at the specified colum nad row location in this tilemap layer, if a tile has been set at that location;
    /// otherwise, null.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified in the location is less than zero or if either is greater than or
    /// equal to the total number of columns or rows respectively.
    /// </exception>
    public Tile? GetTile(Point location) => GetTile(location.X, location.Y);

    /// <summary>
    /// Removes the tile located at the specified index from this tilemap layer.
    /// </summary>
    /// <param name="index">The index of the tile to remove from this tilemap layer.</param>
    /// <returns>
    /// true if the tile was successfully removed; otherwise, false.  This method returns false if the index specified
    /// is less than zero or is greater than or equal to the total number of tile locations in this tilemap layer.
    /// </returns>
    public bool RemoveTile(int index)
    {
        if (index < 0 || index >= TileLocationCount)
        {
            return false;
        }

        _tiles[index] = default;
        return true;
    }

    /// <summary>
    /// Remove the tile located at the specified column and row from this tilemap layer.
    /// </summary>
    /// <param name="column">The column of tile to remove form this tilemap layer. </param>
    /// <param name="row">The row of the tile to remove from this tilemap layer.</param>
    /// <returns>
    /// true if the tile was removed successfully; otherwise, false.  This method returns false if either the specified
    /// column or row are less than zero or if either is greater than or equal to the total number of columns or rows
    /// respectively.
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
    /// Removes the tile located at the specified column and row location from this tilemap layer.
    /// </summary>
    /// <param name="location">The column and row location of the tile to remove from this tilemap layer.</param>
    /// <returns>
    /// true if the tile was successfully removed; otherwise, false.  This method returns false if either the column or
    /// row in the location specified are less than zero or if either are greater than or equal to the total number of
    /// columns or rows respectively.
    /// </returns>
    public bool RemoveTile(Point location) => RemoveTile(location.X, location.Y);

    /// <summary>
    ///     Removes all of the tiles that have been set from this tilemap layer.
    /// </summary>
    public void Clear() => _tiles = new Tile?[_tiles.Length];
}
