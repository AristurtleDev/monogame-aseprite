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
using MonoGame.Aseprite.IO;

namespace MonoGame.Aseprite.Tests;

public sealed class AsepriteFileReaderTests
{
    [Fact]
    public void AsepriteFileReader_ReadTest()
    {
        string path = FileUtils.GetLocalPath("read-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        ValidatePalette(aseFile);
        ValidateLayers(aseFile);
        ValidateTags(aseFile);
        ValidateFrames(aseFile);
        ValidateCels(aseFile);
    }

    [Fact]
    public void AsepriteFileReader_ColorDepthRgbaTest()
    {
        string path = FileUtils.GetLocalPath("color-depth-rgba-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        Color[] pixels = new Color[]
        {
            aseFile.Palette[0],
            aseFile.Palette[1],
            aseFile.Palette[2],
            aseFile.Palette[3],
            aseFile.Palette[4],
            aseFile.Palette[5],
            aseFile.Palette[6],
            aseFile.Palette[7],
        };

        AsepriteImageCel cel = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[0]);
        Assert.Equal(pixels, cel.Pixels.ToArray());
    }

    [Fact]
    public void AsepriteFileReader_ColorDepthIndexedTest()
    {
        string path = FileUtils.GetLocalPath("color-depth-indexed-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        Color[] pixels = new Color[]
        {
            aseFile.Palette[0],
            aseFile.Palette[1],
            aseFile.Palette[2],
            aseFile.Palette[3],
            aseFile.Palette[4],
            aseFile.Palette[5],
            aseFile.Palette[6],
            new Color(0, 0, 0, 0)
        };

        AsepriteImageCel cel = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[0]);
        Assert.Equal(pixels, cel.Pixels.ToArray());
    }

    [Fact]
    public void AsepriteFileReader_ColorDepthGrayscaleTest()
    {
        string path = FileUtils.GetLocalPath("color-depth-grayscale-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        Color[] pixels = new Color[]
        {
            aseFile.Palette[0],
            aseFile.Palette[1],
            aseFile.Palette[2],
            aseFile.Palette[3],
            aseFile.Palette[4],
            aseFile.Palette[5],
            aseFile.Palette[6],
            aseFile.Palette[7]
        };

        AsepriteImageCel cel = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[0]);
        Assert.Equal(pixels, cel.Pixels.ToArray());
    }


    private void ValidatePalette(AsepriteFile file)
    {
        Color[] expected = new Color[10];

        Assert.Equal(new Color(223, 7, 114, 255), file.Palette[0]);
        Assert.Equal(new Color(254, 84, 111, 255), file.Palette[1]);
        Assert.Equal(new Color(255, 158, 125, 255), file.Palette[2]);
        Assert.Equal(new Color(255, 208, 128, 255), file.Palette[3]);
        Assert.Equal(new Color(255, 253, 255, 255), file.Palette[4]);
        Assert.Equal(new Color(11, 255, 230, 255), file.Palette[5]);
        Assert.Equal(new Color(1, 203, 207, 255), file.Palette[6]);
        Assert.Equal(new Color(1, 136, 165, 255), file.Palette[7]);
        Assert.Equal(new Color(62, 50, 100, 255), file.Palette[8]);
        Assert.Equal(new Color(53, 42, 85, 255), file.Palette[9]);
    }

    private void ValidateLayers(AsepriteFile file)
    {
        Assert.Equal(11, file.Layers.Length);
        ValidateLayer(file.Layers[0], true, true, false, AsepriteBlendMode.Normal, 255, "background");
        ValidateLayer(file.Layers[1], true, false, false, AsepriteBlendMode.Normal, 255, "userdata");
        ValidateUserData(file.Layers[1].UserData, "layer-user-data", new Color(255, 208, 128, 255));
        ValidateLayer(file.Layers[2], true, false, false, AsepriteBlendMode.Normal, 75, "75-opacity");
        ValidateLayer(file.Layers[3], true, false, false, AsepriteBlendMode.Difference, 255, "blend-difference");
        ValidateLayer(file.Layers[4], false, false, false, AsepriteBlendMode.Normal, 255, "hidden");
        ValidateLayer(file.Layers[5], true, false, false, AsepriteBlendMode.Normal, 255, "tilemap");
        ValidateTilemapLayer(file.Layers[5], "tileset");
        ValidateLayer(file.Layers[6], true, false, false, AsepriteBlendMode.Normal, 0, "group");
        ValidateLayer(file.Layers[7], true, false, false, AsepriteBlendMode.Normal, 255, "child-2");
        ValidateLayer(file.Layers[8], true, false, false, AsepriteBlendMode.Normal, 255, "child-1");
        ValidateLayer(file.Layers[9], true, false, false, AsepriteBlendMode.Normal, 255, "normal");
        ValidateLayer(file.Layers[10], false, false, true, AsepriteBlendMode.Normal, 255, "reference");
    }

    private void ValidateLayer(AsepriteLayer layer, bool isVisible, bool isBackground, bool isReference, AsepriteBlendMode blendMode, int opacity, string name)
    {
        Assert.Equal(isVisible, layer.IsVisible);
        Assert.Equal(isBackground, layer.IsBackground);
        Assert.Equal(isReference, layer.IsReference);
        Assert.Equal(blendMode, layer.BlendMode);
        Assert.Equal(opacity, layer.Opacity);
        Assert.Equal(name, layer.Name);
    }

    private void ValidateTilemapLayer(AsepriteLayer layer, string tilesetName)
    {
        AsepriteTilemapLayer tilemapLayer = Assert.IsType<AsepriteTilemapLayer>(layer);
        Assert.Equal(tilesetName, tilemapLayer.Tileset.Name);
    }

    private void ValidateTags(AsepriteFile file)
    {
        Assert.Equal(4, file.Tags.Length);

        ValidateTag(file.Tags[0], 0, 2, AsepriteLoopDirection.Forward, "tag-1-3-forward", new Color(0, 0, 0, 255));
        ValidateTag(file.Tags[1], 3, 3, AsepriteLoopDirection.Forward, "tag-4-4-userdata", new Color(1, 136, 165, 255));
        ValidateUserData(file.Tags[1].UserData, "tag user data", new Color(1, 136, 165, 255));
        ValidateTag(file.Tags[2], 4, 4, AsepriteLoopDirection.Reverse, "tag-5-5-reverse", new Color(0, 0, 0, 255));
        ValidateTag(file.Tags[3], 5, 5, AsepriteLoopDirection.PingPong, "tag-6-6-ping-pong", new Color(0, 0, 0, 255));
    }

    private void ValidateTag(AsepriteTag tag, int from, int to, AsepriteLoopDirection direction, string name, Color color)
    {
        Assert.Equal(from, tag.From);
        Assert.Equal(to, tag.To);
        Assert.Equal(direction, tag.Direction);
        Assert.Equal(name, tag.Name);
        Assert.Equal(color, tag.Color);
    }

    private void ValidateFrames(AsepriteFile file)
    {
        Assert.Equal(6, file.Frames.Length);

        ValidateFrame(file.Frames[0], 5, file.CanvasWidth, file.CanvasHeight, 100);
        ValidateFrame(file.Frames[1], 2, file.CanvasWidth, file.CanvasHeight, 200);
        ValidateFrame(file.Frames[2], 2, file.CanvasWidth, file.CanvasHeight, 300);
        ValidateFrame(file.Frames[3], 1, file.CanvasWidth, file.CanvasHeight, 400);
        ValidateFrame(file.Frames[4], 1, file.CanvasWidth, file.CanvasHeight, 500);
        ValidateFrame(file.Frames[5], 1, file.CanvasWidth, file.CanvasHeight, 600);
    }

    private void ValidateFrame(AsepriteFrame frame, int celCount, int width, int height, int duration)
    {
        Assert.Equal(celCount, frame.Cels.Length);
        Assert.Equal(width, frame.Width);
        Assert.Equal(height, frame.Height);
        Assert.Equal(duration, frame.Duration);
    }

    private void ValidateCels(AsepriteFile file)
    {
        Color[] frame_0_cel_0_pixels = new Color[] { file.Palette[0], file.Palette[0], file.Palette[0], file.Palette[0], file.Palette[0], file.Palette[0], file.Palette[0], file.Palette[0] };
        ValidateImageCel(file.Frames[0].Cels[0], frame_0_cel_0_pixels, 2, 4, "background", Point.Zero, 255);

        Color[] frame_0_cel_1_pixels = new Color[] { file.Palette[1], file.Palette[1], file.Palette[1], file.Palette[1], file.Palette[1], file.Palette[1] };
        ValidateImageCel(file.Frames[0].Cels[1], frame_0_cel_1_pixels, 2, 3, "hidden", new Point(0, 1), 255);

        AsepriteTile[] frame_0_cel_2_tiles = new AsepriteTile[]
        {
            new AsepriteTile(1, 0, 0, 0),
            new AsepriteTile(2, 0, 0, 0),
            new AsepriteTile(3, 0, 0, 0),
            new AsepriteTile(4, 0, 0, 0)
        };
        ValidateTilemapCel(file.Frames[0].Cels[2], frame_0_cel_2_tiles, 2, 2, "tileset", "tilemap", new Point(0, 2), 255);

        Color[] frame_0_cel_3_pixels = new Color[] { file.Palette[6], file.Palette[6] };
        ValidateImageCel(file.Frames[0].Cels[3], frame_0_cel_3_pixels, 2, 1, "normal", new Point(0, 3), 255);

        Color[] frame_0_cel_4_pixels = new Color[] { file.Palette[9], file.Palette[9], file.Palette[9], file.Palette[9], file.Palette[9], file.Palette[9], file.Palette[9], file.Palette[9] };
        ValidateImageCel(file.Frames[0].Cels[4], frame_0_cel_4_pixels, 2, 4, "reference", Point.Zero, 255);
    }

    private void ValidateImageCel(AsepriteCel cel, Color[] pixels, int width, int height, string layerName, Point position, int opacity)
    {
        AsepriteImageCel imageCel = Assert.IsType<AsepriteImageCel>(cel);
        Assert.Equal(pixels, imageCel.Pixels.ToArray());
        Assert.Equal(width, imageCel.Width);
        Assert.Equal(height, imageCel.Height);
        Assert.Equal(layerName, cel.Layer.Name);
        Assert.Equal(position, cel.Position);
        Assert.Equal(opacity, cel.Opacity);
    }

    private void ValidateTilemapCel(AsepriteCel cel, AsepriteTile[] tiles, int columns, int rows, string tilesetName, string layerName, Point position, int opacity)
    {
        AsepriteTilemapCel tilemapCel = Assert.IsType<AsepriteTilemapCel>(cel);
        AsepriteTile[] actualTiles = tilemapCel.Tiles.ToArray();
        Assert.Equal(tiles, actualTiles);
        Assert.Equal(columns, tilemapCel.Columns);
        Assert.Equal(rows, tilemapCel.Rows);
        Assert.Equal(layerName, cel.Layer.Name);
        Assert.Equal(position, cel.Position);
        Assert.Equal(opacity, cel.Opacity);
    }

    private void ValidateUserData(AsepriteUserData userData, string? text, Color? color)
    {
        if (text is not null)
        {
            Assert.True(userData.HasText);
            Assert.Equal(text, userData.Text);
        }
        else
        {
            Assert.False(userData.HasText);
        }

        if (color is not null)
        {
            Assert.True(userData.HasColor);
            Assert.Equal(color, userData.Color);
        }
        else
        {
            Assert.False(userData.HasColor);
        }
    }
}
