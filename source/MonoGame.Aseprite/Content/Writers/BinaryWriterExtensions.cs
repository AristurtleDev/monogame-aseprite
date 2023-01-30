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
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Content.Writers;

internal static class BinaryWriterExtensions
{
    internal static void WriteMagic(this BinaryWriter writer)
    {
        writer.Write((byte)'M');    //  [M]onoGame
        writer.Write((byte)'A');    //  [A]seprite
        writer.Write((byte)'C');    //  [C]ontent
    }

    internal static void Write(this BinaryWriter writer, Rectangle value)
    {
        writer.Write(value.Location);
        writer.Write(value.Size);
    }

    internal static void Write(this BinaryWriter writer, Point value)
    {
        writer.Write(value.X);
        writer.Write(value.Y);
    }

    internal static void Write(this BinaryWriter writer, Color value)
    {
        writer.Write(value.R);
        writer.Write(value.G);
        writer.Write(value.B);
        writer.Write(value.A);
    }

    internal static void Write(this BinaryWriter writer, RawTexture value)
    {
        writer.Write(value.Name);
        writer.Write(value.Width);
        writer.Write(value.Height);
        writer.Write(value.Pixels.Length);

        for (int i = 0; i < value.Pixels.Length; i++)
        {
            writer.Write(value.Pixels[i]);
        }
    }

    internal static void Write(this BinaryWriter writer, RawTextureAtlas value)
    {
        writer.Write(value.Name);
        writer.Write(value.RawTexture);
        writer.Write(value.RawTextureRegions.Length);

        for (int i = 0; i < value.RawTextureRegions.Length; i++)
        {
            RawTextureRegion rawTextureRegion = value.RawTextureRegions[i];
            writer.Write(rawTextureRegion.Name);
            writer.Write(rawTextureRegion.Bounds);
        }
    }

    internal static void Write(this BinaryWriter writer, RawTileset value)
    {
        writer.Write(value.ID);
        writer.Write(value.Name);
        writer.Write(value.RawTexture);
        writer.Write(value.TileWidth);
        writer.Write(value.TileHeight);
    }

    internal static void Write(this BinaryWriter writer, RawTilemapLayer value)
    {
        writer.Write(value.Name);
        writer.Write(value.TilesetID);
        writer.Write(value.Columns);
        writer.Write(value.Rows);
        writer.Write(value.Offset);
        writer.Write(value.RawTilemapTiles.Length);

        for (int i = 0; i < value.RawTilemapTiles.Length; i++)
        {
            writer.Write(value.RawTilemapTiles[i]);
        }
    }

    internal static void Write(this BinaryWriter writer, RawTilemapTile value)
    {
        writer.Write(value.TilesetTileID);
        writer.Write(value.FlipHorizontally);
        writer.Write(value.FlipVertically);
        writer.Write(value.Rotation);
    }
}
