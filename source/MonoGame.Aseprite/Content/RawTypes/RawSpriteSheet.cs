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
/// Defines a class that represents the raw values of a spritesheet.
/// </summary>
public sealed class RawSpriteSheet : IEquatable<RawSpriteSheet>
{
    private RawAnimationTag[] _rawAnimationTags;

    /// <summary>
    /// Gets the name assigned to the spritesheet represented by this raw spritesheet record.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets raw texture atlas that represents the source texture atlas for the spritesheet represented by this raw
    /// spritesheet record.
    /// </summary>
    public RawTextureAtlas RawTextureAtlas { get; }

    /// <summary>
    /// Gets a read-only span of the raw animation tags that represent the animations tags for the spritesheet
    /// represented by this raw spritesheet record.
    /// </summary>
    public ReadOnlySpan<RawAnimationTag> RawAnimationTags => _rawAnimationTags;

    internal RawSpriteSheet(string name, RawTextureAtlas rawAtlas, RawAnimationTag[] rawAnimationTags) =>
        (Name, RawTextureAtlas, _rawAnimationTags) = (name, rawAtlas, rawAnimationTags);

    public bool Equals(RawSpriteSheet? other) => other is not null
                                                 && Name == other.Name
                                                 && RawTextureAtlas.Equals(other.RawTextureAtlas)
                                                 && RawAnimationTags.SequenceEqual(other.RawAnimationTags);
}
