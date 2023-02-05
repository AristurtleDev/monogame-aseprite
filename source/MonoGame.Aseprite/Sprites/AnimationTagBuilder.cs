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

namespace MonoGame.Aseprite.Sprites;

/// <summary>
///     Defines a builder building an <see cref="AnimationTag"/> for a <see cref="SpriteSheet"/>.
/// </summary>
public sealed class AnimationTagBuilder
{
    private string _name;
    private List<AnimationFrame> _frames = new();
    private SpriteSheet _spriteSheet;
    private bool _isLooping = true;
    private bool _isReversed = false;
    private bool _isPingPong = false;

    internal AnimationTagBuilder(string name, SpriteSheet spriteSheet) =>
        (_name, _spriteSheet) = (name, spriteSheet);

    /// <summary>
    ///     Adds a new frame of animation to the <see cref="AnimationTag"/> using the <see cref="TextureRegion"/>
    ///     located at the specified index in the <see cref="TextureAtlas"/> of the <see cref="SpriteSheet"/> and with 
    ///     the specified duration.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the source <see cref="TextureRegion"/> in the <see cref="TextureAtlas"/> of the 
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="duration">The duration of the frame of animation.</param>
    /// <returns>This instance of the <see cref="AnimationTagBuilder"/> class.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Throw if the specified index is less than zero or is greater than or equal to the total number of regions in
    ///     the <see cref="TextureAtlas"/>.
    /// </exception>
    public AnimationTagBuilder AddFrame(int regionIndex, TimeSpan duration)
    {
        TextureRegion region = _spriteSheet.TextureAtlas.GetRegion(regionIndex);
        AnimationFrame frame = new(regionIndex, region, duration);
        _frames.Add(frame);
        return this;
    }

    /// <summary>
    ///     Adds a new frame of animation to the <see cref="AnimationTag"/> using the <see cref="TextureRegion"/> with
    ///     the specified name in the <see cref="TextureAtlas"/> of the <see cref="SpriteSheet"/> and with the specified
    ///     duration.
    /// </summary>
    /// <param name="regionName">
    ///     The name of the source <see cref="TextureRegion"/> in the <see cref="TextureAtlas"/> of the 
    ///     <see cref="SpriteSheet"/>.
    /// </param>
    /// <param name="duration">The duration of the frame of animation.</param>
    /// <returns>This instance of the <see cref="AnimationTagBuilder"/> class.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if the <see cref="TextureAtlas"/> of the <see cref="SpriteSheet"/> does not contain a 
    ///     <see cref="TextureRegion"/> with the specified name.
    /// </exception>
    public AnimationTagBuilder AddFrame(string regionName, TimeSpan duration)
    {
        TextureRegion region = _spriteSheet.TextureAtlas.GetRegion(regionName);
        int index = _spriteSheet.TextureAtlas.GetIndexOfRegion(regionName);
        AnimationFrame frame = new(index, region, duration);
        _frames.Add(frame);
        return this;
    }

    /// <summary>
    ///     Sets whether the animation should loop.
    /// </summary>
    /// <param name="isLooping">A value that indicates whether the animation should loop.</param>
    /// <returns>This instance of the <see cref="AnimationTagBuilder"/> class.</returns>
    public AnimationTagBuilder IsLooping(bool isLooping)
    {
        _isLooping = isLooping;
        return this;
    }

    /// <summary>
    ///     Sets whether the animation should play in reverse.
    /// </summary>
    /// <param name="isReversed">A value that indicates whether the animation should play in reverse.</param>
    /// <returns>This instance of the <see cref="AnimationTagBuilder"/> class.</returns>
    public AnimationTagBuilder IsReversed(bool isReversed)
    {
        _isReversed = isReversed;
        return this;
    }

    /// <summary>
    ///     Sets whether the animation should ping-pong once reaching the last frame of animation.
    /// </summary>
    /// <param name="isPingPong">A value that indicates whether the animation should ping-pong.</param>
    /// <returns>This instance of the <see cref="AnimationTagBuilder"/> class.</returns>
    public AnimationTagBuilder IsPingPong(bool isPingPong)
    {
        _isPingPong = isPingPong;
        return this;
    }

    internal AnimationTag Build()
    {
        AnimationTag tag = new(_name, _frames.ToArray(), _isLooping, _isReversed, _isPingPong);
        return tag;
    }
}
