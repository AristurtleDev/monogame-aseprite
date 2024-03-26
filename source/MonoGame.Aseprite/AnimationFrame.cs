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
///     Defines the source <see cref="TextureRegion"/> and duration of a single frame of animation in an
///     <see cref="AnimationTag"/>.
/// </summary>
public sealed class AnimationFrame
{
    /// <summary>
    ///     Gets the index of the source <see cref="TextureRegion"/> in the <see cref="TextureAtlas"/> of the 
    ///     <see cref="SpriteSheet"/>.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    ///     Gets the source <see cref="TextureRegion"/> for this <see cref="AnimationFrame"/>.
    /// </summary>
    public TextureRegion TextureRegion { get; }

    /// <summary>
    ///     Gets the duration of this <see cref="AnimationFrame"/>.
    /// </summary>
    public TimeSpan Duration { get; }

    internal AnimationFrame(int frameIndex, TextureRegion textureRegion, TimeSpan duration) =>
        (FrameIndex, TextureRegion, Duration) = (frameIndex, textureRegion, duration);
}
