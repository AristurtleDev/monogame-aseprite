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
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public sealed class TilesetCollection
{
    private List<Tileset> _tilesetByID = new();
    private Dictionary<string, Tileset> _tilesetByName = new();

    public int Count => _tilesetByID.Count;

    public Tileset this[int id] => GetTileset(id);
    public Tileset this[string name] => GetTileset(name);

    internal TilesetCollection() { }

    internal Tileset CreateTileset(string name, Texture2D texture, Point tileSize)
    {
        if (_tilesetByName.ContainsKey(name))
        {
            throw new InvalidOperationException($"This {nameof(TilesetCollection)} already contains a {nameof(Tileset)} with the name '{name}'");
        }

        int id = _tilesetByID.Count;

        Tileset tileset = new(id, name, texture, tileSize);

        _tilesetByID.Add(tileset);
        _tilesetByName.Add(name, tileset);

        return tileset;
    }

    public Tileset GetTileset(string name)
    {
        if (_tilesetByName.TryGetValue(name, out Tileset? tileset))
        {
            return tileset;
        }

        throw new KeyNotFoundException($"No {nameof(Tileset)} with the name '{name}' is present in this {nameof(TilesetCollection)}.");
    }

    public Tileset GetTileset(int id)
    {
        if (id < 0 || id >= _tilesetByID.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} cannot be less than zero or greater than or equal to the number of elements in this {nameof(TilesetCollection)}.");
        }

        return _tilesetByID[id];
    }
}
