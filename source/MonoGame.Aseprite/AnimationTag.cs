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

public sealed class AnimationTag : IDisposable
{
    private AnimationFrame[] _frames;

    /// <summary>
    ///     Gets the name of this animation tag.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets a read-only span of the frames of animation that make up the animation defined by this animation tag.
    ///     The order of frames in the collection are from the first frame to the last frame.
    /// </summary>
    public ReadOnlySpan<AnimationFrame> Frames => _frames;

    /// <summary>
    ///     Get or Sets a value that indicates whether the animation defined by this animation tag should loop.
    /// </summary>
    public bool IsLooping { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation defined by this animation tag should play the
    ///     frames in reverse order.
    /// </summary>
    public bool IsReversed { get; set; }

    /// <summary>
    ///     Gets or Sets a value that indicates whether the animation defined by this animation tag should ping-pong
    ///     once reaching the last frame of animation.
    /// </summary>
    public bool IsPingPong { get; set; }

    /// <summary>
    ///     Gets a value that indicates whether this instance has been disposed of.
    /// </summary>
    public bool IsDisposed { get; private set; }

    internal AnimationTag(string name, AnimationFrame[] frames, bool isLooping, bool isReversed, bool isPingPong) =>
        (Name, _frames, IsLooping, IsReversed, IsPingPong) = (name, frames, isLooping, isReversed, isPingPong);

    ~AnimationTag() => Dispose();

    /// <summary>
    ///     Releases resources held by this instance.
    /// </summary>
    public void Dispose()
    {
        if(IsDisposed)
        {
            return;
        }

        for (int i = 0; i < _frames.Length; i++)
        {
            _frames[i].Dispose();
        }

        IsDisposed = true;
    }
}
