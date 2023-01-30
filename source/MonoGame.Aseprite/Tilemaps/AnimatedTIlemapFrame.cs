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
/// Defines a frame in a animated tilemap with a collection of tilemap layers.
/// </summary>
public sealed class AnimatedTilemapFrame : IEnumerable<TilemapLayer>
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layerLookup = new();

    /// <summary>
    /// Gets the duration of this animated tilemap frame.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Gets the total number of tilemap layers in this animated tilemap frame.
    /// </summary>
    public int LayerCount => _layers.Count;

    /// <summary>
    /// Get the tilemap layer at the specified index in this animated tilemap frame.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to locate.</param>
    /// <returns>The tilemap layer located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// layers in this animated tilemap frame.
    /// </exception>
    public TilemapLayer this[int layerIndex] => GetLayer(layerIndex);

    /// <summary>
    /// Gets the tilemap layer with the specified name in this animated tilemap frame.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to locate.</param>
    /// <returns>The Tilemap layer located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this animated tilemap frame does not contain a tilemap layer with the specified name.
    /// </exception>
    public TilemapLayer this[string layerName] => GetLayer(layerName);

    /// <summary>
    /// Creates a new tilemap frame.
    /// </summary>
    /// <param name="name">The name of this animated tilemap frame.</param>
    /// <param name="duration">The total amount of time this animated tilemap frame is displayed.</param>
    public AnimatedTilemapFrame(TimeSpan duration) => Duration = duration;

    /// <summary>
    /// Creates a new tilemap layer and adds it to this animated tilemap frame.
    /// </summary>
    /// <param name="layerName">
    /// The name to give the tilemap layer created by this method. The name must be unique for this animated tilemap
    /// frame.
    /// </param>
    /// <param name="tileset">The source tileset used by the tiles of the tilemap layer created by this method.</param>
    /// <param name="columns">The total number of columns in the tilemap layer created by this method. </param>
    /// <param name="rows">The total of rows in the tilemap layer created by this method.</param>
    /// <param name="offset">
    /// The x- and y-position offset to draw the tilemap layer created by this method relative to the position the
    /// tilemap is drawn at.
    /// </param>
    /// <returns>The Tilemap layer created by this method.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this animated tilemap frame already contains a tilemap layer with the specified name.
    /// </exception>
    public TilemapLayer CreateLayer(string layerName, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        TilemapLayer layer = new(layerName, tileset, columns, rows, offset);
        AddLayer(layer);
        return layer;
    }

    /// <summary>
    /// Adds the given tilemap layer to this animated tilemap frame.
    /// </summary>
    /// <param name="layer">The tilemap layer to add.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this animated tilemap frame already contains a tilemap layer with the same name as the tilemap layer
    /// given.
    /// </exception>
    public void AddLayer(TilemapLayer layer)
    {
        if (_layerLookup.ContainsKey(layer.Name))
        {
            throw new InvalidOperationException($"This tileset frame already contains a tilemap layer with the name '{layer.Name}'.");
        }

        _layers.Add(layer);
        _layerLookup.Add(layer.Name, layer);
    }

    /// <summary>
    /// Get the tilemap layer at the specified index in this animated tilemap frame.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to locate.</param>
    /// <returns>The tilemap layer located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// layers in this animated tilemap frame.
    /// </exception>
    public TilemapLayer GetLayer(int index)
    {
        if (index < 0 || index >= LayerCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tilemap layers in this animated tilemap frame.");
        }

        return _layers[index];
    }

    /// <summary>
    /// Gets the tilemap layer with the specified name in this animated tilemap frame.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to locate.</param>
    /// <returns>The Tilemap layer located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this animated tilemap frame does not contain a tilemap layer with the specified name.
    /// </exception>
    public TilemapLayer GetLayer(string name)
    {
        if (_layerLookup.TryGetValue(name, out TilemapLayer? layer))
        {
            return layer;
        }

        throw new KeyNotFoundException($"This tilemap frame does not contain a tilemap layer with the name '{name}'.");
    }

    /// <summary>
    /// Get the tilemap layer at the specified index in this animated tilemap frame.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to locate.</param>
    /// <param name="layer">When this method returns true, contains the tilemap layer located; otherwise, null.</param>
    /// <returns>
    /// true if a tilemap layer was located at the specified index in this animated tilemap frame; otherwise, false.
    /// This method return false when the specified index is less than zero or is greater than or equal to the total
    /// number of tilemap layers in this animated tilemap frame.
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
    /// Gets the tilemap layer with the specified name in this animated tilemap frame.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to locate.</param>
    /// <param name="layer">When this method returns true, contains the tilemap layer located; otherwise, null.</param>
    /// <returns>
    /// true if a tilemap layer was located in this animated tilemap frame with the specified name; otherwise false.
    /// This method returns false if this animated tilemap frame does not contain a tilemap layer with the specified
    /// name.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out TilemapLayer? layer) =>
        _layerLookup.TryGetValue(name, out layer);

    /// <summary>
    /// Removes the tilemap layer at the specified index in this animated tilemap frame.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to remove from this animated tilemap frame.</param>
    /// <returns>
    /// true if the tilemap layer was successfully removed; otherwise, false.  This method returns false if the
    /// specified index is less than zero or is greater than or equal to the total number of tilemap layers in this
    /// tilemap frame.
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
    /// Removes the tilemap layer with the specified name from this animated tilemap frame.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to remove from this animated tilemap frame</param>
    /// <returns>
    /// true if the tilemap layer was successfully removed; otherwise, false.  This method returns false if this tilemap
    /// frame does not contain a tilemap layer with the specified name.
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
    /// Removes the given tilemap layer from this animated tilemap frame.
    /// </summary>
    /// <param name="layer">The tilemap layer to remove from this animated tilemap frame.</param>
    /// <returns>
    /// true if the tilemap layer was removed successfully; otherwise, false.  This method returns false if this tilemap
    /// frame does not contain the tilemap layer given.
    /// </returns>
    public bool RemoveLayer(TilemapLayer layer) =>
        _layers.Remove(layer) && _layerLookup.Remove(layer.Name);

    /// <summary>
    /// Removes all tilemap layers from this animated tilemap frame.
    /// </summary>
    public void Clear()
    {
        _layerLookup.Clear();
        _layers.Clear();
    }

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap layers in this animated tilemap frame.
    /// The order of elements in the enumeration is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap layers in this animated tilemap frame.
    /// </returns>
    public IEnumerator<TilemapLayer> GetEnumerator() => _layers.GetEnumerator();

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap layers in this animated tilemap frame.
    /// The order of elements in the enumeration is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap layers in this animated tilemap frame.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
