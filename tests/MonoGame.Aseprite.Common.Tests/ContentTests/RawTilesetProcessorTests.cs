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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Tests;


public sealed class RawTilesetProcessorTestFixture
{
    public AsepriteFile AsepriteFile { get; }

    public RawTilesetProcessorTestFixture()
    {
        Color[] palette = Array.Empty<Color>();
        AsepriteFrame[] frames = Array.Empty<AsepriteFrame>();
        AsepriteLayer[] layers = Array.Empty<AsepriteLayer>();
        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();
        AsepriteUserData userData = new();

        AsepriteTileset[] tilesets = new AsepriteTileset[]
        {
            new(0, 4, 1, 1, "tileset-0", new Color[] {Color.Transparent, Color.Red, Color.Green, Color.Blue}),
            new(1, 4, 1, 1, "tileset-1", new Color[] {Color.Transparent, Color.White, Color.Gray, Color.Black}),
        };

        AsepriteFile = new("file", 0, 0, palette, frames, layers, tags, slices, tilesets, userData);
    }
}

public sealed class RawTilesetProcessorTests : IClassFixture<RawTilesetProcessorTestFixture>
{
    private readonly RawTilesetProcessorTestFixture _fixture;

    public RawTilesetProcessorTests(RawTilesetProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void By_Index()
    {
        int index = 0;
        AsepriteTileset aseTileset = _fixture.AsepriteFile.Tilesets[index];

        RawTileset rawTileset = RawTilesetProcessor.Process(_fixture.AsepriteFile, index);

        Assert.Equal(aseTileset.ID, rawTileset.ID);
        Assert.Equal(aseTileset.Name, rawTileset.Name);
        Assert.Equal(aseTileset.TileWidth, rawTileset.TileWidth);
        Assert.Equal(aseTileset.TileHeight, rawTileset.TileHeight);
        Assert.Equal(aseTileset.Name, rawTileset.RawTexture.Name);
        Assert.Equal(aseTileset.Width, rawTileset.RawTexture.Width);
        Assert.Equal(aseTileset.Height, rawTileset.RawTexture.Height);
        Assert.Equal(aseTileset.Pixels.ToArray(), rawTileset.RawTexture.Pixels.ToArray());
    }

    [Fact]
    public void By_Name()
    {
        AsepriteTileset aseTileset = _fixture.AsepriteFile.Tilesets[0];

        RawTileset rawTileset = RawTilesetProcessor.Process(_fixture.AsepriteFile, aseTileset.Name);

        Assert.Equal(aseTileset.ID, rawTileset.ID);
        Assert.Equal(aseTileset.Name, rawTileset.Name);
        Assert.Equal(aseTileset.TileWidth, rawTileset.TileWidth);
        Assert.Equal(aseTileset.TileHeight, rawTileset.TileHeight);
        Assert.Equal(aseTileset.Name, rawTileset.RawTexture.Name);
        Assert.Equal(aseTileset.Width, rawTileset.RawTexture.Width);
        Assert.Equal(aseTileset.Height, rawTileset.RawTexture.Height);
        Assert.Equal(aseTileset.Pixels.ToArray(), rawTileset.RawTexture.Pixels.ToArray());
    }

    [Fact]
    public void Index_Out_Of_Range_Throws_Exception()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RawTilesetProcessor.Process(_fixture.AsepriteFile, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => RawTilesetProcessor.Process(_fixture.AsepriteFile, _fixture.AsepriteFile.Tilesets.Length));
        Assert.Throws<ArgumentOutOfRangeException>(() => RawTilesetProcessor.Process(_fixture.AsepriteFile, _fixture.AsepriteFile.Tilesets.Length + 1));
    }

    [Fact]
    public void Bad_Name_Throws_Exception()
    {
        Assert.Throws<InvalidOperationException>(() => RawTilesetProcessor.Process(_fixture.AsepriteFile, string.Empty));
    }
}
