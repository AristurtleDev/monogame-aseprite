/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite;

public sealed class SpriteSheet : IEnumerable<SpriteSheetRegion>
{
    Dictionary<string, SpriteSheetRegion> _regionLookup = new();
    List<SpriteSheetRegion> _regionMap = new();
    Dictionary<string, SpriteSheetAnimationDefinition> _animationDefinitionLookup = new();

    /// <summary>
    ///     Gets the <see cref="SpriteSheetRegion"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetRegion"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="SpriteSheetRegion"/> element
    ///     in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetRegion this[string name] => GetRegion(name);

    /// <summary>
    ///     Gets the <see cref="SpriteSheetRegion"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetRegion"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to <see cref="RegionCount"/>.
    /// </exception>
    public SpriteSheetRegion this[int index] => GetRegion(index);

    /// <summary>
    ///     Gets the name of this <see cref="SpriteSheet"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="Texture2D"/> represented by this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the total number of <see cref="SpriteSheetRegion"/> elements
    ///     in this <see cref="SpriteSheet"/>.
    /// </summary>
    public int RegionCount => _regionMap.Count;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheet"/> class
    ///     with the specified <paramref name="name"/> using the specified
    ///     <paramref name="texture"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give this <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Texture2D"/> that this <see cref="SpriteSheet"/>
    ///     represents.
    /// </param>
    public SpriteSheet(string name, Texture2D texture) => (Name, Texture) = (name, texture);

    /// <summary>
    ///     Returns a value that indicates whether this
    ///     <see cref="SpriteSheet"/> contains a <see cref="SpriteSheetRegion"/>
    ///     element with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetRegion"/> to locate in this
    ///    <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains
    ///     a <see cref="SpriteSheetRegion"/> element with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsRegion(string name) => _regionLookup.ContainsKey(name);

    /// <summary>
    ///     Gets the <see cref="SpriteSheetRegion"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetRegion"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to <see cref="RegionCount"/>.
    /// </exception>
    public SpriteSheetRegion GetRegion(int index)
    {
        if (index < 0 || index >= _regionMap.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} cannot be less than zero or greater than or equal to the total number of {nameof(SpriteSheetRegion)} elements in this {nameof(SpriteSheet)}");
        }

        return _regionMap[index];
    }

    /// <summary>
    ///     Gets the <see cref="SpriteSheetRegion"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetRegion"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="SpriteSheetRegion"/> element
    ///     in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetRegion GetRegion(string name)
    {
        if (_regionLookup.TryGetValue(name, out SpriteSheetRegion? region))
        {
            return region;
        }

        throw new KeyNotFoundException($"No SpriteSheetRegion with the name '{name}' was found in the SpriteSheet '{Name}'");
    }

    /// <summary>
    ///     Gets the <see cref="SpriteSheetRegion"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     THe name of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="region">
    ///     When this method returns, contains the
    ///     <see cref="SpriteSheetRegion"/> element that had the specified
    ///     <paramref name="name"/>, if one was found; otherwise,
    ///     <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="SpriteSheetRegion"/> element
    ///     was found in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetRegion(string name, out SpriteSheetRegion? region) =>
        _regionLookup.TryGetValue(name, out region);

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetRegion"/> element with the specified
    ///     <paramref name="regionName"/> from this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="SpriteSheetRegion"/> element to use when
    ///     creating the <see cref="Sprite"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="Sprite"/> class initialized with
    ///     the <see cref="SpriteSheetRegion"/> element with the specified
    ///     <paramref name="regionName"/> from this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="SpriteSheetRegion"/> element
    ///     in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public Sprite CreateSprite(string regionName)
    {
        SpriteSheetRegion region = GetRegion(regionName);
        return new Sprite(region);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetRegion"/> element at the specified
    ///     <paramref name="regionIndex"/> in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the <see cref="SpriteSheetRegion"/> element to use
    ///     when creating the <see cref="Sprite"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="Sprite"/> class initialized with
    ///     the <see cref="SpriteSheetRegion"/> element at the specified
    ///     <paramref name="regionIndex"/> in this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to <see cref="RegionCount"/>.
    /// </exception>
    public Sprite CreateSprite(int regionIndex)
    {
        SpriteSheetRegion region = GetRegion(regionIndex);
        return new Sprite(region);
    }

    private void AddRegion(SpriteSheetRegion region)
    {
        _regionLookup.Add(region.Name, region);
        _regionMap.Add(region);
    }

    /// <summary>
    ///     Creates a new <see cref="SpriteSheetRegion"/> and adds it to the
    ///     next index of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="x">
    ///     The x-coordinate location of the upper-left corner of the
    ///     <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="y">
    ///     The y-coordinate location of the upper-left corner of the
    ///     <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="width">
    ///     The width, in pixels, of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="height">
    ///     The height, in pixels, of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetRegion"/> class that is
    ///     created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetRegion"/> element has already
    ///     been created for this <see cref="SpriteSheet"/> with the
    ///     specified <paramref name="name"/>.
    /// </exception>
    public SpriteSheetRegion CreateRegion(string name, int x, int y, int width, int height)
    {
        if (_regionLookup.ContainsKey(name))
        {
            throw new InvalidOperationException($"A {nameof(SpriteSheetRegion)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
        }

        SpriteSheetRegion region = new(name, Texture, x, y, width, height);
        AddRegion(region);
        return region;
    }

    /// <summary>
    ///     Creates a new <see cref="SpriteSheetRegion"/> and adds it to the
    ///     next index of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <param name="bounds">
    ///     The bounds of the <see cref="SpriteSheetRegion"/>.
    /// </param>
    /// <returns></returns>
    public SpriteSheetRegion CreateRegion(string name, Rectangle bounds) =>
        CreateRegion(name, bounds.X, bounds.Y, bounds.Width, bounds.Height);

    /// <summary>
    ///     Adds the given <see cref="SpriteSheetAnimationDefinition"/> to this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="definition">
    ///     The <see cref="SpriteSheetAnimationDefinition"/> to add.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="SpriteSheet"/> already contains a
    ///     <see cref="SpriteSheetAnimationDefinition"/> element with the
    ///     same <see cref="SpriteSheetAnimationDefinition.Name"/> as the
    ///     one given.
    /// </exception>
    public void AddAnimationDefinition(SpriteSheetAnimationDefinition definition)
    {
        if (_animationDefinitionLookup.ContainsKey(definition.Name))
        {
            throw new InvalidOperationException($"A {nameof(SpriteSheetAnimationDefinition)} with the name '{definition.Name}' has already been added to this {nameof(SpriteSheet)}.");
        }

        _animationDefinitionLookup.Add(definition.Name, definition);
    }


    public void CreateAnimatedSprite(string definitionName)
    {
        if (_animationDefinitionLookup.TryGetValue(definitionName, out SpriteSheetAnimationDefinition? definition))
        {

        }

        throw new KeyNotFoundException($"No {nameof(SpriteSheetAnimationDefinition)} with the name '{definitionName}' was found in the SpriteSheet '{Name}'");
    }


    /// <summary>
    ///     Returns an enumerator that iterates through the
    ///     <see cref="SpriteSheetRegion"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <returns>
    ///     A <see cref="List{T}.Enumerator"/> that iterates through the
    ///     <see cref="SpriteSheetRegion"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    public IEnumerator<SpriteSheetRegion> GetEnumerator() => _regionMap.GetEnumerator();


    /// <summary>
    ///     Returns an enumerator that iterates through the
    ///     <see cref="SpriteSheetRegion"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <returns>
    ///     A <see cref="List{T}.Enumerator"/> that iterates through the
    ///     <see cref="SpriteSheetRegion"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
