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

        AsepriteTile[] frame0Cel0Tiles = new AsepriteTile[]
        {
            new(0, 0, 0, 0),
            new(1, 0, 0, 0),
            new(2, 0, 0, 0),
            new(3, 0, 0, 0)
        };

        AsepriteTile[] frame0Cel1Tiles = new AsepriteTile[]
        {
            new(2, 0, 0, 0),
            new(3, 0, 0, 0)
        };

        AsepriteCel[] frame0Cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(2, 2, frame0Cel0Tiles, layers[0], Point.Zero, 255),
            new AsepriteTilemapCel(2, 1, frame0Cel1Tiles, layers[1], new Point(0, 1), 255)
        };

        AsepriteTile[] frame1Cel0Tiles = new AsepriteTile[]
        {
            new(3, 0, 0, 0),
            new(2, 0, 0, 0),
            new(1, 0, 0, 0),
            new(0, 0, 0, 0)
        };

        AsepriteTile[] frame1Cel1Tiles = new AsepriteTile[]
        {
            new(3, 0, 0, 0),
            new(2, 0, 0, 0)
        };

        AsepriteCel[] frame1Cels = new AsepriteCel[]
        {
            new AsepriteTilemapCel(2, 2, frame1Cel0Tiles, layers[0], Point.Zero, 255),
            new AsepriteTilemapCel(2, 1, frame1Cel1Tiles, layers[1], new Point(0, 1), 255)
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{fileName} 0", 2, 2, 100, frame0Cels),
            new($"{fileName} 1", 2, 2, 200, frame1Cels)
        };

        AsepriteFile = new(fileName, 0, 0, palette, frames, layers, tags, slices, tilesets, userData);
    }
}

public sealed class RawAnimatedTilemapProcessorTests : IClassFixture<RawAnimatedTilemapProcessorTestFixture>
{
    private readonly RawAnimatedTilemapProcessorTestFixture _fixture;

