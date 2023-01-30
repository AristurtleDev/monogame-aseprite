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

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a texture atlas with a source image and zero or more named texture regions.
/// </summary>
public class TextureAtlas : IEnumerable<TextureRegion>
{
    private List<TextureRegion> _regions = new();
    private Dictionary<string, TextureRegion> _regionLookup = new();

    /// <summary>
    /// Gets the name of this texture atlas.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the source texture image used by this texture atlas.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    /// Gets the total number of texture regions in this texture atlas.
    /// </summary>
    public int RegionCount => _regions.Count;

    /// <summary>
    /// Gets the texture region element at the specified index in this texture atlas.
    /// </summary>
    /// <param name="index">The index of the texture region element in this texture atlas to locate.</param>
    /// <returns>The texture region element that was located at the specified index in this texture atlas.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of texture
    /// region elements in this texture atlas.
    /// </exception>
    public TextureRegion this[int index] => GetRegion(index);

    /// <summary>
    /// Gets the texture region element with the specified name in this texture atlas.
    /// </summary>
    /// <param name="name">The name of the texture region element in this texture atlas to locate.</param>
    /// <returns>The texture region element that was located with the specified name in this texture atlas.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this texture atlas does not contain a texture region with the specified name.
    /// </exception>
    public TextureRegion this[string name] => GetRegion(name);

    /// <summary>
    /// Creates a new texture atlas.
    /// </summary>
    /// <param name="name">The name to give this texture atlas.</param>
    /// <param name="texture">The source texture image used by this atlas.</param>
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
    /// Creates a new texture region and adds it to this texture atlas.
    /// </summary>
    /// <param name="name">
    /// The name to give the texture region that is created by this method. The name must be unique for this
    /// texture atlas.
    /// </param>
    /// <param name="x">
    /// The x-coordinate location of the upper-left corner of the texture region within the source texture.
    /// </param>
    /// <param name="y">
    /// The y-coordinate location of the upper-left corner of the texture region within the source texture.
    /// </param>
    /// <param name="width">The width, in pixels, of the texture region.</param>
    /// <param name="height">The height, in pixels, of the texture region.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this texture atlas already contains a texture region with the specified name.
    /// </exception>
    /// <returns>The texture region that is created by this method.</returns>
    public TextureRegion CreateRegion(string name, int x, int y, int width, int height) =>
        CreateRegion(name, new Rectangle(x, y, width, height));

    /// <summary>
    /// Creates a new texture region and adds it to this texture atlas.
    /// </summary>
    /// <param name="name">
    /// The name to give the texture region that is created by this method.  The name must be unique for this
    /// texture atlas.
    /// </param>
    /// <param name="location">
    /// The x- and y-coordinate location of the upper-left corner of the texture region within the source texture.
    /// </param>
    /// <param name="size">The width and height extents, in pixels, of the texture region.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this texture atlas already contains a texture region with the specified name.
    /// </exception>
    /// <returns>The texture region that is created by this method.</returns>
    public TextureRegion CreateRegion(string name, Point location, Point size) =>
        CreateRegion(name, new Rectangle(location, size));

    /// <summary>
    /// Creates a new texture region and adds it to this texture atlas.
    /// </summary>
    /// <param name="name">
    /// The name to give the texture region that is created by this method.  The name must be unique for this
    /// texture atlas.
    /// </param>
    /// <param name="bounds">The rectangular bounds of the texture region within the source texture.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this texture atlas already contains a texture region with the specified name.
    /// </exception>
    /// <returns>The texture region that is created by this method.</returns>
    public TextureRegion CreateRegion(string name, Rectangle bounds)
    {
        TextureRegion region = new(name, Texture, bounds);
        AddRegion(region);
        return region;
    }

    /// <summary>
    /// Returns a value that indicates whether this texture atlas contains a texture region with the specified name.
    /// </summary>
    /// <param name="name">The name of the texture region to locate in this texture atlas.</param>
    /// <returns>
    /// true if this texture atlas contains a texture region with the specified name; otherwise, false.
    /// </returns>
    public bool ContainsRegion(string name) => _regionLookup.ContainsKey(name);

