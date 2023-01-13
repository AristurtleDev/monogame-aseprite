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
///     Used to build an animation cycle.
/// </summary>
public sealed class AnimationCycleBuilder
{
    private string _name;
    private List<AnimationFrame> _frames = new();
    private SpriteSheet _spriteSheet;
    private bool _isLooping = true;
    private bool _isReversed = false;
    private bool _isPingPong = false;

    internal AnimationCycleBuilder(string name, SpriteSheet spriteSheet)
    {
        _name = name;
        _spriteSheet = spriteSheet;
    }

    /// <summary>
    ///     Adds a new texture region from the spritesheet as the next frame
    ///     in the animation cycle.
    /// </summary>
    /// <param name="regionIndex">
    ///     The index of the texture region in the spritesheet to use as the
    ///     next frame in the animation cycle.
    /// </param>
    /// <param name="duration">
    ///     The duration of the frame that is created.
    /// </param>
    /// <returns>
    ///     This instance of the animation cycle builder.
    /// </returns>
    public AnimationCycleBuilder AddFrame(int regionIndex, TimeSpan duration)
    {
        AnimationFrame frame = new(_spriteSheet.GetRegion(regionIndex), duration);
        _frames.Add(frame);
        return this;
    }

    /// <summary>
    ///     Adds a new frame in the animation cycle using the texture region
    ///     from the spritesheet with the name specified.
    /// </summary>
    /// <param name="name">
    ///     The name of the texture region in the spritesheet to use as the
    ///     next frame in the animation cycle.
    /// </param>
    /// <param name="duration">
    ///     The duration of the frame that is created.
    /// </param>
    /// <returns>
    ///     This instance of the animation cycle builder.
    /// </returns>
    public AnimationCycleBuilder AddFrame(string name, TimeSpan duration)
    {
        AnimationFrame frame = new(_spriteSheet.GetRegion(name), duration);
        _frames.Add(frame);
        return this;
    }

    /// <summary>
    ///     Sets whether the animation cycle should loop.
    /// </summary>
    /// <param name="isLooping">
    ///     A value indicating whether the animation cycle should loop.
    /// </param>
    /// <returns>
    ///     This instance of the animation cycle builder.
    /// </returns>
    public AnimationCycleBuilder IsLooping(bool isLooping)
    {
        _isLooping = isLooping;
        return this;
    }

    /// <summary>
    ///     Sets whether the animation cycle should have the frames played in
    ///     reverse order.
    /// </summary>
    /// <param name="isReversed">
    ///     A value indicating whether the animation cycle should have the
    ///     frames played in reverse order.
    /// </param>
    /// <returns>
    ///     This instance of the animation cycle builder.
    /// </returns>
    public AnimationCycleBuilder IsReversed(bool isReversed)
    {
        _isReversed = isReversed;
        return this;
    }

    /// <summary>
    ///     Sets whether the animation cycle should ping-pong once reaching the
    ///     end of the cycle.
    /// </summary>
    /// <param name="isPingPong">
    ///     A value that indicates whether the animation cycle should ping-pong
    ///     once reaching the end of the cycle.
    /// </param>
    /// <returns>
    ///     This instance of the animation cycle builder.
    /// </returns>
    public AnimationCycleBuilder IsPingPong(bool isPingPong)
    {
        _isPingPong = isPingPong;
        return this;
    }

    internal AnimationCycle Build()
    {
        AnimationCycle animation = new(_name, _frames.ToArray(), _isLooping, _isReversed, _isPingPong);
        return animation;
    }
}
