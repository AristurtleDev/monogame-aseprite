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
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

internal static class ContentWriterExtensions
{
    internal static void Write(this ContentWriter writer, Rectangle rect)
    {
        writer.Write(rect.X);
        writer.Write(rect.Y);
        writer.Write(rect.Width);
        writer.Write(rect.Height);
    }

    internal static void Write(this ContentWriter writer, Point point)
    {
        writer.Write(point.X);
        writer.Write(point.Y);
    }

    internal static void Write(this ContentWriter writer, TextureContent content)
    {
        if (content is not Texture2DContent texture2DContent)
        {
            throw new InvalidOperationException($"Invalid content type");
        }

        MipmapChain mipmaps = texture2DContent.Mipmaps;
        BitmapContent level0 = mipmaps[0];

        if (!level0.TryGetFormat(out SurfaceFormat format))
        {
            throw new InvalidOperationException("Could not get format of texture content");
        }

        writer.Write((int)format);
        writer.Write(level0.Width);
        writer.Write(level0.Height);
        writer.Write(mipmaps.Count);

        foreach (BitmapContent level in mipmaps)
        {
            byte[] pixelData = level.GetPixelData();
            writer.Write(pixelData.Length);
            writer.Write(pixelData);
        }
    }
}
