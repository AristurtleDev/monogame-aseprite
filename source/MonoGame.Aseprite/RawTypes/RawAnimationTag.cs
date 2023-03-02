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

namespace MonoGame.Aseprite.RawTypes;

/// <summary>
///     Defines a class that represents the raw values of an animation tag.
/// </summary>
public sealed class RawAnimationTag : IEquatable<RawAnimationTag>
{
    private RawAnimationFrame[] _rawAnimationFrames;

    /// <summary>
    ///     Gets the name assigned to the animation tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets a read-only span of the <see cref="RawAnimationFrame"/> elements that represent the frames of animation
    ///     for the animation tag.
    /// </summary>
    public ReadOnlySpan<RawAnimationFrame> RawAnimationFrames => _rawAnimationFrames;

    /// <summary>
    ///     Gets a value that indicates the total number of loops/cycles of this animation that should play.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <c>0</c> = infinite looping
    ///     </para>
    ///     <para>
    ///         If <see cref="AnimationTag.IsPingPong"/> is equal to <see langword="true"/>, each direction of the
    ///         ping-pong will count as a loop.  
    ///     </para>
    /// </remarks>
    public int LoopCount { get; }

    /// <summary>
    ///     ets a value that indicates whether the animation defined by the animation tag should play in reverse.
    /// </summary>
    public bool IsReversed { get; }

    /// <summary>
    ///     Gets a value that indicates whether the animation defined by the animation tag should ping-pong once 
    ///     reaching the last frame of animation.
    /// </summary>
    public bool IsPingPong { get; }

    internal RawAnimationTag(string name, RawAnimationFrame[] rawAnimationFrames, int loopCount, bool isReversed, bool isPingPong) =>
        (Name, _rawAnimationFrames, LoopCount, IsReversed, IsPingPong) = (name, rawAnimationFrames, loopCount, isReversed, isPingPong);

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawAnimationTag"/> is equal to this
    ///     <see cref="RawAnimationTag"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawAnimationTag"/> to check for equality with this <see cref="RawAnimationTag"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawAnimationTag"/> is equal to this 
    ///     <see cref="RawAnimationTag"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawAnimationTag? other) => other is not null
                                                  && Name == other.Name
                                                  && RawAnimationFrames.SequenceEqual(other.RawAnimationFrames)
                                                  && LoopCount == other.LoopCount
                                                  && IsReversed == other.IsReversed
                                                  && IsPingPong == other.IsPingPong;
}
