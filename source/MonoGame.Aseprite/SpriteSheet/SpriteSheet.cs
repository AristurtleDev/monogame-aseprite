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
///     A spritesheet that contains a texture2d image, named texture regions,
///     and animation cycles.
/// </summary>
public class SpriteSheet
{
    private static readonly TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    private List<TextureRegion> _regions = new();
    private Dictionary<string, TextureRegion> _regionLookup = new();
    private Dictionary<string, AnimationCycle> _animationCycleLookup = new();

    /// <summary>
    ///     Gets the name of this spritesheet.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the texture2D represented by this spritesheet.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the total number of texture regions in this spritesheet.
    /// </summary>
    public int RegionCount => _regions.Count;

    /// <summary>
    ///     Gets the total number of animation cycles in this spritesheet.
    /// </summary>
    public int AnimationCycleCount => _animationCycleLookup.Count;

    /// <summary>
    ///     Gets the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <param name="index">
    ///     The index of the texture region in this spritesheet to return.
    /// </param>
    /// <returns>
    ///     The texture region at the specified index from this spritesheet.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Throw if the specified index is less than zero or is greater than
    ///     or equal to the total number of texture regions in this
    ///     spritesheet.
    /// </exception>
    public TextureRegion this[int index] => GetRegion(index);

    /// <summary>
    ///     Gets the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region in this spritesheet to locate.
    /// </param>
    /// <returns>
    ///     The texture region in this spritesheet with the specified name.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this spritesheet does not have a texture region with
    ///     the specified name.
    /// </exception>
    public TextureRegion this[string name] => GetRegion(name);

    /// <summary>
    ///     Creates a new spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name to give the spritesheet.
    /// </param>
    /// <param name="texture">
    ///     The texture2D that will be represented by the spritesheet.
    /// </param>
    public SpriteSheet(string name, Texture2D texture)
    {
        Name = name;
        Texture = texture;
    }

    #region Regions

    private void AddRegion(TextureRegion region)
    {
        if (_regionLookup.ContainsKey(region.Name))
        {
            throw new InvalidOperationException();
        }

        _regions.Add(region);
        _regionLookup.Add(region.Name, region);
    }

    private void RemoveRegion(TextureRegion region)
    {
        _regions.Remove(region);
        _regionLookup.Remove(region.Name);
    }

    /// <summary>
    ///     Creates a new texture region and adds it to this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name to give the texture region. Must be unique for this
    ///     spritesheet.
    /// </param>
    /// <param name="x">
    ///     The x-coordinate location of the upper-left corner of the texture
    ///     region that is created by this method.
    /// </param>
    /// <param name="y">
    ///     The y-coordinate location of the upper-left corner of the texture
    ///     region that is created by this method.
    /// </param>
    /// <param name="width">
    ///     The width, in pixels, of the texture region that is created by this
    ///     method.
    /// </param>
    /// <param name="height">
    ///     The height, in pixel, of the texture region that is created by this
    ///     method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this spritesheet already contains a texture region with
    ///     the name given.
    /// </exception>
    /// <returns>
    ///     The texture region that is created by this method.
    /// </returns>
    public TextureRegion CreateRegion(string name, int x, int y, int width, int height) =>
        CreateRegion(name, new Rectangle(x, y, width, height));

    /// <summary>
    ///     Creates a new texture region and adds it to this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name to give the texture region. Must be unique for this
    ///     spritesheet.
    /// </param>
    /// <param name="bounds">
    ///     The rectangular bounds of the texture region that is created by this
    ///     method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this spritesheet already contains a texture region with
    ///     the name given.
    /// </exception>
    /// <returns>
    ///     The texture region that is created by this method.
    /// </returns>
    public TextureRegion CreateRegion(string name, Rectangle bounds)
    {
        TextureRegion region = new(name, Texture, bounds);
        AddRegion(region);
        return region;
    }

    /// <summary>
    ///     Returns a value that indicates whether this spritesheet contains
    ///     a texture region with the specified name.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region to locate in this spritesheet.
    /// </param>
    /// <returns>
    ///     true if this spritesheet contains a texture region with the
    ///     specified name; otherwise, false.
    /// </returns>
    public bool ContainsRegion(string name) => _regionLookup.ContainsKey(name);

