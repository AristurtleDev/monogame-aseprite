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
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a spritesheet that represents an image with named texture regions and animation cycles.
/// </summary>
public class SpriteSheet
{
    private static readonly TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    private List<TextureRegion> _regions = new();
    private Dictionary<string, TextureRegion> _regionLookup = new();
    private Dictionary<string, AnimationCycle> _animationCycleLookup = new();

    /// <summary>
    ///     Gets the name of this <see cref="SpriteSheet"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> image represented by this
    ///     <see cref="SpriteSheet"/>
    /// </summary>
    public Texture2D Texture { get; }

    /// <summary>
    ///     Gets the total number of <see cref="TextureRegion"/> elements in this <see cref="SpriteSheet"/>.
    /// </summary>
    public int RegionCount => _regions.Count;

    /// <summary>
    ///     Gets the total number of <see cref="AnimationCycle"/> elements in this <see cref="SpriteSheet"/>.
    /// </summary>
    public int AnimationCycleCount => _animationCycleLookup.Count;

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> element from this <see cref="SpriteSheet"/> at the specified index.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the <see cref="TextureRegion"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> element that was located at the specified index in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TextureRegion"/> elements in this <see cref="SpriteSheet"/>.  Use
    ///     <see cref="SpriteSheet.RegionCount"/> to determine the total number of elements.
    /// </exception>
    public TextureRegion this[int regionIndex] => GetRegion(regionIndex);

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> element from this <see cref="SpriteSheet"/> with the specified name.
    /// </summary>
    /// <param name="regionName">
    ///     The name fo the <see cref="TextureRegion"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> element that was located with the specified name in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain a <see cref="TextureRegion"/> element with the
    ///     specified name.
    /// </exception>
    public TextureRegion this[string regionName] => GetRegion(regionName);

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheet"/> class.
    /// </summary>
    /// <param name="spritesheetName">
    ///     The name to give the <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="texture">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/> that will be represented by the
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    public SpriteSheet(string spritesheetName, Texture2D texture)
    {
        Name = spritesheetName;
        Texture = texture;
    }

    internal SpriteSheet(string spriteSheetName, Texture2D texture, ReadOnlySpan<Rectangle> regions, Dictionary<string, RawAnimationCycle> cycles)
    {
        Name = spriteSheetName;
        Texture = texture;
        AddRegions(regions);
        AddAnimationCycles(cycles);
    }

    #region Regions

    private void AddRegion(TextureRegion region)
    {
        if (_regionLookup.ContainsKey(region.Name))
        {
            throw new InvalidOperationException($"This {nameof(SpriteSheet)} already contains a {nameof(TextureRegion)} with the name '{region.Name}'.");
        }

        _regions.Add(region);
        _regionLookup.Add(region.Name, region);
    }

    private bool RemoveRegion(TextureRegion region) =>
        _regions.Remove(region) && _regionLookup.Remove(region.Name);


    /// <summary>
    ///     Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name to give the <see cref="TextureRegion"/> that is created by this method. Must be unique for this
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="regionXLocation">
    ///     The x-coordinate location of the upper-left corner of the <see cref="TextureRegion"/> that is created by
    ///     this method.
    /// </param>
    /// <param name="regionYLocation">
    ///     The y-coordinate location of the upper-left corner of the <see cref="TextureRegion"/> that is created by
    ///     this method.
    /// </param>
    /// <param name="regionWidth">
    ///     The width, in pixels, of the <see cref="TextureRegion"/> that is created by this method.
    /// </param>
    /// <param name="regionHeight">
    ///     The height, in pixels, of the <see cref="TextureRegion"/> that is created by this method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="SpriteSheet"/> already contains a <see cref="TextureRegion"/> element with the
    ///     name specified.
    /// </exception>
    /// <returns>
    ///     The <see cref="TextureRegion"/> that is created by this method.
    /// </returns>
    public TextureRegion CreateRegion(string regionName, int regionXLocation, int regionYLocation, int regionWidth, int regionHeight) =>
        CreateRegion(regionName, new Rectangle(regionXLocation, regionYLocation, regionWidth, regionHeight));

