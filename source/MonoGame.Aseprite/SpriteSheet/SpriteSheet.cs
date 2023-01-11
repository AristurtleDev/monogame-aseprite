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

namespace MonoGame.Aseprite;

public class SpriteSheet
{
    private static readonly TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    private List<SpriteSheetFrame> _frames = new();
    private Dictionary<string, SpriteSheetFrame> _frameLookup = new();

    private Dictionary<string, SpriteSheetAnimation> _animationsLookup = new();


    public string Name { get; }
    public TextureAtlas TextureAtlas { get; }

    public SpriteSheetFrame this[int frameIndex] => GetFrame(frameIndex);
    public SpriteSheetFrame this[string frameName] => GetFrame(frameName);

    public SpriteSheet(string name, TextureAtlas textureAtlas)
    {
        Name = name;
        TextureAtlas = textureAtlas;
    }

    #region Frames

    private void AddFrame(SpriteSheetFrame frame)
    {
        if (_frameLookup.ContainsKey(frame.Name))
        {
            throw new InvalidOperationException();
        }

        _frames.Add(frame);
        _frameLookup.Add(frame.Name, frame);
    }

    private void RemoveFrame(SpriteSheetFrame frame)
    {
        _frames.Remove(frame);
        _frameLookup.Remove(frame.Name);
    }

    private SpriteSheetFrame CreateFrame(string frameName, TextureAtlasRegion region, TimeSpan duration)
    {
        SpriteSheetFrame frame = new(frameName, region, duration);
        AddFrame(frame);
        return frame;
    }

    public SpriteSheetFrame CreateFrame(string frameName, int textureAtlasRegionIndex) =>
        CreateFrame(frameName, textureAtlasRegionIndex, s_defaultDuration);

    public SpriteSheetFrame CreateFrame(string frameName, int textureAtlasRegionIndex, TimeSpan duration)
    {
        TextureAtlasRegion region = TextureAtlas[textureAtlasRegionIndex];
        return CreateFrame(frameName, region, duration);
    }

    public SpriteSheetFrame CreateFrame(string frameName, string textureAtlasRegionName) =>
        CreateFrame(frameName, textureAtlasRegionName, s_defaultDuration);

    public SpriteSheetFrame CreateFrame(string frameName, string textureAtlasRegionName, TimeSpan duration)
    {
        TextureAtlasRegion region = TextureAtlas[textureAtlasRegionName];
        return CreateFrame(frameName, region, duration);
    }

    public bool ContainsFrame(string frameName) => _frameLookup.ContainsKey(frameName);

    public SpriteSheetFrame GetFrame(int frameIndex)
    {
        if (frameIndex < 0 || frameIndex >= _frames.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(frameIndex));
        }

