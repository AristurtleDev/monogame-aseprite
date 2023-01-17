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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

public sealed class TilemapReader : ContentTypeReader<Tilemap>
{
    protected override Tilemap Read(ContentReader reader, Tilemap existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        //  Tileset content
        List<Tileset> tilesets = new();
        int tilesetCount = reader.ReadInt32();
        for (int i = 0; i < tilesetCount; i++)
        {
            string name = reader.ReadString();
            int tileCount = reader.ReadInt32();
            int tileWidth = reader.ReadInt32();
            int tileHeight = reader.ReadInt32();

            //  Texture content
            Texture2D texture = reader.ReadTexture2D(existingInstance: null);

            Tileset tileset = new(name, texture, tileWidth, tileHeight);
            tilesets.Add(tileset);
        }

        Tilemap tilemap = new("");

        //  TilemapLayer content
        int layerCount = reader.ReadInt32();
        for (int i = 0; i < layerCount; i++)
        {
            int tilesetID = reader.ReadInt32();
            string layerName = reader.ReadString();
            int columns = reader.ReadInt32();
            int rows = reader.ReadInt32();
            int offsetX = reader.ReadInt32();
            int offsetY = reader.ReadInt32();

            Vector2 offset = new(offsetX, offsetY);
            Tileset tileset = tilesets[tilesetID];

            TilemapLayer layer = new(layerName, tileset, columns, rows, offset);

            //  Tile content
            int tileCount = reader.ReadInt32();
            for (int j = 0; j < tileCount; j++)
            {
                byte flipFlag = reader.ReadByte();
                float rotation = reader.ReadSingle();
                int tilesetTileID = reader.ReadInt32();

                bool flipHorizontal = (flipFlag & 1) != 0;
                bool flipVertical = (flipFlag & 2) != 0;

                layer.SetTile(j, tilesetTileID, flipVertical, flipHorizontal, rotation);
            }

            tilemap.AddLayer(layer);
        }

        return tilemap;
    }
}
