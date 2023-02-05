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
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Readers;

internal static class BinaryReaderExtensions
{
    internal static void ReadMagic(this BinaryReader reader)
    {
        byte m = reader.ReadByte(); //  [M]onoGame
        byte a = reader.ReadByte(); //  [A]seprite
        byte c = reader.ReadByte(); //  [C]ontent


        if (m != 'M' || a != 'A' || c != 'C')
        {
            //  Dispose of reader so stream is closed in case the caller has a try-catch that handles this exception.
            reader.Dispose();
            throw new InvalidOperationException($"File contains invalid magic number.  Was this processed using the MonoGame.Aseprite library?");
        }
    }

    internal static Rectangle ReadRectangle(this BinaryReader reader)
    {
        Rectangle result = new();
        result.Location = ReadPoint(reader);
        result.Size = ReadPoint(reader);
        return result;
    }

    internal static Point ReadPoint(this BinaryReader reader)
    {
        Point result = new();
        result.X = reader.ReadInt32();
        result.Y = reader.ReadInt32();
        return result;
    }

    internal static Color ReadColor(this BinaryReader reader)
    {
        Color result = new();
        result.R = reader.ReadByte();
        result.G = reader.ReadByte();
        result.B = reader.ReadByte();
        result.A = reader.ReadByte();
        return result;
    }

    internal static RawTexture ReadRawTexture(this BinaryReader reader)
    {
        string name = reader.ReadString();
        int width = reader.ReadInt32();
        int height = reader.ReadInt32();
        int pixelCount = reader.ReadInt32();

        Color[] pixels = new Color[pixelCount];

        for (int i = 0; i < pixelCount; i++)
        {
            pixels[i] = ReadColor(reader);
        }

        return new(name, pixels, width, height);
    }

    internal static RawTextureAtlas ReadRawTextureAtlas(this BinaryReader reader)
    {
        string name = reader.ReadString();
        RawTexture texture = reader.ReadRawTexture();

        int regionCount = reader.ReadInt32();
        RawTextureRegion[] regions = new RawTextureRegion[regionCount];

        for (int i = 0; i < regionCount; i++)
        {
            string regionName = reader.ReadString();
            Rectangle regionBounds = reader.ReadRectangle();
            regions[i] = new(regionName, regionBounds);
        }

        return new(name, texture, regions);
    }

    internal static RawTileset ReadRawTileset(this BinaryReader reader)
    {
        int id = reader.ReadInt32();
        string name = reader.ReadString();
        RawTexture texture = reader.ReadRawTexture();
        int tileWidth = reader.ReadInt32();
        int tileHeight = reader.ReadInt32();

        return new(id, name, texture, tileWidth, tileHeight);
    }

    internal static RawTilemapLayer ReadRawTilemapLayer(this BinaryReader reader)
    {
        string name = reader.ReadString();
        int tilesetID = reader.ReadInt32();
        int columns = reader.ReadInt32();
        int rows = reader.ReadInt32();
        Point offset = reader.ReadPoint();
        int tileCount = reader.ReadInt32();

        RawTilemapTile[] tiles = new RawTilemapTile[tileCount];

        for (int i = 0; i < tileCount; i++)
        {
            tiles[i] = reader.ReadRawTilemapTile();
        }

        return new(name, tilesetID, columns, rows, tiles, offset);
    }

    internal static RawTilemapTile ReadRawTilemapTile(this BinaryReader reader)
    {
        int tilesetTileID = reader.ReadInt32();
        bool flipHorizontally = reader.ReadBoolean();
        bool flipVertically = reader.ReadBoolean();
        float rotation = reader.ReadSingle();

        return new(tilesetTileID, flipHorizontally, flipVertically, rotation);
    }
}
