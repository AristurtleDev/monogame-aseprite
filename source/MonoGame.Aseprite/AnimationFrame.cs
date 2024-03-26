// Copyright (c) Christopher Whitley. All rights reserved.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.

namespace MonoGame.Aseprite;

/// <summary>
/// Defines the source <see cref="TextureRegion"/> and duration of a single frame of animation in an 
/// <see cref="AnimationTag"/>.
/// </summary>
public sealed class AnimationFrame
{
    /// <summary>
    /// Gets the index of the source <see cref="TextureRegion"/> in the <see cref="TextureAtlas"/> of the 
    /// <see cref="SpriteSheet"/>.
    /// </summary>
    public int FrameIndex { get; }

    /// <summary>
    /// Gets the source <see cref="TextureRegion"/> for this <see cref="AnimationFrame"/>.
    /// </summary>
    public TextureRegion TextureRegion { get; }

    /// <summary>
    /// Gets the duration of this <see cref="AnimationFrame"/>.
    /// </summary>
    public TimeSpan Duration { get; }

    internal AnimationFrame(int frameIndex, TextureRegion textureRegion, TimeSpan duration) =>
        (FrameIndex, TextureRegion, Duration) = (frameIndex, textureRegion, duration);
}
