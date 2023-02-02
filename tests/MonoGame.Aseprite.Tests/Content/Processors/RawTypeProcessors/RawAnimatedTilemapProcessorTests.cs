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
using MonoGame.Aseprite.Content.Processors.RawProcessors;
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Tests;


public sealed class RawAnimatedTilemapProcessorTestFixture
{
    public AsepriteFile AsepriteFile { get; }

    public RawAnimatedTilemapProcessorTestFixture()
    {
        string fileName = "raw-animated-tilemap-processor-tests";

        Color[] palette = Array.Empty<Color>();
        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();
        AsepriteUserData userData = new();

        AsepriteTileset[] tilesets = new AsepriteTileset[]
        {
            new(0, 4, 1, 1, "tileset-0", new Color[] {Color.Transparent, Color.Red, Color.Green, Color.Blue}),
            new(1, 4, 1, 1, "tileset-1", new Color[] {Color.Transparent, Color.White, Color.Gray, Color.Black}),
        };

        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new AsepriteTilemapLayer(tilesets[0], AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "visible"),
            new AsepriteTilemapLayer(tilesets[1], AsepriteLayerFlags.None, AsepriteBlendMode.Normal, 255, "hidden")
        };

        AsepriteTile[] cel0Tiles = new AsepriteTile[]
        {
            new(0, 0, 0, 0),
            new(1, 0, 0, 0),
            new(2, 0, 0, 0),
            new(3, 0, 0, 0)
        };

        AsepriteTile[] cel1Tiles = new AsepriteTile[]
        {
            new(2, 0, 0, 0),
            new(3, 0, 0, 0)
        };

        AsepriteCel[] cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(2, 2, cel0Tiles, layers[0], Point.Zero, 255),
            new AsepriteTilemapCel(2, 2, cel1Tiles, layers[1], new Point(0, 1), 255)
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{fileName} 0", 2, 2, 100, cels)
        };

        AsepriteFile = new(fileName, 0, 0, palette, frames, layers, tags, slices, tilesets, userData);
    }
}

public sealed class RawAnimatedTilemapProcessorTests : IClassFixture<RawAnimatedTilemapProcessorTestFixture>
{
    private readonly RawAnimatedTilemapProcessorTestFixture _fixture;

    public RawAnimatedTilemapProcessorTests(RawAnimatedTilemapProcessorTestFixture fixture) => _fixture = fixture;

    [Fact(Skip = "Not Implemented")]
    public void OnlyVisibleLayers_True_Processes_Only_Visible_Layers()
    {
        throw new NotImplementedException();
    }

    [Fact(Skip = "Not Implemented")]
    public void OnlyVisibleLayers_False_Process_All_Layers()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Duplciate_AsepriteLayer_Names_Throws_Exception()
    {

        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new AsepriteTilemapLayer(_fixture.AsepriteFile.Tilesets[0], AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "layer-0"),
            new AsepriteTilemapLayer(_fixture.AsepriteFile.Tilesets[0], AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "layer-1"),
            new AsepriteTilemapLayer(_fixture.AsepriteFile.Tilesets[0], AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "layer-0")
        };

        AsepriteTile[] tiles = new AsepriteTile[]
        {
            new(0, 0, 0, 0),
            new(1, 0, 0, 0),
            new(2, 0, 0, 0),
            new(3, 0, 0, 0)
        };

        AsepriteCel[] cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(2, 2, tiles, layers[0], Point.Zero, 255),
            new AsepriteTilemapCel(2, 2, tiles, layers[1], Point.Zero, 255),
            new AsepriteTilemapCel(2, 2, tiles, layers[2], Point.Zero, 255)
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{_fixture.AsepriteFile.Name} 0", 2, 2, 100, cels)
        };

        //  Reuse the fixture, but use the layers array from above with duplicate layer names
        AsepriteFile aseFile = new(_fixture.AsepriteFile.Name,
                                   _fixture.AsepriteFile.CanvasWidth,
                                   _fixture.AsepriteFile.CanvasHeight,
                                   _fixture.AsepriteFile.Palette.ToArray(),
                                   frames,
                                   layers,
                                   _fixture.AsepriteFile.Tags.ToArray(),
                                   _fixture.AsepriteFile.Slices.ToArray(),
                                   _fixture.AsepriteFile.Tilesets.ToArray(),
                                   _fixture.AsepriteFile.UserData);

        Assert.Throws<InvalidOperationException>(() => RawAnimatedTilemapProcessor.Process(aseFile));
    }

    private void AssertRawLayer(RawTilemapLayer layer, string name, int tilesetID, int columns, int rows, Point offset, ReadOnlySpan<AsepriteTile> tiles)
    {
        Assert.Equal(name, layer.Name);
        Assert.Equal(tilesetID, layer.TilesetID);
        Assert.Equal(columns, layer.Columns);
        Assert.Equal(rows, layer.Rows);
        Assert.Equal(offset, layer.Offset);

        for (int i = 0; i < tiles.Length; i++)
        {
            Assert.Equal(tiles[i].TilesetTileID, layer.RawTilemapTiles[i].TilesetTileID);

            bool xFlip = tiles[i].XFlip != 0;
            Assert.Equal(xFlip, layer.RawTilemapTiles[i].FlipHorizontally);

            bool yFlip = tiles[i].YFlip != 0;
            Assert.Equal(yFlip, layer.RawTilemapTiles[i].FlipVertically);
            Assert.Equal(tiles[i].Rotation, layer.RawTilemapTiles[i].Rotation);
        }
    }
}
