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

using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.RawTypes;

/// <summary>
///     Defines a class that represents the raw values of a texture.
/// </summary>
public sealed class RawTexture : IEquatable<RawTexture>
{
    private Color[] _pixels;

    /// <summary>
    ///     Get the name assigned to the texture.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets a read-only span of the color values that represent the pixel data for the texture.
    /// </summary>
    public ReadOnlySpan<Color> Pixels => _pixels;

    /// <summary>
    ///     Gets the width, in pixels of the texture.
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Gets the height, in pixels of the texture.
    /// </summary>
    public int Height { get; }

    internal RawTexture(string name, Color[] pixels, int width, int height) =>
        (Name, _pixels, Width, Height) = (name, pixels, width, height);

    /// <summary>
    ///     Returns a value that indicates if the given <see cref="RawTexture"/> is equal to this
    ///     <see cref="RawTexture"/>.
    /// </summary>
    /// <param name="other">
    ///     The other <see cref="RawTexture"/> to check for equality with this <see cref="RawTexture"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the given <see cref="RawTexture"/> is equal to this 
    ///     <see cref="RawTexture"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Equals(RawTexture? other) => other is not null
                                             && Name == other.Name
                                             && Pixels.SequenceEqual(other.Pixels)
                                             && Width == other.Width
                                             && Height == other.Height;
}
