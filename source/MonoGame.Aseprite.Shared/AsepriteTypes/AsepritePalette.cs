/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents the palette of an Aseprite file.
/// </summary>
public sealed class AsepritePalette
{
    private Color[] _colors;

    /// <summary>
    ///     The index of the color value in this palette that represents
    ///     a transparent color.
    /// </summary>
    /// <remarks>
    ///     This value is only valid when the color depth used by the Aseprite
    ///     file is <see cref="ColorDepth.Index"/>.
    /// </remarks>
    public int TransparentIndex { get; internal set; }

    /// <summary>
    ///     A read-only collection of the color values elements in this palette.
    /// </summary>
    public ReadOnlyCollection<Color> Colors => Array.AsReadOnly<Color>(_colors);

    /// <summary>
    ///     The total number of color values in this palette.
    /// </summary>
    public int ColorCount => _colors.Length;

    /// <summary>
    ///     Returns the color value at the specified index from this palette.
    /// </summary>
    /// <param name="index">
    ///     The index of the color value to return.
    /// </param>
    /// <returns>
    ///     The color value at the specified index from this palette.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of color values in this palette.
    /// </exception>
    internal Color this[int index]
    {
        get
        {
            if (index < 0 || index >= ColorCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _colors[index];
        }
    }

    internal AsepritePalette(Color[] colors, int transparentIndex)
    {
        _colors = colors;
        TransparentIndex = transparentIndex;
    }
}
