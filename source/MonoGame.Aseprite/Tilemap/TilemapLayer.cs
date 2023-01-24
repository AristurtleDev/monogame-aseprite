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

/// <summary>
/// Defines a layer in a tilemap that contains a collection of tiles.
/// </summary>
public sealed class TilemapLayer
{
    private Tile?[] _tiles;
    private Vector2 _offset = Vector2.Zero;

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
    /// Gets the total number of tile elements, including those that are empty, in this tilemap layer.
    /// </summary>
    public int TileCount => _tiles.Length;

    /// <summary>
    /// Gets the tile from this tilemap layer at the specified index.
    /// </summary>
    /// <param name="tileIndex">The index of the tile in this tilemap layer to locate.</param>
    /// <returns>The tile the specified index in this tilemap layer.</returns>
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
    /// <returns>The tile located at the specified column and row in this tilemap layer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public Tile? this[int column, int row] => GetTile(column, row);

    /// <summary>
    /// Gets the tile located at the specified column and row location in this tilemap layer.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tilemap layer.</param>
    /// <returns>The tile located at the specified column and row location in this tilemap layer.</returns>
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
    /// Returns a value that indicates whether the specified index in this tilemap layer contains an empty tile.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap layer to check.</param>
    /// <returns>
    /// true if the tile location at the specified index in this tilemap layer is empty; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tile
    /// locations in this tilemap layer.
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
    /// total number of columns or rows respectively.
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
    ///     Creates and adds a new <see cref="Tile"/> at the specified index in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="index">
    ///     The index in this <see cref="TilemapLayer"/> to set the <see cref="Tile"/> created by this method to.
    /// </param>
    /// <param name="tilesetIndex">
    ///     The index of the <see cref="TextureRegion"/> in the <see cref="Tileset"/> used by this
    ///     <see cref="TilemapLayer"/>  that represents the image to draw for the <see cref="Tile"/> created by
    ///     this method.
    /// </param>
    /// <param name="flipVertically">
    ///     A value that indicates whether the <see cref="Tile"/> created by this method should be flipped vertically
    ///     when rendered.
    /// </param>
    /// <param name="flipHorizontally">
    ///     A value that indicates whether the <see cref="Tile"/> created by this method should be flipped horizontally
    ///     when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radian, to apply when rendering the <see cref="Tile"/> created by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> that is create by this method.
    /// </returns>
    /// <returns>
    ///     The <see cref="Tile"/> that is create by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified tileset index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TextureRegion"/> elements in the <see cref="Tileset"/> used by this <see cref="TilemapLayer"/>.
    ///     Use <see cref="Tileset.TileCount"/> to determine the total number of elements.
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
    ///     Creates and sets a new <see cref="Tile"/> at the specified column and row location in this
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="location">
    ///     The column and row location in this <see cref="TilemapLayer"/> to set the <see cref="Tile"/> created by this
    ///     method to.
    /// </param>
    /// <param name="tilesetIndex">
    ///     The index of the <see cref="TextureRegion"/> in the <see cref="Tileset"/> used by this
    ///     <see cref="TilemapLayer"/>  that represents the image to draw for the <see cref="Tile"/> created by
    ///     this method.
    /// </param>
    /// <param name="flipVertically">
    ///     A value that indicates whether the <see cref="Tile"/> created by this method should be flipped vertically
    ///     when rendered.
    /// </param>
    /// <param name="flipHorizontally">
    ///     A value that indicates whether the <see cref="Tile"/> created by this method should be flipped horizontally
    ///     when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radian, to apply when rendering the <see cref="Tile"/> created by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> that is create by this method.
    /// </returns>
    /// <returns>
    ///     The <see cref="Tile"/> that is create by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the specified location is less than zero or is either greater than or
    ///     equal to the total number of columns or rows respectively.  Use <see cref="TilemapLayer.ColumnCount"/> and
    ///     <see cref="TilemapLayer.RowCount"/> to determine the total number of columns and rows.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified tileset index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TextureRegion"/> elements in the <see cref="Tileset"/> used by this <see cref="TilemapLayer"/>.
    ///     Use <see cref="Tileset.TileCount"/> to determine the total number of elements.
    /// </exception>
    public Tile SetTile(Point location, int tilesetIndex, bool flipVertically = false, bool flipHorizontally = false, float rotation = 0.0f) =>
        SetTile(location.X, location.Y, tilesetIndex, flipVertically, flipHorizontally, rotation);

