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
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
///     Defines a grid-like layer in a tilemap that contains a collection of tiles.
/// </summary>
public sealed class TilemapLayer : IEnumerable<Tile>
{
    private Tile[] _tiles;
    private Vector2 _offset = Vector2.Zero;

    /// <summary>
    ///     Gets a read-only span of the <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </summary>
    public ReadOnlySpan<Tile> Tiles => _tiles;

    /// <summary>
    ///     Gets the name assigned to this  <see cref="TilemapLayer"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets or Sets the source <see cref="Tileset"/> referenced by the <see cref="Tile"/> elements in this 
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    public Tileset Tileset { get; set; }

    /// <summary>
    ///     Gets the total number of columns in this <see cref="TilemapLayer"/>.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    ///     Gets the total number of rows in this <see cref="TilemapLayer"/>.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    ///     Gets the width, in pixels, of this <see cref="TilemapLayer"/>.
    ///     <code>Width = Tileset.TileWidth * Columns</code>
    /// </summary>
    public int Width => Tileset.TileWidth * Columns;

    /// <summary>
    ///     Gets the height, in pixels, of this <see cref="TilemapLayer"/>.
    ///     <code>Height = Tileset.TileHeight * Rows</code>
    /// </summary>
    public int Height => Tileset.TileHeight * Rows;

    /// <summary>
    ///     Gets or Sets the transparency of this <see cref="TilemapLayer"/>.
    /// </summary>
    public float Transparency { get; set; } = 1.0f;

    /// <summary>
    ///     Gets or Sets a value that indicates whether this <see cref="TilemapLayer"/> is visible and should be 
    ///     rendered.
    /// </summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>
    ///     Gets or Sets the x- and y-coordinate position offset, relative to the position of the <see cref="Tilemap"/>,
    ///     to render this <see cref="TilemapLayer"/> at 
    /// </summary>
    public Vector2 Offset
    {
        get => _offset;
        set => _offset = value;
    }

    /// <summary>
    ///     Gets or Sets the x-position offset, relative to the position of the <see cref="Tilemap"/>, to render this 
    ///     <see cref="TilemapLayer"/> at 
    /// </summary>
    public float OffsetX
    {
        get => _offset.X;
        set => _offset.X = value;
    }

    /// <summary>
    ///     Gets or Sets the y-position offset, relative to the position of the <see cref="Tilemap"/>, to render this 
    ///     <see cref="TilemapLayer"/> at 
    /// </summary>
    public float OffsetY
    {
        get => _offset.Y;
        set => _offset.Y = value;
    }

    /// <summary>
    ///     Gets the total number of <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </summary>
    public int TileCount => _tiles.Length;

    /// <summary>
    ///     Gets the <see cref="Tile"/> element at the specified index in this <see cref="TilemapLayer"/> 
    /// </summary>
    /// <param name="tileIndex">
    ///     The index of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or equal to the total number of 
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </exception>
    public Tile this[int tileIndex] => GetTile(tileIndex);

    /// <summary>
    ///     Gets the <see cref="Tile"/> element located at the specified column and row in this 
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <param name="row">
    /// The row of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <returns>
    ///     The  <see cref="Tile"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or rows specified is less than zero or if either is greater than or equal to the
    ///     total number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public Tile this[int column, int row] => GetTile(column, row);

    /// <summary>
    ///     Gets the <see cref="Tile"/> element located at the specified column and row location in this 
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <returns>
    /// The <see cref="Tile"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or rows specified in the location is less than zero or if either is greater
    ///     than or equal to the total number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public Tile this[Point location] => GetTile(location);

    /// <summary>
    ///     Initializes a new instance of the <see cref="TilemapLayer"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name assign the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="tileset">
    ///     The source tileset used by the tiles in this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="columns">
    ///     The total number of columns to assign the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="rows">
    /// The total number of rows to assign the <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="offset">
    ///     The x- and y-coordinate position offset, relative to the position of the <see cref="Tilemap"/> to assign the
    ///     <see cref="TilemapLayer"/>.
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
    ///     Returns a value that indicates whether the <see cref="Tile"/> element at the specified index in this 
    ///     <see cref="TilemapLayer"/>  is empty.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tile"/> element to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Tile"/> element at the specified index is empty; otherwise, 
    ///     <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or equal to the total number of 
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </exception>
    public bool IsEmpty(int index) => GetTile(index).IsEmpty;

