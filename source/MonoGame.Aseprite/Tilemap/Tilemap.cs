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

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a tilemap with a collection of zero or more tilemap layers.
/// </summary>
public sealed class Tilemap : IEnumerable<TilemapLayer>
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layerLookup = new();

    /// <summary>
    ///     Gets the name of this tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the total number of tilemap layers in this tilemap.
    /// </summary>
    public int LayerCount => _layers.Count;

    /// <summary>
    ///     Gets the tilemap layer located at the specified index in this
    ///     tilemap.
    /// </summary>
    /// <param name="index">
    ///     The index of the tilemap layer to locate in this tilemap.
    /// </param>
    /// <returns>
    ///     The tilemap layer located at the specified index in this tilemap.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tilemap layers in this tilemap.
    /// </exception>
    public TilemapLayer this[int index] => GetLayer(index);

    /// <summary>
    ///     Gets the tilemap layer with the specified name in this tilemap.
    /// </summary>
    /// <param name="name">
    ///     The name of the tilemap layer to locate in this tilemap.
    /// </param>
    /// <returns>
    ///     The tilemap layer with the specified name in this tilemap.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this tilemap does not contain a tilemap layer with the
    ///     specified name.
    /// </exception>
    public TilemapLayer this[string name] => GetLayer(name);

    /// <summary>
    ///     Creates a new tilemap.
    /// </summary>
    /// <param name="name">
    ///     The name to give the tilemap.
    /// </param>
    public Tilemap(string name) => Name = name;

    /// <summary>
    ///     Creates and adds a new tilemap layer to this tilemap.
    /// </summary>
    /// <param name="layerName">
    ///     The name to give the tilemap layer that is created by this method.
    /// </param>
    /// <param name="tileset">
    ///     The tileset that the tiles of the tilemap layer created by this
    ///     method will use.
    /// </param>
    /// <param name="columns">
    ///     The total number of columns in the tilemap layer that is created by
    ///     this method.
    /// </param>
    /// <param name="rows">
    ///     The total number of rows in the tilemap layer that is created by
    ///     this method.
    /// </param>
    /// <param name="offset">
    ///     The x- and y-position offset to draw the tilemap layer that is
    ///     created by this method, relative to the position that this tilemap
    ///     drawn at.
    /// </param>
    /// <returns>
    ///     The tilemap layer that is created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this tilemap already contains a tilemap layer with the
    ///     name specified.
    /// </exception>
    public TilemapLayer AddLayer(string layerName, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        TilemapLayer layer = new(layerName, tileset, columns, rows, offset);
        AddLayer(layer);
        return layer;
    }

    /// <summary>
    ///     Adds the given tilemap layer to this tilemap.
    /// </summary>
    /// <param name="layer">
    ///     The tilemap layer to add.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this tilemap already contains a tilemap layer with the
    ///     same name as the tilemap layer given.
    /// </exception>
    public void AddLayer(TilemapLayer layer)
    {
        if (_layerLookup.ContainsKey(layer.Name))
        {
            throw new InvalidOperationException();
        }

        _layers.Add(layer);
        _layerLookup.Add(layer.Name, layer);
    }

    /// <summary>
    ///     Gets the tilemap layer located at the specified index in this
    ///     tilemap.
    /// </summary>
    /// <param name="index">
    ///     The index of the tilemap layer to locate in this tilemap.
    /// </param>
    /// <returns>
    ///     The tilemap layer located at the specified index in this tilemap.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of tilemap layers in this tilemap.
    /// </exception>
    public TilemapLayer GetLayer(int index)
    {
        if (index < 0 || index >= LayerCount)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _layers[index];
    }

    /// <summary>
    ///     Gets the tilemap layer with the specified name in this tilemap.
    /// </summary>
    /// <param name="name">
    ///     The name of the tilemap layer to locate in this tilemap.
    /// </param>
    /// <returns>
    ///     The tilemap layer with the specified name in this tilemap.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this tilemap does not contain a tilemap layer with the
    ///     specified name.
    /// </exception>
    public TilemapLayer GetLayer(string name)
    {
        if (_layerLookup.TryGetValue(name, out TilemapLayer? layer))
        {
            return layer;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    ///     Returns a new collection containing all of the tilemap layers in
    ///     this tilemap.  The order of the tilemap layers in the collection
    ///     returned is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    ///     A new collection containing all of the tilemap layers in this
    ///     tilemap.
    /// </returns>
    public List<TilemapLayer> GetLayers() => new List<TilemapLayer>(_layers);

    /// <summary>
    ///     Gets the tilemap layer located at the specified index in this
    ///     tilemap.
    /// </summary>
    /// <param name="index">
    ///     The index of the tilemap layer to locate in this tilemap.
    /// </param>
    /// <param name="layer">
    ///     When this method returns, contains the tilemap layer that was
    ///     located, if the index specified is valid; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if the index specified is valid; otherwise, false.  This method
    ///     returns false if the index is less than zero or is greater than or
    ///     equal to the total number of tilemap layers in this tilemap.
    /// </returns>
    public bool TryGetLayer(int index, [NotNullWhen(true)] out TilemapLayer? layer)
    {
        layer = default;

        if (index < 0 || index >= LayerCount)
        {
            return false;
        }

        layer = _layers[index];
        return true;
    }

    /// <summary>
    ///     Gets the tilemap layer with the specified name in this tilemap.
    /// </summary>
    /// <param name="name">
    ///     The name of the tilemap layer to locate in this tilemap.
    /// </param>
    /// <param name="layer">
    ///     When this method returns, contains the tilemap layer that was
    ///     located, if this tilemap contains a tilemap layer with the
    ///     specified name; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if this tilemap contains a layer with the specified name;
    ///     otherwise, false.  This method returns false if this tilemap does
    ///     not contain a tilemap layer with the specified name.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out TilemapLayer? layer) =>
        _layerLookup.TryGetValue(name, out layer);


    /// <summary>
    ///     Removes the tilemap layer located at the specified index from this
    ///     tilemap layer.
    /// </summary>
    /// <param name="index">
    ///     The index of the tilemap layer to remove from this tilemap.
    /// </param>
    /// <returns>
    ///     true if the tilemap layer located at the specified index was removed
    ///     successfully from this tilemap; otherwise, false.  This method
    ///     returns false if the index specified is less than zero or is greater
    ///     than or equal to the total number of tilemap layers in this tilemap.
    /// </returns>
    public bool RemoveLayer(int index)
    {
        if (index < 0 || index >= LayerCount)
        {
            return false;
        }

        TilemapLayer layer = _layers[index];
        return RemoveLayer(layer);
    }

    /// <summary>
    ///     Removes the tilemap layer with the specified name from this tilemap.
    /// </summary>
    /// <param name="name">
    ///     The name of the tilemap layer to locate and remove from this
    ///     tilemap.
    /// </param>
    /// <returns>
    ///     true if the tilemap layer with the specified name was successfully
    ///     removed from this tilemap; otherwise, false.  This method returns
    ///     false if this tilemap does not contain a tileset with the specified
    ///     name.
    /// </returns>
    public bool RemoveLayer(string name)
    {
        if (_layerLookup.TryGetValue(name, out TilemapLayer? layer))
        {
            return RemoveLayer(layer);
        }

        return false;
    }

    /// <summary>
    ///     Removes the given tilemap layer from this tilemap.
    /// </summary>
    /// <param name="layer">
    ///     The tilemap layer to remove from this tilemap.
    /// </param>
    /// <returns>
    ///     true if the tilemap layer was successfully removed from this
    ///     tilemap; otherwise, false.  This method returns false if this
    ///     tilemap does not contain the given tilemap layer.
    /// </returns>
    public bool RemoveLayer(TilemapLayer layer) =>
        _layers.Remove(layer) && _layerLookup.Remove(layer.Name);

    /// <summary>
    ///     Removes all tilemap layers from this tilemap.
    /// </summary>
    public void Clear()
    {
        _layerLookup.Clear();
        _layers.Clear();
    }

    /// <summary>
    ///     Returns an enumerator used to iterate through all the tilemap layers
    ///     in this tilemap. Order of elements in the enumeration is from the
    ///     bottom most layer to the top most layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all the tilemap layers in this
    ///     tilemap.
    /// </returns>
    public IEnumerator<TilemapLayer> GetEnumerator() => _layers.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator used to iterate through all the tilemap layers
    ///     in this tilemap. Order of elements in the enumeration is from the
    ///     bottom most layer to the top most layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all the tilemap layers in this
    ///     tilemap.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
