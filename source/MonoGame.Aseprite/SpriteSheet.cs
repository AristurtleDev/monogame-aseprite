// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MonoGame.Aseprite;

/// <summary>
///     Defines a spritesheet with a source <see cref="TextureAtlas"/> and methods for creating <see cref="Sprite"/>
///     and <see cref="AnimatedSprite"/> elements.
/// </summary>
public sealed class SpriteSheet
{
    private static readonly TimeSpan s_defaultDuration = TimeSpan.FromMilliseconds(100);

    private Dictionary<string, AnimationTag> _animationTagLookup = new();

    /// <summary>
    ///     Gets the total number of <see cref="AnimationTag"/> elements that have been defined for this
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    public int AnimationTagCount => _animationTagLookup.Count;

    /// <summary>
    ///     Gets the name assigned to this <see cref="SpriteSheet"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the source <see cref="TextureAtlas"/> of this <see cref="SpriteSheet"/>.
    /// </summary>
    public TextureAtlas TextureAtlas { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheet"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name assign the <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="atlas">
    ///     The source <see cref="TextureAtlas"/> to give the <see cref="SpriteSheet"/>.
    /// </param>
    public SpriteSheet(string name, TextureAtlas atlas) => (Name, TextureAtlas) = (name, atlas);

    /// <summary>
    ///     Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> at the specified index in the
    ///     <see cref="TextureAtlas"/> of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="spriteName">
    ///     The name to assign the <see cref="Sprite"/> that is created.
    /// </param>
    /// <param name="regionIndex">
    ///     The index of the <see cref="TextureRegion"/> element in the <see cref="TextureAtlas"/> assign the 
    ///     <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>
    ///     The <see cref="Sprite"/> that is created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TextureRegion"/> elements in the <see cref="TextureAtlas"/>.
    /// </exception>
    public Sprite CreateSprite(string spriteName, int regionIndex)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionIndex);
        Sprite sprite = new(spriteName, region);
        return sprite;
    }

    /// <summary>
    ///     Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> at the specified index in the
    ///     <see cref="TextureAtlas"/> of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the <see cref="TextureRegion"/> element to assign the <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>The <see cref="Sprite"/> that is created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than or equal to the total number of
    ///     <see cref="TextureRegion"/> elements in the <see cref="TextureAtlas"/>.
    /// </exception>
    public Sprite CreateSprite(int regionIndex)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionIndex);
        Sprite sprite = new(region.Name, region);
        return sprite;
    }

    /// <summary>
    ///     Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> at the specified index in the
    ///     <see cref="TextureAtlas"/> of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="spriteName">
    ///     The name to assign the <see cref="Sprite"/> that is created.
    /// </param>
    /// <param name="regionName">
    ///     The name of the <see cref="TextureRegion"/> element in the <see cref="TextureAtlas"/> assign the 
    ///     <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>
    ///     The <see cref="Sprite"/> that is created by this method.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if the <see cref="TextureAtlas"/> does not contain a <see cref="TextureRegion"/> with the name 
    ///     specified.
    /// </exception>
    public Sprite CreateSprite(string spriteName, string regionName)
    {
        TextureRegion region = TextureAtlas.GetRegion(regionName);
        Sprite sprite = new(spriteName, region);
        return sprite;
    }

    /// <summary>
    ///     Creates a new <see cref="Sprite"/> from the <see cref="TextureRegion"/> at the specified index in the
    ///     <see cref="TextureAtlas"/> of this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the <see cref="TextureRegion"/> element in the <see cref="TextureAtlas"/> assign the 
    ///     <see cref="Sprite"/> that is created.
    /// </param>
    /// <returns>The <see cref="Sprite"/> that is created by this method.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if the <see cref="TextureAtlas"/> does not contain a <see cref="TextureRegion"/> with the name 
    ///     specified.
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
    ///     Creates a new <see cref="AnimationTag"/> and adds it to this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name to assign the <see cref="AnimationTag"/> that is created by this method.  This name must be unique
    ///     across all <see cref="AnimationTag"/> elements defined in this <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="builder">
    ///     An <see cref="Action"/> method used to build the <see cref="AnimationTag"/> with an 
    ///     <see cref="AnimationTagBuilder"/>.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimationTag"/> that is created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if this <see cref="SpriteSheet"/> already contains an <see cref="AnimationTag"/> element with the 
    ///     name specified.
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
    ///     Gets the <see cref="AnimationTag"/> element with the specified name in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="AnimationTag"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimationTag"/> that was located.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain an <see cref="AnimationTag"/> element with the
    ///     specified name.
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
    ///     Gets the <see cref="AnimationTag"/> element with the specified name in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="AnimationTag"/> to locate.
    /// </param>
    /// <param name="tag">
    ///     When this method returns <see langword="true"/>, contains the <see cref="AnimationTag"/> located; otherwise, 
    ///     <see langword="null"/>
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="AnimationTag"/> was located; otherwise, <see langword="false"/>.  
    ///     This method returns <see langword="false"/> if this <see cref="SpriteSheet"/> does not contain an 
    ///     <see cref="AnimationTag"/> element with the specified name.
    /// </returns>
    public bool TryGetAnimationTag(string name, [NotNullWhen(true)] out AnimationTag? tag) =>
        _animationTagLookup.TryGetValue(name, out tag);

    /// <summary>
    ///     Returns a new <see cref="List{T}"/> containing the name of all <see cref="AnimationTag"/> elements that
    ///     have been defined in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <returns>
    ///     A new <see cref="List{T}"/> containing the name of all <see cref="AnimationTag"/> elements that have been 
    ///     defined in this <see cref="SpriteSheet"/>.
    /// </returns>
    public List<string> GetAnimationTagNames() => _animationTagLookup.Keys.ToList();

    /// <summary>
    ///     Returns a value that indicates whether this <see cref="SpriteSheet"/> contains an <see cref="AnimationTag"/> 
    ///     with the specified name.
    /// </summary>
    /// <param name="name">
    ///     The name fo the <see cref="AnimationTag"/> element to locate.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this <see cref="SpriteSheet"/> contains an <see cref="AnimationTag"/> with the 
    ///     specified name; otherwise, <see langword="false"/>.
    /// </returns>
    public bool ContainsAnimationTag(string name) => _animationTagLookup.ContainsKey(name);

    /// <summary>
    ///     Removes the <see cref="AnimationTag"/> element with the specified name from this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="name">
    ///     The name of the <see cref="AnimationTag"/> element to remove from this <see cref="SpriteSheet"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="AnimationTag"/> element  was successfully removed from this 
    ///     <see cref="SpriteSheet"/>; otherwise, <see langword="false"/>.  This method returns <see langword="false"/>
    ///     if this <see cref="SpriteSheet"/> does not contain an <see cref="AnimationTag"/> element with the specified
    ///     name.
    /// </returns>
    public bool RemoveAnimationTag(string name) => _animationTagLookup.Remove(name);

    /// <summary>
    ///     Creates a new <see cref="AnimatedSprite"/> using the <see cref="AnimationTag"/> element with the specified
    ///     name in this <see cref="SpriteSheet"/>.
    /// </summary>
    /// <param name="tagName">
    ///     The name of the <see cref="AnimationTag"/> element in this <see cref="SpriteSheet"/> to create the 
    ///     <see cref="AnimatedSprite"/> with.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimatedSprite"/> that is created by this method.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if this <see cref="SpriteSheet"/> does not contain an <see cref="AnimationTag"/> element with the 
    ///     specified name.
    /// </exception>
    public AnimatedSprite CreateAnimatedSprite(string tagName)
    {
        AnimationTag tag = GetAnimationTag(tagName);
        AnimatedSprite sprite = new(tag);
        return sprite;
    }

    #endregion Animations
}