        return _frames[frameIndex];
    }

    public SpriteSheetFrame GetFrame(string frameName)
    {
        if (_frameLookup.TryGetValue(frameName, out SpriteSheetFrame? frame))
        {
            return frame;
        }

        throw new KeyNotFoundException();
    }

    public bool TryGetFrame(int frameIndex, [NotNullWhen(true)] out SpriteSheetFrame? frame)
    {
        frame = default;

        if (frameIndex < 0 || frameIndex >= _frames.Count)
        {
            return false;
        }

        frame = _frames[frameIndex];
        return true;
    }

    public bool TryGetFrame(string frameName, [NotNullWhen(true)] out SpriteSheetFrame? frame) =>
        _frameLookup.TryGetValue(frameName, out frame);

    public void RemoveFrame(int frameIndex)
    {
        SpriteSheetFrame frame = GetFrame(frameIndex);
        RemoveFrame(frame);
    }

    public void RemoveFrame(string frameName)
    {
        SpriteSheetFrame frame = GetFrame(frameName);
        RemoveFrame(frame);
    }

    public bool TryRemoveFrame(int frameIndex)
    {
        if (TryGetFrame(frameIndex, out SpriteSheetFrame? frame))
        {
            RemoveFrame(frame);
            return true;
        }

        return false;
    }

    public bool TryRemoveFrame(string frameName)
    {
        if (TryGetFrame(frameName, out SpriteSheetFrame? frame))
        {
            RemoveFrame(frame);
            return true;
        }

        return false;
    }

    #endregion Frames

    #region Sprite

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element from this
    ///     <see cref="SpriteSheet"/> at the specified
    ///     <paramref name="frameName"/>.
    /// </summary>
    /// <param name="frameName">
    ///     The index of the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> to use when creating the
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Sprite"/> class created by this
    ///     method.
    /// </returns>
    public Sprite CreateSprite(int frameIndex) =>
        CreateSprite(frameIndex, Vector2.Zero, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element from this
    ///     <see cref="SpriteSheet"/> at the specified
    ///     <paramref name="frameIndex"/>.
    /// </summary>
    /// <param name="frameIndex">
    ///     The index of the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> to use when creating the
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to use when rendering the
    ///     <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="color">
    ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    ///     color mask when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Sprite"/> class created by this
    ///     method.
    /// </returns>
    public Sprite CreateSprite(int frameIndex, Vector2 position, Color color) =>
        CreateSprite(frameIndex, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element from this
    ///     <see cref="SpriteSheet"/> at the specified
    ///     <paramref name="frameIndex"/>.
    /// </summary>
    /// <param name="frameIndex">
    ///     The index of the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> to use when creating the
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to use when rendering the
    ///     <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="color">
    ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    ///     color mask when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the
    ///     <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point to use as the point of origin when
    ///     rendering the <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="scale">
    ///     The amount of x-axis (horizontal) and y-axis (vertical) scale to
    ///     apply when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    ///     apply when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="Sprite"/>
    ///     created by this method.
    /// </param>
    /// <returns></returns>
    public Sprite CreateSprite(int frameIndex, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        SpriteSheetFrame frame = GetFrame(frameIndex);
        Sprite sprite = new(frame);

        sprite.Position = position;
        sprite.Color = color;
        sprite.Rotation = rotation;
        sprite.Origin = origin;
        sprite.Scale = scale;
        sprite.SpriteEffects = effects;
        sprite.LayerDepth = layerDepth;

        return sprite;
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element from this
    ///     <see cref="SpriteSheet"/> at the specified
    ///     <paramref name="frameName"/>.
    /// </summary>
    /// <param name="frameName">
    ///     The name of the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> to use when creating the
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Sprite"/> class created by this
    ///     method.
    /// </returns>
    public Sprite CreateSprite(string frameName) =>
        CreateSprite(frameName, Vector2.Zero, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element from this
    ///     <see cref="SpriteSheet"/> at the specified
    ///     <paramref name="frameName"/>.
    /// </summary>
    /// <param name="frameName">
    ///     The name of the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> to use when creating the
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to use when rendering the
    ///     <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="color">
    ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    ///     color mask when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="Sprite"/> class created by this
    ///     method.
    /// </returns>
    public Sprite CreateSprite(string frameName, Vector2 position, Color color) =>
        CreateSprite(frameName, position, color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);

    /// <summary>
    ///     Creates a new instance of the <see cref="Sprite"/> class using the
    ///     <see cref="SpriteSheetFrame"/> element from this
    ///     <see cref="SpriteSheet"/> at the specified
    ///     <paramref name="frameName"/>.
    /// </summary>
    /// <param name="frameName">
    ///     The name of the <see cref="SpriteSheetFrame"/> element in this
    ///     <see cref="SpriteSheet"/> to use when creating the
    ///     <see cref="Sprite"/>.
    /// </param>
    /// <param name="position">
    ///     The x- and y-coordinate position to use when rendering the
    ///     <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="color">
    ///     The <see cref="Microsoft.Xna.Framework.Color"/> value to use as the
    ///     color mask when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <param name="rotation">
    ///     The amount of rotation, in radians, to apply when rendering the
    ///     <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="origin">
    ///     The x- and y-coordinate point to use as the point of origin when
    ///     rendering the <see cref="Sprite"/> created by this method.
    /// </param>
    /// <param name="scale">
    ///     The amount of x-axis (horizontal) and y-axis (vertical) scale to
    ///     apply when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <param name="effects">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.SpriteEffects"/> to
    ///     apply when rendering the <see cref="Sprite"/> created by this
    ///     method.
    /// </param>
    /// <param name="layerDepth">
    ///     The layer depth to apply when rendering the <see cref="Sprite"/>
    ///     created by this method.
    /// </param>
    /// <returns></returns>
    public Sprite CreateSprite(string frameName, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        SpriteSheetFrame frame = GetFrame(frameName);
        Sprite sprite = new(frame);

        sprite.Position = position;
        sprite.Color = color;
        sprite.Rotation = rotation;
        sprite.Origin = origin;
        sprite.Scale = scale;
        sprite.SpriteEffects = effects;
        sprite.LayerDepth = layerDepth;

        return sprite;
    }

    #endregion Sprite

    #region Animations

    /// <summary>
    ///     Creates and adds a <see cref="SpriteSheetAnimation"/> element to
    ///     this <see cref="SpriteSheet"/>
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetAnimation"/>.
    /// </param>
    /// <param name="frameIndexes">
    ///     The indexes of the <see cref="SpriteSheetFrame"/> element that make
    ///     up the <see cref="SpriteSheetAnimation"/> being created.  Order of
    ///     frames should be from first to last.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetAnimation"/> that is created by this
    ///     method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetAnimation"/> has already been
    ///     created by this <see cref="SpriteSheet"/> with the same
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetAnimation CreateAnimation(string name, params int[] frameIndexes) =>
        CreateAnimation(name, frameIndexes, true, false, false);

    /// <summary>
    ///     Creates and adds a <see cref="SpriteSheetAnimation"/> element to
    ///     this <see cref="SpriteSheet"/>
    /// </summary>
    /// <param name="name">
    ///     The name to give the <see cref="SpriteSheetAnimation"/>.
    /// </param>
    /// <param name="frameIndexes">
    ///     The indexes of the <see cref="SpriteSheetFrame"/> element that make
    ///     up the <see cref="SpriteSheetAnimation"/> being created.  Order of
    ///     frames should be from first to last.
    /// </param>
    /// <param name="isLooping">
    ///     Indicates whether the <see cref="SpriteSheetAnimation"/> defines
    ///     that the animation playback should loop.
    /// </param>
    /// <param name="isReversed">
    ///     Indicates whether the <see cref="SpriteSheetAnimation"/> defines
    ///     that the animation playback should be in reverse frame order.
    /// </param>
    /// <param name="isPingPong">
    ///     Indicates whether the <see cref="SpriteSheetAnimation"/> defines
    ///     that the animation playback should ping-pong.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetAnimation"/> that is created by this
    ///     method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a <see cref="SpriteSheetAnimation"/> has already been
    ///     created by this <see cref="SpriteSheet"/> with the same
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetAnimation CreateAnimation(string name, int[] frameIndexes, bool isLooping, bool isReversed, bool isPingPong)
    {
        if (_animationsLookup.ContainsKey(name))
        {
            throw new InvalidOperationException();
        }

        SpriteSheetAnimation animation = new(name, frameIndexes, isLooping, isReversed, isPingPong);
        _animationsLookup.Add(name, animation);
        return animation;
    }

    /// <summary>
    ///     Returns a value that indicates whether this
    ///     <see cref="SpriteSheet"/> contains a
    ///     <see cref="SpriteSheetAnimation"/> element with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetAnimation"/> element in this
    ///     <see cref="SpriteSheet"/> to locate.
    /// </param>
    /// <returns></returns>
    public bool ContainsAnimation(string name) => _animationsLookup.ContainsKey(name);

    /// <summary>
    ///     Returns the <see cref="SpriteSheetAnimation"/> element from this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetAnimation"/> to get.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheetAnimation"/> element in this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain a
    ///     <see cref="SpriteSheetAnimation"/> element with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public SpriteSheetAnimation GetAnimation(string name)
    {
        if (TryGetAnimation(name, out SpriteSheetAnimation? animation))
        {
            return animation;
        }

        throw new KeyNotFoundException();
    }

    /// <summary>
    ///     Gets the <see cref="SpriteSheetAnimation"/> element from this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetAnimation"/> to get.
    /// </param>
    /// <param name="animation">
    ///     When this method returns, if <see langword="true"/>, contains the
    ///     <see cref="SpriteSheetAnimation"/> element from this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains a
    ///     <see cref="SpriteSheetAnimation"/> element with the specified
    ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetAnimation(string name, [NotNullWhen(true)] out SpriteSheetAnimation? animation) =>
        _animationsLookup.TryGetValue(name, out animation);

    /// <summary>
    ///     Removes the <see cref="SpriteSheetAnimation"/> element from this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetAnimation"/> to remove from
    ///     this <see cref="SpriteSheet"/>.
    /// </param>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain a
    ///     <see cref="SpriteSheetAnimation"/> element with the specified
    ///     <paramref name="name"/>.
    /// </exception>
    public void RemoveAnimation(string name)
    {
        if (!TryGetAnimation(name, out SpriteSheetAnimation? animation))
        {
            throw new KeyNotFoundException();
        }

        _animationsLookup.Remove(name);
    }

    /// <summary>
    ///     Removes the <see cref="SpriteSheetAnimation"/> element from this
    ///     <see cref="SpriteSheet"/> with the specified
    ///     <paramref name="name"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="SpriteSheetAnimation"/> element to remove
    ///     from this <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="SpriteSheetAnimation"/>
    ///     element was successfully found and removed; otherwise,
    ///     <see langword="false"/>.  This method returns
    ///     <see langword="false"/> if this <see cref="SpriteSheet"/> does not
    ///     contain a <see cref="SpriteSheetAnimation"/> element with the
    ///     specified <paramref name="name"/>.
    /// </returns>
    public bool TryRemoveAnimation(string name) => _animationsLookup.Remove(name);

    #endregion Animations

#region Animated Sprite

// vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
//
//  This is where I left off.
//  Tie in the new spritesheet animation class with the
//  animated sprite class
//
// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

#endregion Animated Sprite

}

// public sealed class SpriteSheet : IEnumerable<SpriteSheetFrame>
// {
//     private static TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

//     public TextureAtlas TextureAtlas { get; }

//     private Dictionary<string, SpriteSheetAnimationDefinition> _animationLookup = new();

//     /// <summary>
//     ///     Gets the <see cref="SpriteSheetFrame"/> element in this
//     ///     <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="SpriteSheetFrame"/> element in this
//     ///     <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </returns>
//     /// <exception cref="KeyNotFoundException">
//     ///     Thrown if there is no <see cref="SpriteSheetFrame"/> element
//     ///     in this <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrame this[string name] => GetRegion(name);

//     /// <summary>
//     ///     Gets the <see cref="SpriteSheetFrame"/> element at the specified
//     ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="index">
//     ///     The index of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="SpriteSheetFrame"/> element at the specified
//     ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
//     /// </returns>
//     /// <exception cref="ArgumentOutOfRangeException">
//     ///     Thrown if the specified <paramref name="index"/> is less than zero
//     ///     or is greater than or equal to <see cref="RegionCount"/>.
//     /// </exception>
//     public SpriteSheetFrame this[int index] => GetRegion(index);

//     /// <summary>
//     ///     Gets the name of this <see cref="SpriteSheet"/>.
//     /// </summary>
//     public string Name { get; }

//     /// <summary>
//     ///     Gets the <see cref="Texture2D"/> represented by this
//     ///     <see cref="SpriteSheet"/>.
//     /// </summary>
//     public Texture2D Texture { get; }

//     /// <summary>
//     ///     Gets the total number of <see cref="SpriteSheetFrame"/> elements
//     ///     in this <see cref="SpriteSheet"/>.
//     /// </summary>
//     public int RegionCount => _regions.Count;

//     /// <summary>
//     ///     Initializes a new instance of the <see cref="SpriteSheet"/> class
//     ///     with the specified <paramref name="name"/> using the specified
//     ///     <paramref name="texture"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name to give this <see cref="SpriteSheet"/>.
//     /// </param>
//     /// <param name="texture">
//     ///     The <see cref="Texture2D"/> that this <see cref="SpriteSheet"/>
//     ///     represents.
//     /// </param>
//     public SpriteSheet(string name, Texture2D texture) => (Name, Texture) = (name, texture);

//     /// <summary>
//     ///     Returns a value that indicates whether this
//     ///     <see cref="SpriteSheet"/> contains a <see cref="SpriteSheetFrame"/>
//     ///     element with the specified <paramref name="name"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name of the <see cref="SpriteSheetFrame"/> to locate in this
//     ///    <see cref="SpriteSheet"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains
//     ///     a <see cref="SpriteSheetFrame"/> element with the specified
//     ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
//     /// </returns>
//     public bool ContainsRegion(string name) => _regionLookup.ContainsKey(name);

//     /// <summary>
//     ///     Gets the <see cref="SpriteSheetFrame"/> element at the specified
//     ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="index">
//     ///     The index of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="SpriteSheetFrame"/> element at the specified
//     ///     <paramref name="index"/> in this <see cref="SpriteSheet"/>.
//     /// </returns>
//     /// <exception cref="ArgumentOutOfRangeException">
//     ///     Thrown if the specified <paramref name="index"/> is less than zero
//     ///     or is greater than or equal to <see cref="RegionCount"/>.
//     /// </exception>
//     public SpriteSheetFrame GetRegion(int index)
//     {
//         if (index < 0 || index >= _regions.Count)
//         {
//             throw new ArgumentOutOfRangeException(nameof(index), $"The {nameof(index)} cannot be less than zero or greater than or equal to the total number of {nameof(SpriteSheetFrame)} elements in this {nameof(SpriteSheet)}");
//         }

//         return _regions[index];
//     }

//     /// <summary>
//     ///     Gets the <see cref="SpriteSheetFrame"/> element in this
//     ///     <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <returns>
//     ///     The <see cref="SpriteSheetFrame"/> element in this
//     ///     <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </returns>
//     /// <exception cref="KeyNotFoundException">
//     ///     Thrown if there is no <see cref="SpriteSheetFrame"/> element
//     ///     in this <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrame GetRegion(string name)
//     {
//         if (_regionLookup.TryGetValue(name, out SpriteSheetFrame? region))
//         {
//             return region;
//         }

//         throw new KeyNotFoundException($"No SpriteSheetRegion with the name '{name}' was found in the SpriteSheet '{Name}'");
//     }

//     /// <summary>
//     ///     Gets the <see cref="SpriteSheetFrame"/> element in this
//     ///     <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     THe name of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="region">
//     ///     When this method returns, contains the
//     ///     <see cref="SpriteSheetFrame"/> element that had the specified
//     ///     <paramref name="name"/>, if one was found; otherwise,
//     ///     <see langword="null"/>.
//     /// </param>
//     /// <returns>
//     ///     <see langword="true"/> if a <see cref="SpriteSheetFrame"/> element
//     ///     was found in this <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>; otherwise, <see langword="false"/>.
//     /// </returns>
//     public bool TryGetRegion(string name, out SpriteSheetFrame? region) =>
//         _regionLookup.TryGetValue(name, out region);

//     /// <summary>
//     ///     Creates a new instance of the <see cref="Sprite"/> class using the
//     ///     <see cref="SpriteSheetFrame"/> element with the specified
//     ///     <paramref name="regionName"/> from this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="regionName">
//     ///     The name of the <see cref="SpriteSheetFrame"/> element to use when
//     ///     creating the <see cref="Sprite"/>.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="Sprite"/> class initialized with
//     ///     the <see cref="SpriteSheetFrame"/> element with the specified
//     ///     <paramref name="regionName"/> from this <see cref="SpriteSheet"/>.
//     /// </returns>
//     /// <exception cref="KeyNotFoundException">
//     ///     Thrown if there is no <see cref="SpriteSheetFrame"/> element
//     ///     in this <see cref="SpriteSheet"/> with the specified
//     ///     <paramref name="name"/>.
//     /// </exception>
//     public Sprite CreateSprite(string regionName)
//     {
//         SpriteSheetFrame region = GetRegion(regionName);
//         return new Sprite(region);
//     }

//     /// <summary>
//     ///     Creates a new instance of the <see cref="Sprite"/> class using the
//     ///     <see cref="SpriteSheetFrame"/> element at the specified
//     ///     <paramref name="regionIndex"/> in this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="regionIndex">
//     ///     The index of the <see cref="SpriteSheetFrame"/> element to use
//     ///     when creating the <see cref="Sprite"/>.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="Sprite"/> class initialized with
//     ///     the <see cref="SpriteSheetFrame"/> element at the specified
//     ///     <paramref name="regionIndex"/> in this <see cref="SpriteSheet"/>.
//     /// </returns>
//     /// <exception cref="ArgumentOutOfRangeException">
//     ///     Thrown if the specified <paramref name="index"/> is less than zero
//     ///     or is greater than or equal to <see cref="RegionCount"/>.
//     /// </exception>
//     public Sprite CreateSprite(int regionIndex)
//     {
//         SpriteSheetFrame region = GetRegion(regionIndex);
//         return new Sprite(region);
//     }

//     private void AddFrame(SpriteSheetFrame region)
//     {
//         _regions.Add(region);
//         _regionLookup.Add(region.Name, region);
//         _regionMap.Add(region.Name, _regions.Count - 1);
//     }

//     /// <summary>
//     ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
//     ///     next index of this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name to give the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="bounds">
//     ///     The bounds of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
//     ///     created by this method.
//     /// </returns>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
//     ///     been created for this <see cref="SpriteSheet"/> with the
//     ///     specified <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrame CreateFrame(string name, Rectangle bounds) =>
//         CreateFrame(name, bounds.X, bounds.Y, bounds.Width, bounds.Height, s_defaultDuration);

//     /// <summary>
//     ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
//     ///     next index of this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name to give the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="bounds">
//     ///     The bounds of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="duration">
//     ///     The duration of the <see cref="SpriteSheetFrame"/> when used in
//     ///     an animation.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
//     ///     created by this method.
//     /// </returns>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
//     ///     been created for this <see cref="SpriteSheet"/> with the
//     ///     specified <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrame CreateFrame(string name, Rectangle bounds, TimeSpan duration) =>
//         CreateFrame(name, bounds.X, bounds.Y, bounds.Width, bounds.Height, duration);

//     /// <summary>
//     ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
//     ///     next index of this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name to give the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="x">
//     ///     The x-coordinate location of the upper-left corner of the
//     ///     <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="y">
//     ///     The y-coordinate location of the upper-left corner of the
//     ///     <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="width">
//     ///     The width, in pixels, of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="height">
//     ///     The height, in pixels, of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
//     ///     created by this method.
//     /// </returns>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
//     ///     been created for this <see cref="SpriteSheet"/> with the
//     ///     specified <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrame CreateFrame(string name, int x, int y, int width, int height) =>
//         CreateFrame(name, x, y, width, height, s_defaultDuration);


//     /// <summary>
//     ///     Creates a new <see cref="SpriteSheetFrame"/> and adds it to the
//     ///     next index of this <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <param name="name">
//     ///     The name to give the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="x">
//     ///     The x-coordinate location of the upper-left corner of the
//     ///     <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="y">
//     ///     The y-coordinate location of the upper-left corner of the
//     ///     <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="width">
//     ///     The width, in pixels, of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="height">
//     ///     The height, in pixels, of the <see cref="SpriteSheetFrame"/>.
//     /// </param>
//     /// <param name="duration">
//     ///     The duration of the <see cref="SpriteSheetFrame"/> when used in
//     ///     an animation.
//     /// </param>
//     /// <returns>
//     ///     A new instance of the <see cref="SpriteSheetFrame"/> class that is
//     ///     created by this method.
//     /// </returns>
//     /// <exception cref="InvalidOperationException">
//     ///     Thrown if a <see cref="SpriteSheetFrame"/> element has already
//     ///     been created for this <see cref="SpriteSheet"/> with the
//     ///     specified <paramref name="name"/>.
//     /// </exception>
//     public SpriteSheetFrame CreateFrame(string name, int x, int y, int width, int height, TimeSpan duration)
//     {
//         if (_regionLookup.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"A {nameof(SpriteSheetFrame)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
//         }

//         SpriteSheetFrame frame = new(name, Texture, new Rectangle(x, y, width, height), duration);

//         AddFrame(frame);
//         return frame;
//     }



//     public SpriteSheetAnimationDefinition GetAnimationDefinition(string name)
//     {
//         if (_animationLookup.TryGetValue(name, out SpriteSheetAnimationDefinition? definition))
//         {
//             return definition;
//         }

//         throw new KeyNotFoundException($"No {nameof(SpriteSheetAnimationDefinition)} with the name '{name}' exists in this {nameof(SpriteSheet)}.");
//     }

//     public void AddAnimationDefinition(string name, params int[] frames) => AddAnimationDefinition(name, true, false, false, frames);

//     public void AddAnimationDefinition(string name, bool isLooping, bool isReversed, bool isPingPong, params int[] frames)
//     {
//         if (_animationLookup.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"A {nameof(SpriteSheetAnimationDefinition)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
//         }

//         SpriteSheetAnimationDefinition definition = new(frames, name, isLooping, isReversed, isPingPong);
//         _animationLookup.Add(name, definition);
//     }

//     public void AddAnimationDefinition(string name, params string[] frames) => AddAnimationDefinition(name, true, false, false, frames);

//     public void AddAnimationDefinition(string name, bool isLooping, bool isReversed, bool isPingPong, params string[] frames)
//     {
//         if (_animationLookup.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"A {nameof(SpriteSheetAnimationDefinition)} with the name '{name}' has already been added to this {nameof(SpriteSheet)}.");
//         }

//         int[] frameIndexes = new int[frames.Length];

//         for (int i = 0; i < frames.Length; i++)
//         {
//             if (_regionMap.TryGetValue(frames[i], out int index))
//             {
//                 frameIndexes[i] = index;
//             }
//             else
//             {
//                 throw new KeyNotFoundException($"No {nameof(SpriteSheetFrame)} with the name '{frames[i]}' exists in this {nameof(SpriteSheet)}.");
//             }
//         }

//         SpriteSheetAnimationDefinition definition = new(frameIndexes, name, isLooping, isReversed, isPingPong);
//         _animationLookup.Add(name, definition);
//     }


//     /// <summary>
//     ///     Returns an enumerator that iterates through the
//     ///     <see cref="SpriteSheetFrame"/> elements in this
//     ///     <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <returns>
//     ///     A <see cref="List{T}.Enumerator"/> that iterates through the
//     ///     <see cref="SpriteSheetFrame"/> elements in this
//     ///     <see cref="SpriteSheet"/>.
//     /// </returns>
//     public IEnumerator<SpriteSheetFrame> GetEnumerator() => _regions.GetEnumerator();


//     /// <summary>
//     ///     Returns an enumerator that iterates through the
//     ///     <see cref="SpriteSheetFrame"/> elements in this
//     ///     <see cref="SpriteSheet"/>.
//     /// </summary>
//     /// <returns>
//     ///     A <see cref="List{T}.Enumerator"/> that iterates through the
//     ///     <see cref="SpriteSheetFrame"/> elements in this
//     ///     <see cref="SpriteSheet"/>.
//     /// </returns>
//     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
// }