    public RawAnimatedTilemapProcessorTests(RawAnimatedTilemapProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void OnlyVisibleLayers_True_Processes_Only_Visible_Layers()
    {
        RawAnimatedTilemap tilemap = RawAnimatedTilemapProcessor.Process(_fixture.AsepriteFile, onlyVisibleLayers: true);

        //  Expect the name of the aseprite file to be the name of the tilemap.
        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        //  File has 2 layers, but one is hidden, so expect only 1 layer.
        Assert.Equal(1, tilemap.RawTilesets.Length);

        //  Each layer uses a different tileset, but again, one is hidden, so expect only one tileset.
        Assert.Equal(1, tilemap.RawTilesets.Length);

        //  Expect that the tileset was processed correctly
        RawTileset tileset = RawTilesetProcessor.Process(_fixture.AsepriteFile, 0);
        Assert.Equal(tileset, tilemap.RawTilesets[0]);

        //  The file has 2 frames, so expect 2 frames
        Assert.Equal(2, tilemap.RawTilemapFrames.Length);

        //  Expect that the duration of the tilemap frames was taken from the frames in the file
        Assert.Equal(_fixture.AsepriteFile.Frames[0].Duration, tilemap.RawTilemapFrames[0].DurationInMilliseconds);
        Assert.Equal(_fixture.AsepriteFile.Frames[1].Duration, tilemap.RawTilemapFrames[1].DurationInMilliseconds);

        //  Again, one of the layers is hidden, so expect only one layer processed for each frame.
        Assert.Equal(1, tilemap.RawTilemapFrames[0].RawTilemapLayers.Length);
        Assert.Equal(1, tilemap.RawTilemapFrames[1].RawTilemapLayers.Length);

        // Getting reference to the aseprite layers and cels used to construct the raw tilemap layers
        AsepriteTilemapLayer aseLayer0 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapCel aseFrame0Cel = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[0];
        AsepriteTilemapCel aseFrame1Cel = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[1].Cels[0];

        //  Expect that the single layer of each frame was constructed properly from the aseprite layer and cel data.
        AssertRawLayer(tilemap.RawTilemapFrames[0].RawTilemapLayers[0], aseLayer0, aseFrame0Cel);
        AssertRawLayer(tilemap.RawTilemapFrames[1].RawTilemapLayers[0], aseLayer0, aseFrame1Cel);
    }

    [Fact]
    public void OnlyVisibleLayers_False_Processes_All_Layers()
    {
        RawAnimatedTilemap tilemap = RawAnimatedTilemapProcessor.Process(_fixture.AsepriteFile, onlyVisibleLayers: false);

        //  Expect the name of the aseprite file to be the name of the tilemap.
        Assert.Equal(_fixture.AsepriteFile.Name, tilemap.Name);

        //  Since all layers were processed, and there are 2 layers, each with a different tileset, expect 2 tilesets
        Assert.Equal(2, tilemap.RawTilesets.Length);

        //  Each layer uses a different tileset, and since both were processed, expect two tilesets.
        Assert.Equal(2, tilemap.RawTilesets.Length);

        //  Expect that both of the tilesets were processed correctly
        RawTileset tileset0 = RawTilesetProcessor.Process(_fixture.AsepriteFile, 0);
        RawTileset tileset1 = RawTilesetProcessor.Process(_fixture.AsepriteFile, 1);
        Assert.Equal(tileset0, tilemap.RawTilesets[0]);
        Assert.Equal(tileset1, tilemap.RawTilesets[1]);

        //  The file has 2 frames, so expect 2 frames
        Assert.Equal(2, tilemap.RawTilemapFrames.Length);

        //  Expect that the duration of the tilemap frames was taken from the frames in the file
        Assert.Equal(_fixture.AsepriteFile.Frames[0].Duration, tilemap.RawTilemapFrames[0].DurationInMilliseconds);
        Assert.Equal(_fixture.AsepriteFile.Frames[1].Duration, tilemap.RawTilemapFrames[1].DurationInMilliseconds);

        //  Since all layers were processed, and there are 2 layers, expect that each frame has 2 layers.
        Assert.Equal(2, tilemap.RawTilemapFrames[0].RawTilemapLayers.Length);
        Assert.Equal(2, tilemap.RawTilemapFrames[1].RawTilemapLayers.Length);

        // Getting reference to the aseprite layers and cels used to construct the raw tilemap layers
        AsepriteTilemapLayer aseLayer0 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[0];
        AsepriteTilemapLayer aseLayer1 = (AsepriteTilemapLayer)_fixture.AsepriteFile.Layers[1];
        AsepriteTilemapCel aseFrame0Cel0 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[0];
        AsepriteTilemapCel aseFrame0Cel1 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[0].Cels[1];
        AsepriteTilemapCel aseFrame1Cel0 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[1].Cels[0];
        AsepriteTilemapCel aseFrame1Cel1 = (AsepriteTilemapCel)_fixture.AsepriteFile.Frames[1].Cels[1];

        //  Expect that the the layers on each frame were constructed properly from the aseprite layer and cel data
        AssertRawLayer(tilemap.RawTilemapFrames[0].RawTilemapLayers[0], aseLayer0, aseFrame0Cel0);
        AssertRawLayer(tilemap.RawTilemapFrames[0].RawTilemapLayers[1], aseLayer1, aseFrame0Cel1);
        AssertRawLayer(tilemap.RawTilemapFrames[1].RawTilemapLayers[0], aseLayer0, aseFrame1Cel0);
        AssertRawLayer(tilemap.RawTilemapFrames[1].RawTilemapLayers[1], aseLayer1, aseFrame1Cel1);
    }

    [Fact]
    public void Duplicate_AsepriteLayer_Names_Throws_Exception()
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

    private void AssertRawLayer(RawTilemapLayer tilemapLayer, AsepriteTilemapLayer aseLayer, AsepriteTilemapCel aseCel)
    {
        Assert.Equal(aseLayer.Name, tilemapLayer.Name);
        Assert.Equal(aseLayer.Tileset.ID, tilemapLayer.TilesetID);
        Assert.Equal(aseCel.Columns, tilemapLayer.Columns);
        Assert.Equal(aseCel.Rows, tilemapLayer.Rows);
        Assert.Equal(aseCel.Position, tilemapLayer.Offset);

        for (int i = 0; i < aseCel.Tiles.Length; i++)
        {
            AsepriteTile aseTile = aseCel.Tiles[i];
            RawTilemapTile tilemapTile = tilemapLayer.RawTilemapTiles[i];

            Assert.Equal(aseTile.TilesetTileID, tilemapTile.TilesetTileID);

            bool xFlip = aseTile.XFlip != 0;
            Assert.Equal(xFlip, tilemapTile.FlipHorizontally);

            bool yFlip = aseTile.YFlip != 0;
            Assert.Equal(yFlip, tilemapTile.FlipVertically);
            Assert.Equal(aseTile.Rotation, tilemapTile.Rotation);
        }
    }
}
