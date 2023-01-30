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

using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Sprites;

/// <summary>
/// Defines an animation tag that represents an the definition of an animation.
/// </summary>
public sealed class AnimationTag
{
    private AnimationFrame[] _frames;

    /// <summary>
    /// Gets the name of this animation tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only span of the frames of animations that make up the animation defined by this animation tag.  The
    /// order of frames in the collection are from first frame to last frame in non-reverse order, even if IsReversed is
    /// set to true.
    /// </summary>
    public ReadOnlySpan<AnimationFrame> Frames => _frames;

    /// <summary>
    /// Gets the total number of frames of animation for the animation defined by this animation tag.
    /// </summary>
    public int FrameCount => _frames.Length;

    /// <summary>
    /// Gets the animation frame at the specified index from this animation tag.
    /// </summary>
    /// <param name="index">The index of the animation frame to locate.</param>
    /// <returns>The animation frame located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of animation
    /// frames in this animation tag.
    /// </exception>
    public AnimationFrame this[int index] => GetFrame(index);

    /// <summary>
    /// Gets or Sets a value that indicates whether the animation defined by this animation tag should loop.
    /// </summary>
    public bool IsLooping { get; set; }

    /// <summary>
    /// Gets or Sets a value that indicates whether the animation defined by this animation tag should play the frames
    /// in reverse order.
    /// </summary>
    public bool IsReversed { get; set; }

    /// <summary>
    /// Gets or Sets a value that indicates whether the animation defined by this animation tag should ping-pong once
    /// reaching the last frame of animation.
    /// </summary>
    public bool IsPingPong { get; set; }

    internal AnimationTag(string name, AnimationFrame[] frames, bool isLooping, bool isReversed, bool isPingPong) =>
        (Name, _frames, IsLooping, IsReversed, IsPingPong) = (name, frames, isLooping, isReversed, isPingPong);

    /// <summary>
    /// Gets the animation frame at the specified index from this animation tag.
    /// </summary>
    /// <param name="index">The index of the animation frame to locate.</param>
    /// <returns>The animation frame located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the index specified is less than zero or is greater than or equal to the total number of animation
    /// frames in this animation tag.
    /// </exception>
    public AnimationFrame GetFrame(int index)
    {
        if (index < 0 || index >= FrameCount)
        {
            ArgumentOutOfRangeException ex = new(nameof(index), $"{nameof(index)} cannot be less than zero or greater than or equal to the total number of animation frames in this animation tag.");
            ex.Data.Add(nameof(index), index);
            ex.Data.Add(nameof(FrameCount), FrameCount);
            throw ex;
        }

        return _frames[index];
    }
}
