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
///     Defines a tilemap with a collection of zero or more <see cref="TilemapLayer"/> elements.
/// </summary>
public sealed class Tilemap : IEnumerable<TilemapLayer>
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layerLookup = new();

    /// <summary>
    ///     Gets the name of this <see cref="Tilemap"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the total number of <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.
    /// </summary>
    public int LayerCount => _layers.Count;

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/> at the specified index.
    /// </summary>
    /// <param name="layerIndex">
    ///     The index of the <see cref="TilemapLayer"/> element in this <see cref="Tilemap"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element that was located at the specified index in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.  Use
    ///     <see cref="Tilemap.LayerCount"/> to determine the total number of elements.
    /// </exception>
    public TilemapLayer this[int layerIndex] => GetLayer(layerIndex);

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/> with the specified name.
    /// </summary>
    /// <param name="layerName">
    ///     The name of the <see cref="TilemapLayer"/> element in this <see cref="Tilemap"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element that was located with the specified name in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="Tilemap"/> does not contain a <see cref="TilemapLayer"/> element with the
    ///     specified name.
    /// </exception>
    public TilemapLayer this[string layerName] => GetLayer(layerName);

    /// <summary>
    ///     Initializes a new instance of the <see cref="Tilemap"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="Tilemap"/>.
    /// </param>
    public Tilemap(string name) => Name = name;

    /// <summary>
    ///     Creates a new <see cref="TilemapLayer"/> and adds it to this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layerName">
    ///     The name to give the <see cref="TilemapLayer"/> that is created by this method.  Must be unique for this
    ///     <see cref="Tilemap"/>.
    /// </param>
    /// <param name="tileset">
    ///     The <see cref="Tileset"/> that the <see cref="Tile"/> elements of the <see cref="TilemapLayer"/> created by
    ///     this method will use.
    /// </param>
    /// <param name="columns">
    ///     The total number of columns in the <see cref="TilemapLayer"/> that is created by this method.
    /// </param>
    /// <param name="rows">
    ///     The total number of rows in the <see cref="TilemapLayer"/> that is created by this method.
    /// </param>
    /// <param name="offset">
    ///     The x- and y-position offset to draw the <see cref="TilemapLayer"/> that is created by this method, relative
    ///     to the position tha this <see cref="Tilemap"/> draws at.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="TilemapLayer"/> class that is created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="Tilemap"/> already contains a <see cref="TilemapLayer"/> with the specified name.
    /// </exception>
    public TilemapLayer CreateLayer(string layerName, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        TilemapLayer layer = new(layerName, tileset, columns, rows, offset);
        AddLayer(layer);
        return layer;
    }

    /// <summary>
    ///     Adds the given <see cref="TilemapLayer"/> to this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layer">
    ///     The <see cref="TilemapLayer"/> to add.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="Tilemap"/> already contains a <see cref="TilemapLayer"/> with the same name as the
    ///     <see cref="TilemapLayer"/> given.
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
    ///     Gets the <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/> at the specified index.
    /// </summary>
    /// <param name="layerIndex">
    ///     The index of the <see cref="TilemapLayer"/> element in this <see cref="Tilemap"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element that was located at the specified index in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.  Use
    ///     <see cref="Tilemap.LayerCount"/> to determine the total number of elements.
    /// </exception>
    public TilemapLayer GetLayer(int layerIndex)
    {
        if (layerIndex < 0 || layerIndex >= LayerCount)
        {
            throw new ArgumentOutOfRangeException();
        }

        return _layers[layerIndex];
    }

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/> with the specified name.
    /// </summary>
    /// <param name="layerName">
    ///     The name of the <see cref="TilemapLayer"/> element in this <see cref="Tilemap"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element that was located with the specified name in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="Tilemap"/> does not contain a <see cref="TilemapLayer"/> element with the
    ///     specified name.
    /// </exception>
    public TilemapLayer GetLayer(string layerName)
    {
        if (_layerLookup.TryGetValue(layerName, out TilemapLayer? layer))
        {
            return layer;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    ///     Returns a new <see cref="List{T}"/> containing all of the <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.  The order of elements in teh collection returns is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    ///     A new <see cref="List{T}"/> containing all of the <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    public List<TilemapLayer> GetLayers() => new List<TilemapLayer>(_layers);

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/> at the specified index.
    /// </summary>
    /// <param name="layerIndex">
    ///     The index of the <see cref="TilemapLayer"/> element in this <see cref="Tilemap"/> to locate.
    /// </param>
    /// <param name="layer">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TilemapLayer"/> located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TilemapLayer"/> element was located at the specified index;
    ///     otherwise, <see langword="false"/>.  This method returns <see langword="false"/> if the index specified is
    ///     less than zero or is greater than or equal to the total number of <see cref="TilemapLayer"/> elements in
    ///     this <see cref="Tilemap"/>.  Use <see cref="Tilemap.LayerCount"/> to determine the total number of elements.
    /// </returns>
    public bool TryGetLayer(int layerIndex, [NotNullWhen(true)] out TilemapLayer? layer)
    {
        layer = default;

        if (layerIndex < 0 || layerIndex >= LayerCount)
        {
            return false;
        }

        layer = _layers[layerIndex];
        return true;
    }

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/> with the specified name.
    /// </summary>
    /// <param name="layerName">
    ///     The name of the <see cref="TilemapLayer"/> element in this <see cref="Tilemap"/> to locate.
    /// </param>
    /// <param name="layer">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TilemapLayer"/> located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TilemapLayer"/> element was located with the specified name;
    ///     otherwise, <see langword="false"/>.  This method returns <see langword="false"/> if this
    ///     <see cref="Tilemap"/> does not contain a <see cref="TilemapLayer"/> element with the specified name.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out TilemapLayer? layer) =>
        _layerLookup.TryGetValue(name, out layer);


    /// <summary>
    ///     Removes the <see cref="TilemapLayer"/> element at the specified index from this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layerIndex">
    ///     The index of the <see cref="TilemapLayer"/> element to remove from this <see cref="Tilemap"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method return <see langword="false"/> if the specified index is less than
    ///     zero or is greater than or equal to the total number of <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.  Use <see cref="Tilemap.LayerCount"/> to determine the total number of elements.
    /// </returns>
    public bool RemoveLayer(int layerIndex)
    {
        if (layerIndex < 0 || layerIndex >= LayerCount)
        {
            return false;
        }

        TilemapLayer layer = _layers[layerIndex];
        return RemoveLayer(layer);
    }

    /// <summary>
    ///     Removes the <see cref="TilemapLayer"/> element with the specified name from this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layerName">
    ///     The name of the <see cref="TilemapLayer"/> element to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="Tilemap"/> does not
    ///     contain a <see cref="TilemapLayer"/> element with the specified name.
    /// </returns>
    public bool RemoveLayer(string layerName)
    {
        if (_layerLookup.TryGetValue(layerName, out TilemapLayer? layer))
        {
            return RemoveLayer(layer);
        }

        return false;
    }

    /// <summary>
    ///     Removes the given <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layer">
    ///     The <see cref="TilemapLayer"/> element to remove from this <see cref="Tilemap"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> was successfully removed; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="Tilemap"/> does not
    ///     contain the given <see cref="TilemapLayer"/>.
    /// </returns>
    public bool RemoveLayer(TilemapLayer layer) =>
        _layers.Remove(layer) && _layerLookup.Remove(layer.Name);

    /// <summary>
    ///     Removes all <see cref="TilemapLayer"/> elements from this <see cref="Tilemap"/>.
    /// </summary>
    public void Clear()
    {
        _layerLookup.Clear();
        _layers.Clear();
    }

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.  Order of elements in the enumeration is from the bottom most layer to the top most
    ///     layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    public IEnumerator<TilemapLayer> GetEnumerator() => _layers.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.  Order of elements in the enumeration is from the bottom most layer to the top most
    ///     layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
