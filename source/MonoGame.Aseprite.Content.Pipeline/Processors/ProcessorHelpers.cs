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
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

internal static class ProcessorHelpers
{
    // internal static Texture2DContent CreateTextureContent(Color[] pixels, int width, int height)
    // {
    //     PixelBitmapContent<Color> face = new(width, height);

    //     for (int i = 0; i < pixels.Length; i++)
    //     {
    //         int x = i % width;
    //         int y = i / width;
    //         face.SetPixel(x, y, pixels[i]);
    //     }

    //     Texture2DContent texture2DContent = new();
    //     texture2DContent.Faces[0].Add(face);
    //     return texture2DContent;
    // }

    // internal static Texture2DContent CreateTextureContent(RawTexture raw, string sourceFileName)
    // {
    //     PixelBitmapContent<Color> face = new(raw.Width, raw.Height);

    //     for (int i = 0; i < raw.Pixels.Length; i++)
    //     {
    //         int x = i % raw.Width;
    //         int y = i / raw.Width;

    //         face.SetPixel(x, y, raw.Pixels[i]);
    //     }

    //     Texture2DContent textureContent = new();
    //     textureContent.Identity = new ContentIdentity(sourceFileName);
    //     textureContent.Faces[0].Add(face);
    //     return textureContent;
    // }

    internal static Texture2DContent CreateTexture2DContent(ReadOnlySpan<Color> pixels, int width, int height)
    {
        PixelBitmapContent<Color> face = new(width, height);

        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % width;
            int y = i / width;

            face.SetPixel(x, y, pixels[i]);
        }

        Texture2DContent textureContent = new();
        textureContent.Faces[0].Add(face);
        return textureContent;
    }
}
