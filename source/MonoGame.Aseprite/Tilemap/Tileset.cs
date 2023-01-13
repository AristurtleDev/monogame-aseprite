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
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public sealed class Tileset
{
    private TextureRegion[] _regions;

    /// <summary>
    ///     Gets the name of this <see cref="Tileset"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/>
    ///     used by this <see cref="Tileset"/>,
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the width and height extents, in pixels, of each tile in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    public Size TileSize { get; }

    /// <summary>
    ///     Gets the total number of rows (i.e. how many tiles vertically) in
    ///     this <see cref="Tileset"/>.
    /// </summary>
    public int Rows { get; }

    /// <summary>
    ///     Gets the total number of columns (i.e. how many tiles horizontally)
    ///     in this <see cref="Tileset"/>.
    /// </summary>
    public int Columns { get; }

    /// <summary>
    ///     Gets the total number of regions (tiles) in this
    ///     <see cref="Tileset"/>.
    /// </summary>
    public int TileCount { get; }

    public Tileset(string name, Texture2D texture, Size tileSize)
    {
        if (tileSize.IsEmpty)
        {
            throw new ArgumentException();
        }

        if (texture.Width % tileSize.Width != 0)
        {
            throw new InvalidOperationException();
        }

        if (texture.Height % tileSize.Height != 0)
        {
            throw new InvalidOperationException();
        }

        Name = name;
        Texture = texture;
        TileSize = tileSize;
        Columns = texture.Width / tileSize.Width;
        Rows = texture.Height / tileSize.Height;
        TileCount = Columns * Rows;
        CreateTextureRegions();
    }

    [MemberNotNull(nameof(_regions))]
    private void CreateTextureRegions()
    {
        _regions = new TextureRegion[TileCount];

        for (int i = 0; i < TileCount; i++)
        {
            int x = (i % Columns) * TileSize.Width;
            int y = (i / Columns) * TileSize.Height;

            Rectangle bounds = new(x, y, TileSize.Width, TileSize.Height);
            _regions[i] = new TextureRegion($"{Name}_{i}", Texture, bounds);
        }
    }

    public TextureRegion GetTile(int index)
    {
        if(index < 0 || index >= TileCount)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _regions[index];
    }

    public TextureRegion GetTile(Point location) => GetTile(location.X, location.Y);

    public TextureRegion GetTile(int column, int row)
    {
        int index = row * Columns + column;
        return GetTile(index);
    }

    public bool TryGetTile(int index, [NotNullWhen(true)] out TextureRegion? tile)
    {
        tile = default;

        if(index < 0 || index >= TileCount)
        {
            return false;
        }

        tile = _regions[index];
        return true;
    }

    public bool TryGetTile(Point location, [NotNullWhen(true)] out TextureRegion? tile) =>
        TryGetTile(location.X, location.Y, out tile);

    public bool TryGetTile(int column, int row, [NotNullWhen(true)] out TextureRegion? tile)
    {
        int index = row * Columns + column;
        return TryGetTile(index, out tile);
    }


    // private TilesetTile[] _tiles;

    // public string Name { get; }
    // public Texture2D Texture { get; }
    // public Point TileSize { get; }
    // public int TileCount { get; }

    // public TilesetTile this[int id] => GetTile(id);

    // internal Tileset(string name, Texture2D texture, Point tileSize)
    // {
    //     Name = name;
    //     Texture = texture;
    //     TileSize = tileSize;
    //     TileCount = Texture.Height / TileSize.Y;
    //     GenerateTiles();
    // }

    // [MemberNotNull(nameof(_tiles))]
    // private void GenerateTiles()
    // {
    //     _tiles = new TilesetTile[TileCount];

    //     for (int i = 0; i < TileCount; i++)
    //     {
    //         int y = TileSize.Y * i;
    //         Rectangle bounds = new(0, y, TileSize.X, TileSize.Y);
    //         TilesetTile tile = new(i, $"{Name}_{i}", Texture, bounds);
    //         _tiles[i] = tile;
    //     }
    // }

    // public TilesetTile GetTile(int id)
    // {
    //     if (id < 0 || id >= _tiles.Length)
    //     {
    //         throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} cannot be less than zero or greater than or equal {nameof(TileCount)}.");
    //     }

    //     return _tiles[id];
    // }
}
