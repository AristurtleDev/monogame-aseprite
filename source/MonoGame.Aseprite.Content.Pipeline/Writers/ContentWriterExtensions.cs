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
// using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
// using MonoGame.Aseprite.RawTypes;

// internal static class ContentWriterExtensions
// {
//     internal static void Write(this ContentWriter writer, Rectangle rectangle)
//     {
//         writer.Write(rectangle.X);
//         writer.Write(rectangle.Y);
//         writer.Write(rectangle.Width);
//         writer.Write(rectangle.Height);
//     }

//     internal static void Write(this ContentWriter writer, RawTextureRegion rawTextureRegion)
//     {
//         writer.Write(rawTextureRegion.Name);
//         writer.Write(rawTextureRegion.Bounds);
//         writer.Write(rawTextureRegion.Slices.Length);
//         for (int i = 0; i < rawTextureRegion.Slices.Length; i++)
//         {
//             writer.Write(rawTextureRegion.Slices[i]);
//         }
//     }

//     internal static void Write(this ContentWriter writer, RawSlice rawSlice)
//     {
//         writer.Write(rawSlice.Name);
//         writer.Write(rawSlice.Bounds);
//         writer.Write(rawSlice.Origin);
//         writer.Write(rawSlice.Color);
//     }
// }