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

// using System.Diagnostics;
// using System.Reflection;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Content;
// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGame.Aseprite;

// internal static class ContentReaderHelper
// {
//     public static Texture2D ReadTexture(this ContentReader reader, Texture2D? existingInstance)
//     {
//         if (Assembly.GetAssembly(typeof(ContentReader)) is not Assembly assembly)
//         {
//             throw new InvalidOperationException($"Unable to load Microsoft.Xna.Framework assembly");
//         }

//         if (assembly.GetType("Microsoft.Xna.Framework.Content.Texture2DReader") is not Type texture2DReaderType)
//         {
//             throw new InvalidOperationException($"Unable to load Texture2DReader type from assembly");
//         }

//         if (Activator.CreateInstance(texture2DReaderType) is not object texture2DReaderInstance)
//         {
//             throw new InvalidOperationException($"Unable to create instance of Texture2DReader");
//         }

//         if (texture2DReaderType.GetMethod("Read", BindingFlags.NonPublic | BindingFlags.Instance, new [] {typeof(ContentReader), typeof(Texture2D)}) is not MethodInfo readMethod)
//         {
//             throw new InvalidOperationException($"Unable to get Process method form Texture2DReader type");
//         }

//         if (readMethod.Invoke(texture2DReaderInstance, new object?[] { reader, existingInstance }) is not Texture2D texture)
//         {
//             throw new InvalidOperationException("$Unable to create texture from Texture2DReader.Read method");
//         }

//         return texture;
//     }
// }
