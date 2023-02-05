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
/// Defines the definition of an animation.
/// </summary>
public sealed class AnimationTag
{
    private AnimationFrame[] _frames;

    /// <summary>
    ///     Gets the name of the animation
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets a read-only span of the <see cref="AnimationFrame"/> elements that make up the animation.  The order of
    ///     elements is from first frame to last frame in non-reverse order.
    /// </summary>
    public ReadOnlySpan<AnimationFrame> Frames => _frames;

    /// <summary>
    ///     Gets the total number of ,<see cref="AnimationFrame"/> elements.
    /// </summary>
    public int FrameCount => _frames.Length;

    /// <summary>
    ///     Gets the <see cref="AnimationFrame"/> element at the specified index from this <see cref="AnimationTag"/>.
    /// </summary>
    /// <param name="index">The index of the <see cref="AnimationFrame"/> to locate.</param>
    /// <returns>The <see cref="AnimationFrame"/> located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero or is greater than or equal to the total
    ///     number of <see cref="AnimationFrame"/> elements in this <see cref="AnimationTag"/>.
    /// </exception>
    public AnimationFrame this[int index] => GetFrame(index);

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation should loop.
    /// </summary>
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation should play in reverse.
    /// </summary>
    public bool IsReversed { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation should ping-pong once reaching the last frame of
    ///     animation.
    /// </summary>
    public bool IsPingPong { get; set; }

    internal AnimationTag(string name, AnimationFrame[] frames, bool isLooping, bool isReversed, bool isPingPong) =>
        (Name, _frames, IsLooping, IsReversed, IsPingPong) = (name, frames, isLooping, isReversed, isPingPong);

    /// <summary>
    ///     Gets the <see cref="AnimationFrame"/> element at the specified index from this <see cref="AnimationTag"/>.
    /// </summary>
    /// <param name="index">The index of the <see cref="AnimationFrame"/> to locate.</param>
    /// <returns>The <see cref="AnimationFrame"/> located.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero or is greater than or equal to the total
    ///     number of <see cref="AnimationFrame"/> elements in this <see cref="AnimationTag"/>.
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
