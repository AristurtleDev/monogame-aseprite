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

namespace MonoGame.Aseprite;

/// <summary>
/// Defines a builder for building an animation tag for a spritesheet.
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
    /// Adds a new frame of animation to the animation tag using the texture region located at the specified index in
    /// the spritesheet with the specified duration.
    /// </summary>
    /// <param name="regionIndex">
    /// The index of the texture region in the spritesheet to use for the source image of the animation frame.
    /// </param>
    /// <param name="duration">The duration of the animation frame.</param>
    /// <returns>This instance of the animation tag builder.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified texture region index is less than zero or is greater than or equal to the total number
    /// of texture regions in the texture atlas used by the spritesheet this animation tag is being built for.
    /// </exception>
    public AnimationTagBuilder AddFrame(int regionIndex, TimeSpan duration)
    {
        TextureRegion region = _spriteSheet.TextureAtlas.GetRegion(regionIndex);
        AnimationFrame frame = new(region, duration);
        _frames.Add(frame);
        return this;
    }

    /// <summary>
    /// Adds a new frame of animation to the animation tag using the texture region with the specified name from the
    /// spritesheet with the specified duration.
    /// </summary>
    /// <param name="regionName">
    /// The name of the texture region in the spritesheet ot use for the source image fo the animation frame.
    /// </param>
    /// <param name="duration">The duration of the animation frame.</param>
    /// <returns>This instance of the animation tag builder.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if the texture atlas used by the spritesheet this animation tag is being built for does not contain a
    /// texture region with the specified name.
    /// </exception>
    public AnimationTagBuilder AddFrame(string regionName, TimeSpan duration)
    {
        TextureRegion region = _spriteSheet.TextureAtlas.GetRegion(regionName);
        AnimationFrame frame = new(region, duration);
        _frames.Add(frame);
        return this;
    }

    /// <summary>
    /// Sets whether the animation defined by the animation tag being built should loop.
    /// </summary>
    /// <param name="isLooping">Indicates whether the animation should loop.</param>
    /// <returns>This instance of the animation tag builder.</returns>
    public AnimationTagBuilder IsLooping(bool isLooping)
    {
        _isLooping = isLooping;
        return this;
    }

    /// <summary>
    /// Sets whether the animation defined by the animation tag being built should have the frames played in reverse
    /// order.
    /// </summary>
    /// <param name="isReversed">
    /// Indicates whether the frames of the animation should be played in reverse order.
    /// </param>
    /// <returns>This instance of the animation tag builder.</returns>
    public AnimationTagBuilder IsReversed(bool isReversed)
    {
        _isReversed = isReversed;
        return this;
    }

    /// <summary>
    /// Sets whether the animation defined by the animation tag being built should ping-pong once reaching the last
    /// frame of animation.
    /// </summary>
    /// <param name="isPingPong">
    /// Indicates whether the animation should ping-pong once reaching the last frame of animation.
    /// </param>
    /// <returns>This instance of the animation tag builder.</returns>
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
