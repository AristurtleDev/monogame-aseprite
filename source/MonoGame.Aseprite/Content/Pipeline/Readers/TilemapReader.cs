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
    Dictionary<int, Tileset> _tilesetsById = new();

    protected override Tilemap Read(ContentReader reader, Tilemap existingInstance)
    {
        if (existingInstance is not null)
        {
            return existingInstance;
        }

        string name = reader.ReadString();

        Tilemap tilemap = new(name);
        ReadTilesets(reader);
        ReadLayers(reader, tilemap);
        return tilemap;
    }

    private void ReadTilesets(ContentReader reader)
    {
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            Texture2D texture = ReadTexture(reader);
            ReadTileset(reader, texture);
        }
    }

    private Texture2D ReadTexture(ContentReader reader)
    {
        Texture2D texture = reader.ReadTexture2D();
        texture.Name = reader.ReadString();
        return texture;
    }

    private void ReadTileset(ContentReader reader, Texture2D texture)
    {
        int id = reader.ReadInt32();
        string name = reader.ReadString();
        int tileWidth = reader.ReadInt32();
        int tileHeight = reader.ReadInt32();

        Tileset tileset =  new(name, texture, tileWidth, tileHeight);
        _tilesetsById.Add(id, tileset);
    }

    private void ReadLayers(ContentReader reader, Tilemap tilemap)
    {
        int count = reader.ReadInt32();

        for (int i = 0; i < count; i++)
        {
            ReadLayer(reader, tilemap);
        }
    }

    private void ReadLayer(ContentReader reader, Tilemap tilemap)
    {
        string name = reader.ReadString();
        int tilesetID = reader.ReadInt32();
        int columns = reader.ReadInt32();
        int rows = reader.ReadInt32();
        Point offset = reader.ReadPoint();

        Tileset tileset = _tilesetsById[tilesetID];
        TilemapLayer layer = new(name, tileset, columns, rows, offset.ToVector2());
        ReadTiles(reader, layer);

        tilemap.AddLayer(layer);
    }

    private void ReadTiles(ContentReader reader, TilemapLayer layer)
    {
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
        {
            ReadTile(reader, layer, i);
        }
    }

    private void ReadTile(ContentReader reader, TilemapLayer layer, int index)
    {
        int tilesetTileID = reader.ReadInt32();
        bool xFlip = reader.ReadBoolean();
        bool yFlip = reader.ReadBoolean();
        float rotation = reader.ReadSingle();

        layer.SetTile(index, tilesetTileID, xFlip, yFlip, rotation);
    }
}
