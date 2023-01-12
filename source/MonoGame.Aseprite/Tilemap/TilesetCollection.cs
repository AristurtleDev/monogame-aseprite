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
///     Represents a collection of <see cref="Tileset"/> elements.
/// </summary>
public sealed class TilesetCollection
{
    private List<Tileset> _tilesets = new();
    private Dictionary<string, Tileset> _tilesetByName = new();

    /// <summary>
    ///     Gets the total number of <see cref="Tileset"/> elements in this
    ///     <see cref="TilesetCollection"/>.
    /// </summary>
    public int Count => _tilesets.Count;

    /// <summary>
    ///     Returns the <see cref="Tileset"/> element at the specified
    ///     <paramref name="index"/> from this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tileset"/> element.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> element at the specified
    ///     <paramref name="index"/> from this <see cref="TilesetCollection"/>.
    /// </returns>
    public Tileset this[int index] => GetTileset(index);

    /// <summary>
    ///     Returns the <see cref="Tileset"/> element with the specified
    ///     <paramref name="name"/> from this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="Tileset"/> element.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> element with the specified
    ///     <paramref name="name"/> from this <see cref="TilesetCollection"/>.
    /// </returns>
    public Tileset this[string name] => GetTileset(name);

    /// <summary>
    ///     Initializes a new instance of the <see cref="TilesetCollection"/>
    ///     class.
    /// </summary>
    public TilesetCollection() { }

    /// <summary>
    ///     Creates a new instance of the <see cref="Tileset"/> class and adds
    ///     it to this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="Tileset"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> used
    ///     by the <see cref="Tileset"/>.
    /// </param>
    /// <param name="tileSize">
    ///     The width and height extents, in pixels, of each tile in the
    ///     <see cref="Tileset"/>.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Tileset"/> class that is created
    ///     by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="TilesetCollection"/> already contains a
    ///     <see cref="Tileset"/> element with the same
    ///     <see cref="Tileset.Name"/> value as the one given.
    /// </exception>
    public Tileset AddTileset(string name, Texture2D texture, Point tileSize)
    {
        Tileset tileset = new(name, texture, tileSize);
        AddTileset(tileset);
        return tileset;
    }

    /// <summary>
    ///     Adds the given <see cref="Tileset"/> to this
    ///     <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="tileset">
    ///     The <see cref="Tileset"/> to add.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="TilesetCollection"/> already contains a
    ///     <see cref="Tileset"/> element with the same
    ///     <see cref="Tileset.Name"/> value as the one given.
    /// </exception>
    public void AddTileset(Tileset tileset)
    {
        if (_tilesetByName.ContainsKey(tileset.Name))
        {
            throw new InvalidOperationException($"This {nameof(TilesetCollection)} already contains a {nameof(Tileset)} with the name '{tileset.Name}'");
        }

        _tilesets.Add(tileset);
        _tilesetByName.Add(tileset.Name, tileset);
    }

    /// <summary>
    ///     Returns the <see cref="Tileset"/> element at the specified
    ///     <paramref name="index"/> from this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tileset"/> element.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> element at the specified
    ///     <paramref name="index"/> from this <see cref="TilesetCollection"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to the number of <see cref="Tileset"/>
    ///     elements in this <see cref="TilesetCollection"/>.
    /// </exception>
    public Tileset GetTileset(int index)
    {
        if (index < 0 || index >= _tilesets.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the number of elements in this {nameof(TilesetCollection)}.");
        }

        return _tilesets[index];
    }

    /// <summary>
    ///     Returns the <see cref="Tileset"/> element with the specified
    ///     <paramref name="name"/> from this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="Tileset"/> element.
    /// </param>
    /// <returns>
    ///     The <see cref="Tileset"/> element with specified
    ///     <paramref name="name"/> from this <see cref="TilesetCollection"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="Tileset"/> element in this
    ///     <see cref="TilesetCollection"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public Tileset GetTileset(string name)
    {
        if (_tilesetByName.TryGetValue(name, out Tileset? tileset))
        {
            return tileset;
        }

        throw new KeyNotFoundException($"No {nameof(Tileset)} with the name '{name}' is present in this {nameof(TilesetCollection)}.");
    }

    /// <summary>
    ///     Returns the <see cref="Tileset"/> element at the specified
    ///     <paramref name="index"/> from this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="Tileset"/> element.
    /// </param>
    /// <param name="tileset">
    ///     When this method returns, contains the <see cref="Tileset"/> element
    ///     from this <see cref="TilesetCollection"/> at the specified
    ///     <paramref name="index"/>, if the index if valid; otherwise,
    ///     <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="index"/> specified
    ///     is valid; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTileset(int index, [NotNullWhen(true)] out Tileset? tileset)
    {
        tileset = default;

        if (index < 0 || index >= _tilesets.Count)
        {
            return false;
        }

        tileset = _tilesets[index];
        return true;
    }

    /// <summary>
    ///     Returns the <see cref="Tileset"/> element with the specified
    ///     <paramref name="name"/> from this <see cref="TilesetCollection"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="Tileset"/> element.
    /// </param>
    /// <param name="tileset">
    ///     When this method returns, contains the <see cref="Tileset"/> element
    ///     from this <see cref="TilesetCollection"/> with the specified
    ///     <paramref name="name"/>, if a match is found; otherwise,
    ///     <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="Tileset"/> element was found
    ///     in this <see cref="TilesetCollection"/> with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetTileset(string name, [NotNullWhen(true)] out Tileset? tileset) =>
        _tilesetByName.TryGetValue(name, out tileset);
}
