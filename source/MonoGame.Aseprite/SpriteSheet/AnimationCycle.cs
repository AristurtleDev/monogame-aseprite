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
///     Represents the cycle of an animation including the frames and how it
///     loops.
/// </summary>
public class AnimationCycle
{
    /// <summary>
    ///     The name of this animation cycle.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     The animation frames that make up this animation cycle. The order
    ///     of frames is from start to end.
    /// </summary>
    public AnimationFrame[] Frames { get; }

    /// <summary>
    ///     Get or Sets whether this animation cycle should loop.
    /// </summary>
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or Sets whether this animation cycle should have the
    ///     frames played in reverse order.
    /// </summary>
    public bool IsReversed { get; set; }

    /// <summary>
    ///     Gets or Sets whether this animation cycle should ping-pong once
    ///     reaching the end of the cycle.
    /// </summary>
    public bool IsPingPong { get; set; }

    internal AnimationCycle(string name, AnimationFrame[] frames, bool isLooping, bool isReversed, bool isPingPong) =>
        (Name, Frames, IsLooping, IsReversed, IsPingPong) = (name, frames, isLooping, isReversed, isPingPong);
}
