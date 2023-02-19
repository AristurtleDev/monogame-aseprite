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
///     Defines a class that represents the raw values of a texture atlas.
/// </summary>
public sealed class RawTextureAtlas : IEquatable<RawTextureAtlas>
{
    private RawTextureRegion[] _rawTextureRegions;

    /// <summary>
    ///     Gets the name assigned to the texture atlas.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the raw texture that represents the source texture of the texture atlas.
    /// </summary>
    public RawTexture RawTexture { get; }

    /// <summary>
    ///     Gets a read-only span of the raw texture regions that represent the texture regions for the texture atlas.
    /// </summary>
    public ReadOnlySpan<RawTextureRegion> RawTextureRegions => _rawTextureRegions;

    internal RawTextureAtlas(string name, RawTexture rawTexture, RawTextureRegion[] rawRegions) =>
        (Name, RawTexture, _rawTextureRegions) = (name, rawTexture, rawRegions);

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawTextureAtlas"/> is equal to this
    ///     <see cref="RawTextureAtlas"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawTextureAtlas"/> to check for equality with this <see cref="RawTextureAtlas"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawTextureAtlas"/> is equal to this 
    ///     <see cref="RawTextureAtlas"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawTextureAtlas? other) => other is not null
                                                  && Name == other.Name
                                                  && RawTexture.Equals(other.RawTexture)
                                                  && RawTextureRegions.SequenceEqual(other.RawTextureRegions);
}
