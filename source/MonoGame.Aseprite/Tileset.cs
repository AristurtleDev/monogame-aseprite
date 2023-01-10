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
    private TilesetTile[] _tiles;

    public int ID { get; }
    public string Name { get; }
    public Texture2D Texture { get; }
    public Point TileSize { get; }
    public int TileCount { get; }

    public TilesetTile this[int id] => GetTile(id);

    internal Tileset(int id, string name, Texture2D texture, Point tileSize)
    {
        ID = id;
        Name = name;
        Texture = texture;
        TileSize = tileSize;
        TileCount = Texture.Height / TileSize.Y;
        GenerateTiles();
    }

    [MemberNotNull(nameof(_tiles))]
    private void GenerateTiles()
    {
        _tiles = new TilesetTile[TileCount];

        for (int i = 0; i < TileCount; i++)
        {
            int y = TileSize.Y * i;
            Rectangle bounds = new(0, y, TileSize.X, TileSize.Y);
            TilesetTile tile = new(i, $"{Name}_{i}", Texture, bounds);
            _tiles[i] = tile;
        }
    }

    public TilesetTile GetTile(int id)
    {
        if (id < 0 || id >= _tiles.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} cannot be less than zero or greater than or equal {nameof(TileCount)}.");
        }

        return _tiles[id];
    }
}