    /// <summary>
    ///     Gets the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <param name="index">
    ///     The index of the texture region in this spritesheet to locate.
    /// </param>
    /// <returns>
    ///     The texture region at the specified index from this spritesheet.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Throw if the specified index is less than zero or is greater than
    ///     or equal to the total number of texture regions in this
    ///     spritesheet.
    /// </exception>
    public TextureRegion GetRegion(int index)
    {
        if (index < 0 || index >= _regions.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _regions[index];
    }

    /// <summary>
    ///     Gets the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region in this spritesheet to locate.
    /// </param>
    /// <returns>
    ///     The texture region in this spritesheet with the specified name.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this spritesheet does not have a texture region with
    ///     the specified name.
    /// </exception>
    public TextureRegion GetRegion(string name)
    {
        if (_regionLookup.TryGetValue(name, out TextureRegion? frame))
        {
            return frame;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    ///     Gets a collection of the texture regions at the specified indexes
    ///     in this spritesheet.
    /// </summary>
    /// <param name="indexes">
    ///     The indexes of the texture regions in this spritesheet to locate.
    /// </param>
    /// <returns>
    ///     A new collection containing the texture regions at the specified
    ///     indexes in this spritesheet.  The order of the elements in the
    ///     collection is the same order as the indexes provided.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if any of the indexes specified is less than zero or is
    ///     greater than or equal to the total number of texture regions in
    ///     this spritesheet.
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
    ///     Gest a collection of the texture regions with the specified names
    ///     in this spritesheet.
    /// </summary>
    /// <param name="names">
    ///     The names of the texture regions in this spritesheet to locate.
    /// </param>
    /// <returns>
    ///     A new collection containing the texture regions with the specified
    ///     names in this spritesheet.  The order of the elements in the
    ///     collection is the same order as the indexes provided.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if a texture region cannot be located in this spritesheet
    ///     for any of the names provided.
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
    ///     Gets the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <param name="index">
    ///     The index of the texture region in this spritesheet to locate.
    /// </param>
    /// <param name="region">
    ///     When this method returns, contains the texture region located, if
    ///     the specified index is valid; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if a texture region is located at the index specified in this
    ///     spritesheet; otherwise, false.
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
    ///     Gets the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region in this spritesheet to locate.
    /// </param>
    /// <param name="region">
    ///     When this method returns, contains the texture region located, if
    ///     this spritesheet contains a texture region with the specified name;
    ///     otherwise, null.
    /// </param>
    /// <returns>
    ///     true if a texture region is located with the specified name in this
    ///     spritesheet; otherwise, false.
    /// </returns>
    public bool TryGetRegion(string name, [NotNullWhen(true)] out TextureRegion? region) =>
        _regionLookup.TryGetValue(name, out region);

    /// <summary>
    ///     Removes the texture region at the specified index from this
    ///     spritesheet.
    /// </summary>
    /// <param name="index">
    ///     The index of the texture region in this spritesheet to remove.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Throw if the specified index is less than zero or is greater than
    ///     or equal to the total number of texture regions in this
    ///     spritesheet.
    /// </exception>
    public void RemoveRegion(int index) => RemoveRegion(GetRegion(index));

    /// <summary>
    ///     Removes the texture region with the specified name from this
    ///     spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region in this spritesheet to remove.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if any of the indexes specified is less than zero or is
    ///     greater than or equal to the total number of texture regions in
    ///     this spritesheet.
    /// </exception>
    public void RemoveRegion(string name) => RemoveRegion(GetRegion(name));

    /// <summary>
    ///     Removes the texture region at the specified index from this
    ///     spritesheet.
    /// </summary>
    /// <param name="index">
    ///     The index of the texture region in this spritesheet to remove.
    /// </param>
    /// <returns>
    ///     true if the texture region was removed successfully; otherwise,
    ///     false.  This will be false if the index provided is less than
    ///     zero or is greater than or equal to the total number of texture
    ///     regions in this spritesheet.
    /// </returns>
    public bool TryRemoveRegion(int index)
    {
        if (TryGetRegion(index, out TextureRegion? region))
        {
            RemoveRegion(region);
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Removes the texture region with the specified name from this
    ///     spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region in this spritesheet to remove.
    /// </param>
    /// <returns>
    ///     true if the texture region was removed successfully; otherwise,
    ///     false.  This will be false if this spritesheet does not contain
    ///     a texture region with the specified name.
    /// </returns>
    public bool TryRemoveRegion(string name)
    {
        if (TryGetRegion(name, out TextureRegion? region))
        {
            RemoveRegion(region);
            return true;
        }

        return false;
    }

    #endregion Regions

    #region Sprite

    // /// <summary>
    // ///     Creates a new instance of the <see cref="Sprite"/> class using the
    // ///     <see cref="TextureRegion"/> element at the specified
    // ///     <paramref name="regionIndex"/> from this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </summary>
    // /// <param name="regionIndex">
    // ///     The index of the <see cref="TextureRegion"/> element in this
    // ///     instance <see cref="SpriteSheet"/> class to use when creating the
    // ///     instance of the <see cref="Sprite"/> class.
    // /// </param>
    // /// <returns>
    // ///     The instance of the <see cref="Sprite"/> class created by this
    // ///     method.
    // /// </returns>
    // /// <exception cref="ArgumentOutOfRangeException">
    // ///     Thrown if the specified <paramref name="regionIndex"/> is less than
    // ///     zero or is greater than or equal to the total number of
    // ///     <see cref="TextureRegion"/> elements in this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </exception>
    // public Sprite CreateSprite(int regionIndex) =>
    //     CreateSprite(regionIndex, Vector2.Zero, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    // /// <summary>
    // ///     Creates a new instance of the <see cref="Sprite"/> class using the
    // ///     <see cref="TextureRegion"/> element at the specified
    // ///     <paramref name="regionIndex"/> from this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </summary>
    // /// <param name="regionIndex">
    // ///     The index of the <see cref="TextureRegion"/> element in this
    // ///     instance <see cref="SpriteSheet"/> class to use when creating the
    // ///     instance of the <see cref="Sprite"/> class.
    // /// </param>
    // /// <param name="position">
    // ///     The x- and y-coordinate position to use when rendering the instance
    // ///     of the <see cref="Sprite"/> class created by this method.
    // /// </param>
    // /// <param name="color">
    // ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    // ///     color mask when rendering the instance <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <returns>
    // ///     The instance of the <see cref="Sprite"/> class created by this
    // ///     method.
    // /// </returns>
    // /// <exception cref="ArgumentOutOfRangeException">
    // ///     Thrown if the specified <paramref name="regionIndex"/> is less than
    // ///     zero or is greater than or equal to the total number of
    // ///     <see cref="TextureRegion"/> elements in this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </exception>
    // public Sprite CreateSprite(int regionIndex, Vector2 position, Color color) =>
    //     CreateSprite(regionIndex, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    // /// <summary>
    // ///     Creates a new instance of the <see cref="Sprite"/> class using the
    // ///     <see cref="TextureRegion"/> element at the specified
    // ///     <paramref name="regionIndex"/> from this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </summary>
    // /// <param name="regionIndex">
    // ///     The index of the <see cref="TextureRegion"/> element in this
    // ///     instance <see cref="SpriteSheet"/> class to use when creating the
    // ///     instance of the <see cref="Sprite"/> class.
    // /// </param>
    // /// <param name="position">
    // ///     The x- and y-coordinate position to use when rendering the instance
    // ///     of the <see cref="Sprite"/> class created by this method.
    // /// </param>
    // /// <param name="color">
    // ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    // ///     color mask when rendering the instance <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="rotation">
    // ///     The amount of rotation, in radians, to apply when rendering the
    // ///     instance of the <see cref="Sprite"/> class created by this method.
    // /// </param>
    // /// <param name="origin">
    // ///     The x- and y-coordinate point to use as the point of origin when
    // ///     rendering the instance of the <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="scale">
    // ///     The amount of x-axis (horizontal) and y-axis (vertical) scale to
    // ///     apply when rendering the instance of the <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="effects">
    // ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    // ///     apply when rendering the instance of the <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="layerDepth">
    // ///     The layer depth to apply when rendering the instance of the
    // ///     <see cref="Sprite"/>  class created by this method.
    // /// </param>
    // /// <returns>
    // ///     The instance of the <see cref="Sprite"/> class created by this
    // ///     method.
    // /// </returns>
    // /// <exception cref="ArgumentOutOfRangeException">
    // ///     Thrown if the specified <paramref name="regionIndex"/> is less than
    // ///     zero or is greater than or equal to the total number of
    // ///     <see cref="TextureRegion"/> elements in this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </exception>
    // public Sprite CreateSprite(int regionIndex, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    // {
    //     TextureRegion region = GetRegion(regionIndex);
    //     Sprite sprite = new(region);

    //     sprite.Position = position;
    //     sprite.Color = color;
    //     sprite.Rotation = rotation;
    //     sprite.Origin = origin;
    //     sprite.Scale = scale;
    //     sprite.SpriteEffects = effects;
    //     sprite.LayerDepth = layerDepth;

    //     return sprite;
    // }

    // /// <summary>
    // ///     Creates a new instance of the <see cref="Sprite"/> class using the
    // ///     <see cref="TextureRegion"/> element with the specified
    // ///     <paramref name="regionName"/> in this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </summary>
    // /// <param name="regionName">
    // ///     The name of the <see cref="TextureRegion"/> element in this instance
    // ///     of the <see cref="SpriteSheet"/> class to use when creating the
    // ///     <see cref="Sprite"/>.
    // /// </param>
    // /// <returns>
    // ///     The instance of the <see cref="Sprite"/> class created by this
    // ///     method.
    // /// </returns>
    // /// <exception cref="KeyNotFoundException">
    // ///     Thrown if this instance of the <see cref="SpriteSheet"/> class does
    // ///     not contain a <see cref="TextureRegion"/> element with the specified
    // ///     <paramref name="regionName"/>.
    // /// </exception>
    // public Sprite CreateSprite(string regionName) =>
    //     CreateSprite(regionName, Vector2.Zero, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    // /// <summary>
    // ///     Creates a new instance of the <see cref="Sprite"/> class using the
    // ///     <see cref="TextureRegion"/> element with the specified
    // ///     <paramref name="regionName"/> in this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </summary>
    // /// <param name="regionName">
    // ///     The name of the <see cref="TextureRegion"/> element in this instance
    // ///     of the <see cref="SpriteSheet"/> class to use when creating the
    // ///     <see cref="Sprite"/>.
    // /// </param>
    // /// <param name="position">
    // ///     The x- and y-coordinate position to use when rendering the instance
    // ///     of the <see cref="Sprite"/> class created by this method.
    // /// </param>
    // /// <param name="color">
    // ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    // ///     color mask when rendering the instance <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <returns>
    // ///     The instance of the <see cref="Sprite"/> class created by this
    // ///     method.
    // /// </returns>
    // /// <exception cref="KeyNotFoundException">
    // ///     Thrown if this instance of the <see cref="SpriteSheet"/> class does
    // ///     not contain a <see cref="TextureRegion"/> element with the specified
    // ///     <paramref name="regionName"/>.
    // /// </exception>
    // public Sprite CreateSprite(string regionName, Vector2 position, Color color) =>
    //     CreateSprite(regionName, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    // /// <summary>
    // ///     Creates a new instance of the <see cref="Sprite"/> class using the
    // ///     <see cref="TextureRegion"/> element with the specified
    // ///     <paramref name="regionName"/> in this instance of the
    // ///     <see cref="SpriteSheet"/> class.
    // /// </summary>
    // /// <param name="regionName">
    // ///     The name of the <see cref="TextureRegion"/> element in this instance
    // ///     of the <see cref="SpriteSheet"/> class to use when creating the
    // ///     <see cref="Sprite"/>.
    // /// </param>
    // /// <param name="position">
    // ///     The x- and y-coordinate position to use when rendering the instance
    // ///     of the <see cref="Sprite"/> class created by this method.
    // /// </param>
    // /// <param name="color">
    // ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    // ///     color mask when rendering the instance <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="rotation">
    // ///     The amount of rotation, in radians, to apply when rendering the
    // ///     instance of the <see cref="Sprite"/> class created by this method.
    // /// </param>
    // /// <param name="origin">
    // ///     The x- and y-coordinate point to use as the point of origin when
    // ///     rendering the instance of the <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="scale">
    // ///     The amount of x-axis (horizontal) and y-axis (vertical) scale to
    // ///     apply when rendering the instance of the <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="effects">
    // ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    // ///     apply when rendering the instance of the <see cref="Sprite"/> class
    // ///     created by this method.
    // /// </param>
    // /// <param name="layerDepth">
    // ///     The layer depth to apply when rendering the instance of the
    // ///     <see cref="Sprite"/>  class created by this method.
    // /// </param>
    // /// <returns>
    // ///     The instance of the <see cref="Sprite"/> class created by this
    // ///     method.
    // /// </returns>
    // /// <exception cref="KeyNotFoundException">
    // ///     Thrown if this instance of the <see cref="SpriteSheet"/> class does
    // ///     not contain a <see cref="TextureRegion"/> element with the specified
    // ///     <paramref name="regionName"/>.
    // /// </exception>
    // public Sprite CreateSprite(string regionName, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    // {
    //     TextureRegion frame = GetRegion(regionName);
    //     Sprite sprite = new(frame);

    //     sprite.Position = position;
    //     sprite.Color = color;
    //     sprite.Rotation = rotation;
    //     sprite.Origin = origin;
    //     sprite.Scale = scale;
    //     sprite.SpriteEffects = effects;
    //     sprite.LayerDepth = layerDepth;

    //     return sprite;
    // }

    #endregion Sprite

    #region Animations

    internal void AddAnimationCycle(AnimationCycle cycle)
    {
        if (_animationCycleLookup.ContainsKey(cycle.Name))
        {
            throw new InvalidOperationException();
        }

        _animationCycleLookup.Add(cycle.Name, cycle);
    }


    /// <summary>
    ///     Creates a new animation cycle and adds it to this spritesheet.
    /// </summary>
    /// <param name="cycleName">
    ///     The name to give the animation cycle that is created by this
    ///     method.
    /// </param>
    /// <param name="buildAction">
    ///     An action used to build the animation cycle using an animation cycle
    ///     builder.
    /// </param>
    /// <returns>
    ///     The animation cycle that is created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this spritesheet already contains an animation cycle with
    ///     the specified name.
    /// </exception>
    public AnimationCycle CreateAnimationCycle(string cycleName, Action<AnimationCycleBuilder> buildAction)
    {
        if (_animationCycleLookup.ContainsKey(cycleName))
        {
            throw new InvalidOperationException();
        }

        AnimationCycleBuilder builder = new(cycleName, this);
        buildAction(builder);

        AnimationCycle cycle = builder.Build();
        AddAnimationCycle(cycle);

        return cycle;
    }

    /// <summary>
    ///     Removes the animation cycle with the specified name from this
    ///     spritesheet.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the animation cycle to remove from this spritesheet.
    /// </param>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this spritesheet does not contain an animation cycle with
    ///     the specified name.
    /// </exception>
    public void RemoveAnimationCycle(string cycleName)
    {
        if (!TryRemoveAnimationCycle(cycleName))
        {
            throw new KeyNotFoundException();
        }
    }

    /// <summary>
    ///     Removes the animation cycle with the specified name from this
    ///     spritesheet.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the animation cycle in this spritesheet to removed.
    /// </param>
    /// <returns>
    ///     true if the animation cycle was successfully removed; otherwise,
    ///     false.  This method returns false if this spritesheet does not
    ///     have an animation cycle with the specified name.
    /// </returns>
    public bool TryRemoveAnimationCycle(string cycleName) => _animationCycleLookup.Remove(cycleName);

    /// <summary>
    ///     Returns a value that indicates whether this spritesheet contains
    ///     an animation cycle with the specified name.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the animation cycle to locate in this spritesheet.
    /// </param>
    /// <returns>
    ///     true if this spritesheet contains an animation cycle with the
    ///     specified name; otherwise, false.
    /// </returns>
    public bool ContainsAnimationCycle(string cycleName) => _animationCycleLookup.ContainsKey(cycleName);

    /// <summary>
    ///     Gets the animation cycle with the specified name from this
    ///     spritesheet.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the animation cycle to locate in this spritesheet.
    /// </param>
    /// <returns>
    ///     The animation cycle in this spritesheet with the specified name.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this spritesheet does not contain an animation cycle
    ///     with the specified name.
    /// </exception>
    public AnimationCycle GetAnimationCycle(string cycleName)
    {
        if (TryGetAnimationCycle(cycleName, out AnimationCycle? animation))
        {
            return animation;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    ///     Gets the animation cycle with the specified name from this
    ///     spritesheet.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the animation cycle to locate in this spritesheet.
    /// </param>
    /// <param name="animation">
    ///     When this method returns, contains the animation cycle located, if
    ///     this spritesheet contains an animation cycle with the specified
    ///     name; otherwise, null.
    /// </param>
    /// <returns>
    ///     true if an animation cycle is located with the specified name in
    ///     this spritesheet; otherwise, false.
    /// </returns>
    public bool TryGetAnimationCycle(string cycleName, [NotNullWhen(true)] out AnimationCycle? animation) =>
        _animationCycleLookup.TryGetValue(cycleName, out animation);

    /// <summary>
    ///     Creates and returns a new animation based on the animation cycle
    ///     with the specified name from this spritesheet.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the animation cycle in this spritesheet to create
    ///     the animation with.
    /// </param>
    /// <returns>
    ///     The animation that is created by this method.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this spritesheet does not contain an animation cycle
    ///     with the specified name.
    /// </exception>
    public Animation CreateAnimation(string cycleName)
    {
        AnimationCycle cycle = GetAnimationCycle(cycleName);
        Animation animation = new(cycle);
        return animation;
    }

    #endregion Animations
}
