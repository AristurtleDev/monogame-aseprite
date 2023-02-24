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

namespace MonoGame.Aseprite.Content;

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

    internal static Vector2 ReadVector2(this BinaryReader reader)
    {
        Vector2 result = new();
        result.X = reader.ReadSingle();
        result.Y = reader.ReadSingle();
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

        Color[] pixels = reader.ReadColors();

        return new(name, pixels, width, height);
    }

    internal static Color[] ReadColors(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        Color[] pixels = new Color[count];
        for (int i = 0; i < count; i++)
        {
            pixels[i] = reader.ReadColor();
        }
        return pixels;
    }

    internal static RawSlice ReadRawSlice(this BinaryReader reader)
    {
        string name = reader.ReadString();
        Rectangle bounds = reader.ReadRectangle();
        Vector2 origin = reader.ReadVector2();
        Color color = reader.ReadColor();

        bool isNine = reader.ReadBoolean();

        if (isNine)
        {
            Rectangle centerBounds = reader.ReadRectangle();
            return new RawNinePatchSlice(name, bounds, centerBounds, origin, color);
        }

        return new RawSlice(name, bounds, origin, color);
    }

    internal static RawAnimationTag[] ReadRawAnimationTags(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        RawAnimationTag[] tags = new RawAnimationTag[count];
        for (int i = 0; i < count; i++)
        {
            tags[i] = reader.ReadRawAnimationTag();
        }
        return tags;
    }

    internal static RawAnimationTag ReadRawAnimationTag(this BinaryReader reader)
    {
        string name = reader.ReadString();
        bool isLooping = reader.ReadBoolean();
        bool isReversed = reader.ReadBoolean();
        bool isPingPong = reader.ReadBoolean();
        RawAnimationFrame[] frames = reader.ReadRawAnimationFrames();
        return new(name, frames, isLooping, isReversed, isPingPong);
    }

    internal static RawAnimationFrame[] ReadRawAnimationFrames(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        RawAnimationFrame[] frames = new RawAnimationFrame[count];
        for (int i = 0; i < count; i++)
        {
            frames[i] = reader.ReadRawAnimationFrame();
        }
        return frames;
    }

    internal static RawAnimationFrame ReadRawAnimationFrame(this BinaryReader reader)
    {
        int index = reader.ReadInt32();
        int duration = reader.ReadInt32();
        return new(index, duration);
    }

    internal static RawTextureAtlas ReadRawTextureAtlas(this BinaryReader reader)
    {
        string name = reader.ReadString();
        RawTexture texture = reader.ReadRawTexture();
        RawTextureRegion[] regions = reader.ReadRawTextureRegions();
        return new(name, texture, regions);
    }

    internal static RawTextureRegion[] ReadRawTextureRegions(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        RawTextureRegion[] regions = new RawTextureRegion[count];

        for (int i = 0; i < count; i++)
        {
            regions[i] = reader.ReadRawTextureRegion();
        }

        return regions;
    }

    internal static RawTextureRegion ReadRawTextureRegion(this BinaryReader reader)
    {
        string name = reader.ReadString();
        Rectangle bounds = reader.ReadRectangle();
        RawSlice[] slices = reader.ReadRawSlices();
        return new(name, bounds, slices);
    }

    internal static RawSlice[] ReadRawSlices(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        RawSlice[] slices = new RawSlice[count];

        for (int i = 0; i < count; i++)
        {
            slices[i] = reader.ReadRawSlice();
        }

        return slices;
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

    internal static RawTilemapLayer[] ReadRawTilemapLayers(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        RawTilemapLayer[] layers = new RawTilemapLayer[count];
        for (int i = 0; i < count; i++)
        {
            layers[i] = reader.ReadRawTilemapLayer();
        }
        return layers;
    }

    internal static RawTilemapLayer ReadRawTilemapLayer(this BinaryReader reader)
    {
        string name = reader.ReadString();
        int tilesetID = reader.ReadInt32();
        int columns = reader.ReadInt32();
        int rows = reader.ReadInt32();
        Point offset = reader.ReadPoint();
        RawTilemapTile[] tiles = reader.ReadRawTilemapTiles();
        return new(name, tilesetID, columns, rows, tiles, offset);
    }

    internal static RawTilemapTile[] ReadRawTilemapTiles(this BinaryReader reader)
    {
        int count = reader.ReadInt32();
        RawTilemapTile[] rawTilemapTiles = new RawTilemapTile[count];

        for (int i = 0; i < count; i++)
        {
            rawTilemapTiles[i] = reader.ReadRawTilemapTile();
        }

        return rawTilemapTiles;
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
