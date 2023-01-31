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
using MonoGame.Aseprite.Content.Processors.RawProcessors;
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Tests;

public sealed class RawAnimatedTilemapProcessorTests
{
    [Fact]
    public void RawAnimatedTilemapProcessorTest_Process()
    {
        string name = "animated-tilemap-processor-test";

        //  Build expected texture for expected tileset
        Color[] pixels = new Color[]
        {
            new(0, 0, 0, 0),
            new(223, 7, 114, 255),
            new(254, 84, 111, 255),
            new(255, 158, 125, 255),
            new(255, 208, 128, 255)
        };
        RawTexture expectedTilesetTexture = new("tileset-0", pixels, 1, 5);

        //  Build expected tileset and tileset collection
        RawTileset expectedTileset = new(0, "tileset-0", expectedTilesetTexture, 1, 1);
        RawTileset[] expectedTilesets = new RawTileset[1] { expectedTileset };

        //  Build expected frames and frame collection
        //  *****************************************************
        //                        FRAME 0
        //  *****************************************************
        RawTilemapTile[] expectedFrame0Layer0Tiles = new RawTilemapTile[]
        {
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame0Layer0 = new("layer-0", 0, 4, 4, expectedFrame0Layer0Tiles, new(0, 1));
        RawTilemapLayer[] expectedFrame0Layers = new RawTilemapLayer[] { expectedFrame0Layer0 };
        RawTilemapFrame expectedFrame0 = new(100, expectedFrame0Layers);

        //  *****************************************************
        //                        FRAME 1
        //  *****************************************************
        RawTilemapTile[] expectedFrame1Layer0Tiles = new RawTilemapTile[]
        {
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame1Layer0 = new("layer-0", 0, 4, 4, expectedFrame1Layer0Tiles, Point.Zero);
        RawTilemapLayer[] expectedFrame1Layers = new RawTilemapLayer[] { expectedFrame1Layer0 };
        RawTilemapFrame expectedFrame1 = new(200, expectedFrame1Layers);

        //  *****************************************************
        //                        FRAME 2
        //  *****************************************************
        RawTilemapTile[] expectedFrame2Layer0Tiles = new RawTilemapTile[]
        {
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame2Layer0 = new("layer-0", 0, 4, 4, expectedFrame2Layer0Tiles, Point.Zero);
        RawTilemapLayer[] expectedFrame2Layers = new RawTilemapLayer[] { expectedFrame2Layer0 };
        RawTilemapFrame expectedFrame2 = new(300, expectedFrame2Layers);

        //  *****************************************************
        //                        FRAME 3
        //  *****************************************************
        RawTilemapTile[] expectedFrame3Layer0Tiles = new RawTilemapTile[]
        {
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame3Layer0 = new("layer-0", 0, 4, 4, expectedFrame3Layer0Tiles, Point.Zero);
        RawTilemapLayer[] expectedFrame3Layers = new RawTilemapLayer[] { expectedFrame3Layer0 };
        RawTilemapFrame expectedFrame3 = new(400, expectedFrame3Layers);

        RawTilemapFrame[] expectedFrames = new RawTilemapFrame[4]
        {
            expectedFrame0, expectedFrame1, expectedFrame2, expectedFrame3
        };

        RawAnimatedTilemap expectedTilemap = new(name, expectedTilesets, expectedFrames);
        string path = FileUtils.GetLocalPath("animated-tilemap-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawAnimatedTilemap actualTilemap = RawAnimatedTilemapProcessor.Process(aseFile, onlyVisibleLayers: true);

        Assert.Equal(expectedTilemap, actualTilemap);
    }

    [Fact]
    public void RawAnimatedTilemapProcessorTest_Process_OnlyVisibleLayers_FalseTest()
    {
        string name = "animated-tilemap-processor-test";

        //  Build expected texture for expected tileset 0
        Color[] tileset0Pixels = new Color[]
        {
            new(0, 0, 0, 0),
            new(223, 7, 114, 255),
            new(254, 84, 111, 255),
            new(255, 158, 125, 255),
            new(255, 208, 128, 255)
        };
        RawTexture expectedTileset0Texture = new("tileset-0", tileset0Pixels, 1, 5);

        //  Build expected texture for expected tileset 1
        Color[] tileset1Pixels = new Color[]
        {
            new(0, 0, 0, 0),
            new(255, 253, 255, 255),
            new(11, 255, 230, 255),
            new(1, 203, 207, 255),
            new(1, 136, 165, 255)
        };
        RawTexture expectedTileset1Texture = new("tileset-1", tileset1Pixels, 1, 5);


        //  Build expected tileset and tileset collection
        RawTileset expectedTileset0 = new(0, "tileset-0", expectedTileset0Texture, 1, 1);
        RawTileset expectedTileset1 = new(1, "tileset-1", expectedTileset1Texture, 1, 1);
        RawTileset[] expectedTilesets = new RawTileset[2] { expectedTileset0, expectedTileset1 };

        //  Build expected frames and frame collection
        //  *****************************************************
        //                        FRAME 0
        //  *****************************************************
        RawTilemapTile[] expectedFrame0Layer1Tiles = new RawTilemapTile[]
        {
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
        };

        RawTilemapTile[] expectedFrame0Layer0Tiles = new RawTilemapTile[]
        {
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame0Layer1 = new("layer-1", 1, 4, 4, expectedFrame0Layer1Tiles, Point.Zero);
        RawTilemapLayer expectedFrame0Layer0 = new("layer-0", 0, 4, 4, expectedFrame0Layer0Tiles, new(0, 1));
        RawTilemapLayer[] expectedFrame0Layers = new RawTilemapLayer[] { expectedFrame0Layer0, expectedFrame0Layer1 };
        RawTilemapFrame expectedFrame0 = new(100, expectedFrame0Layers);

        //  *****************************************************
        //                        FRAME 1
        //  *****************************************************
        RawTilemapTile[] expectedFrame1Layer1Tiles = new RawTilemapTile[]
        {
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
        };

        RawTilemapTile[] expectedFrame1Layer0Tiles = new RawTilemapTile[]
        {
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame1Layer1 = new("layer-1", 0, 4, 4, expectedFrame1Layer1Tiles, Point.Zero);
        RawTilemapLayer expectedFrame1Layer0 = new("layer-0", 0, 4, 4, expectedFrame1Layer0Tiles, Point.Zero);
        RawTilemapLayer[] expectedFrame1Layers = new RawTilemapLayer[] { expectedFrame1Layer0, expectedFrame1Layer1 };
        RawTilemapFrame expectedFrame1 = new(200, expectedFrame1Layers);

        //  *****************************************************
        //                        FRAME 2
        //  *****************************************************
        RawTilemapTile[] expectedFrame2Layer1Tiles = new RawTilemapTile[]
        {
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
        };

        RawTilemapTile[] expectedFrame2Layer0Tiles = new RawTilemapTile[]
        {
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame2Layer1 = new("layer-1", 0, 4, 4, expectedFrame2Layer1Tiles, Point.Zero);
        RawTilemapLayer expectedFrame2Layer0 = new("layer-0", 0, 4, 4, expectedFrame2Layer0Tiles, Point.Zero);
        RawTilemapLayer[] expectedFrame2Layers = new RawTilemapLayer[] { expectedFrame2Layer0, expectedFrame2Layer1 };
        RawTilemapFrame expectedFrame2 = new(300, expectedFrame2Layers);

        //  *****************************************************
        //                        FRAME 3
        //  *****************************************************
        RawTilemapTile[] expectedFrame3Layer1Tiles = new RawTilemapTile[]
        {
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
            new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f), new(0, false, false, 0.0f),
        };

        RawTilemapTile[] expectedFrame3Layer0Tiles = new RawTilemapTile[]
        {
            new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f), new(1, false, false, 0.0f),
            new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f), new(2, false, false, 0.0f),
            new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f), new(3, false, false, 0.0f),
        };
        RawTilemapLayer expectedFrame3Layer1 = new("layer-1", 0, 4, 4, expectedFrame3Layer1Tiles, Point.Zero);
        RawTilemapLayer expectedFrame3Layer0 = new("layer-0", 0, 4, 4, expectedFrame3Layer0Tiles, Point.Zero);
        RawTilemapLayer[] expectedFrame3Layers = new RawTilemapLayer[] { expectedFrame3Layer0, expectedFrame3Layer1 };
        RawTilemapFrame expectedFrame3 = new(400, expectedFrame3Layers);

        RawTilemapFrame[] expectedFrames = new RawTilemapFrame[4]
        {
            expectedFrame0, expectedFrame1, expectedFrame2, expectedFrame3
        };

        RawAnimatedTilemap expectedTilemap = new(name, expectedTilesets, expectedFrames);
        string path = FileUtils.GetLocalPath("animated-tilemap-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawAnimatedTilemap actualTilemap = RawAnimatedTilemapProcessor.Process(aseFile, onlyVisibleLayers: false);

        Assert.Equal(expectedTilemap, actualTilemap);
    }
}
