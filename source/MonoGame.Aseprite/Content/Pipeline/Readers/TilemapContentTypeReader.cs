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

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Pipeline.Readers;

internal sealed class TilemapContentTypeReader : ContentTypeReader<Tilemap>
{
    protected override Tilemap Read(ContentReader reader, Tilemap? existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string name = reader.ReadString();
        Tilemap tilemap = new(name);

        Dictionary<int, Tileset> tilesets = ReadTilesets(reader);
        RawTilemapLayer[] layers = reader.ReadRawTilemapLayers();

        for (int i = 0; i < layers.Length; i++)
        {
            RawTilemapLayer rawLayer = layers[i];
            Tileset tileset = tilesets[rawLayer.TilesetID];

            TilemapLayer layer = tilemap.CreateLayer(rawLayer.Name, tileset, rawLayer.Columns, rawLayer.Rows, rawLayer.Offset.ToVector2());
            for (int j = 0; j < rawLayer.RawTilemapTiles.Length; j++)
            {
                RawTilemapTile rawTile = rawLayer.RawTilemapTiles[j];
                layer.SetTile(j, rawTile.TilesetTileID, rawTile.FlipVertically, rawTile.FlipHorizontally, rawTile.Rotation);
            }
        }

        return tilemap;
    }

    private Dictionary<int, Tileset> ReadTilesets(ContentReader reader)
    {
        Dictionary<int, Tileset> tilesets = new();

        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            string name = reader.ReadString();
            int id = reader.ReadInt32();
            int tileWidth = reader.ReadInt32();
            int tileHeight = reader.ReadInt32();
            Texture2D texture = reader.ReadObject<Texture2D>();
            Tileset tileset = new(name, texture, tileWidth, tileHeight);
            tilesets.Add(id, tileset);
        }

        return tilesets;
    }
}
