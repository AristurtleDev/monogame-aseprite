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

namespace MonoGame.Aseprite.Content.RawTypes;

/// <summary>
/// Defines a record that represents the raw values of an animation tag.
/// </summary>
public sealed record RawAnimationTag
{
    private RawAnimationFrame[] _rawAnimationFrames;

    /// <summary>
    /// Gets the name assigned to the animation tag represented by this raw animation tag record.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a read-only span of the raw animation frames that represent the frames of animation for the animation tag
    /// represented by this raw animation tag record.
    /// </summary>
    public ReadOnlySpan<RawAnimationFrame> RawAnimationFrames => _rawAnimationFrames;

    /// <summary>
    /// Gets a value that indicates whether the animation defined by the animation tag represented by this raw animation
    /// tag record should loop.
    /// </summary>
    public bool IsLooping { get; }

    /// <summary>
    /// Gets a value that indicates whether the animation defined by the animation tag represented by this raw animation
    /// tag record should play in reverse.
    /// </summary>
    public bool IsReversed { get; }

    /// <summary>
    /// Gets a value that indicates whether the animation defined by the animation tag represented by this raw animation
    /// tag record should ping-pong once reaching the last frame of animation.
    /// </summary>
    public bool IsPingPong { get; }

    internal RawAnimationTag(string name, RawAnimationFrame[] rawAnimationFrames, bool isLooping, bool isReversed, bool isPingPong) =>
        (Name, _rawAnimationFrames, IsLooping, IsReversed, IsPingPong) = (name, rawAnimationFrames, isLooping, isReversed, isPingPong);
}
