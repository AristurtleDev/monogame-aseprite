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

using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

/// <summary>
///     Provides method for reading a <see cref="TilesetCollection"/> from an
///     xnb file that was generated with the MonoGame.Aseprite library.
/// </summary>
public sealed class TilesetCollectionReader : ContentTypeReader<TilesetCollection>
{
    protected override TilesetCollection Read(ContentReader reader, TilesetCollection? existingInstance)
    {
        if(existingInstance is not null)
        {
            return existingInstance;
        }

        TilesetCollection collection = new();

        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
        {
            string name = reader.ReadString();
            int tileWidth = reader.ReadInt32();
            int tileHeight = reader.ReadInt32();

            //  Texture Content
            Texture2D texture = reader.ReadTexture2D();

            Tileset tileset = new(name, texture, tileWidth, tileHeight);

            collection.AddTileset(tileset);
        }

        return collection;
    }
}
