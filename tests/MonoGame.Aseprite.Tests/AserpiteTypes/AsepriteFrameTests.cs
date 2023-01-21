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

public sealed class AsepriteFrameTests
{
    [Theory]
    [InlineData(true, false, false)]
    [InlineData(true, true, false)]
    [InlineData(true, false, true)]
    [InlineData(true, true, true)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    [InlineData(false, true, true)]
    [InlineData(false, false, false)]
    public void AsepriteFrame_FlattenFrameTest(bool onlyVisible, bool includeBackground, bool includeTilemap)
    {
        AsepriteTileset tileset = new(0, 2, 1, 1, "tileset", new Color[] { Color.Transparent, Color.Yellow });

        AsepriteLayer layer_0_background = new(AsepriteLayerFlags.Background | AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "background");
        AsepriteLayer layer_1_invisible = new(AsepriteLayerFlags.None, AsepriteBlendMode.Normal, 255, "invisible");
        AsepriteTilemapLayer layer_2_tilemap_visible = new AsepriteTilemapLayer(tileset, AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "tilemap");
        AsepriteLayer layer_3_Visible = new(AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "top");

        Color[] layer_0_background_cel_pixels = new Color[]
        {
            Color.Red, Color.Red,
            Color.Red, Color.Red,
            Color.Red, Color.Red,
            Color.Red, Color.Red
        };

        Color[] layer_1_invisible_cel_pixels = new Color[]
        {
            Color.Transparent, Color.Transparent,
            Color.Orange, Color.Orange,
            Color.Orange, Color.Orange,
            Color.Orange, Color.Orange
        };

        AsepriteTile[] layer_2_tilemap_tiles = new AsepriteTile[]
        {
            new AsepriteTile(0, 0, 0, 0), new AsepriteTile(0, 0, 0, 0),
            new AsepriteTile(0, 0, 0, 0), new AsepriteTile(0, 0, 0, 0),
            new AsepriteTile(1, 0, 0, 0), new AsepriteTile(1, 0, 0, 0),
            new AsepriteTile(1, 0, 0, 0), new AsepriteTile(1, 0, 0, 0),
        };

        Color[] layer_3_visible_cel_pixels = new Color[]
        {
            Color.Transparent, Color.Transparent,
            Color.Transparent, Color.Transparent,
            Color.Transparent, Color.Transparent,
            Color.Green, Color.Green
        };

        AsepriteImageCel layer_0_cel = new(2, 4, layer_0_background_cel_pixels, layer_0_background, Point.Zero, 255);
        AsepriteImageCel layer_1_cel = new(2, 4, layer_1_invisible_cel_pixels, layer_1_invisible, Point.Zero, 255);
        AsepriteTilemapCel layer_2_cel = new(2, 4, layer_2_tilemap_tiles, layer_2_tilemap_visible, Point.Zero, 255);
        AsepriteImageCel layer_3_cel = new(2, 3, layer_3_visible_cel_pixels, layer_3_Visible, Point.Zero, 255);

        AsepriteFrame frame = new(2, 4, 0, new AsepriteCel[] { layer_0_cel, layer_1_cel, layer_2_cel, layer_3_cel });

        Color[] expected = new Color[8];

        expected[0] = includeBackground ? layer_0_background_cel_pixels[0] : Color.Transparent;
        expected[1] = includeBackground ? layer_0_background_cel_pixels[1] : Color.Transparent;

        expected[2] = !onlyVisible ? layer_1_invisible_cel_pixels[2] :
                      includeBackground ? layer_0_background_cel_pixels[2] :
                      Color.Transparent;

        expected[3] = !onlyVisible ? layer_1_invisible_cel_pixels[3] :
                      includeBackground ? layer_0_background_cel_pixels[3] :
                      Color.Transparent;

        expected[4] = includeTilemap ? tileset[layer_2_tilemap_tiles[4].TilesetTileID][0] :
                      !onlyVisible ? layer_1_invisible_cel_pixels[4] :
                      includeBackground ? layer_0_background_cel_pixels[4] :
                      Color.Transparent;

        expected[5] = includeTilemap ? tileset[layer_2_tilemap_tiles[5].TilesetTileID][0] :
                      !onlyVisible ? layer_1_invisible_cel_pixels[5] :
                      includeBackground ? layer_0_background_cel_pixels[5] :
                      Color.Transparent;

        expected[6] = layer_3_visible_cel_pixels[6];
        expected[7] = layer_3_visible_cel_pixels[7];

        Color[] actual = frame.FlattenFrame(onlyVisible, includeBackground, includeTilemap);
        Assert.Equal(expected, actual);
    }
}