    /// <summary>
    ///     Returns a value that indicates whether the <see cref="Tile"/> element at the specified column and row in 
    ///     this <see cref="TilemapLayer"/> is empty.
    /// </summary>
    /// <param name="column">
    ///     The column of the <see cref="Tile"/> element to check.
    /// </param>
    /// <param name="row">
    ///     The row of the <see cref="Tile"/> element to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Tile"/> element at the specified column and row in this 
    ///     <see cref="TilemapLayer"/> is empty; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row specified is less than zero or if either is greater than or equal to the
    ///     total number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public bool IsEmpty(int column, int row) => GetTile(column, row).IsEmpty;

    /// <summary>
    ///     Returns a value that indicates whether the <see cref="Tile"/> element at the specified column and row 
    ///     location in  this <see cref="TilemapLayer"/> is empty.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the <see cref="Tile"/> element to check.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="Tile"/> element at the specified column and row location in this 
    ///     <see cref="TilemapLayer"/> is empty; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the specified location is less than zero or if either is greater 
    ///     than or equal to the total number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public bool IsEmpty(Point location) => GetTile(location).IsEmpty;

    /// <summary>
    ///     Sets the <see cref="Tile"/> element at the specified index in this <see cref="TilemapLayer"/> using the 
    ///     values provided.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tile"/> element in this <see cref="TilemapLayer"/> to set.
    /// </param>
    /// <param name="tilesetTileID">
    ///     The ID of the source tile in the <see cref="Tileset"/> that represents the <see cref="TextureRegion"/> to 
    ///     render for the <see cref="Tile"/> element being set.
    /// </param>
    /// <param name="flipHorizontally">
    ///     Indicates whether the <see cref="Tile"/> element being set should be flipped horizontally when rendered.
    /// </param>
    /// <param name="flipVertically">
    ///     Indicates if the <see cref="Tile"/> element being set should be flipped vertically when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the <see cref="Tile"/> element being set.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or equal to the total number of 
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
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
    ///     Sets the <see cref="Tile"/> element at the specified column and row in this <see cref="TilemapLayer"/> 
    ///     using the values provided.
    /// </summary>
    /// <param name="column">
    ///     The column in this <see cref="TilemapLayer"/> to set the <see cref="Tile"/> element at.
    /// </param>
    /// <param name="row">
    ///     The row in this <see cref="TilemapLayer"/> to set the <see cref="Tile"/> element at.
    /// </param>
    /// <param name="tilesetTileID">
    ///     The ID of the source tile in the <see cref="Tileset"/> that represents the <see cref="TextureRegion"/> to 
    ///     render for the <see cref="Tile"/> element being set.
    /// </param>
    /// <param name="flipHorizontally">
    ///     Indicates whether the <see cref="Tile"/> element being set should be flipped horizontally when rendered.
    /// </param>
    /// <param name="flipVertically">
    ///     Indicates if the <see cref="Tile"/> element being set should be flipped vertically when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the <see cref="Tile"/> element being set.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row  specified is less than zero or are greater than or equal to the total 
    ///     number of columns or rows in this <see cref="TilemapLayer"/>.
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
    ///     Sets the <see cref="Tile"/> element at the specified column and row location in this <see cref="TilemapLayer"/> 
    ///     using the values provided.
    /// </summary>
    /// <param name="location">
    ///     The column and row location in this <see cref="TilemapLayer"/> to set the <see cref="Tile"/> element at.
    /// </param>
    /// <param name="tilesetTileID">
    ///     The ID of the source tile in the <see cref="Tileset"/> that represents the <see cref="TextureRegion"/> to 
    ///     render for the <see cref="Tile"/> element being set.
    /// </param>
    /// <param name="flipHorizontally">
    ///     Indicates whether the <see cref="Tile"/> element being set should be flipped horizontally when rendered.
    /// </param>
    /// <param name="flipVertically">
    ///     Indicates if the <see cref="Tile"/> element being set should be flipped vertically when rendered.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the <see cref="Tile"/> element being set.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the specified location is less than zero or are greater than or equal 
    ///     to the total number of columns or rows in this <see cref="TilemapLayer"/>.
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
    ///     Sets the specified index in this <see cref="TilemapLayer"/> to the <see cref="Tile"/> element given.
    /// </summary>
    /// <param name="index">
    ///     The index in this <see cref="TilemapLayer"/> to set.
    /// </param>
    /// <param name="tile">
    ///     The <see cref="Tile"/> element to set at the index.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or equal to the total number of
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </exception>
    public void SetTile(int index, Tile tile)
    {
        CheckIndex(index);
        _tiles[index] = tile;
    }

