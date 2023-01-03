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
///     Represents a cel that contains image data on a frame in an Aseprite
///     image.
/// </summary>
public sealed class AsepriteImageCel : AsepriteCel
{
    private Color[] _pixels;

    /// <summary>
    ///     The width and heigh extents, in pixels, of this image cel.
    /// </summary>
    public Point Size { get; }

    /// <summary>
    ///     The width, in pixels, of this image cel.
    /// </summary>
    public int Width => Size.X;

    /// <summary>
    ///     The height, in pixels, of this image cel.
    /// </summary>
    public int Height => Size.Y;

    /// <summary>
    ///     A read-only collection of the pixel data for this image cel. Pixels
    ///     are in order of top-to-bottom, read left-to-right.
    /// </summary>
    public ReadOnlyCollection<Color> Pixels => Array.AsReadOnly<Color>(_pixels);

    /// <summary>
    ///     The total number of pixels in this image cel.
    /// </summary>
    public int PixelCount => _pixels.Length;

    /// <summary>
    ///     Returns the color value of the pixel in this image cel at the
    ///     specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the pixel in this image cel.
    /// </param>
    /// <returns>
    ///     The color value of the pixel in this image at the specified index.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified index is less than zero or is greater than
    ///     or equal to the total number of pixels in this image cel.
    /// </exception>
    internal Color this[int index]
    {
        get
        {
            if (index < 0 || index >= PixelCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _pixels[index];
        }
    }

    internal AsepriteImageCel(Point size, Color[] pixels, AsepriteLayer layer, Point position, int opacity)
        : base(layer, position, opacity)
    {
        Size = size;
        _pixels = pixels;
    }
}
