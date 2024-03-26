// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MonoGame.Aseprite;

/// <summary>
///     Defines a <see cref="Tilemap"/> with zero or more <see cref="TilemapLayer"/> elements.
/// </summary>
public sealed class Tilemap : IEnumerable<TilemapLayer>
{
    private List<TilemapLayer> _layers = new();
    private Dictionary<string, TilemapLayer> _layerLookup = new();

    /// <summary>
    ///     Gets the name assigned to this <see cref="Tilemap"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the total number of <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.
    /// </summary>
    public int LayerCount => _layers.Count;

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element at the specified index in this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layerIndex">
    ///     The index of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of 
    ///     <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.
    /// </exception>
    public TilemapLayer this[int layerIndex] => GetLayer(layerIndex);

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element with the specified name in this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layerName">
    ///     The name of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
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
    ///     The name to assign <see cref="Tilemap"/>.
    /// </param>
    public Tilemap(string name) => Name = name;

    /// <summary>
    ///     Creates a new <see cref="TilemapLayer"/> element and adds it to this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layerName">
    ///     The name to give the <see cref="TilemapLayer"/> element created by this method. The name must be unique
    ///     across all <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="tileset">
    ///     The source tileset to assign the <see cref="TilemapLayer"/> element created by this method.
    /// </param>
    /// <param name="columns">
    ///     The total number of columns to assign the <see cref="TilemapLayer"/> element created by this method. 
    /// </param>
    /// <param name="rows">
    ///     The total of rows to assign the <see cref="TilemapLayer"/> element created by this method.
    /// </param>
    /// <param name="offset">
    ///     The x- and y-position offset, relative to the location the <see cref="Tilemap"/> is rendered, to 
    ///     assign the <see cref="TilemapLayer"/> element created by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="Tilemap"/> already contains a <see cref="TilemapLayer"/> element with the 
    ///     specified name.
    /// </exception>
    public TilemapLayer CreateLayer(string layerName, Tileset tileset, int columns, int rows, Vector2 offset)
    {
        TilemapLayer layer = new(layerName, tileset, columns, rows, offset);
        AddLayer(layer);
        return layer;
    }

    /// <summary>
    ///     Adds the given <see cref="TilemapLayer"/> element to this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layer">
    ///     The <see cref="TilemapLayer"/> element to add.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="Tilemap"/> already contains a <see cref="TilemapLayer"/> element with the same 
    ///     name as the <see cref="TilemapLayer"/> element given.
    /// </exception>
    public void AddLayer(TilemapLayer layer)
    {
        if (_layerLookup.ContainsKey(layer.Name))
        {
            throw new InvalidOperationException($"This tileset already contains a tilemap layer element with the name '{layer.Name}'.");
        }

        _layers.Add(layer);
        _layerLookup.Add(layer.Name, layer);
    }

    /// <summary>
    ///     Get the <see cref="TilemapLayer"/> element at the specified index in this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> element located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of 
    ///     <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.
    /// </exception>
    public TilemapLayer GetLayer(int index)
    {
        if (index < 0 || index >= LayerCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of tilemap layer elements in this tilemap.");
        }

        return _layers[index];
    }

    /// <summary>
    ///     Gets the <see cref="TilemapLayer"/> element with the specified name in this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TilemapLayer"/> located.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="Tilemap"/> does not contain a <see cref="TilemapLayer"/> element with the 
    ///     specified name.
    /// </exception>
    public TilemapLayer GetLayer(string name)
    {
        if (_layerLookup.TryGetValue(name, out TilemapLayer? layer))
        {
            return layer;
        }

        throw new KeyNotFoundException($"This tilemap does not contain a tilemap layer element with the name '{name}'.");
    }

    /// <summary>
    ///     Get the <see cref="TilemapLayer"/> element at the specified index in this <see cref="Tilemap"/>.
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
    ///     <see cref="Tilemap"/>; otherwise, <see langword="false"/>.  This method return <see langword="false"/> when
    ///     the specified index is less than zero or is greater than or equal to the total number of 
    ///     <see cref="TilemapLayer"/> elements in this <see cref="Tilemap"/>.
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
    ///     Gets the <see cref="TilemapLayer"/> element with the specified name in this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="TilemapLayer"/> element to locate.
    /// </param>
    /// <param name="layer">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TilemapLayer"/> element located; 
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TilemapLayer"/> element was located in this <see cref="Tilemap"/> 
    ///     with the specified name; otherwise <see langword="false"/>.  This method returns <see langword="false"/> if 
    ///     this <see cref="Tilemap"/> does not contain a <see cref="TilemapLayer"/> element with the specified name.
    /// </returns>
    public bool TryGetLayer(string name, [NotNullWhen(true)] out TilemapLayer? layer) =>
        _layerLookup.TryGetValue(name, out layer);

    /// <summary>
    ///     Removes the <see cref="TilemapLayer"/> element at the specified index in this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="TilemapLayer"/> element to remove from this <see cref="Tilemap"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> element was successfully removed; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if the specified index is less than 
    ///     zero or is greater than or equal to the total number of <see cref="TilemapLayer"/> elements in this
    ///     <see cref="Tilemap"/>.
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
    ///     Removes the <see cref="TilemapLayer"/> element with the specified name from this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="TilemapLayer"/> element to remove from this <see cref="Tilemap"/>
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> element was successfully removed; otherwise, 
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="Tilemap"/> does not
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
    ///     Removes the given <see cref="TilemapLayer"/> element from this <see cref="Tilemap"/>.
    /// </summary>
    /// <param name="layer">
    /// The <see cref="TilemapLayer"/> element to remove from this <see cref="Tilemap"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TilemapLayer"/> element was removed successfully; otherwise, 
    ///     <see langword="false"/>.  This method returns false if this <see cref="Tilemap"/> does not contain the 
    ///     <see cref="TilemapLayer"/> element given.
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
    ///     Draws this <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) =>
        Draw(spriteBatch, position, color, Vector2.One, 0.0f);

    /// <summary>
    ///     Draws this <see cref="Tilemap"/> using the <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, float scale, float layerDepth) =>
        Draw(spriteBatch, position, color, new Vector2(scale, scale), layerDepth);

    /// <summary>
    ///     Draws a <see cref="Tilemap"/> using this <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/>.
    /// </summary>
    /// <param name="spriteBatch">
    /// The <see cref="Microsoft.Xna.Framework.Graphics.SpriteBatch"/> to use for rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate location to render this <see cref="Tilemap"/> at.
    /// </param>
    /// <param name="color">
    ///     The color mask to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="scale">
    ///     The amount of scaling to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering this <see cref="Tilemap"/>.
    /// </param>
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color, Vector2 scale, float layerDepth) =>
        spriteBatch.Draw(this, position, color, scale, layerDepth);

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="Tilemap"/>.  The order of elements in the enumeration is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="Tilemap"/>.
    /// </returns>
    public IEnumerator<TilemapLayer> GetEnumerator() => _layers.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="Tilemap"/>.  The order of elements in the enumeration is from bottom layer to top layer.
    /// </summary>
    /// <returns>
    ///     An enumerator used to iterate through all of the <see cref="TilemapLayer"/> elements in this 
    ///     <see cref="Tilemap"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
