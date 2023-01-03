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
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteImageCel : AsepriteCel
{
    internal Point Size { get; }
    internal Color[] Pixels { get; }
    internal int PixelCount => Pixels.Length;

    internal Color this[int index]
    {
        get
        {
            if (index < 0 || index >= PixelCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return Pixels[index];
        }
    }

    internal AsepriteImageCel(Point size, Color[] pixels, AsepriteLayer layer, Point position, int opacity)
        : base(layer, position, opacity)
    {
        Size = size;
        Pixels = pixels;
    }
}
