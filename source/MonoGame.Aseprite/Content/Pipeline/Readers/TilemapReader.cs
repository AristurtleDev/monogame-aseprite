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

// public sealed class TilemapReader : ContentTypeReader<Tilemap>
// {
//     protected override Tilemap Read(ContentReader input, Tilemap existingInstance)
//     {
//         if (existingInstance is not null)
//         {
//             return existingInstance;
//         }

//         TilesetCollection tilesetCollection = new();
//         ReadTilesets(input, tilesetCollection);


//     }

//     private void ReadTilesets(ContentReader input, TilesetCollection collection)
//     {
//         int count = input.ReadInt32();

//         for (int i = 0; i < count; i++)
//         {
//             ReadSingleTileset(input, collection);
//         }
//     }

//     private void ReadSingleTileset(ContentReader input, TilesetCollection collection)
//     {
//         int id = input.ReadInt32();
//         string name = input.ReadString();
//         int count = input.ReadInt32();
//         int tileWidth = input.ReadInt32();
//         int tileHeight = input.ReadInt32();
//         int width = input.ReadInt32();
//         int height = input.ReadInt32();
//         int pixelCount = input.ReadInt32();

//         Color[] pixels = new Color[pixelCount];

//         for (int i = 0; i < pixelCount; i++)
//         {
//             pixels[i] = input.ReadColor();
//         }

//         Texture2D texture = new(input.GetGraphicsDevice(), width, height, false, SurfaceFormat.Color);
//         texture.SetData<Color>(pixels);
//         Point tileSize = new(tileWidth, tileHeight);

//         Tileset tileset = collection.CreateTileset(name, texture, tileSize);

//         if (tileset.ID != id)
//         {
//             throw new InvalidOperationException("Tile ID after creation is not the expected value");
//         }

//         if (tileset.TileCount != count)
//         {
//             throw new InvalidOperationException($"Tile count after creation is not the expected value");
//         }
//     }
// }
