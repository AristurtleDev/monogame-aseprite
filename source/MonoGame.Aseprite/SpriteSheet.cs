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

namespace MonoGame.Aseprite.Sprites;

/// <summary>
///     Defines a spritesheet that contains a source image and named texture regions, with method for creating sprites
///     and animated sprites.
/// </summary>
public sealed class SpriteSheet : TextureAtlas
{
    private static readonly TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    private Dictionary<string, AnimationCycle> _animationCycleLookup = new();

    /// <summary>
    ///     Gets the total number of animation cycles in this spritesheet.
    /// </summary>
    public int AnimationCycleCount => _animationCycleLookup.Count;

    /// <summary>
    ///     Creates a new spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name to give the spritesheet.
    /// </param>
    /// <param name="texture">
    ///     The source texture for this spritesheet.
    /// </param>
    public SpriteSheet(string name, Texture2D texture) : base(name, texture) { }

    /// <summary>
    ///     Creates a new sprite based on a the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the sprite that is created.
    /// </param>
    /// <param name="regionIndex">
    ///     The index of the texture region in this spritesheet to assign to the sprite that is created.
    /// </param>
    /// <returns>
    ///     The sprite that is created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified texture region index is less than zero or is greater than or equal to the total
    ///     number of texture region elements in this spritesheet.
    /// </exception>
    public Sprite CreateSprite(string name, int regionIndex)
    {
        TextureRegion region = GetRegion(regionIndex);
        Sprite sprite = new(name, region);
        return sprite;
    }

    /// <summary>
    ///     Creates a new sprite based on the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <remarks>
    ///     The name of the texture region will be assigned as the name of the sprite that is created.
    /// </remarks>
    /// <param name="regionIndex">
    ///     The index of the texture region in this spritesheet ot assign to the sprite that is created.
    /// </param>
    /// <returns>
    ///     The sprite that is created by this method.
    /// </returns>
    public Sprite CreateSprite(int regionIndex)
    {
        TextureRegion region = GetRegion(regionIndex);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

    /// <summary>
    ///     Creates a new sprite based on the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the sprite that is created.
    /// </param>
    /// <param name="regionName">
    ///     The name of the texture region in this spritesheet to assign to the sprite that is created.
    /// </param>
    /// <returns>
    ///     The sprite that is created by this method.
    /// </returns>
    public Sprite CreateSprite(string name, string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        Sprite sprite = new(name, region);
        return sprite;
    }

    /// <summary>
    ///     Creates a new sprite based on the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <remarks>
    ///     The name of the texture region will be assigned as the name of the sprite that is created.
    /// </remarks>
    /// <param name="regionName">
    ///     The name of the texture region in this spritesheet to assign to the sprite that is created.
    /// </param>
    /// <returns>
    ///     The sprite that is created by this method.
    /// </returns>
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

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
        foreach (KeyValuePair<string, RawAnimationCycle> kvp in cycles)
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