    /// <summary>
    /// Returns the index of the texture region with the specified name in this texture atlas.
    /// </summary>
    /// <param name="name">The name of the texture region to locate in this texture atlas.</param>
    /// <returns>The index of the texture region located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this texture atlas does not contain a texture region with the name specified.
    /// </exception>
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
    /// Gets the texture region element at the specified index in this texture atlas.
    /// </summary>
    /// <param name="index">The index of the texture region element in this texture atlas to locate.</param>
    /// <returns>The texture region element that was located at the specified index in this texture atlas.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified index is less than zero or is greater than or equal to the total number of texture
    /// region elements in this texture atlas.
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
    /// Gets the texture region element with the specified name in this texture atlas.
    /// </summary>
    /// <param name="name">The name of the texture region element in this texture atlas to locate.</param>
    /// <returns>The texture region element that was located with the specified name in this texture atlas.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this texture atlas does not contain a texture region with the specified name.
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
    /// Gets a collection of all texture region at the specified indexes in this texture atlas. Order of the texture
    /// regions in the collection returned is the same as the order of the indexes specified.
    /// </summary>
    /// <param name="indexes">The indexes of the texture regions in this texture atlas to locate.</param>
    /// <returns>A new collection containing the texture regions located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if any of the specified indexes are less than zero or if any are greater than or equal to the total
    /// number of texture regions in this texture atlas.
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
    /// Gets a collection of all texture regions with the specified names in this texture atlas. Order of the
    /// texture regions in the collection returned is the same as the order of names specified.
    /// </summary>
    /// <param name="names">The names of the texture regions in this texture atlas to locate.</param>
    /// <returns>A new collection containing the texture texture region elements located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if any of the specified names do not match a texture region in this texture atlas.
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
    /// Gets the texture region element at the specified index in this texture atlas.
    /// </summary>
    /// <param name="index">The index of the texture region element in this texture atlas to locate.</param>
    /// <param name="region">
    /// When this method returns true, contains the texture region located; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a texture region element was located at the specified index; otherwise, false.  This method returns
    /// false if the index specified is less than zero or is greater than or equal to the total number of texture
    /// regions in this texture atlas.
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
    /// Gets the texture region element with the specified name in this texture atlas.
    /// </summary>
    /// <param name="name">The name of the texture region element in this texture atlas to locate.</param>
    /// <param name="region">
    /// When this method returns true, contains the texture region located; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a texture region element was located with the specified name; otherwise, false.  This method returns
    /// false if this texture atlas does not contain a texture region with the specified name.
    /// </returns>
    public bool TryGetRegion(string name, [NotNullWhen(true)] out TextureRegion? region) =>
        _regionLookup.TryGetValue(name, out region);

    /// <summary>
    /// Removes the texture region at the specified index from this texture atlas.
    /// </summary>
    /// <remarks>
    /// When a texture region is removed from this texture atlas, the texture region instance will be disposed of.
    /// </remarks>
    /// <param name="index">The index of the texture region in this texture atlas to remove.</param>
    /// <returns>
    /// true if the texture region was successfully removed; otherwise, false.  This method returns false if the
    /// specified index is less than zero or is greater than or equal to the total number of texture regions in this
    /// texture atlas.
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
    /// Removes the texture region with the specified name from this texture atlas.
    /// </summary>
    /// <remarks>
    /// When a texture region is removed from this texture atlas, the texture region instance will be disposed of.
    /// </remarks>
    /// <param name="name">The name of the texture region to remove.</param>
    /// <returns>
    /// true if the texture region was successfully removed; otherwise, false.  This method returns false if this
    /// texture atlas does not contain a texture region with the specified name.
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
    ///Removes all texture regions from this texture atlas.
    /// </summary>
    /// <remarks>
    ///When a texture region is removed from this texture atlas, the texture region instance will be disposed of.
    /// </remarks>
    public void Clear()
    {
        //  Remove them in a foreach so that each region is disposed of properly as it's removed
        foreach (TextureRegion region in this)
        {
            RemoveRegion(region);
        }
    }

    /// <summary>
    ///     Returns an enumerator that iterates each texture region in this texture atlas.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates each texture region in this texture atlas.
    /// </returns>
    public IEnumerator<TextureRegion> GetEnumerator() => _regions.GetEnumerator();

    /// <summary>
    ///     Returns an enumerator that iterates each texture region in this texture atlas.
    /// </summary>
    /// <returns>
    ///     An enumerator that iterates each texture region in this texture atlas.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Creates a new texture atlas from the given raw texture atlas record.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="rawTextureAtlas">The raw texture atlas record to create the texture atlas from.</param>
    /// <returns>The texture atlas created by this method.</returns>
    public static TextureAtlas FromRaw(GraphicsDevice device, RawTextureAtlas rawTextureAtlas)
    {
        RawTexture rawTexture = rawTextureAtlas.RawTexture;

        Texture2D texture = new(device, rawTexture.Width, rawTexture.Height, mipmap: false, SurfaceFormat.Color);
        texture.SetData<Color>(rawTexture.Pixels.ToArray());
        texture.Name = rawTexture.Name;

        TextureAtlas atlas = new(rawTextureAtlas.Name, texture);

        ReadOnlySpan<RawTextureRegion> rawTextureRegions = rawTextureAtlas.RawTextureRegions;

        for (int i = 0; i < rawTextureRegions.Length; i++)
        {
            atlas.CreateRegion(rawTextureRegions[i].Name, rawTextureRegions[i].Bounds);
        }

        return atlas;
    }
}
