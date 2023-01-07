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

public sealed class SpriteSheet : IEnumerable<SpriteSheetFrame>
{
    private static TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    Dictionary<string, SpriteSheetFrame> _regionLookup = new();
    List<SpriteSheetFrame> _regions = new();
    Dictionary<string, int> _regionMap = new();

    private Dictionary<string, SpriteSheetAnimationDefinition> _animationLookup = new();

    /// <summary>
    ///     Gets the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="SpriteSheetFrame"/> element
    ///     in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetFrame this[string name] => GetRegion(name);

    /// <summary>
    ///     Gets the <see cref="SpriteSheetFrame"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetFrame"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to <see cref="RegionCount"/>.
    /// </exception>
    public SpriteSheetFrame this[int index] => GetRegion(index);

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
    ///     Gets the total number of <see cref="SpriteSheetFrame"/> elements
    ///     in this <see cref="SpriteSheet"/>.
    /// </summary>
    public int RegionCount => _regions.Count;

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
    ///     <see cref="SpriteSheet"/> contains a <see cref="SpriteSheetFrame"/>
    ///     element with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetFrame"/> to locate in this
    ///    <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains
    ///     a <see cref="SpriteSheetFrame"/> element with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsRegion(string name) => _regionLookup.ContainsKey(name);

    /// <summary>
    ///     Gets the <see cref="SpriteSheetFrame"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetFrame"/> element at the specified
    ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to <see cref="RegionCount"/>.
    /// </exception>
    public SpriteSheetFrame GetRegion(int index)
    {
        if (index < 0 || index >= _regions.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} cannot be less than zero or greater than or equal to the total number of {nameof(SpriteSheetFrame)} elements in this {nameof(SpriteSheet)}");
        }

