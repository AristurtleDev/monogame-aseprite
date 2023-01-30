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
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Tilemaps;

/// <summary>
/// Defines a tilemap with zero or more tilemap layers.
/// </summary>
public sealed class Tilemap : IEnumerable<TilemapLayer>
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layerLookup = new();

    /// <summary>
    /// Gets the name of this tilemap.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the total number of tilemap layers in this tilemap.
    /// </summary>
    public int LayerCount => _layers.Count;

    /// <summary>
    /// Get the tilemap layer at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to locate.</param>
    /// <returns>The tilemap layer located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// layers in this tilemap.
    /// </exception>
    public TilemapLayer this[int layerIndex] => GetLayer(layerIndex);

    /// <summary>
    /// Gets the tilemap layer with the specified name in this tilemap.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to locate.</param>
    /// <returns>The Tilemap layer located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this tilemap does not contain a tilemap layer with the specified name.
    /// </exception>
    public TilemapLayer this[string layerName] => GetLayer(layerName);

    /// <summary>
    /// Creates a new tilemap.
    /// </summary>
    /// <param name="name">The name of this tilemap.</param>
    public Tilemap(string name) => Name = name;

    /// <summary>
    /// Creates a new tilemap layer and adds it to this tilemap.
    /// </summary>
    /// <param name="layerName">
    /// The name to give the tilemap layer created by this method. The name must be unique for this tilemap.
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
    /// Thrown if this tilemap already contains a tilemap layer with the specified name.
    /// </exception>
    public TilemapLayer CreateLayer(string layerName, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        TilemapLayer layer = new(layerName, tileset, columns, rows, offset);
        AddLayer(layer);
        return layer;
    }

    /// <summary>
    /// Adds the given tilemap layer to this tilemap.
    /// </summary>
    /// <param name="layer">The tilemap layer to add.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this tilemap already contains a tilemap layer with the same name as the tilemap layer given.
    /// </exception>
    public void AddLayer(TilemapLayer layer)
    {
        if (_layerLookup.ContainsKey(layer.Name))
        {
            throw new InvalidOperationException($"This tileset already contains a tilemap layer with the name '{layer.Name}'.");
        }

        _layers.Add(layer);
        _layerLookup.Add(layer.Name, layer);
    }

    /// <summary>
    /// Get the tilemap layer at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to locate.</param>
    /// <returns>The tilemap layer located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of tilemap
    /// layers in this tilemap.
    /// </exception>
    public TilemapLayer GetLayer(int index)
    {
        if (index < 0 || index >= LayerCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tilemap layers in this tilemap.");
        }

        return _layers[index];
    }

    /// <summary>
    /// Gets the tilemap layer with the specified name in this tilemap.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to locate.</param>
    /// <returns>The Tilemap layer located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this tilemap does not contain a tilemap layer with the specified name.
    /// </exception>
    public TilemapLayer GetLayer(string name)
    {
        if (_layerLookup.TryGetValue(name, out TilemapLayer? layer))
        {
            return layer;
        }

        throw new KeyNotFoundException($"This tilemap does not contain a tilemap layer with the name '{name}'.");
    }

    /// <summary>
    /// Get the tilemap layer at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to locate.</param>
    /// <param name="layer">When this method returns true, contains the tilemap layer located; otherwise, null.</param>
    /// <returns>
    /// true if a tilemap layer was located at the specified index in this tilemap; otherwise, false.  This method
    /// return false when the specified index is less than zero or is greater than or equal to the total number of
    /// tilemap layers in this tilemap.
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
    /// Gets the tilemap layer with the specified name in this tilemap.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to locate.</param>
    /// <param name="layer">When this method returns true, contains the tilemap layer located; otherwise, null.</param>
    /// <returns>
    /// true if a tilemap layer was located in this tilemap with the specified name; otherwise false.  This method
    /// returns false if this tilemap does not contain a tilemap layer with the specified name.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out TilemapLayer? layer) =>
        _layerLookup.TryGetValue(name, out layer);

    /// <summary>
    /// Removes the tilemap layer at the specified index in this tilemap.
    /// </summary>
    /// <param name="index">The index of the tilemap layer to remove from this tilemap.</param>
    /// <returns>
    /// true if the tilemap layer was successfully removed; otherwise, false.  This method returns false if the
    /// specified index is less than zero or is greater than or equal to the total number of tilemap layers in this
    /// tilemap .
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
    /// Removes the tilemap layer with the specified name from this tilemap.
    /// </summary>
    /// <param name="name">The name of the tilemap layer to remove from this tilemap</param>
    /// <returns>
    /// true if the tilemap layer was successfully removed; otherwise, false.  This method returns false if this tilemap
    /// does not contain a tilemap layer with the specified name.
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
    /// Removes the given tilemap layer from this tilemap.
    /// </summary>
    /// <param name="layer">The tilemap layer to remove from this tilemap.</param>
    /// <returns>
    /// true if the tilemap layer was removed successfully; otherwise, false.  This method returns false if this tilemap
    /// does not contain the tilemap layer given.
    /// </returns>
    public bool RemoveLayer(TilemapLayer layer) =>
        _layers.Remove(layer) && _layerLookup.Remove(layer.Name);

    /// <summary>
    /// Removes all tilemap layers from this tilemap.
    /// </summary>
    public void Clear()
    {
        _layerLookup.Clear();
        _layers.Clear();
    }

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap layers in this tilemap.  The order of
    /// elements in the enumeration is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap layers in this tilemap.
    /// </returns>
    public IEnumerator<TilemapLayer> GetEnumerator() => _layers.GetEnumerator();

    /// <summary>
    /// Returns an enumerator used to iterate through all of the tilemap layers in this tilemap.  The order of
    /// elements in the enumeration is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    /// An enumerator used to iterate through all of the tilemap layers in this tilemap.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a new tilemap from a raw tilemap record.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="rawTilemap">The raw tilemap to create the tilemap from.</param>
    /// <returns>The tilemap created by this method.</returns>
    public static Tilemap FromRaw(GraphicsDevice device, RawTilemap rawTilemap)
    {
        Tilemap tilemap = new(rawTilemap.Name);
        Dictionary<int, Tileset> tilesetLookup = new();

        for (int i = 0; i < rawTilemap.RawTilesets.Length; i++)
        {
            RawTileset rawTileset = rawTilemap.RawTilesets[i];
            Tileset tileset = Tileset.FromRaw(device, rawTileset);
            tilesetLookup.Add(rawTileset.ID, tileset);
        }

        for (int l = 0; l < rawTilemap.RawLayers.Length; l++)
        {
            RawTilemapLayer rawLayer = rawTilemap.RawLayers[l];
            Tileset tileset = tilesetLookup[rawLayer.TilesetID];

            TilemapLayer layer = tilemap.CreateLayer(rawLayer.Name, tileset, rawLayer.Columns, rawLayer.Rows, rawLayer.Offset.ToVector2());

            for (int t = 0; t < rawLayer.RawTilemapTiles.Length; t++)
            {
                RawTilemapTile rawTile = rawLayer.RawTilemapTiles[t];

                layer.SetTile(t, rawTile.TilesetTileID, rawTile.FlipVertically, rawTile.FlipHorizontally, rawTile.Rotation);
            }
        }

        return tilemap;
    }
}
