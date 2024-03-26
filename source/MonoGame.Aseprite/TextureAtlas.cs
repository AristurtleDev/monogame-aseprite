// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a <see cref="TextureAtlas"/> with a source image and zero or more <see cref="TextureRegion"/> elements.
/// </summary>
public class TextureAtlas : IEnumerable<TextureRegion>
{
    private List<TextureRegion> _regions = new();
    private Dictionary<string, TextureRegion> _regionLookup = new();

    /// <summary>
    /// Gets the name assigned to this <see cref="TextureAtlas"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source image of this <see cref="TextureAtlas"/>.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    /// Gets the total number of <see cref="TextureRegion"/> elements in this <see cref="TextureAtlas"/>.
    /// </summary>
    public int RegionCount => _regions.Count;

    /// <summary>
    /// Gets the <see cref="TextureRegion"/> element at the specified index in this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="index">
    /// The index of the <see cref="TextureRegion"/> element in this <see cref="TextureAtlas"/> to locate.
    /// </param>
    /// <returns>
    /// The <see cref="TextureRegion"/> element that was located at the specified index in this 
    /// <see cref="TextureAtlas"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of
    /// <see cref="TextureRegion"/> elements in this <see cref="TextureAtlas"/>.
    /// </exception>
    public TextureRegion this[int index] => GetRegion(index);

    /// <summary>
    /// Gets the <see cref="TextureRegion"/> element with the specified name in this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">
    /// The name of the <see cref="TextureRegion"/> element in this <see cref="TextureAtlas"/> to locate.
    /// </param>
    /// <returns>
    /// The <see cref="TextureRegion"/> element that was located with the specified name in this 
    /// <see cref="TextureAtlas"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this <see cref="TextureAtlas"/> does not contain a <see cref="TextureRegion"/> with the specified 
    /// name.
    /// </exception>
    public TextureRegion this[string name] => GetRegion(name);

    /// <summary>
    /// Initializes a new instance of the <see cref="TextureAtlas"/> class.
    /// </summary>
    /// <param name="name">The name to assign the <see cref="TextureAtlas"/>.</param>
    /// <param name="texture">The source image to give the <see cref="TextureAtlas"/>.</param>
    public TextureAtlas(string name, Texture2D texture) => (Name, Texture) = (name, texture);

    private void AddRegion(TextureRegion region)
    {
        if (_regionLookup.ContainsKey(region.Name))
        {
            throw new InvalidOperationException($"This {nameof(TextureAtlas)} already contains a {nameof(TextureRegion)} with the name '{region.Name}'.");
        }

        _regions.Add(region);
        _regionLookup.Add(region.Name, region);
    }

    private bool RemoveRegion(TextureRegion region) =>
         _regions.Remove(region) && _regionLookup.Remove(region.Name);

    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">
    /// The name to assign the <see cref="TextureRegion"/> that is created. The name must be unique across all  
    /// <see cref="TextureRegion"/> in this <see cref="TextureAtlas"/>.
    /// </param>
    /// <param name="x">
    /// The x-coordinate location of the upper-left corner of the <see cref="TextureRegion"/> within the source
    /// image of this <see cref="TextureAtlas"/>.
    /// </param>
    /// <param name="y">
    /// The y-coordinate location of the upper-left corner of the <see cref="TextureRegion"/> within the source
    /// image of this <see cref="TextureAtlas"/>.
    /// </param>
    /// <param name="width">The width, in pixels, of the <see cref="TextureRegion"/>.</param>
    /// <param name="height">The height, in pixels, of the <see cref="TextureRegion"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this <see cref="TextureAtlas"/> already contains a <see cref="TextureRegion"/> element with the 
    /// specified name.
    /// </exception>
    /// <returns>The <see cref="TextureRegion"/> created by this method.</returns>
    public TextureRegion CreateRegion(string name, int x, int y, int width, int height) =>
        CreateRegion(name, new Rectangle(x, y, width, height));

    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">
    /// The name to assign the <see cref="TextureRegion"/> that is created. The name must be unique across all  
    /// <see cref="TextureRegion"/> in this <see cref="TextureAtlas"/>.
    /// </param>
    /// <param name="location">
    /// The x- and y-coordinate location of the upper-left corner of the <see cref="TextureRegion"/> within the
    /// source image of this <see cref="TextureAtlas"/>.
    /// </param>
    /// <param name="size">The width and height extents, in pixels, of the <see cref="TextureRegion"/>.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this <see cref="TextureAtlas"/> already contains a <see cref="TextureRegion"/> element with the 
    /// specified name.
    /// </exception>
    /// <returns>The <see cref="TextureRegion"/> created by this method.</returns>
    public TextureRegion CreateRegion(string name, Point location, Point size) =>
        CreateRegion(name, new Rectangle(location, size));