    /// <summary>
    ///     Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name to give the <see cref="TextureRegion"/> that is created by this method. Must be unique for this
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="regionLocation">
    ///     The x- and y-coordinate location of the upper-left corner of the <see cref="TextureRegion"/> that is created
    ///     by this method.
    /// </param>
    /// <param name="regionSize">
    ///     The width and height extents, in pixels, of the <see cref="TextureRegion"/> that is created by this method.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> that is created by this method.
    /// </returns>
    public TextureRegion CreateRegion(string regionName, Point regionLocation, Point regionSize) =>
        CreateRegion(regionName, new Rectangle(regionLocation, regionSize));

    /// <summary>
    ///     Creates a new <see cref="TextureRegion"/> and adds it to this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name to give the <see cref="TextureRegion"/> that is created by this method. Must be unique for this
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="regionBounds">
    ///     The rectangular bounds that define the upper-left corner location and the width and height extents, in
    ///     pixels, of the <see cref="TextureRegion"/> that is created by this method.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="SpriteSheet"/> already contains a <see cref="TextureRegion"/> element with the
    ///     name specified.
    /// </exception>
    /// <returns>
    ///     The <see cref="TextureRegion"/> that is created by this method.
    /// </returns>
    public TextureRegion CreateRegion(string regionName, Rectangle regionBounds)
    {
        TextureRegion region = new(regionName, Texture, regionBounds);
        AddRegion(region);
        return region;
    }

    internal void AddRegions(ReadOnlySpan<Rectangle> regions)
    {
        for (int i = 0; i < regions.Length; i++)
        {
            CreateRegion($"{Name} {i}", regions[i]);
        }
    }

    /// <summary>
    ///     Returns a value that indicates whether this <see cref="SpriteSheet"/> contains a <see cref="TextureRegion"/>
    ///     element with the specified name.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="TextureRegion"/> element to locate in this <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains a <see cref="TextureRegion"/> with the
    ///     specified name; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsRegion(string regionName) => _regionLookup.ContainsKey(regionName);

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> element from this <see cref="SpriteSheet"/> at the specified index.
    /// </summary>
    /// <param name="reginIndex">
    ///     The index of the <see cref="TextureRegion"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> element that was located at the specified index in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TextureRegion"/> elements in this <see cref="SpriteSheet"/>.  Use
    ///     <see cref="SpriteSheet.RegionCount"/> to determine the total number of elements.
    /// </exception>
    public TextureRegion GetRegion(int reginIndex)
    {
        if (reginIndex < 0 || reginIndex >= _regions.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(reginIndex), $"{nameof(reginIndex)} cannot be less than zero or greater than or equal to the total number of {nameof(TextureRegion)} elements in this {nameof(SpriteSheet)}.");
        }

