// /* ----------------------------------------------------------------------------
// MIT License

// Copyright (c) 2018-2023 Christopher Whitley

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ---------------------------------------------------------------------------- */

// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Content.Pipeline;
// using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
// using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// namespace MonoGame.Aseprite.Content.Pipeline.Processors;

// internal static class ContentProcessorHelpers
// {
//     internal static TextureContent CreateTextureContent(string fileName, Color[] pixels, int width, int height, ContentProcessorContext context, Color colorKey, bool colorKeyEnabled, bool generateMipMaps, bool premultiplyAlpha, bool resizePowerOfTwo, bool makeSquare, TextureProcessorOutputFormat textureFormat)
//     {
//         //  During the Aseprite file import, no matter the color mode used,
//         //  color data was translated to 32-bits per pixel
//         const int bpp = 32;
//         byte[] data = new byte[((width * height * bpp - 1) / 8) + 1];
//         PixelsToBytes(pixels, data);

//         BitmapContent face = new PixelBitmapContent<Color>(width, height);
//         face.SetPixelData(data);

//         TextureContent content = new Texture2DContent() { Identity = new(fileName) };
//         content.Faces[0].Add(face);

//         //  Make use of the default MonoGame Texture processor instead of
//         //  implementing something custom.
//         TextureProcessor internalProcessor = new()
//         {
//             ColorKeyColor = colorKey,
//             ColorKeyEnabled = colorKeyEnabled,
//             GenerateMipmaps = generateMipMaps,
//             PremultiplyAlpha = premultiplyAlpha,
//             ResizeToPowerOfTwo = resizePowerOfTwo,
//             MakeSquare = makeSquare,
//             TextureFormat = textureFormat
//         };

//         content = internalProcessor.Process(content, context);
//         return content;
//     }

//     private static void PixelsToBytes(Color[] src, byte[] dest)
//     {
//         for (int i = 0, b = 0; i < src.Length; i++, b += 4)
//         {
//             dest[b] = src[i].R;
//             dest[b + 1] = src[i].G;
//             dest[b + 2] = src[i].B;
//             dest[b + 3] = src[i].A;
//         }
//     }
// }
