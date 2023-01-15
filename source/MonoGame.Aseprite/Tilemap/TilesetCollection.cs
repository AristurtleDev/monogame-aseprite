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
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a collection of uniquely named tilesets.
/// </summary>
public sealed class TilesetCollection : IEnumerable<Tileset>
{
    private List<Tileset> _tilesets = new();
    private Dictionary<string, Tileset> _tilesetByName = new();

    /// <summary>
    ///     Gets the total number of tilesets in this tileset collection.
    /// </summary>
    public int Count => _tilesets.Count;

    /// <summary>
    ///     Gets the tileset located at the specified index in this tileset
    ///     collection.
    /// </summary>
    /// <param name="index">
    ///     The index of the tileset to locate in this tileset collection.
    /// </param>
    /// <returns>
    ///     The tileset located at the specified index in this tileset
    ///     collection.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tilesets in this tileset collection.
    /// </exception>
    public Tileset this[int index] => GetTileset(index);

    /// <summary>
    ///     Gets the tileset with the specified name in this tileset collection.
    /// </summary>
    /// <param name="name">
    ///     The name of the tileset to locate in this tileset collection.
    /// </param>
    /// <returns>
    ///     The tileset located with the specified name in this tileset
    ///     collection.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this tileset collection does not contain a tileset with
    ///     the specified name.
    /// </exception>
    public Tileset this[string name] => GetTileset(name);

    /// <summary>
    ///     Creates a new tileset collection.
    /// </summary>
    public TilesetCollection() { }

    /// <summary>
    ///     Creates a new tileset and adds it to this tileset collection.
    /// </summary>
    /// <param name="name">
    ///     The name to give the tileset that is created by this method.
    /// </param>
    /// <param name="texture">
    ///     The Texture2D to give to give to the tileset that is created by this
    ///     method.
    /// </param>
    /// <param name="tileWidth">
    ///     The width, in pixels, of each tile in the tileset.
    /// </param>
    /// <param name="tileHeight">
    ///     The height, in pixels, of each tile in the tileset.
    /// </param>
    /// <returns>
    ///     The tileset that is created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this tileset collection already contains a tileset with
    ///     the name specified.
    /// </exception>
    public Tileset AddTileset(string name, Texture2D texture, int tileWidth, int tileHeight)
    {
        Tileset tileset = new(name, texture, tileWidth, tileHeight);
        AddTileset(tileset);
        return tileset;
    }

    /// <summary>
    ///     Adds the given tileset to this tileset collection.
    /// </summary>
    /// <param name="tileset">
    ///     The tileset to add.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this tileset collection already contains a tileset with
    ///     same name as the tileset given.
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
    ///     Gets the tileset located at the specified index in this tileset
    ///     collection.
    /// </summary>
    /// <param name="index">
    ///     The index of the tileset to locate in this tileset collection.
    /// </param>
    /// <returns>
    ///     The tileset located at the specified index in this tileset
    ///     collection.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tilesets in this tileset collection.
    /// </exception>
    public Tileset GetTileset(int index)
    {
        if (index < 0 || index >= _tilesets.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the number of tilesets in this {nameof(TilesetCollection)}.");
        }

        return _tilesets[index];
    }

    /// <summary>
    ///     Gets the tileset with the specified name in this tileset collection.
    /// </summary>
    /// <param name="name">
    ///     The name of the tileset to locate in this tileset collection.
    /// </param>
    /// <returns>
    ///     The tileset located with the specified name in this tileset
    ///     collection.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this tileset collection does not contain a tileset with
    ///     the specified name.
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
    ///     Returns a new collection containing all of the tilesets from this
    ///     tileset collection.
    /// </summary>
    /// <returns>
    ///     A new collection containing all of the tilesets from this tileset
    ///     collection.
    /// </returns>
    public List<Tileset> GetTilesets() => new List<Tileset>(_tilesets);

    /// <summary>
    ///     Gets the tileset located at the specified index in this tileset
    ///     collection.
    /// </summary>
    /// <param name="index">
    ///     The index of the tileset to locate in this tileset collection.
    /// </param>
    /// <param name="tileset">
    ///     When this method returns, contains the tileset that was located, if
    ///     the index specified is valid; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if the index specified is valid; otherwise, false.  This method
    ///     returns false if the index is less than zero or is greater than or
    ///     equal to the total number of tilesets in this tileset collection.
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
    ///     Gets the tileset with the specified name in this tileset collection.
    /// </summary>
    /// <param name="name">
    ///     The name of the tileset to locate in this tileset collection.
    /// </param>
    /// <param name="tileset">
    ///     When this method returns, contains the tileset that was located, if
    ///     this tileset collection contains a tileset with the specified name;
    ///     otherwise, null.
    /// </param>
    /// <returns>
    ///     true if this tileset contains a tileset with the specified name;
    ///     otherwise, false.  This method returns false if this tileset does
    ///     not contain a tileset with the specified name.
    /// </returns>
    public bool TryGetTileset(string name, [NotNullWhen(true)] out Tileset? tileset) =>
        _tilesetByName.TryGetValue(name, out tileset);


    /// <summary>
    ///     Removes the tileset located at the specified index from this tileset
    ///     collection.
    /// </summary>
    /// <param name="index">
    ///     The index of the tileset to locate and remove from this tileset
    ///     collection.
    /// </param>
    /// <returns>
    ///     true if the tileset located at the specified index was removed
    ///     successfully from this tileset collection; otherwise, false.  This
    ///     method returns false if the index specified is less than zero or is
    ///     greater than or equal to the total number of tilesets in this
    ///     tileset collection.
    /// </returns>
    public bool RemoveTileset(int index)
    {
        if (index < 0 || index >= Count)
        {
            return false;
        }

        Tileset tileset = _tilesets[index];
        return RemoveTileset(tileset);
    }

    /// <summary>
    ///     Removes the tileset with the specified name from this tileset
    ///     collection.
    /// </summary>
    /// <param name="name">
    ///     The name of the tileset to locate and remove from this tileset
    ///     collection.
    /// </param>
    /// <returns>
    ///     true if the tileset with the specified name was successfully removed
    ///     from this tileset collection; otherwise, false.  This method returns
    ///     false if this tileset collection does not contain a tileset with the
    ///     specified name.
    /// </returns>
    public bool RemoveTileset(string name)
    {
        if (_tilesetByName.TryGetValue(name, out Tileset? tileset))
        {
            return RemoveTileset(tileset);
        }

        return false;
    }

    /// <summary>
    ///     Removes the given tileset from this tileset collection.
    /// </summary>
    /// <param name="tileset">
    ///     The tileset to remove from this tileset collection.
    /// </param>
    /// <returns>
    ///     true if the tileset given was successfully removed from this tileset
    ///     collection; otherwise, false.  This method returns false if this
    ///     tileset collection does not contain the given tileset.
    /// </returns>
    public bool RemoveTileset(Tileset tileset) =>
        _tilesets.Remove(tileset) && _tilesetByName.Remove(tileset.Name);

    /// <summary>
    ///     Removes all tilesets from this tileset collection.
    /// </summary>
    public void Clear()
    {
        _tilesetByName.Clear();
        _tilesets.Clear();
    }

    /// <summary>
    ///     Returns an enumerator that iterates through all of the tilesets in
    ///     this tileset collection.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through all of the tilesets in this
    ///     tileset collection.
    /// </returns>
    public IEnumerator<Tileset> GetEnumerator() => _tilesets.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator that iterates through all of the tilesets in
    ///     this tileset collection.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates through all of the tilesets in this
    ///     tileset collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