        return _regions[reginIndex];
    }

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> element from this <see cref="SpriteSheet"/> with the specified name.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="TextureRegion"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="TextureRegion"/> element that was located with the specified name in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain a <see cref="TextureRegion"/> element with the
    ///     specified name.
    /// </exception>
    public TextureRegion GetRegion(string regionName)
    {
        if (_regionLookup.TryGetValue(regionName, out TextureRegion? frame))
        {
            return frame;
        }

        throw new KeyNotFoundException($"This {nameof(SpriteSheet)} does not contain a {nameof(TextureRegion)} with the name '{regionName}'.");
    }

    /// <summary>
    ///     Gets a collection of all <see cref="TextureRegion"/> elements at the specified indexes in this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionIndexes">
    ///     The indexes of the <see cref="TextureRegion"/> elements in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     A new <see cref="List{T}"/> containing the <see cref="TextureRegion"/> elements located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if anu of the specified indexes are less than zero or are greater than or equal to the total number
    ///     of <see cref="TextureRegion"/> elements in this <see cref="SpriteSheet"/>. Use
    ///     <see cref="SpriteSheet.RegionCount"/> to determine the total number of elements.
    /// </exception>
    public List<TextureRegion> GetRegions(params int[] regionIndexes)
    {
        List<TextureRegion> regions = new();
        for (int i = 0; i < regionIndexes.Length; i++)
        {
            regions.Add(GetRegion(regionIndexes[i]));
        }

        return regions;
    }

    /// <summary>
    ///     Gets a collection of all <see cref="TextureRegion"/> elements with the specified names in this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionNames">
    ///     The names of the <see cref="TextureRegion"/> elements in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     A new <see cref="List{T}"/> containing the <see cref="TextureRegion"/> elements located.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if any of the specified names do not match a <see cref="TextureRegion"/> element from this
    ///     <see cref="SpriteSheet"/>.
    /// </exception>
    public List<TextureRegion> GetRegions(params string[] regionNames)
    {
        List<TextureRegion> regions = new();

        for (int i = 0; i < regionNames.Length; i++)
        {
            regions.Add(GetRegion(regionNames[i]));
        }

        return regions;
    }

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> element from this <see cref="SpriteSheet"/> at the specified index.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the <see cref="TextureRegion"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <param name="region">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TextureRegion"/> element was located at the specified index;
    ///     otherwise, <see langword="false"/>.  This method returns <see langword="false"/> if the index specified is
    ///     less than zero or is greater than or equal to the total number of <see cref="TextureRegion"/> elements in
    ///     this <see cref="SpriteSheet"/>.  Use <see cref="SpriteSheet.RegionCount"/> to determine the total number of
    ///     elements.
    /// </returns>
    public bool TryGetRegion(int regionIndex, [NotNullWhen(true)] out TextureRegion? region)
    {
        region = default;

        if (regionIndex < 0 || regionIndex >= _regions.Count)
        {
            return false;
        }

        region = _regions[regionIndex];
        return true;
    }

    /// <summary>
    ///     Gets the <see cref="TextureRegion"/> element from this <see cref="SpriteSheet"/> with the specified name.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="TextureRegion"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <param name="region">
    ///     When this method returns <see langword="true"/>, contains the <see cref="TextureRegion"/> located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if a <see cref="TextureRegion"/> element was located with the specified name;
    ///     otherwise, <see langword="false"/>.  This method returns <see langword="false"/> if this
    ///     <see cref="SpriteSheet"/> does not contain a <see cref="TextureRegion"/> element with the specified name.
    /// </returns>
    public bool TryGetRegion(string regionName, [NotNullWhen(true)] out TextureRegion? region) =>
        _regionLookup.TryGetValue(regionName, out region);

    /// <summary>
    ///     Removes the <see cref="TextureRegion"/> element at the specified index from this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the <see cref="TextureRegion"/> element to remove from this <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TextureRegion"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method return <see langword="false"/> if the specified index is less than
    ///     zero or is greater than or equal to the total number of <see cref="TextureRegion"/> elements in this
    ///     <see cref="SpriteSheet"/>.  Use <see cref="SpriteSheet.RegionCount"/> to determine the total number of
    ///     elements.
    /// </returns>
    public bool RemoveRegion(int regionIndex) => RemoveRegion(GetRegion(regionIndex));

    /// <summary>
    ///     Removes the <see cref="TextureRegion"/> element with the specified name from this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="TextureRegion"/> element to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="TextureRegion"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this
    ///     <see cref="SpriteSheet"/> does not contain a <see cref="TextureRegion"/> element with the specified name.
    /// </returns>
    public bool RemoveRegion(string regionName) => RemoveRegion(GetRegion(regionName));

    #endregion Regions

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
    ///     Creates a new <see cref="AnimationCycle"/> and adds it this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="cycleName">
    ///     The name to give the <see cref="AnimationCycle"/> that is created by this method.  Must be unique for this
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="buildAction">
    ///     An action delegate used to build the <see cref="AnimationCycle"/> using an
    ///     <see cref="AnimationCycleBuilder"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimationCycle"/> that is created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="SpriteSheet"/> already contains an <see cref="AnimationCycle"/> element with the
    ///     specified name.
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

    internal void AddAnimationCycles(Dictionary<string, RawAnimationCycle> cycles)
    {
        foreach(KeyValuePair<string, RawAnimationCycle> kvp in cycles)
        {
            string name = kvp.Key;
            RawAnimationCycle animation = kvp.Value;

            CreateAnimationCycle(name, builder =>
            {
                for (int i = 0; i < animation.FrameIndexes.Length; i++)
                {
                    int index = animation.FrameIndexes[i];
                    TimeSpan duration = TimeSpan.FromMilliseconds(animation.FrameDurations[i]);
                    builder.AddFrame(index, duration);
                }

                builder.IsLooping(animation.IsLooping);
                builder.IsReversed(animation.IsReversed);
                builder.IsPingPong(animation.IsPingPong);
            });
        }
    }

    /// <summary>
    ///     Gets the <see cref="AnimationCycle"/> element from this <see cref="SpriteSheet"/> with the specified name.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the <see cref="AnimationCycle"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimationCycle"/> element that was located with the specified name in this
    ///     <see cref="SpriteSheet"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain a <see cref="AnimationCycle"/> element with the
    ///     specified name.
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
    ///     Gets the <see cref="AnimationCycle"/> element from this <see cref="SpriteSheet"/> with the specified name.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the <see cref="AnimationCycle"/> element in this <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <param name="animation">
    ///     When this method returns <see langword="true"/>, contains the <see cref="AnimationCycle"/> located;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if an <see cref="AnimationCycle"/> element was located with the specified name;
    ///     otherwise, <see langword="false"/>.  This method returns <see langword="false"/> if this
    ///     <see cref="SpriteSheet"/> does not contain an <see cref="AnimationCycle"/> element with the specified name.
    /// </returns>
    public bool TryGetAnimationCycle(string cycleName, [NotNullWhen(true)] out AnimationCycle? animation) =>
        _animationCycleLookup.TryGetValue(cycleName, out animation);

    public List<string> GetAnimationCycleNames() => _animationCycleLookup.Keys.ToList();

    /// <summary>
    ///     Returns a value that indicates whether this <see cref="SpriteSheet"/> contains an
    ///     <see cref="AnimationCycle"/> element with the specified name.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the <see cref="AnimationCycle"/> element to locate in this <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains an <see cref="AnimationCycle"/> with the
    ///     specified name; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsAnimationCycle(string cycleName) => _animationCycleLookup.ContainsKey(cycleName);

    /// <summary>
    ///     Removes the <see cref="AnimationCycle"/> element with the specified name from this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the <see cref="AnimationCycle"/> to remove.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="AnimationCycle"/> was removed successfully; otherwise,
    ///     <see langword="false"/>.  This method returns <see langword="false"/> if this <see cref="SpriteSheet"/> does
    ///     not contain an <see cref="AnimationCycle"/> element with the specified name.
    /// </returns>
    public bool RemoveAnimationCycle(string cycleName) => _animationCycleLookup.Remove(cycleName);

    /// <summary>
    ///     Creates a new <see cref="Animation"/> based on the <see cref="AnimationCycle"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified name.
    /// </summary>
    /// <param name="cycleName">
    ///     The name of the <see cref="AnimationCycle"/> in this <see cref="SpriteSheet"/> to locate and use to create
    ///     the <see cref="Animation"/> with.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Animation"/> class that is created by this method.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain an <see cref="AnimationCycle"/> element with the
    ///     name specified.
    /// </exception>
    public Animation CreateAnimation(string cycleName)
    {
        AnimationCycle cycle = GetAnimationCycle(cycleName);
        Animation animation = new(cycle);
        return animation;
    }

    #endregion Animations
}