    /// <summary>
    ///     Gets the <see cref="Tile"/> element from this <see cref="TilemapLayer"/> at the specified index.
    /// </summary>
    /// <param name="tileIndex">
    ///     The index of the <see cref="Tile"/> element in this <see cref="TilemapLayer"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> element that was located at the specified index in this <see cref="TilemapLayer"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.  Use <see cref="TilemapLayer.TileCount"/> to
    ///     determine the total number of elements.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no <see cref="Tile"/> has been set at the specified index in this <see cref="TilemapLayer"/>.
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
    ///     Gets the <see cref="Tile"/> located at the specified column and row in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="row">
    ///     The row of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> located at the specified column and row in this <see cref="TilemapLayer"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row specified is less than zero or if either is greater than or equal to the
    ///     total number of columns or rows respectively.  Use <see cref="TilemapLayer.ColumnCount"/> and
    ///     <see cref="TilemapLayer.RowCount"/> to determine the total number of columns and rows.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no <see cref="Tile"/> has been set at the specified column and row in this
    ///     <see cref="TilemapLayer"/>.
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
    ///     Gets the <see cref="Tile"/> located at the specified location in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> located at the specified column and row location in this <see cref="TilemapLayer"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the location specified is less than zero or if either is greater than
    ///     or equal to the total number of columns or rows respectively.  Use <see cref="TilemapLayer.ColumnCount"/>
    ///     and <see cref="TilemapLayer.RowCount"/> to determine the total number of columns and rows.
    /// </exception>
    public Tile GetTile(Point location) => GetTile(location.X, location.Y);

    /// <summary>
    ///     Returns a new <see cref="Array"/> containing all of the <see cref="Tile"/> elements in this
    ///     <see cref="TilemapLayer"/>. Order of elements is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <returns>
    ///     A new <see cref="Array"/> containing all of the <see cref="Tile"/> elements in this
    ///     <see cref="TilemapLayer"/>
    /// </returns>
    public Tile?[] GetTiles() => new List<Tile?>(_tiles).ToArray();

    /// <summary>
    ///     Gets the <see cref="Tile"/> located at the specified index in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="tile">
    ///     When this method returns <see langword="true"/>, contains the <see cref="Tile"/> element that was located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified index is valid and a <see cref="Tile"/> element has been set at the
    ///     specified index in this <see cref="TilemapLayer"/>; otherwise, <see langword="false"/>.  This method returns
    ///     <see langword="false"/> if the index specified is less than zero or is greater than or equal to the total
    ///     number of <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>, or if no <see cref="Tile"/> has
    ///     been set at the specified index in this <see cref="TilemapLayer"/>.
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
    ///     Gets the <see cref="Tile"/> located at the specified column and row in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="row">
    ///     The row of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="tile">
    ///     When this method returns <see langword="true"/>, contains the <see cref="Tile"/> element that was located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified column and row are valid and a <see cref="Tile"/> element has been
    ///     set at the specified column and row in this <see cref="TilemapLayer"/>; otherwise, <see langword="false"/>.
    ///     This method return <see langword="false"/> if either the column or row specified is less than zero or if
    ///     either is greater than or equal to the total number of columns or rows respectively, of if no
    ///     <see cref="Tile"/> has been set at the specified column and row in this <see cref="TilemapLayer"/>.
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
    ///     Gets the <see cref="Tile"/> located at the specified location in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the <see cref="Tile"/> to locate in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="tile">
    ///     When this method returns <see langword="true"/>, contains the <see cref="Tile"/> element that was located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the specified column and row location are valid and a <see cref="Tile"/> element
    ///     has been set at the specified column and row in this <see cref="TilemapLayer"/>; otherwise,
    ///     <see langword="false"/>. This method return <see langword="false"/> if the column or row in the location
    ///     specified is less than zero or if either is greater than or equal to the total number of columns or rows
    ///     respectively, of if no <see cref="Tile"/> has been set at the specified column and row in this
    ///     <see cref="TilemapLayer"/>.
    /// </returns>
    public bool TryGetTile(Point location, [NotNullWhen(true)] out Tile? tile) =>
        TryGetTile(location.X, location.Y, out tile);

    /// <summary>
    ///     Removes the <see cref="Tile"/> located at the specified index in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tile"/> to remove from this <see cref="TilemapLayer"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Tile"/> was successfully removed; otherwise,
    ///     <see langword="false"/>.  This method return <see langword="false"/> if the index specified is less than
    ///     zero or is greater than or equal to the total number of <see cref="Tile"/> elements in this
    ///     <see cref="TilemapLayer"/>.  Use <see cref="TilemapLayer.TileCount"/> to determine the total number of
    ///     elements.
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
    ///     Removes the <see cref="Tile"/> located at the specified column and row from this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of <see cref="Tile"/> to remove in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="row">
    ///     The row of the <see cref="Tile"/> to remove in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Tile"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method return <see langword="false"/> if either the specified column or row
    ///     are less than zero or if either is greater than the total number of columns and rows respectively.  Use
    ///     <see cref="TilemapLayer.ColumnCount"/> and <see cref="TilemapLayer.RowCount"/> to determine the total number
    ///     of columns and rows.
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
    ///     Removes the <see cref="Tile"/> located at the column and row location specified from this
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the <see cref="Tile"/> in this <see cref="TilemapLayer"/> to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Tile"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method return <see langword="false"/> if either the column or row in the
    ///     location specified are less than zero or if either is greater than the total number of columns and rows
    ///     respectively.  Use <see cref="TilemapLayer.ColumnCount"/> and <see cref="TilemapLayer.RowCount"/> to
    ///     determine the total number of columns and rows.
    /// </returns>
    public bool RemoveTile(Point location) => RemoveTile(location.X, location.Y);

    /// <summary>
    ///     Removes all of the <see cref="Tile"/> elements that have been set from this <see cref="TilemapLayer"/>.
    /// </summary>
    public void Clear() => _tiles = new Tile?[_tiles.Length];
}
