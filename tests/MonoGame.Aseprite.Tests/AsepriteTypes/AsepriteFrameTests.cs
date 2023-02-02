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

namespace MonoGame.Aseprite.Tests;

public sealed class AsepriteFrameTestsFixture
{
    public string Name { get; }
    public AsepriteFrame Frame { get; }

    public AsepriteFrameTestsFixture()
    {
        Name = "aseprite-frame-test";

        AsepriteTileset[] tilesets = new AsepriteTileset[]
        {
            new(0, 4, 1, 1, "tileset", new Color[] {Color.Transparent, Color.Yellow, Color.Purple, Color.Teal})
        };

        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new(AsepriteLayerFlags.Visible | AsepriteLayerFlags.Background, AsepriteBlendMode.Normal, 255, "background"),
            new(AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "visible"),
            new AsepriteTilemapLayer(tilesets[0], AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "tilemap"),
            new(AsepriteLayerFlags.None, AsepriteBlendMode.Normal, 255, "hidden")
        };

        AsepriteTile[] tiles = new AsepriteTile[]
        {
            new(1, 0, 0, 0),
            new(2, 0, 0, 0),
            new(3, 0, 0, 0),
            new(0, 0, 0, 0)
        };

        AsepriteCel[] cels = new AsepriteCel[]
        {
            new AsepriteImageCel(2, 2, new Color[]{Color.Black, Color.Black, Color.Black, Color.Black}, layers[0], Point.Zero, 255),
            new AsepriteImageCel(2, 2, new Color[]{Color.Red, Color.Transparent, Color.Transparent, Color.Red}, layers[1], Point.Zero, 255),
            new AsepriteTilemapCel(2, 2, tiles, layers[2], Point.Zero, 255),
            new AsepriteImageCel(2, 1, new Color[] {Color.White, Color.White}, layers[3], new Point(0, 1), 255)
        };

        Frame = new($"{Name} 0", 2, 2, 100, cels);
    }
}

public sealed class AsepriteFrameTests : IClassFixture<AsepriteFrameTestsFixture>
{
    private readonly AsepriteFrameTestsFixture _fixture;

    public AsepriteFrameTests(AsepriteFrameTestsFixture fixture) => _fixture = fixture;

    [Fact]
    public void Flatten_OnlyVisible_True_IncludeBackground_True_IncludeTilemap_True()
    {
        Color[] expected = new Color[] { Color.Yellow, Color.Purple, Color.Teal, Color.Red };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: true,
                                                     includeBackgroundLayer: true,
                                                     includeTilemapCel: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_True_IncludeBackground_True_IncludeTilemap_False()
    {
        Color[] expected = new Color[] { Color.Red, Color.Black, Color.Black, Color.Red };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: true,
                                                     includeBackgroundLayer: true,
                                                     includeTilemapCel: false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_True_IncludeBackground_False_IncludeTilemap_True()
    {
        Color[] expected = new Color[] { Color.Yellow, Color.Purple, Color.Teal, Color.Red };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: true,
                                                     includeBackgroundLayer: false,
                                                     includeTilemapCel: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_True_IncludeBackground_False_IncludeTilemap_False()
    {
        Color[] expected = new Color[] { Color.Red, Color.Transparent, Color.Transparent, Color.Red };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: true,
                                                     includeBackgroundLayer: false,
                                                     includeTilemapCel: false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_False_IncludeBackground_True_IncludeTilemap_True()
    {
        Color[] expected = new Color[] { Color.Yellow, Color.Purple, Color.White, Color.White };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: false,
                                                     includeBackgroundLayer: true,
                                                     includeTilemapCel: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_False_IncludeBackground_True_IncludeTilemap_False()
    {
        Color[] expected = new Color[] { Color.Red, Color.Black, Color.White, Color.White };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: false,
                                                     includeBackgroundLayer: true,
                                                     includeTilemapCel: false);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_False_IncludeBackground_False_IncludeTilemap_True()
    {
        Color[] expected = new Color[] { Color.Yellow, Color.Purple, Color.White, Color.White };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: false,
                                                     includeBackgroundLayer: false,
                                                     includeTilemapCel: true);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Flatten_OnlyVisible_False_IncludeBackground_False_IncludeTilemap_False()
    {
        Color[] expected = new Color[] {Color.Red, Color.Transparent, Color.White, Color.White };
        Color[] actual = _fixture.Frame.FlattenFrame(onlyVisibleLayers: false,
                                                     includeBackgroundLayer: false,
                                                     includeTilemapCel: false);

        Assert.Equal(expected, actual);
    }
}
