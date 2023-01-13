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

public sealed class TilemapLayer
{
    private Tile[] _tiles;

    public string Name { get; }
    public Size Size { get; }
    public Tileset Tileset { get; }
    public int TileCount => _tiles.Length;

    public Tile? this[int index] => GetTile(index);
    public Tile? this[int column, int row] => GetTile(column, row);
    public Tile? this[Point location] => GetTile(location);

    //  Change size to int columns, int rows????
    //  Since it's not a really a Size?
    internal TilemapLayer(Tileset tileset, string name, Size size)
    {
        Tileset = tileset;
        Name = name;
        Size = size;

        _tiles = new Tile[size.Width * size.Height];
    }

    public AddTile(Tile tile)
    {
        if(tile.TilesetIndex >= Tileset.)
    }

    public Tile? GetTile(int index)
    {
        if (index < 0 || index >= TileCount)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _tiles[index];
    }

    public Tile? GetTile(int column, int row)
    {
        int index = row * Size.X + column;
        return GetTile(index);
    }

    public Tile? GetTile(Point location) => GetTile(location.X, location.Y);

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

    public bool TryGetTile(int column, int row, [NotNullWhen(true)] out Tile? tile)
    {
        tile = default;

        if (column < 0 || column >= Size.X || row < 0 || row >= Size.Y)
        {
            return false;
        }

        int index = row * Size.X + column;
        tile = _tiles[index];
        return tile is not null;
    }

    public bool TryGetTile(Point location, [NotNullWhen(true)] out Tile? tile) =>
        TryGetTile(location.X, location.Y, out tile);
}
