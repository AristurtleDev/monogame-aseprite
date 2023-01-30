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

using System.Collections;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
/// Defines a layer in a tilemap that contains a collection of tiles.
/// </summary>
public sealed class TilemapLayer : IEnumerable<Tile>
{
    private Tile[] _tiles;
    private Vector2 _offset = Vector2.Zero;

    /// <summary>
    /// Gets a read-only span of all tiles in this tilemap layer.
    /// </summary>
    public ReadOnlySpan<Tile> Tiles => _tiles;

    /// <summary>
    /// Gets the name of this tilemap layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or Sets the source tileset referenced by the tiles in this tilemap layer.
    /// </summary>
    public Tileset Tileset { get; set; }

    /// <summary>
    /// Gets the total number of columns in this tilemap layer.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    /// Gets the total number of rows in this tilemap layer.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the width, in pixels, of this tilemap layer.
    /// <code>Width = Tileset.TileWidth * Columns</code>
    /// </summary>
    public int Width => Tileset.TileWidth * Columns;

    /// <summary>
    /// Gets the height, in pixels, of this tilemap layer.
    /// <code>Height = Tileset.TileHeight * Rows</code>
    /// </summary>
    public int Height => Tileset.TileHeight * Rows;

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
    /// Gets the total number of tiles in this tilemap layer.
    /// </summary>
    public int TileCount => _tiles.Length;

    /// <summary>
    /// Gets the tile from this tilemap layer at the specified index.
    /// </summary>
    /// <param name="tileIndex">The index of the tile in this tilemap layer to locate.</param>
    /// <returns>The tile at the specified index in this tilemap layer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tiles in this
    /// tilemap layer.
    /// </exception>
    public Tile this[int tileIndex] => GetTile(tileIndex);

    /// <summary>
    /// Gets the tile located at the specified column and row in this tilemap layer.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tilemap layer.</param>
    /// <param name="row">The row of the tile to locate in this tilemap layer.</param>
    /// <returns>The tile at the specified column and row in this tilemap layer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public Tile this[int column, int row] => GetTile(column, row);

