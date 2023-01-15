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
// using Microsoft.Xna.Framework.Content;
// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGame.Aseprite.Content.Pipeline.Readers;

// /// <summary>
// ///     An abstract class that provides static methods for reading common data
// ///     from an xnb file for the implementing type.
// /// </summary>
// /// <typeparam name="T">
// /// </typeparam>
// public abstract class CommonReader<T> : ContentTypeReader<T>
// {
//     internal static Point ReadPoint(ContentReader input)
//     {
//         int x = input.ReadInt32();
//         int y = input.ReadInt32();
//         return new(x, y);
//     }

//     internal static Rectangle ReadRectangle(ContentReader input)
//     {
//         Point location = ReadPoint(input);
//         Point size = ReadPoint(input);
//         return new(location, size);
//     }

//     internal static void ReadPixels(ContentReader input, Color[] pixels)
//     {
//         int len = input.ReadInt32();

//         if (pixels.Length != len)
//         {
//             throw new InvalidOperationException($"Pixel count does not match expected number of pixels to read");
//         }

//         for (int i = 0; i < len; i++)
//         {
//             pixels[i] = input.ReadColor();
//         }
//     }

//     internal static Texture2D ReadTexture(ContentReader input)
//     {
//         Point size = ReadPoint(input);

//         Color[] pixels = new Color[size.X * size.Y];
//         ReadPixels(input, pixels);

//         Texture2D texture = CreateTexture(input.GetGraphicsDevice(), size, pixels);
//         return texture;
//     }

//     private static Texture2D CreateTexture(GraphicsDevice device, Point size, Color[] pixels)
//     {
//         Texture2D texture = new(device, size.X, size.Y, false, SurfaceFormat.Color);
//         texture.SetData<Color>(pixels);
//         return texture;
//     }

//     internal static SpriteSheet ReadSpriteSheet(ContentReader input)
//     {
//         string name = input.ReadString();
//         Texture2D texture = ReadTexture(input);

//         SpriteSheet spriteSheet = new(name, texture);

//         ReadSpriteSheetRegions(input, spriteSheet);
//         ReadSpriteSheetAnimations(input, spriteSheet);

//         return spriteSheet;
//     }

//     internal static void ReadSpriteSheetRegions(ContentReader input, SpriteSheet spriteSheet)
//     {
//         int count = input.ReadInt32();
//         for (int i = 0; i < count; i++)
//         {
//             string name = input.ReadString();
//             Rectangle bounds = ReadRectangle(input);
//             spriteSheet.CreateRegion(name, bounds);
//         }
//     }

//     internal static void ReadSpriteSheetAnimations(ContentReader input, SpriteSheet spriteSheet)
//     {
//         int count = input.ReadInt32();
//         for (int i = 0; i < count; i++)
//         {
//             string name = input.ReadString();
//             byte flags = input.ReadByte();

//             bool isLooping = (flags & 1) != 0;
//             bool isReversed = (flags & 2) != 0;
//             bool isPingPong = (flags & 4) != 0;

//             int frameCount = input.ReadInt32();

//             AnimationCycleBuilder builder = new(name, spriteSheet);
//             builder.IsLooping(isLooping)
//                    .IsReversed(isReversed)
//                    .IsPingPong(isPingPong);

//             for (int j = 0; j < frameCount; j++)
//             {
//                 int index = input.ReadInt32();
//                 long ticks = input.ReadInt64();
//                 builder.AddFrame(index, TimeSpan.FromTicks(ticks));
//             }

//             spriteSheet.AddAnimationCycle(builder.Build());
//         }
//     }

//     internal static TilesetCollection ReadTilesetCollection(ContentReader input)
//     {
//         TilesetCollection collection = new();
//         ReadTilesets(input, collection);
//         return collection;
//     }

//     internal static void ReadTilesets(ContentReader input, TilesetCollection collection)
//     {
//         int count = input.ReadInt32();

//         for (int i = 0; i < count; i++)
//         {
//             Tileset tileset = ReadTileset(input);
//             collection.AddTileset(tileset);
//         }
//     }

//     internal static Tileset ReadTileset(ContentReader input)
//     {
//         string name = input.ReadString();
//         int count = input.ReadInt32();
//         Point tileSize = ReadPoint(input);
//         Texture2D texture = ReadTexture(input);
//         Tileset tileset = new(name, texture, tileSize.X, tileSize.Y);
//         return tileset;
//     }




// }
