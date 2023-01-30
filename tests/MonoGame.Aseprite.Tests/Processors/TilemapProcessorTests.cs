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
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Tests;

public sealed class TilemapProcessorTests
{
    [Fact]
    public void TilemapProcessorTest_CreateRawTilemap()
    {
        string path = FileUtils.GetLocalPath("tilemap-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        //  ********************************************************
        //  Default configuration
        //  ********************************************************
        TilemapProcessorConfiguration config = new();

        RawTilemap tilemap = AnimatedTilemapProcessor.CreateRawTilemap(aseFile, config);

        Assert.Equal("tilemap-processor-test", tilemap.Name);

        //  The test file should have two tilesets total, but one layer is hidden, so only the tileset for the visible
        //  layer should have been processed
        Assert.Equal(1, tilemap.Tilesets.Length);

        //  Ensure it got the correct tileset
        RawTileset tileset = tilemap.Tilesets[0];
        Assert.Equal("tileset-0", tileset.Name);

        //  There should only be the one layer that was visible
        Assert.Equal(1, tilemap.Layers.Length);

        //  Ensure it is the correct layer
        RawTilemapLayer layer = tilemap.Layers[0];
        Assert.Equal("layer-0", layer.Name);
        Assert.Equal(tileset.ID, layer.TilesetID);
        Assert.Equal(4, layer.Columns);
        Assert.Equal(3, layer.Rows);

        //  Ensure offset is read correctly
        Assert.Equal(new Point(0, 1), layer.Offset);

        RawTile[] tiles = new RawTile[]
        {
            new(2, false, false, 0.0f),
            new(2, false, false, 0.0f),
            new(2, false, false, 0.0f),
            new(2, false, false, 0.0f),
            new(3, false, false, 0.0f),
            new(3, false, false, 0.0f),
            new(3, false, false, 0.0f),
            new(3, false, false, 0.0f),
            new(4, false, false, 0.0f),
            new(4, false, false, 0.0f),
            new(4, false, false, 0.0f),
            new(4, false, false, 0.0f)
        };

        Assert.Equal(tiles, layer.RawTilemapTiles.ToArray());
    }

    [Fact]
    public void TilemapProcessorTest_CreateRawTilemap_OnlyVisibleLayers_FalseTest()
    {
        string path = FileUtils.GetLocalPath("tilemap-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        //  ********************************************************
        //  Default configuration
        //  ********************************************************
        TilemapProcessorConfiguration config = new()
        {
            OnlyVisibleLayers = false
        };

        RawTilemap tilemap = AnimatedTilemapProcessor.CreateRawTilemap(aseFile, config);

        //  One layer is hidden, but since only visible layers is false, we should still have two layers
        Assert.Equal(2, tilemap.Layers.Length);
        Assert.Equal("layer-0", tilemap.Layers[0].Name);
        Assert.Equal("layer-1", tilemap.Layers[1].Name);
    }
}
