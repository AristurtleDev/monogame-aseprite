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

using System.Diagnostics.CodeAnalysis;

namespace MonoGame.Aseprite.Sprites;

/// <summary>
///     Defines the source texture region and the duration of a single frame of animation in an animation tag.
/// </summary>
public sealed class AnimationFrame : IDisposable
{
    private TextureRegion? _textureRegion;

    /// <summary>
    ///     Gets the source texture region to render during this frame of animation.
    /// </summary>
    public TextureRegion TextureRegion
    {
        get
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(AnimationFrame), $"This {nameof(AnimationFrame)} was previously disposed");
            }

            return _textureRegion;
        }
    }

    /// <summary>
    ///     Gets the duration of this frame of animation.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    ///     Gets a value that indicates if this animation frame has been disposed of.
    /// </summary>
    [MemberNotNullWhen(false, nameof(_textureRegion))]
    public bool IsDisposed { get; private set; }

    internal AnimationFrame(TextureRegion region, TimeSpan duration) =>
        (_textureRegion, Duration) = (region, duration);

    ~AnimationFrame() => Dispose();

    /// <summary>
    ///     Releases resources held by this instance.
    /// </summary>
    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _textureRegion = null;
        IsDisposed = true;
    }
}
