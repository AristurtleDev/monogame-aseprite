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

using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.RawReaders;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.RawWriters;

namespace MonoGame.Aseprite.Tests;

public sealed class RawTypeWriteReadTests
{
    [Fact]
    public void Write_Then_Read_RawSprite()
    {
        TextureContent texture = new(nameof(TextureContent), new Color[] { Color.Transparent, Color.Red, Color.Green, Color.Blue }, 2, 2);
        SpriteContent sprite = new(nameof(SpriteContent), texture);

        Write((writer) => RawSpriteWriter.Write(writer, sprite), out MemoryStream stream);
        Read(stream, (reader) => RawSpriteReader.Read(reader), out SpriteContent actual);

        Assert.Equal(sprite, actual);
    }

    [Fact]
    public void Write_Then_Read_RawSpriteSheet()
    {
        TextureContent texture = new(nameof(TextureContent), new Color[] { Color.Transparent, Color.Red, Color.Green, Color.Blue }, 2, 2);
        TextureRegionContent[] regions = new TextureRegionContent[]
        {
            new(nameof(TextureRegionContent), new Rectangle(0, 0, 0, 0)),
            new(nameof(TextureRegionContent), new Rectangle(1, 1, 1, 1)),
            new(nameof(TextureRegionContent), new Rectangle(2, 2, 2, 2)),
            new(nameof(TextureRegionContent), new Rectangle(3, 3, 3, 3))
        };
        TextureAtlasContent atlas = new(nameof(TextureAtlasContent), texture, regions);
        AnimationFrameContent[] frames = new AnimationFrameContent[]
        {
            new(0, 0),
            new(1, 100),
            new(2, 200),
            new(3, 300)
        };
        AnimationTagContent[] tags = new AnimationTagContent[]
        {
            new(nameof(AnimationTagContent), frames, true, true, true),
            new(nameof(AnimationTagContent), frames, false, false, false),
            new(nameof(AnimationTagContent), frames, true, false, true),
            new(nameof(AnimationTagContent), frames, false, true, false),
        };
        SpriteSheetContent sheet = new(nameof(SpriteSheetContent), atlas, tags);

        Write(writer => RawSpriteSheetWriter.Write(writer, sheet), out MemoryStream stream);
        Read(stream, (reader) => RawSpriteSheetReader.Read(reader), out SpriteSheetContent actual);

        Assert.Equal(sheet, actual);
    }

    [Fact]
    public void Write_Then_Read_RawTextureAtlas()
    {
        TextureContent texture = new(nameof(TextureContent), new Color[] { Color.Transparent, Color.Red, Color.Green, Color.Blue }, 2, 2);
        TextureRegionContent[] regions = new TextureRegionContent[]
        {
            new(nameof(TextureRegionContent), new Rectangle(0, 0, 0, 0)),
            new(nameof(TextureRegionContent), new Rectangle(1, 1, 1, 1)),
            new(nameof(TextureRegionContent), new Rectangle(2, 2, 2, 2)),
            new(nameof(TextureRegionContent), new Rectangle(3, 3, 3, 3))
        };
        TextureAtlasContent atlas = new(nameof(TextureAtlasContent), texture, regions);

        Write((writer) => RawTextureAtlasWriter.Write(writer, atlas), out MemoryStream stream);
        Read(stream, (reader) => RawTextureAtlasReader.Read(reader), out TextureAtlasContent actual);

        Assert.Equal(atlas, actual);
    }

    [Fact]
    public void Write_Then_Read_RawTileset()
    {
        TextureContent texture = new(nameof(TextureContent), new Color[] { Color.Transparent, Color.Red, Color.Green, Color.Blue }, 2, 2);
        TilesetContent tileset = new(0, nameof(TilesetContent), texture, 1, 1);

        Write((writer) => RawTilesetWriter.Write(writer, tileset), out MemoryStream stream);
        Read(stream, (reader) => RawTilesetReader.Read(reader), out TilesetContent actual);

        Assert.Equal(tileset, actual);
    }

    [Fact]
    public void Write_Then_Read_RawTilemap()
    {
        TextureContent texture = new(nameof(TextureContent), new Color[] { Color.Transparent, Color.Red, Color.Green, Color.Blue }, 2, 2);

        TilesetContent[] tilesets = new TilesetContent[]
        {
            new(0, nameof(TilesetContent), texture, 1, 1)
        };

        TilemapTileContent[] tiles = new TilemapTileContent[]
        {
            new(0, true, true, 0.0f),
            new(1, true, false, 1.0f),
            new(2, false, true, 2.0f),
            new(3, false, false, 3.0f)
        };

        TilemapLayerContent[] layers = new TilemapLayerContent[]
        {
            new(nameof(TilemapLayerContent), 0, 1, 2, tiles, new(1, 2))
        };

        TilemapContent tilemap = new(nameof(TilemapContent), layers, tilesets);

        Write((writer) => RawTilemapWriter.Write(writer, tilemap), out MemoryStream stream);
        Read(stream, (reader) => RawTilemapReader.Read(reader), out TilemapContent actual);

        Assert.Equal(tilemap, actual);
    }

    [Fact]
    public void Write_Then_Read_RawAnimatedTilemap()
    {
        TextureContent texture = new(nameof(TextureContent), new Color[] { Color.Transparent, Color.Red, Color.Green, Color.Blue }, 2, 2);

        TilesetContent[] tilesets = new TilesetContent[]
        {
            new(0, nameof(TilesetContent), texture, 1, 1)
        };

        TilemapTileContent[] tiles = new TilemapTileContent[]
        {
            new(0, true, true, 0.0f),
            new(1, true, false, 1.0f),
            new(2, false, true, 2.0f),
            new(3, false, false, 3.0f)
        };

        TilemapLayerContent[] layers = new TilemapLayerContent[]
        {
            new(nameof(TilemapLayerContent), 0, 1, 2, tiles, new(1, 2))
        };

        TilemapFrameContent[] frames = new TilemapFrameContent[]
        {
            new(100, layers)
        };

        AnimatedTilemapContent tilemap = new(nameof(AnimatedTilemapContent), tilesets, frames);

        Write((writer) => RawAnimatedTilemapWriter.Write(writer, tilemap), out MemoryStream stream);
        Read(stream, (reader) => RawAnimatedTilemapReader.Read(reader), out AnimatedTilemapContent actual);

        Assert.Equal(tilemap, actual);
    }
    private void Write(Action<BinaryWriter> writeAction, out MemoryStream stream)
    {
        stream = new();
        using BinaryWriter writer = new(stream, Encoding.UTF8, leaveOpen: true);
        writeAction(writer);
    }

    private void Read<T>(MemoryStream stream, Func<BinaryReader, T> readFunc, out T result)
    {
        stream.Position = 0;
        using (BinaryReader reader = new(stream, Encoding.UTF8, leaveOpen: false))
        {
            result = readFunc(reader);
        }
    }
}