        return _regions[index];
    }

    /// <summary>
    ///     Gets the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="SpriteSheetFrame"/> element
    ///     in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetFrame GetRegion(string name)
    {
        if (_regionLookup.TryGetValue(name, out SpriteSheetFrame? region))
        {
            return region;
        }

        throw new KeyNotFoundException($"No SpriteSheetRegion with the name '{name}' was found in the SpriteSheet '{Name}'");
    }

    /// <summary>
    ///     Gets the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     THe name of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="region">
    ///     When this method returns, contains the
    ///     <see cref="SpriteSheetFrame"/> element that had the specified
    ///     <paramref name="name"/>, if one was found; otherwise,
    ///     <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="SpriteSheetFrame"/> element
    ///     was found in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetRegion(string name, out SpriteSheetFrame? region) =>
        _regionLookup.TryGetValue(name, out region);

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element with the specified
    ///     <paramref name="regionName"/> from this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="SpriteSheetFrame"/> element to use when
    ///     creating the <see cref="Sprite"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="Sprite"/> class initialized with
    ///     the <see cref="SpriteSheetFrame"/> element with the specified
    ///     <paramref name="regionName"/> from this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if there is no <see cref="SpriteSheetFrame"/> element
    ///     in this <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public Sprite CreateSprite(string regionName)
    {
        SpriteSheetFrame region = GetRegion(regionName);
        return new Sprite(region);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element at the specified
    ///     <paramref name="regionIndex"/> in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the <see cref="SpriteSheetFrame"/> element to use
    ///     when creating the <see cref="Sprite"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="Sprite"/> class initialized with
    ///     the <see cref="SpriteSheetFrame"/> element at the specified
    ///     <paramref name="regionIndex"/> in this <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero
    ///     or is greater than or equal to <see cref="RegionCount"/>.
    /// </exception>
    public Sprite CreateSprite(int regionIndex)
    {
        SpriteSheetFrame region = GetRegion(regionIndex);
        return new Sprite(region);
    }

    private void AddFrame(SpriteSheetFrame region)
    {
        _regions.Add(region);
        _regionLookup.Add(region.Name, region);
        _regionMap.Add(region.Name, _regions.Count - 1);
    }

    /// <summary>
    ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
    ///     next index of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="bounds">
    ///     The bounds of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
    ///     created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
    ///     been created for this <see cref="SpriteSheet"/> with the
    ///     specified <paramref name="name"/>.
    /// </exception>
    public SpriteSheetFrame CreateFrame(string name, Rectangle bounds) =>
        CreateFrame(name, bounds.X, bounds.Y, bounds.Width, bounds.Height, s_defaultDuration);

    /// <summary>
    ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
    ///     next index of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="bounds">
    ///     The bounds of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="duration">
    ///     The duration of the <see cref="SpriteSheetFrame"/> when used in
    ///     an animation.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
    ///     created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
    ///     been created for this <see cref="SpriteSheet"/> with the
    ///     specified <paramref name="name"/>.
    /// </exception>
    public SpriteSheetFrame CreateFrame(string name, Rectangle bounds, TimeSpan duration) =>
        CreateFrame(name, bounds.X, bounds.Y, bounds.Width, bounds.Height, duration);

    /// <summary>
    ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
    ///     next index of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="x">
    ///     The x-coordinate location of the upper-left corner of the
    ///     <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="y">
    ///     The y-coordinate location of the upper-left corner of the
    ///     <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="width">
    ///     The width, in pixels, of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="height">
    ///     The height, in pixels, of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
    ///     created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
    ///     been created for this <see cref="SpriteSheet"/> with the
    ///     specified <paramref name="name"/>.
    /// </exception>
    public SpriteSheetFrame CreateFrame(string name, int x, int y, int width, int height) =>
        CreateFrame(name, x, y, width, height, s_defaultDuration);


    /// <summary>
    ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
    ///     next index of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="x">
    ///     The x-coordinate location of the upper-left corner of the
    ///     <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="y">
    ///     The y-coordinate location of the upper-left corner of the
    ///     <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="width">
    ///     The width, in pixels, of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="height">
    ///     The height, in pixels, of the <see cref="SpriteSheetFrame"/>.
    /// </param>
    /// <param name="duration">
    ///     The duration of the <see cref="SpriteSheetFrame"/> when used in
    ///     an animation.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
    ///     created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
    ///     been created for this <see cref="SpriteSheet"/> with the
    ///     specified <paramref name="name"/>.
    /// </exception>
    public SpriteSheetFrame CreateFrame(string name, int x, int y, int width, int height, TimeSpan duration)
    {
        if (_regionLookup.ContainsKey(name))
        {
            throw new InvalidOperationException($"A {nameof(SpriteSheetFrame)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
        }

        SpriteSheetFrame frame = new(name, Texture, new Rectangle(x, y, width, height), duration);

        AddFrame(frame);
        return frame;
    }



    public SpriteSheetAnimationDefinition GetAnimationDefinition(string name)
    {
        if (_animationLookup.TryGetValue(name, out SpriteSheetAnimationDefinition? definition))
        {
            return definition;
        }

        throw new KeyNotFoundException($"No {nameof(SpriteSheetAnimationDefinition)} with the name '{name}' exists in this {nameof(SpriteSheet)}.");
    }

    public void AddAnimationDefinition(string name, params int[] frames) => AddAnimationDefinition(name, true, false, false, frames);

    public void AddAnimationDefinition(string name, bool isLooping, bool isReversed, bool isPingPong, params int[] frames)
    {
        if (_animationLookup.ContainsKey(name))
        {
            throw new InvalidOperationException($"A {nameof(SpriteSheetAnimationDefinition)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
        }

        SpriteSheetAnimationDefinition definition = new(name, isLooping, isReversed, isPingPong, frames);
        _animationLookup.Add(name, definition);
    }

    public void AddAnimationDefinition(string name, params string[] frames) => AddAnimationDefinition(name, true, false, false, frames);

    public void AddAnimationDefinition(string name, bool isLooping, bool isReversed, bool isPingPong, params string[] frames)
    {
        if (_animationLookup.ContainsKey(name))
        {
            throw new InvalidOperationException($"A {nameof(SpriteSheetAnimationDefinition)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
        }

        int[] frameIndexes = new int[frames.Length];

        for (int i = 0; i < frames.Length; i++)
        {
            if (_regionMap.TryGetValue(frames[i], out int index))
            {
                frameIndexes[i] = index;
            }
            else
            {
                throw new KeyNotFoundException($"No {nameof(SpriteSheetFrame)} with the name '{frames[i]}' exists in this {nameof(SpriteSheet)}.");
            }
        }

        SpriteSheetAnimationDefinition definition = new(name, isLooping, isReversed, isPingPong, frameIndexes);
        _animationLookup.Add(name, definition);
    }


    /// <summary>
    ///     Returns an enumerator that iterates through the
    ///     <see cref="SpriteSheetFrame"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <returns>
    ///     A <see cref="List{T}.Enumerator"/> that iterates through the
    ///     <see cref="SpriteSheetFrame"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    public IEnumerator<SpriteSheetFrame> GetEnumerator() => _regions.GetEnumerator();


    /// <summary>
    ///     Returns an enumerator that iterates through the
    ///     <see cref="SpriteSheetFrame"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <returns>
    ///     A <see cref="List{T}.Enumerator"/> that iterates through the
    ///     <see cref="SpriteSheetFrame"/> elements in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