    /// <summary>
    ///     Sets the specified column and row in this <see cref="TilemapLayer"/> to the <see cref="Tile"/> element 
    ///     given.
    /// </summary>
    /// <param name="column">
    ///     The column in this <see cref="TilemapLayer"/> to set.
    /// </param>
    /// <param name="row">
    ///     The row in this <see cref="TilemapLayer"/> to set.
    /// </param>
    /// <param name="tile">
    ///     The <see cref="Tile"/> element to set at the column and row.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row specified are less than zero or are greater than or equal to the total 
    ///     number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public void SetTile(int column, int row, Tile tile)
    {
        CheckColumn(column);
        CheckRow(row);
        int index = ToIndex(column, row);
        _tiles[index] = tile;
    }

    /// <summary>
    ///     Sets the specified column and row location in this <see cref="TilemapLayer"/> to the <see cref="Tile"/>
    ///     element given.
    /// </summary>
    /// <param name="location">
    ///     The column and row location in this <see cref="TilemapLayer"/> to set.
    /// </param>
    /// <param name="tile">
    ///     The <see cref="Tile"/> element to set at the column and row location.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or row in the specified location are less than zero or are greater than or equal
    ///     to the total  number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public void SetTile(Point location, Tile tile)
    {
        CheckLocation(location);
        int index = ToIndex(location.X, location.Y);
        _tiles[index] = tile;
    }

    /// <summary>
    ///     Gets the <see cref="Tile"/> element located at the specified index in this <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tile"/> element in this to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> element located
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or equal to the total number of 
    ///     <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </exception>
    public Tile GetTile(int index)
    {
        CheckIndex(index);
        return _tiles[index];
    }

    /// <summary>
    ///     Gets the <see cref="Tile"/> element located at the specified column and row in this 
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="column">
    ///     The column of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <param name="row">
    ///     The row of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or rows specified are less than zero or are greater than or equal to the total
    ///     number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public Tile GetTile(int column, int row)
    {
        CheckColumn(column);
        CheckRow(row);
        int index = ToIndex(column, row);
        return _tiles[index];
    }

    /// <summary>
    ///     Gets the <see cref="Tile"/> element located at the specified column and row location in this 
    ///     <see cref="TilemapLayer"/>.
    /// </summary>
    /// <param name="location">
    ///     The column and row location of the <see cref="Tile"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="Tile"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if either the column or rows in the specified location are less than zero or are greater than or 
    ///     equal to the total number of columns or rows in this <see cref="TilemapLayer"/>.
    /// </exception>
    public Tile GetTile(Point location)
    {
        CheckLocation(location);
        int index = ToIndex(location.X, location.Y);
        return _tiles[index];
    }


    /// <summary>
    ///     Clears all <see cref="Tile"/> elements in this <see cref="TilemapLayer"/> by resetting them to an empty 
    ///     value.
    /// </summary>
    public void Clear()
    {
        Array.Clear(_tiles);
    }

    /// <summary>
    ///     Draws this <see cref="TilemapLayer"/> layer using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this 
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw this <see cref="TilemapLayer"/> at.  Drawing this 
    ///     <see cref="TilemapLayer"/> using this method ignores the <see cref="TilemapLayer.Offset"/>.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) =>
        Draw(spriteBatch, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws this <see cref="TilemapLayer"/> layer using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this 
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw this <see cref="TilemapLayer"/> at.  Drawing this 
    ///     <see cref="TilemapLayer"/> using this method ignores the <see cref="TilemapLayer.Offset"/>.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws this <see cref="TilemapLayer"/> layer using the 
    ///     <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this 
    ///     <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layer">
    /// The <see cref="TilemapLayer"/> to draw.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to draw this <see cref="TilemapLayer"/> at.  Drawing this 
    ///     <see cref="TilemapLayer"/> using this method ignores the <see cref="TilemapLayer.Offset"/>.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="TilemapLayer"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, Vector2 scale, float layerDepth) =>
        spriteBatch.Draw(this, position, color, scale, layerDepth);


    /// <summary>
    ///     Returns an enumerator that iterates through all <see cref="Tile"/> elements in this 
    ///     <see cref="TilemapLayer"/>.  The order tiles in the enumeration is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through all <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
    /// </returns>
    public IEnumerator<Tile> GetEnumerator()
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            yield return _tiles[i];
        }
    }

    /// <summary>
    ///     Returns an enumerator that iterates through all <see cref="Tile"/> elements in this 
    ///     <see cref="TilemapLayer"/>.  The order tiles in the enumeration is from top-to-bottom, read left-to-right.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through all <see cref="Tile"/> elements in this <see cref="TilemapLayer"/>.
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
