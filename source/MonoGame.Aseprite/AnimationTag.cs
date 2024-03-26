// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Aseprite;

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
    /// <param name="index">
    ///     The index of the <see cref="AnimationFrame"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimationFrame"/> located.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified <paramref name="index"/> is less than zero or is greater than or equal to the total
    ///     number of <see cref="AnimationFrame"/> elements in this <see cref="AnimationTag"/>.
    /// </exception>
    public AnimationFrame this[int index] => GetFrame(index);

    /// <summary>
    ///     Gets a value that indicates whether the animation should loop.
    /// </summary>
    public bool IsLooping => LoopCount == 0;

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation should play in reverse.
    /// </summary>
    public bool IsReversed { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation should ping-pong once reaching the last frame of
    ///     animation.
    /// </summary>
    public bool IsPingPong { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates the total number of loops/cycles of this animation that should play.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <c>0</c> = infinite looping
    ///     </para>
    ///     <para>
    ///         If <see cref="IsPingPong"/> is equal to <see langword="true"/>, each direction of the
    ///         ping-pong will count as a loop.  
    ///     </para>
    /// </remarks>
    public int LoopCount { get; set; }

    internal AnimationTag(string name, AnimationFrame[] frames, int loopCount, bool isReversed, bool isPingPong) =>
        (Name, _frames, LoopCount, IsReversed, IsPingPong) = (name, frames, loopCount, isReversed, isPingPong);

    /// <summary>
    ///     Gets the <see cref="AnimationFrame"/> element at the specified index from this <see cref="AnimationTag"/>.
    /// </summary>
    /// <param name="index">
    ///     The index of the <see cref="AnimationFrame"/> to locate.
    /// </param>
    /// <returns>
    ///     The <see cref="AnimationFrame"/> located.
    /// </returns>
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