    /// <summary>
    /// Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">
    /// The name to assign the <see cref="TextureRegion"/> that is created. The name must be unique across all  
    /// <see cref="TextureRegion"/> in this <see cref="TextureAtlas"/>.
    /// </param>
    /// <param name="bounds">
    /// The rectangular bounds of the <see cref="TextureRegion"/> within the source image of this 
    /// <see cref="TextureAtlas"/>.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this <see cref="TextureAtlas"/> already contains a <see cref="TextureRegion"/> element with the 
    /// specified name.
    /// </exception>
    /// <returns>The <see cref="TextureRegion"/> created by this method.</returns>
    public TextureRegion CreateRegion(string name, Rectangle bounds)
    {
        TextureRegion region = new(name, Texture, bounds);
        AddRegion(region);
        return region;
    }

    /// <summary>
    /// Returns a value that indicates whether this <see cref="TextureAtlas"/> contains a 
    /// <see cref="TextureRegion"/> element with the specified name.
    /// </summary>
    /// <param name="name">The name of the <see cref="TextureRegion"/> to locate.</param>
    /// <returns>
    /// <see langword="true"/> if this <see cref="TextureAtlas"/> contains a <see cref="TextureRegion"/> element
    /// with the specified name; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsRegion(string name) => _regionLookup.ContainsKey(name);

    /// <summary>
    /// Returns the index of the <see cref="TextureRegion"/> element with the specified name in this 
    /// <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="TextureRegion"/> to locate.</param>
    /// <returns>The index of the <see cref="TextureRegion"/> located.</returns>
    /// <returns>
    /// <see langword="true"/> if this <see cref="TextureAtlas"/> contains a <see cref="TextureRegion"/> element
    /// with the specified name; otherwise, <see langword="false"/>.
    /// </returns>
    public int GetIndexOfRegion(string name)
    {
        for (int i = 0; i < _regions.Count; i++)
        {
            if (_regions[i].Name == name)
            {
                return i;
            }
        }

        KeyNotFoundException ex = new($"This texture atlas does not contain a texture region with the name '{name}'.");
        ex.Data.Add("TextureRegions", _regions);
        throw ex;
    }

    /// <summary>
    /// Gets the <see cref="TextureRegion"/> element at the specified index in this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="index">The index of the <see cref="TextureRegion"/> element to locate.</param>
    /// <returns>The <see cref="TextureRegion"/> element that was located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of
    /// <see cref="TextureRegion"/> elements in this <see cref="TextureAtlas"/>.
    /// </exception>
    public TextureRegion GetRegion(int index)
    {
        if (index < 0 || index >= _regions.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of {nameof(TextureRegion)} elements in this {nameof(TextureAtlas)}.");
        }

        return _regions[index];
    }

    /// <summary>
    /// Gets the <see cref="TextureRegion"/> element with the specified name in this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="TextureRegion"/> element to locate.</param>
    /// <returns>The <see cref="TextureRegion"/> element that was located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this <see cref="TextureAtlas"/> does not contain a <see cref="TextureRegion"/> element with the 
    /// specified name.
    /// </exception>
    public TextureRegion GetRegion(string name)
    {
        if (_regionLookup.TryGetValue(name, out TextureRegion? frame))
        {
            return frame;
        }

        KeyNotFoundException ex = new($"This texture atlas does not contain a texture region with the name '{name}'.");
        ex.Data.Add("TextureRegions", _regions);
        throw ex;
    }

    /// <summary>
    /// Gets a new <see cref="List{T}"/> of all <see cref="TextureRegion"/> elements at the specified indexes in 
    /// this <see cref="TextureAtlas"/>. Order of the elements in the collection returned is the same as the order 
    /// of the indexes specified.
    /// </summary>
    /// <param name="indexes">The indexes of the <see cref="TextureRegion"/> elements to locate.</param>
    /// <returns>A new <see cref="List{T}"/> containing the <see cref="TextureRegion"/> elements located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if any of the specified indexes are less than zero or if any are greater than or equal to the total
    /// number of <see cref="TextureRegion"/> elements in this <see cref="TextureAtlas"/>.
    /// </exception>
    public List<TextureRegion> GetRegions(params int[] indexes)
    {
        List<TextureRegion> regions = new();
        for (int i = 0; i < indexes.Length; i++)
        {
            regions.Add(GetRegion(indexes[i]));
        }

        return regions;
    }

    /// <summary>
    /// Gets a new <see cref="List{T}"/> of all <see cref="TextureRegion"/> elements with the specified names in 
    /// this <see cref="TextureAtlas"/>. Order of the elements in the collection returned is the same as the order 
    /// of names specified.
    /// </summary>
    /// <param name="names">The names of the <see cref="TextureRegion"/> elements to locate.</param>
    /// <returns>A new <see cref="List{T}"/> containing the <see cref="TextureRegion"/> elements located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if any of the specified names do not match a <see cref="TextureRegion"/> element in this 
    /// <see cref="TextureAtlas"/>.
    /// </exception>
    public List<TextureRegion> GetRegions(params string[] names)
    {
        List<TextureRegion> regions = new();

        for (int i = 0; i < names.Length; i++)
        {
            regions.Add(GetRegion(names[i]));
        }

        return regions;
    }

