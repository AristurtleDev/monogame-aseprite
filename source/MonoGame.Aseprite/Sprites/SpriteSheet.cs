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
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Sprites;

/// <summary>
/// Defines a spritesheet that contains a source image and named texture regions, with method for creating sprites and
/// animated sprites.
/// </summary>
public sealed class SpriteSheet
{
    private static readonly TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    private Dictionary<string, AnimationTag> _animationTagLookup = new();

    /// <summary>
    /// Gets the total number of animation tags that have been defined for this spritesheet.
    /// </summary>
    public int AnimationTagCount => _animationTagLookup.Count;

    /// <summary>
    /// Gets the name of this spritesheet.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the texture atlas that defines the texture regions used by this spritesheet.
    /// </summary>
    public TextureAtlas TextureAtlas { get; }

    /// <summary>
    /// Creates a new spritesheet.
    /// </summary>
    /// <param name="name">The name to give the spritesheet.</param>
    /// <param name="atlas">The texture atlas that defines the source texture regions used by this spritesheet.</param>
    public SpriteSheet(string name, TextureAtlas atlas) => (Name, TextureAtlas) = (name, atlas);

    /// <summary>
    /// Creates a new sprite based on a the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <param name="name">The name to assign the sprite that is created.</param>
    /// <param name="regionIndex">
    /// The index of the texture region in the texture atlas of this spritesheet that will be assigned to the sprite
    /// created by this method.
    /// </param>
    /// <returns>The sprite that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified texture region index is less than zero or is greater than or equal to the total number
    /// of texture regions in the texture atlas used by this spritesheet
    /// </exception>
    public Sprite CreateSprite(string name, int regionIndex)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionIndex);
        Sprite sprite = new(name, region);
        return sprite;
    }

    /// <summary>
    /// Creates a new sprite based on the texture region at the specified index in this spritesheet.
    /// </summary>
    /// <remarks>
    /// The name of the texture region will be assigned as the name of the sprite that is created.
    /// </remarks>
    /// <param name="regionIndex">
    /// The index of the texture region in the texture atlas of this spritesheet that will be assigned to the sprite
    /// created by this method.
    /// </param>
    /// <returns> The sprite that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified texture region index is less than zero or is greater than or equal to the total number
    /// of texture regions in the texture atlas used by this spritesheet
    /// </exception>
    public Sprite CreateSprite(int regionIndex)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionIndex);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

    /// <summary>
    /// Creates a new sprite based on the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <param name="name">The name to assign the sprite that is created.
    /// </param>
    /// <param name="regionName">
    /// The name of the texture region in the texture atlas of this spritesheet that will be assigned to the sprite
    /// created by this method.
    /// </param>
    /// <returns>The sprite that is created by this method.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the texture atlas of this spritesheet does not contain a texture region with the name specified.
    /// </exception>
    public Sprite CreateSprite(string name, string regionName)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionName);
        Sprite sprite = new(name, region);
        return sprite;
    }

    /// <summary>
    /// Creates a new sprite based on the texture region with the specified name in this spritesheet.
    /// </summary>
    /// <remarks>
    /// The name of the texture region will be assigned as the name of the sprite that is created.
    /// </remarks>
    /// <param name="regionName">
    /// The name of the texture region in the texture atlas of this spritesheet that will be assigned to the sprite
    /// created by this method.
    /// </param>
    /// <returns>The sprite that is created by this method.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the texture atlas of this spritesheet does not contain a texture region with the name specified.
    /// </exception>
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionName);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

    #region Animations

    internal void AddAnimationTag(AnimationTag tag)
    {
        if (_animationTagLookup.ContainsKey(tag.Name))
        {
            throw new InvalidOperationException($"{nameof(SpriteSheet)} '{Name}' already contains an {nameof(AnimationTag)} with the name '{tag.Name}'");
        }

        _animationTagLookup.Add(tag.Name, tag);
    }

    /// <summary>
    /// Creates and adds a new animation tag using an animation tag build to this spritesheet.
    /// </summary>
    /// <param name="name">
    /// The name to assign the animation tag that is created by this method.  This name must be unique across all other
    /// animation tags defined in this spritesheet.
    /// </param>
    /// <param name="build">An action method used to build the animation tag using an animation tag builder.</param>
    /// <returns>The animation tag that is created by this method.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if this spritesheet already contains an animation tag with the name specified.
    /// </exception>
    public AnimationTag CreateAnimationTag(string name, Action<AnimationTagBuilder> builder)
    {
        AnimationTagBuilder localBuilder = new(name, this);
        builder(localBuilder);

        AnimationTag tag = localBuilder.Build();
        AddAnimationTag(tag);

        return tag;
    }

    /// <summary>
    /// Gets the animation tag with the specified name that has been defined in this spritesheet.
    /// </summary>
    /// <param name="name">The name of the animation tag to locate in this spritesheet.</param>
    /// <returns>The animation tag that was located.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if an animation tag with the specified name has not been defined in this spritesheet.
    /// </exception>
    public AnimationTag GetAnimationTag(string name)
    {
        if (TryGetAnimationTag(name, out AnimationTag? tag))
        {
            return tag;
        }

        throw new KeyNotFoundException($"{nameof(SpriteSheet)} '{Name}' does not contain an {nameof(AnimationTag)} with the name '{name}'");
    }

    /// <summary>
    /// Gets the animation tag with the specified name that has been defined in this spritesheet.
    /// </summary>
    /// <param name="name">The name of the animation tag to locate in this spritesheet.</param>
    /// <param name="tag">When this method returns true, contains the animation tag located; otherwise, null.</param>
    /// <returns>
    /// true if the animation tag was located; otherwise, false.  This method returns false if this spritesheet does not
    /// contain an animation tag with the specified name.
    /// </returns>
    public bool TryGetAnimationTag(string name, [NotNullWhen(true)] out AnimationTag? tag) =>
        _animationTagLookup.TryGetValue(name, out tag);

    /// <summary>
    /// Gets a new collection containing the name of all animation tags that have been defined in this spritesheet.
    /// </summary>
    /// <returns>
    /// A new collection containing the name of all animation tags that have been defined in this spritesheet.
    /// </returns>
    public List<string> GetAnimationTagNames() => _animationTagLookup.Keys.ToList();

    /// <summary>
    /// Returns a value that indicates whether this spritesheet contains an animation tag with the specified name.
    /// </summary>
    /// <param name="name">The name fo the animation tag to locate in this spritesheet.</param>
    /// <returns>true if this spritesheet contains an animation tag with the specified name; otherwise, false.</returns>
    public bool ContainsAnimationCycle(string name) => _animationTagLookup.ContainsKey(name);

    /// <summary>
    /// Removes the animation tag with the specified name from this spritesheet.
    /// </summary>
    /// <param name="name">The name of the animation tag to remove from this spritesheet.</param>
    /// <returns>
    /// true if the animation tag was successfully removed from this spritesheet; otherwise, false.  This method returns
    /// false if this spritesheet does not contain an animation tag with the specified name.
    /// </returns>
    public bool RemoveAnimationCycle(string name) => _animationTagLookup.Remove(name);

    /// <summary>
    /// Creates a new animated sprite using the animation tag with the specified name defined in this spritesheet.
    /// </summary>
    /// <param name="tagName">
    /// The name of the animation tag defined in this spritesheet to create the animated sprite with.
    /// </param>
    /// <returns>The animated sprite that is created by this method.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if this spritesheet does not contain an animation tag with the specified name.
    /// </exception>
    public AnimatedSprite CreateAnimatedSprite(string tagName)
    {
        AnimationTag tag = GetAnimationTag(tagName);
        AnimatedSprite sprite = new(tag);
        return sprite;
    }

    #endregion Animations

    /// <summary>
    /// Creates a new spritesheet from the given raw spritesheet.
    /// </summary>
    /// <param name="device">The graphics device used to create graphical resources.</param>
    /// <param name="rawSpriteSheet">The raw spritesheet to create the spritesheet from.</param>
    /// <returns>The spritesheet created by this method.</returns>
    public static SpriteSheet FromRaw(GraphicsDevice device, SpriteSheetContent rawSpriteSheet)
    {
        TextureAtlas atlas = TextureAtlas.FromRaw(device, rawSpriteSheet.RawTextureAtlas);
        SpriteSheet spriteSheet = new(rawSpriteSheet.Name, atlas);

        for (int i = 0; i < rawSpriteSheet.RawAnimationTags.Length; i++)
        {
            AnimationTagContent tag = rawSpriteSheet.RawAnimationTags[i];

            spriteSheet.CreateAnimationTag(tag.Name, builder =>
            {
                builder.IsLooping(tag.IsLooping)
                       .IsReversed(tag.IsReversed)
                       .IsPingPong(tag.IsPingPong);

                for (int j = 0; j < tag.RawAnimationFrames.Length; j++)
                {
                    AnimationFrameContent rawAnimationFrame = tag.RawAnimationFrames[j];
                    TimeSpan duration = TimeSpan.FromMilliseconds(rawAnimationFrame.DurationInMilliseconds);
                    builder.AddFrame(rawAnimationFrame.FrameIndex, duration);
                }
            });
        }

        return spriteSheet;
    }
}