    /// <summary>
    /// Gets the tile located at the specified column and row location in this tilemap layer.
    /// </summary>
    /// <param name="location">The column and row location of the tile to locate in this tilemap layer.</param>
    /// <returns>The tile at the specified column and row location in this tilemap layer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified in the location is less than zero or if either is greater than or
    /// equal to the total number of columns or rows respectively.
    /// </exception>
    public Tile this[Point location] => GetTile(location);

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
        _offset = offset;
        Columns = columns;
        Rows = rows;
        _tiles = new Tile[columns * rows];
    }

    /// <summary>
    /// Returns a value that indicates whether the tile at the specified index in this tilemap layer is empty.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap layer to check.</param>
    /// <returns>true if the tile at the specified index is empty; otherwise, false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tiles in this
    /// tilemap layer.
    /// </exception>
    public bool IsEmpty(int index) => GetTile(index).IsEmpty;

    /// <summary>
    /// Returns a value that indicates whether the tile at the specified column and row in this tilemap layer is empty.
    /// </summary>
    /// <param name="column">The column of the tile to check.</param>
    /// <param name="row">The row of the tile to check.</param>
    /// <returns>true if the tile at the specified column and row in this tilemap is empty; otherwise, false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified is less than zero or if either is greater than or equal to the
    /// total number of columns or rows respectively.
    /// </exception>
    public bool IsEmpty(int column, int row) => GetTile(column, row).IsEmpty;

    /// <summary>
    /// Returns a value that indicates whether the tile at the specified column and row location in this tilemap layer
    /// is empty.
    /// </summary>
    /// <param name="location">The column and row location of the tile to check.</param>
    /// <returns>
    /// true if the tile at the specified column and row location in this tilemap is empty; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row location specified is less than zero or if either is greater than or equal to
    /// the total number of columns or rows respectively.
    /// </exception>
    public bool IsEmpty(Point location) => GetTile(location).IsEmpty;

    /// <summary>
    /// Sets the tile at the specified index in this tilemap layer using the values provided.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap layer to set.</param>
    /// <param name="tilesetTileID">
    /// The ID of the source tile in the tileset that represents the texture region to render for the tile being set.
    /// </param>
    /// <param name="flipHorizontally">
    /// Indicates whether the tile being set should be flipped horizontally along its x-axis when rendered.
    /// </param>
    /// <param name="flipVertically">
    /// Indicates if the tile being set should be flipped vertically along its y-axis when rendered.
    /// </param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when rendering the tile being set.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tiles in this
    /// tilemap layer.
    /// </exception>
    public void SetTile(int index, int tilesetTileID, bool flipHorizontally = false, bool flipVertically = false, float rotation = 0.0f)
    {
        Tile tile;
        tile.TilesetTileID = tilesetTileID;
        tile.FlipHorizontally = flipHorizontally;
        tile.FlipVertically = flipVertically;
        tile.Rotation = rotation;
        SetTile(index, tile);
    }

    /// <summary>
    /// Sets the tile at the specified column and row in this tilemap layer using the values provided.
    /// </summary>
    /// <param name="column">The column in this tilemap layer to set the tile at.</param>
    /// <param name="row">The row in this tilemap layer to set the tile at.</param>
    /// <param name="tilesetTileID">
    /// The ID of the source tile in the tileset that represents the texture region to render for the tile being set.
    /// </param>
    /// <param name="flipHorizontally">
    /// Indicates whether the tile being set should be flipped horizontally along its x-axis when rendered.
    /// </param>
    /// <param name="flipVertically">
    /// Indicates if the tile being set should be flipped vertically along its y-axis when rendered.
    /// </param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when rendering the tile being set.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row  specified is less than zero or are greater than or equal to the total number
    /// of columns or rows respectively.
    /// </exception>
    public void SetTile(int column, int row, int tilesetTileID, bool flipHorizontally = false, bool flipVertically = false, float rotation = 0.0f)
    {
        Tile tile;
        tile.TilesetTileID = tilesetTileID;
        tile.FlipHorizontally = flipHorizontally;
        tile.FlipVertically = flipVertically;
        tile.Rotation = rotation;
        SetTile(column, row, tile);
    }

    /// <summary>
    /// Sets the tile at the specified column and row location in this tilemap layer using the values provided.
    /// </summary>
    /// <param name="location">The column and row location in this tilemap layer to set the tile at.</param>
    /// <param name="tilesetTileID">
    /// The ID of the source tile in the tileset that represents the texture region to render for the tile being set.
    /// </param>
    /// <param name="flipHorizontally">
    /// Indicates whether the tile being set should be flipped horizontally along its x-axis when rendered.
    /// </param>
    /// <param name="flipVertically">
    /// Indicates if the tile being set should be flipped vertically along its y-axis when rendered.
    /// </param>
    /// <param name="rotation">The amount of rotation, in radians, to apply when rendering the tile being set.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row in the location specified is less than zero or are greater than or equal to
    /// the total number of columns or rows respectively.
    /// </exception>
    public void SetTile(Point location, int tilesetTileID, bool flipHorizontally = false, bool flipVertically = false, float rotation = 0.0f)
    {
        Tile tile;
        tile.TilesetTileID = tilesetTileID;
        tile.FlipHorizontally = flipHorizontally;
        tile.FlipVertically = flipVertically;
        tile.Rotation = rotation;
        SetTile(location, tile);
    }

    /// <summary>
    /// Sets the specified index in this tilemap layer to the tile given.
    /// </summary>
    /// <param name="index">The index in this tilemap layer to set.</param>
    /// <param name="tile">The tile to set at the index.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tiles in this
    /// tilemap layer.
    /// </exception>
    public void SetTile(int index, Tile tile)
    {
        CheckIndex(index);
        _tiles[index] = tile;
    }

    /// <summary>
    /// Sets the specified column and row to the tile given.
    /// </summary>
    /// <param name="column">The column in this tilemap layer to set.</param>
    /// <param name="row">The row in this tilemap layer to set.</param>
    /// <param name="tile">The tile to set at the column and row.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row specified are less than zero or are greater than or equal to the total number
    /// of columns or rows in this tilemap layer.
    /// </exception>
    public void SetTile(int column, int row, Tile tile)
    {
        CheckColumn(column);
        CheckRow(row);
        int index = ToIndex(column, row);
        _tiles[index] = tile;
    }

    /// <summary>
    /// Sets the specified column and row location to the tile given.
    /// </summary>
    /// <param name="location">The column and row location in this tilemap layer to set.</param>
    /// <param name="tile">The tile to set at the column and row location.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or row in the location specified are less than zero or are greater than or equal to
    /// the total number of columns or rows in this tilemap layer.
    /// </exception>
    public void SetTile(Point location, Tile tile)
    {
        CheckLocation(location);
        int index = ToIndex(location.X, location.Y);
        _tiles[index] = tile;
    }

    /// <summary>
    /// Gets the tile located at the specified index in this tilemap layer.
    /// </summary>
    /// <param name="index">The index of the tile in this tilemap layer to locate.</param>
    /// <returns>The tile located</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of tile indexes
    /// in this tilemap layer.
    /// </exception>
    public Tile GetTile(int index)
    {
        CheckIndex(index);
        return _tiles[index];
    }

    /// <summary>
    /// Gets the tile located at the specified column and row in this tilemap layer.
    /// </summary>
    /// <param name="column">The column of the tile to locate in this tilemap layer.</param>
    /// <param name="row">The row of the tile to locate in this tilemap layer.</param>
    /// <returns>The tile located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if either the column or rows specified are less than zero or are greater than or equal to the total
    /// number of columns or rows in this tilemap layer.
    /// </exception>
    public Tile GetTile(int column, int row)
    {
        CheckColumn(column);
        CheckRow(row);
        int index = ToIndex(column, row);
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
    /// Thrown if either the column or rows specified in the location are less than zero or are greater than or equal to
    /// the total number of columns or rows in this tilemap layer..
    /// </exception>
    public Tile GetTile(Point location)
    {
        CheckLocation(location);
        int index = ToIndex(location.X, location.Y);
        return _tiles[index];
    }


    /// <summary>
    /// Clears all tiles in this tilemap layer by resetting them to an empty value.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_tiles);
    }

    /// <summary>
    /// Returns an enumerator that iterates through all tiles in this tilemap layer.  The order tiles in the enumeration
    /// is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <returns>
    /// An enumerator that iterates through all tiles in this tilemap layer.
    /// </returns>
    public IEnumerator<Tile> GetEnumerator()
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            yield return _tiles[i];
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through all tiles in this tilemap layer.  The order tiles in the enumeration
    /// is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <returns>
    /// An enumerator that iterates through all tiles in this tilemap layer.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private int ToIndex(int column, int row) => row * Columns + column;

    private void CheckIndex(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            ArgumentOutOfRangeException ex = new(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tiles in this tilemap layer.");
            ex.Data.Add(nameof(index), index);
            ex.Data.Add(nameof(TileCount), TileCount);
            throw ex;
        }
    }

    private void CheckRow(int row)
    {
        if (row < 0 || row >= Rows)
        {
            ArgumentOutOfRangeException ex = new(nameof(row), $"{nameof(row)} cannot be less than zero or greater than or equal to the total number of rows in this tilemap layer.");
            ex.Data.Add(nameof(row), row);
            ex.Data.Add(nameof(Rows), Rows);
            throw ex;
        }
    }

    private void CheckColumn(int column)
    {
        if (column < 0 || column >= Columns)
        {
            ArgumentOutOfRangeException ex = new(nameof(column), $"{nameof(column)} cannot be less than zero or greater than or equal to the total number of columns in this tilemap layer.");
            ex.Data.Add(nameof(column), column);
            ex.Data.Add(nameof(Columns), Columns);
            throw ex;
        }
    }

    private void CheckLocation(Point location)
    {
        if (location.X < 0 || location.X >= Columns)
        {
            ArgumentOutOfRangeException ex = new(nameof(location), $"The column in the location cannot be less than zero or greater than or equal to the total number of columns in this tilemap layer.");
            ex.Data.Add(nameof(location), location);
            ex.Data.Add(nameof(Columns), Columns);
            throw ex;
        }

        if (location.Y < 0 || location.Y >= Rows)
        {
            ArgumentOutOfRangeException ex = new(nameof(location), $"The row in the location cannot be less than zero or greater than or equal to the total number of rows in this tilemap layer.");
            ex.Data.Add(nameof(location), location);
            ex.Data.Add(nameof(Rows), Rows);
            throw ex;
        }
    }
}
