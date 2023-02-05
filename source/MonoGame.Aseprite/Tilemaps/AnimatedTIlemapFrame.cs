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

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
///     Defines a frame of animation in an <see cref="AnimatedTilemap"/>, containing zero or more 
///     <see cref="TilemapLayer"/> elements.
/// </summary>
public sealed class AnimatedTilemapFrame : IEnumerable<TilemapLayer>
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layerLookup = new();

    /// <summary>
    ///     Gets the duration of this <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    ///     Gets the total number of <see cref="TilemapLayer"/> elements in this <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    public int LayerCount => _layers.Count;

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TilemapLayer"/> elements in this <see cref="AnimatedTilemapFrame"/>.
    /// </exception>
    public TilemapLayer this[int layerIndex] => GetLayer(layerIndex);

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element with the specified name in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="AnimatedTilemapFrame"/> does not contain a <see cref="TilemapLayer"/> element with
    ///     the specified name.
    /// </exception>
    public TilemapLayer this[string layerName] => GetLayer(layerName);

    /// <summary>
    ///     Initializes a new instance of the <see cref="AnimatedTilemapFrame"/> class.
    /// </summary>
    /// <param name="duration">
    ///     The duration to assign the <see cref="AnimatedTilemapFrame"/>.
    /// </param>
    public AnimatedTilemapFrame(TimeSpan duration) => Duration = duration;

    /// <summary>
    ///     Creates a new <see cref="TilemapLayer"/> element and adds it to this <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="layerName">
    ///     The name to assign the <see cref="TilemapLayer"/> element created by this method. The name must be unique 
    ///     across all <see cref="TilemapLayer"/> elements in this <see cref="AnimatedTilemapFrame"/>.
    /// </param>
    /// <param name="tileset">
    ///     The source <see cref="Tileset"/> to assign the <see cref="TilemapLayer"/> element created by this method.
    /// </param>
    /// <param name="columns">
    ///     The total number of columns to assign the <see cref="TilemapLayer"/> element created by this method. 
    /// </param>
    /// <param name="rows">
    ///     The total of rows in the <see cref="TilemapLayer"/> element created by this method.
    /// </param>
    /// <param name="offset">
    ///     The x- and y-position offset, relative to the location the <see cref="AnimatedTilemap"/> is rendered, to 
    ///     assign the <see cref="TilemapLayer"/> element created by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/>  created by this method.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="AnimatedTilemapFrame"/> already contains a <see cref="TilemapLayer"/> element with
    ///     the specified name.
    /// </exception>
    public TilemapLayer CreateLayer(string layerName, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        TilemapLayer layer = new(layerName, tileset, columns, rows, offset);
        AddLayer(layer);
        return layer;
    }

    /// <summary>
    ///     Adds the given <see cref="TilemapLayer"/> element to this <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="layer">The <see cref="TilemapLayer"/> element to add.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="AnimatedTilemapFrame"/> already contains a <see cref="TilemapLayer"/> element with
    ///     the same name as the <see cref="TilemapLayer"/> element given.
    /// </exception>
    public void AddLayer(TilemapLayer layer)
    {
        if (_layerLookup.ContainsKey(layer.Name))
        {
            throw new InvalidOperationException($"This tileset frame already contains a tilemap layer element with the name '{layer.Name}'.");
        }

        _layers.Add(layer);
        _layerLookup.Add(layer.Name, layer);
    }

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TilemapLayer"/> elements in this <see cref="AnimatedTilemapFrame"/>.
    /// </exception>
    public TilemapLayer GetLayer(int index)
    {
        if (index < 0 || index >= LayerCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tilemap layer elements in this animated tilemap frame.");
        }

        return _layers[index];
    }

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element with the specified name in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="AnimatedTilemapFrame"/> does not contain a <see cref="TilemapLayer"/> element with
    ///     the specified name.
    /// </exception>
    public TilemapLayer GetLayer(string name)
    {
        if (_layerLookup.TryGetValue(name, out TilemapLayer? layer))
        {
            return layer;
        }

        throw new KeyNotFoundException($"This animated tilemap frame does not contain a tilemap layer element with the name '{name}'.");
    }

    /// <summary>
    ///     Get the <see cref="TilemapLayer"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <param name="layer">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TilemapLayer"/> element located; 
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TilemapLayer"/> element was located at the specified index in this 
    ///     <see cref="AnimatedTilemapFrame"/>; otherwise, <see langword="false"/>.  This method return 
    ///     <see langword="false"/> when the specified index is less than zero or is greater than or equal to the total
    ///     number of <see cref="TilemapLayer"/> elements in this <see cref="AnimatedTilemapFrame"/>.
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
    ///     Gets the <see cref="TilemapLayer"/> element with the specified name in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <param name="layer">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TilemapLayer"/> element located; 
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TilemapLayer"/> element was located in this 
    ///     <see cref="AnimatedTilemapFrame"/> with the specified name; otherwise <see langword="false"/>.  This method 
    ///     returns <see langword="false"/> if this <see cref="AnimatedTilemapFrame"/> does not contain a 
    ///     <see cref="TilemapLayer"/> element with the specified name.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out TilemapLayer? layer) =>
        _layerLookup.TryGetValue(name, out layer);

    /// <summary>
    ///     Removes the <see cref="TilemapLayer"/> element at the specified index in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="TilemapLayer"/> element to remove from this <see cref="AnimatedTilemapFrame"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> element was successfully removed; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if the specified index is less than 
    ///     zero or is greater than or equal to the total number of <see cref="TilemapLayer"/> elements in this tilemap
    ///     frame.
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
    ///     Removes the <see cref="TilemapLayer"/> element with the specified name from this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="TilemapLayer"/> element to remove from this <see cref="AnimatedTilemapFrame"/>
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> element was successfully removed; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this tilemap frame does not 
    ///     contain a <see cref="TilemapLayer"/> element with the specified name.
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
    ///     Removes the given <see cref="TilemapLayer"/> element from this <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    /// <param name="layer">
    /// The <see cref="TilemapLayer"/> element to remove from this <see cref="AnimatedTilemapFrame"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> element was removed successfully; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this tilemap frame does not contain
    ///     the <see cref="TilemapLayer"/> element given.
    /// </returns>
    public bool RemoveLayer(TilemapLayer layer) =>
        _layers.Remove(layer) && _layerLookup.Remove(layer.Name);

    /// <summary>
    ///     Removes all <see cref="TilemapLayer"/> elements from this <see cref="AnimatedTilemapFrame"/>.
    /// </summary>
    public void Clear()
    {
        _layerLookup.Clear();
        _layers.Clear();
    }

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="AnimatedTilemapFrame"/>. The order of elements in the enumeration is from bottom layer to top 
    ///     layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </returns>
    public IEnumerator<TilemapLayer> GetEnumerator() => _layers.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="AnimatedTilemapFrame"/>. The order of elements in the enumeration is from bottom layer to top 
    ///     layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="AnimatedTilemapFrame"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