    /// <summary>
    /// Gets the <see cref="TextureRegion"/> element at the specified index in this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="index">The index of the <see cref="TextureRegion"/> element to locate.</param>
    /// <param name="region">
    /// When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> located; 
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a <see cref="TextureRegion"/> element was located; otherwise, 
    /// <see langword="false"/>.  This method returns <see langword="false"/> if the index specified is less than 
    /// zero or is greater than or equal to the total number of <see cref="TextureRegion"/> elements in this 
    /// <see cref="TextureAtlas"/>.
    /// </returns>
    public bool TryGetRegion(int index, [NotNullWhen(true)] out TextureRegion? region)
    {
        region = default;

        if (index < 0 || index >= _regions.Count)
        {
            return false;
        }

        region = _regions[index];
        return true;
    }

    /// <summary>
    /// Gets the <see cref="TextureRegion"/> element with the specified name in this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="TextureRegion"/> element to locate.</param>
    /// <param name="region">
    /// When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> located; 
    /// otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if a <see cref="TextureRegion"/> element was located; otherwise, 
    /// <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="TextureAtlas"/> 
    /// does not contain a <see cref="TextureRegion"/> element with the specified name.
    /// </returns>
    public bool TryGetRegion(string name, [NotNullWhen(true)] out TextureRegion? region) =>
        _regionLookup.TryGetValue(name, out region);

    /// <summary>
    /// Removes the <see cref="TextureRegion"/> element at the specified index from this <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="index">The index of the <see cref="TextureRegion"/> element to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TextureRegion"/> element was successfully removed; otherwise, 
    /// <see langword="false"/>.  This method returns <see langword="false"/> if the specified index is less than
    /// zero or is greater than or equal to the total number of <see cref="TextureRegion"/> element in this
    /// <see cref="TextureAtlas"/>.
    /// </returns>
    public bool RemoveRegion(int index)
    {
        if (TryGetRegion(index, out TextureRegion? region))
        {
            return RemoveRegion(region);
        }

        return false;
    }

    /// <summary>
    /// Removes the <see cref="TextureRegion"/> element with the specified name from this 
    /// <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="name">The name of the <see cref="TextureRegion"/> element to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the <see cref="TextureRegion"/> element was successfully removed; otherwise, 
    /// <see langword="false"/>.  This method returns <see langword="false"/> if this<see cref="TextureAtlas"/> 
    /// does not contain a <see cref="TextureRegion"/> element with the specified name.
    /// </returns>
    public bool RemoveRegion(string name)
    {
        if (TryGetRegion(name, out TextureRegion? region))
        {
            return RemoveRegion(region);
        }

        return false;
    }

    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> at the specified index in this
    /// <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="spriteName">The name to assign the <see cref="Sprite"/> that is created.</param>
    /// <param name="regionIndex">
    /// The index of the <see cref="TextureRegion"/> element in this <see cref="TextureAtlas"/> assign the 
    /// <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>The <see cref="Sprite"/> that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of
    /// <see cref="TextureRegion"/> elements in this <see cref="TextureAtlas"/>.
    /// </exception>
    public Sprite CreateSprite(string spriteName, int regionIndex)
    {
        TextureRegion region = GetRegion(regionIndex);
        Sprite sprite = new(spriteName, region);
        return sprite;
    }

    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> at the specified index in this
    /// <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="regionIndex">
    /// The index of the <see cref="TextureRegion"/> element to assign the <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>The <see cref="Sprite"/> that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of
    /// <see cref="TextureRegion"/> elements in this <see cref="TextureAtlas"/>.
    /// </exception>
    public Sprite CreateSprite(int regionIndex)
    {
        TextureRegion region = GetRegion(regionIndex);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/>  with the specified name in this
    /// <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="spriteName">The name to assign the <see cref="Sprite"/> that is created.</param>
    /// <param name="regionName">
    /// The name of the <see cref="TextureRegion"/> element in this <see cref="TextureAtlas"/> assign the 
    /// <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>The <see cref="Sprite"/> that is created by this method.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this <see cref="TextureAtlas"/> does not contain a <see cref="TextureRegion"/> with the name 
    /// specified.
    /// </exception>
    public Sprite CreateSprite(string spriteName, string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        Sprite sprite = new(spriteName, region);
        return sprite;
    }

    /// <summary>
    /// Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> with the specified name in this
    /// <see cref="TextureAtlas"/>.
    /// </summary>
    /// <param name="regionName">
    /// The name of the <see cref="TextureRegion"/> element in this <see cref="TextureAtlas"/> assign the 
    /// <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>The <see cref="Sprite"/> that is created by this method.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this <see cref="TextureAtlas"/> does not contain a <see cref="TextureRegion"/> with the name 
    /// specified.
    /// </exception>
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

    /// <summary>
    /// Removes all <see cref="TextureRegion"/> elements from this <see cref="TextureAtlas"/>.
    /// </summary>
    public void Clear()
    {
        //  Remove them in a foreach so that each region is disposed of properly as it's removed
        foreach (TextureRegion region in this)
        {
            RemoveRegion(region);
        }
    }

    /// <inheritdoc/>
    public IEnumerator<TextureRegion> GetEnumerator() => _regions.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
