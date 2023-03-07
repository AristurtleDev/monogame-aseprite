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
using MonoGame.Aseprite.Content.Readers;

namespace MonoGame.Aseprite.Tests;

public sealed class AsepriteFileReaderTests
{
    private readonly Color _black = new Color(0, 0, 0, 255);
    private readonly Color _white = new Color(255, 255, 255, 255);
    private readonly Color _red = new Color(255, 0, 0, 255);
    private readonly Color _green = new Color(0, 255, 0, 255);
    private readonly Color _blue = new Color(0, 0, 255, 255);
    private readonly Color _transparent = new Color(0, 0, 0, 0);


    [Fact]
    public void ReadsStream_Version_1_3_0_RC1_Expected_Values()
    {
        string path = FileUtils.GetLocalPath("aseprite-1.3.0-rc1-file-reader-test.aseprite");
        Stream fileStream = File.OpenRead(path);
        AsepriteFile aseFile = AsepriteFileReader.ReadStream(Path.GetFileNameWithoutExtension(path), fileStream);

        Reads_Version_1_3_0_RC1_Assertions(aseFile);
    }

    [Fact]
    public void Reads_Version_1_3_0_RC1_Expected_Values()
    {
        string path = FileUtils.GetLocalPath("aseprite-1.3.0-rc1-file-reader-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        Reads_Version_1_3_0_RC1_Assertions(aseFile);
    }

    private void Reads_Version_1_3_0_RC1_Assertions(AsepriteFile aseFile)
    {
        //  ************************************************************
        //  File properties
        //  ************************************************************
        Assert.Equal("aseprite-1.3.0-rc1-file-reader-test", aseFile.Name);
        Assert.Equal(2, aseFile.CanvasWidth);
        Assert.Equal(2, aseFile.CanvasHeight);
        AssertUserData(aseFile.UserData, "hello sprite", _blue);

        //  ************************************************************
        //  Layers
        //  ************************************************************
        Assert.Equal(9, aseFile.Layers.Length);
        AssertLayer(aseFile.Layers[0], "background", true, true, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[1], "hidden", false, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[2], "100-opacity", true, false, false, AsepriteBlendMode.Normal, 100, null, null);
        AssertLayer(aseFile.Layers[3], "userdata", true, false, false, AsepriteBlendMode.Normal, 255, "hello layer", _blue);
        AssertLayer(aseFile.Layers[4], "group", true, false, false, AsepriteBlendMode.Normal, 0, null, null);
        AssertLayer(aseFile.Layers[5], "child", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[6], "tilemap", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AsepriteTilemapLayer tilemapLayer = Assert.IsType<AsepriteTilemapLayer>(aseFile.Layers[6]);
        AssertTilemapLayer(tilemapLayer, "tileset");
        AssertLayer(aseFile.Layers[7], "eight-frames", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[8], "blend-color-dodge", true, false, false, AsepriteBlendMode.ColorDodge, 255, null, null);


        //  ************************************************************
        //  Tags
        //  ************************************************************
        Assert.Equal(6, aseFile.Tags.Length);
        AssertTag(aseFile.Tags[0], "forward", 0, 0, AsepriteLoopDirection.Forward, 0, _black, null, _black);
        AssertTag(aseFile.Tags[1], "reversed", 1, 1, AsepriteLoopDirection.Reverse, 0, _black, null, _black);
        AssertTag(aseFile.Tags[2], "pingpong", 2, 2, AsepriteLoopDirection.PingPong, 0, _black, null, _black);
        AssertTag(aseFile.Tags[3], "frames-4-to-6", 3, 5, AsepriteLoopDirection.Forward, 0, _black, null, _black);
        AssertTag(aseFile.Tags[4], "userdata", 6, 6, AsepriteLoopDirection.Forward, 0, _green, "hello tag", _green);
        AssertTag(aseFile.Tags[5], "repeat-x100", 7, 7, AsepriteLoopDirection.Forward, 100, _black, null, _black);

        //  ************************************************************
        //  Frames
        //  ************************************************************
        Assert.Equal(8, aseFile.Frames.Length);
        AssertFrame(aseFile.Frames[0], 3, 2, 2, 100);
        AssertFrame(aseFile.Frames[1], 2, 2, 2, 200);
        AssertFrame(aseFile.Frames[2], 2, 2, 2, 300);
        AssertFrame(aseFile.Frames[3], 2, 2, 2, 400);
        AssertFrame(aseFile.Frames[4], 2, 2, 2, 500);
        AssertFrame(aseFile.Frames[5], 2, 2, 2, 600);
        AssertFrame(aseFile.Frames[6], 2, 2, 2, 700);
        AssertFrame(aseFile.Frames[7], 2, 2, 2, 800);

        //  ************************************************************
        //  Cels
        //  ************************************************************
        //  Frame 0
        Assert.Equal(3, aseFile.Frames[0].Cels.Length);
        AssertCel(aseFile.Frames[0].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame0Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[0]);
        AssertImageCel(frame0Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[0].Cels[1], "tilemap", 0, 0, 255, null, null);
        AsepriteTilemapCel frame0cel1 = Assert.IsType<AsepriteTilemapCel>(aseFile.Frames[0].Cels[1]);
        AssertTilemapCel(frame0cel1, 2, 2, 4, "tileset");
        AssertTile(frame0cel1.Tiles[0], 1, 0, 0, 0);
        AssertTile(frame0cel1.Tiles[1], 2, 0, 0, 0);
        AssertTile(frame0cel1.Tiles[2], 3, 0, 0, 0);
        AssertTile(frame0cel1.Tiles[3], 4, 0, 0, 0);

        AssertCel(aseFile.Frames[0].Cels[2], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame0cel2 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[2]);
        AssertImageCel(frame0cel2, 1, 1, new Color[] { _red });

        //  Frame 1
        AssertCel(aseFile.Frames[1].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame1Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[1].Cels[0]);
        AssertImageCel(frame1Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[1].Cels[1], "eight-frames", 1, 0, 255, null, null);
        AsepriteImageCel frame1cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[1].Cels[1]);
        AssertImageCel(frame1cel1, 1, 1, new Color[] { _red });

        //  Frame 2
        AssertCel(aseFile.Frames[2].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame2Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[2].Cels[0]);
        AssertImageCel(frame2Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[2].Cels[1], "eight-frames", 0, 1, 255, null, null);
        AsepriteImageCel frame2cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[2].Cels[1]);
        AssertImageCel(frame2cel1, 1, 1, new Color[] { _red });

        //  Frame 3
        AssertCel(aseFile.Frames[3].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame3cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[3].Cels[0]);
        AssertImageCel(frame3cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[3].Cels[1], "eight-frames", 1, 1, 255, null, null);
        AsepriteImageCel frame3cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[3].Cels[1]);
        AssertImageCel(frame3cel1, 1, 1, new Color[] { _red });

        //  Frame 4
        AssertCel(aseFile.Frames[4].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame4cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[4].Cels[0]);
        AssertImageCel(frame4cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[4].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame4cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[4].Cels[1]);
        AssertImageCel(frame4cel1, 2, 1, new Color[] { _red, _red });

        //  Frame 5
        AssertCel(aseFile.Frames[5].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame5cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[5].Cels[0]);
        AssertImageCel(frame5cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[5].Cels[1], "eight-frames", 0, 1, 255, null, null);
        AsepriteImageCel frame5cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[5].Cels[1]);
        AssertImageCel(frame5cel1, 2, 1, new Color[] { _red, _red });

        //  Frame 6
        AssertCel(aseFile.Frames[6].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame6cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[6].Cels[0]);
        AssertImageCel(frame6cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[6].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame6cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[6].Cels[1]);
        AssertImageCel(frame6cel1, 2, 2, new Color[] { _red, _red, _red, _transparent });

        //  Frame 7
        AssertCel(aseFile.Frames[7].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame7cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[7].Cels[0]);
        AssertImageCel(frame7cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[7].Cels[1], "eight-frames", 0, 0, 255, "hello cel", _red);
        AsepriteImageCel frame7cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[7].Cels[1]);
        AssertImageCel(frame7cel1, 2, 2, new Color[] { _red, _red, _transparent, _red });

        //  ************************************************************
        //  Slices
        //  ************************************************************
        Assert.Equal(1, aseFile.Slices.Length);
        AssertSlice(aseFile.Slices[0], "slice", 1, true, true, "hello slice", _green);
        AssertSliceKey(aseFile.Slices[0].Keys[0], 0, new Rectangle(0, 0, 2, 2), new Rectangle(0, 0, 1, 2), new Point(1, 2));

        //  ************************************************************
        //  Tilesets
        //  ************************************************************
        Assert.Equal(1, aseFile.Tilesets.Length);
        AssertTileset(aseFile.Tilesets[0], 0, "tileset", 5, 1, 1, 1, 5, new Color[] { _transparent, _white, _red, _green, _blue });
    }



    [Fact]
    public void ReadsStream_Version_1_3_0_Expected_Values()
    {
        string path = FileUtils.GetLocalPath("aseprite-1.3.0-file-reader-test.aseprite");
        Stream fileStream = File.OpenRead(path);
        AsepriteFile aseFile = AsepriteFileReader.ReadStream(Path.GetFileNameWithoutExtension(path), fileStream);

        Reads_Version_1_3_0_Assertions(aseFile);
    }

    [Fact]
    public void Reads_Version_1_3_0_Expected_Values()
    {
        string path = FileUtils.GetLocalPath("aseprite-1.3.0-file-reader-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        Reads_Version_1_3_0_Assertions(aseFile);
    }

    private void Reads_Version_1_3_0_Assertions(AsepriteFile aseFile)
    {
        //  ************************************************************
        //  File properties
        //  ************************************************************
        Assert.Equal("aseprite-1.3.0-file-reader-test", aseFile.Name);
        Assert.Equal(2, aseFile.CanvasWidth);
        Assert.Equal(2, aseFile.CanvasHeight);
        AssertUserData(aseFile.UserData, "hello sprite", _blue);

        //  ************************************************************
        //  Layers
        //  ************************************************************
        Assert.Equal(9, aseFile.Layers.Length);
        AssertLayer(aseFile.Layers[0], "background", true, true, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[1], "hidden", false, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[2], "100-opacity", true, false, false, AsepriteBlendMode.Normal, 100, null, null);
        AssertLayer(aseFile.Layers[3], "userdata", true, false, false, AsepriteBlendMode.Normal, 255, "hello layer", _blue);
        AssertLayer(aseFile.Layers[4], "group", true, false, false, AsepriteBlendMode.Normal, 0, null, null);
        AssertLayer(aseFile.Layers[5], "child", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[6], "tilemap", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AsepriteTilemapLayer tilemapLayer = Assert.IsType<AsepriteTilemapLayer>(aseFile.Layers[6]);
        AssertTilemapLayer(tilemapLayer, "tileset");
        AssertLayer(aseFile.Layers[7], "eight-frames", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[8], "blend-color-dodge", true, false, false, AsepriteBlendMode.ColorDodge, 255, null, null);


        //  ************************************************************
        //  Tags
        //  ************************************************************
        Assert.Equal(5, aseFile.Tags.Length);
        AssertTag(aseFile.Tags[0], "forward", 0, 0, AsepriteLoopDirection.Forward, 0, _black, null, _black);
        AssertTag(aseFile.Tags[1], "reversed", 1, 1, AsepriteLoopDirection.Reverse, 0, _black, null, _black);
        AssertTag(aseFile.Tags[2], "pingpong", 2, 2, AsepriteLoopDirection.PingPong, 0, _black, null, _black);
        AssertTag(aseFile.Tags[3], "frames-4-to-6", 3, 5, AsepriteLoopDirection.Forward, 0, _black, null, _black);
        AssertTag(aseFile.Tags[4], "userdata", 6, 6, AsepriteLoopDirection.Forward, 0, _green, "hello tag", _green);

        //  ************************************************************
        //  Frames
        //  ************************************************************
        Assert.Equal(8, aseFile.Frames.Length);
        AssertFrame(aseFile.Frames[0], 3, 2, 2, 100);
        AssertFrame(aseFile.Frames[1], 2, 2, 2, 200);
        AssertFrame(aseFile.Frames[2], 2, 2, 2, 300);
        AssertFrame(aseFile.Frames[3], 2, 2, 2, 400);
        AssertFrame(aseFile.Frames[4], 2, 2, 2, 500);
        AssertFrame(aseFile.Frames[5], 2, 2, 2, 600);
        AssertFrame(aseFile.Frames[6], 2, 2, 2, 700);
        AssertFrame(aseFile.Frames[7], 2, 2, 2, 800);

        //  ************************************************************
        //  Cels
        //  ************************************************************
        //  Frame 0
        Assert.Equal(3, aseFile.Frames[0].Cels.Length);
        AssertCel(aseFile.Frames[0].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame0Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[0]);
        AssertImageCel(frame0Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[0].Cels[1], "tilemap", 0, 0, 255, null, null);
        AsepriteTilemapCel frame0cel1 = Assert.IsType<AsepriteTilemapCel>(aseFile.Frames[0].Cels[1]);
        AssertTilemapCel(frame0cel1, 2, 2, 4, "tileset");
        AssertTile(frame0cel1.Tiles[0], 1, 0, 0, 0);
        AssertTile(frame0cel1.Tiles[1], 2, 0, 0, 0);
        AssertTile(frame0cel1.Tiles[2], 3, 0, 0, 0);
        AssertTile(frame0cel1.Tiles[3], 4, 0, 0, 0);

        AssertCel(aseFile.Frames[0].Cels[2], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame0cel2 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[2]);
        AssertImageCel(frame0cel2, 1, 1, new Color[] { _red });

        //  Frame 1
        AssertCel(aseFile.Frames[1].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame1Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[1].Cels[0]);
        AssertImageCel(frame1Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[1].Cels[1], "eight-frames", 1, 0, 255, null, null);
        AsepriteImageCel frame1cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[1].Cels[1]);
        AssertImageCel(frame1cel1, 1, 1, new Color[] { _red });

        //  Frame 2
        AssertCel(aseFile.Frames[2].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame2Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[2].Cels[0]);
        AssertImageCel(frame2Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[2].Cels[1], "eight-frames", 0, 1, 255, null, null);
        AsepriteImageCel frame2cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[2].Cels[1]);
        AssertImageCel(frame2cel1, 1, 1, new Color[] { _red });

        //  Frame 3
        AssertCel(aseFile.Frames[3].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame3cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[3].Cels[0]);
        AssertImageCel(frame3cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[3].Cels[1], "eight-frames", 1, 1, 255, null, null);
        AsepriteImageCel frame3cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[3].Cels[1]);
        AssertImageCel(frame3cel1, 1, 1, new Color[] { _red });

        //  Frame 4
        AssertCel(aseFile.Frames[4].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame4cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[4].Cels[0]);
        AssertImageCel(frame4cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[4].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame4cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[4].Cels[1]);
        AssertImageCel(frame4cel1, 2, 1, new Color[] { _red, _red });

        //  Frame 5
        AssertCel(aseFile.Frames[5].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame5cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[5].Cels[0]);
        AssertImageCel(frame5cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[5].Cels[1], "eight-frames", 0, 1, 255, null, null);
        AsepriteImageCel frame5cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[5].Cels[1]);
        AssertImageCel(frame5cel1, 2, 1, new Color[] { _red, _red });

        //  Frame 6
        AssertCel(aseFile.Frames[6].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame6cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[6].Cels[0]);
        AssertImageCel(frame6cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[6].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame6cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[6].Cels[1]);
        AssertImageCel(frame6cel1, 2, 2, new Color[] { _red, _red, _red, _transparent });

        //  Frame 7
        AssertCel(aseFile.Frames[7].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame7cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[7].Cels[0]);
        AssertImageCel(frame7cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[7].Cels[1], "eight-frames", 0, 0, 255, "hello cel", _red);
        AsepriteImageCel frame7cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[7].Cels[1]);
        AssertImageCel(frame7cel1, 2, 2, new Color[] { _red, _red, _transparent, _red });

        //  ************************************************************
        //  Slices
        //  ************************************************************
        Assert.Equal(1, aseFile.Slices.Length);
        AssertSlice(aseFile.Slices[0], "slice", 1, true, true, "hello slice", _green);
        AssertSliceKey(aseFile.Slices[0].Keys[0], 0, new Rectangle(0, 0, 2, 2), new Rectangle(0, 0, 1, 2), new Point(1, 2));

        //  ************************************************************
        //  Tilesets
        //  ************************************************************
        Assert.Equal(1, aseFile.Tilesets.Length);
        AssertTileset(aseFile.Tilesets[0], 0, "tileset", 5, 1, 1, 1, 5, new Color[] { _transparent, _white, _red, _green, _blue });
    }

    [Fact]
    public void Reads_Version_1_2_9_Expected_Values()
    {
        string path = FileUtils.GetLocalPath("aseprite-1.2.9-file-reader-test.aseprite");
        AsepriteFile aseFile = AsepriteFileReader.ReadFile(path);

        //  ************************************************************
        //  File properties
        //  ************************************************************
        Assert.Equal("aseprite-1.2.9-file-reader-test", aseFile.Name);
        Assert.Equal(2, aseFile.CanvasWidth);
        Assert.Equal(2, aseFile.CanvasHeight);
        AssertUserData(aseFile.UserData, default, default);

        //  ************************************************************
        //  Layers
        //  ************************************************************
        Assert.Equal(8, aseFile.Layers.Length);
        AssertLayer(aseFile.Layers[0], "background", true, true, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[1], "hidden", false, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[2], "100-opacity", true, false, false, AsepriteBlendMode.Normal, 100, null, null);
        AssertLayer(aseFile.Layers[3], "userdata", true, false, false, AsepriteBlendMode.Normal, 255, "hello layer", _blue);
        AssertLayer(aseFile.Layers[4], "group", true, false, false, AsepriteBlendMode.Normal, 0, null, null);
        AssertLayer(aseFile.Layers[5], "child", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[6], "eight-frames", true, false, false, AsepriteBlendMode.Normal, 255, null, null);
        AssertLayer(aseFile.Layers[7], "blend-color-dodge", true, false, false, AsepriteBlendMode.ColorDodge, 255, null, null);


        //  ************************************************************
        //  Tags
        //  ************************************************************
        Assert.Equal(5, aseFile.Tags.Length);
        AssertTag(aseFile.Tags[0], "forward", 0, 0, AsepriteLoopDirection.Forward, 0, _black, null, null);
        AssertTag(aseFile.Tags[1], "reversed", 1, 1, AsepriteLoopDirection.Reverse, 0, _black, null, null);
        AssertTag(aseFile.Tags[2], "pingpong", 2, 2, AsepriteLoopDirection.PingPong, 0, _black, null, null);
        AssertTag(aseFile.Tags[3], "frames-4-to-6", 3, 5, AsepriteLoopDirection.Forward, 0, _black, null, null);
        AssertTag(aseFile.Tags[4], "userdata", 6, 6, AsepriteLoopDirection.Forward, 0, _green, null, null);

        //  ************************************************************
        //  Frames
        //  ************************************************************
        Assert.Equal(8, aseFile.Frames.Length);
        AssertFrame(aseFile.Frames[0], 2, 2, 2, 100);
        AssertFrame(aseFile.Frames[1], 2, 2, 2, 200);
        AssertFrame(aseFile.Frames[2], 2, 2, 2, 300);
        AssertFrame(aseFile.Frames[3], 2, 2, 2, 400);
        AssertFrame(aseFile.Frames[4], 2, 2, 2, 500);
        AssertFrame(aseFile.Frames[5], 2, 2, 2, 600);
        AssertFrame(aseFile.Frames[6], 2, 2, 2, 700);
        AssertFrame(aseFile.Frames[7], 2, 2, 2, 800);

        //  ************************************************************
        //  Cels
        //  ************************************************************
        //  Frame 0
        Assert.Equal(2, aseFile.Frames[0].Cels.Length);
        AssertCel(aseFile.Frames[0].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame0Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[0]);
        AssertImageCel(frame0Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[0].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame0cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[0].Cels[1]);
        AssertImageCel(frame0cel1, 1, 1, new Color[] { _red });

        //  Frame 1
        AssertCel(aseFile.Frames[1].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame1Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[1].Cels[0]);
        AssertImageCel(frame1Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[1].Cels[1], "eight-frames", 1, 0, 255, null, null);
        AsepriteImageCel frame1cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[1].Cels[1]);
        AssertImageCel(frame1cel1, 1, 1, new Color[] { _red });

        //  Frame 2
        AssertCel(aseFile.Frames[2].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame2Cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[2].Cels[0]);
        AssertImageCel(frame2Cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[2].Cels[1], "eight-frames", 0, 1, 255, null, null);
        AsepriteImageCel frame2cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[2].Cels[1]);
        AssertImageCel(frame2cel1, 1, 1, new Color[] { _red });

        //  Frame 3
        AssertCel(aseFile.Frames[3].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame3cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[3].Cels[0]);
        AssertImageCel(frame3cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[3].Cels[1], "eight-frames", 1, 1, 255, null, null);
        AsepriteImageCel frame3cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[3].Cels[1]);
        AssertImageCel(frame3cel1, 1, 1, new Color[] { _red });

        //  Frame 4
        AssertCel(aseFile.Frames[4].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame4cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[4].Cels[0]);
        AssertImageCel(frame4cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[4].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame4cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[4].Cels[1]);
        AssertImageCel(frame4cel1, 2, 1, new Color[] { _red, _red });

        //  Frame 5
        AssertCel(aseFile.Frames[5].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame5cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[5].Cels[0]);
        AssertImageCel(frame5cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[5].Cels[1], "eight-frames", 0, 1, 255, null, null);
        AsepriteImageCel frame5cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[5].Cels[1]);
        AssertImageCel(frame5cel1, 2, 1, new Color[] { _red, _red });

        //  Frame 6
        AssertCel(aseFile.Frames[6].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame6cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[6].Cels[0]);
        AssertImageCel(frame6cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[6].Cels[1], "eight-frames", 0, 0, 255, null, null);
        AsepriteImageCel frame6cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[6].Cels[1]);
        AssertImageCel(frame6cel1, 2, 2, new Color[] { _red, _red, _red, _transparent });

        //  Frame 7
        AssertCel(aseFile.Frames[7].Cels[0], "background", 0, 0, 255, null, null);
        AsepriteImageCel frame7cel0 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[7].Cels[0]);
        AssertImageCel(frame7cel0, 2, 2, new Color[] { _black, _black, _black, _black });

        AssertCel(aseFile.Frames[7].Cels[1], "eight-frames", 0, 0, 255, "hello cel", _red);
        AsepriteImageCel frame7cel1 = Assert.IsType<AsepriteImageCel>(aseFile.Frames[7].Cels[1]);
        AssertImageCel(frame7cel1, 2, 2, new Color[] { _red, _red, _transparent, _red });

        //  ************************************************************
        //  Slices
        //  ************************************************************
        Assert.Equal(1, aseFile.Slices.Length);
        AssertSlice(aseFile.Slices[0], "slice", 1, true, true, "hello slice", _green);
        AssertSliceKey(aseFile.Slices[0].Keys[0], 0, new Rectangle(0, 0, 2, 2), new Rectangle(0, 0, 1, 2), new Point(1, 2));

        //  ************************************************************
        //  Tilesets
        //  ************************************************************
        Assert.Equal(0, aseFile.Tilesets.Length);
    }

    private void AssertLayer(AsepriteLayer layer, string name, bool isVisible, bool isBackground, bool isReference, AsepriteBlendMode blendMode, int opacity, string? userDataText, Color? userDataColor)
    {
        Assert.Equal(name, layer.Name);
        Assert.Equal(isVisible, layer.IsVisible);
        Assert.Equal(isBackground, layer.IsBackground);
        Assert.Equal(isReference, layer.IsReference);
        Assert.Equal(blendMode, layer.BlendMode);
        Assert.Equal(opacity, layer.Opacity);
        AssertUserData(layer.UserData, userDataText, userDataColor);
    }

    private void AssertTilemapLayer(AsepriteTilemapLayer tilemapLayer, string tilesetName)
    {
        Assert.Equal(tilesetName, tilemapLayer.Tileset.Name);
    }

    private void AssertTag(AsepriteTag tag, string name, int from, int to, AsepriteLoopDirection direction, int repeat, Color color, string? userDataText, Color? userDataColor)
    {
        Assert.Equal(name, tag.Name);
        Assert.Equal(from, tag.From);
        Assert.Equal(to, tag.To);
        Assert.Equal(repeat, tag.Repeat);
        Assert.Equal(direction, tag.Direction);
        Assert.Equal(color, tag.Color);
        AssertUserData(tag.UserData, userDataText, userDataColor);
    }

    private void AssertFrame(AsepriteFrame frame, int celCount, int width, int height, int duration)
    {
        Assert.Equal(celCount, frame.Cels.Length);
        Assert.Equal(width, frame.Width);
        Assert.Equal(height, frame.Height);
        Assert.Equal(duration, frame.DurationInMilliseconds);
    }

    private void AssertCel(AsepriteCel cel, string layerName, int x, int y, int opacity, string? userDataText, Color? userDataColor)
    {
        Assert.Equal(layerName, cel.Layer.Name);
        Assert.Equal(x, cel.Position.X);
        Assert.Equal(y, cel.Position.Y);
        Assert.Equal(opacity, cel.Opacity);
        AssertUserData(cel.UserData, userDataText, userDataColor);
    }

    private void AssertImageCel(AsepriteImageCel imageCel, int width, int height, Color[] pixels)
    {
        Assert.Equal(width, imageCel.Width);
        Assert.Equal(height, imageCel.Height);
        Assert.Equal(pixels, imageCel.Pixels.ToArray());
    }

    private void AssertTilemapCel(AsepriteTilemapCel tilemapCel, int columns, int rows, int tileCount, string tilesetName)
    {
        Assert.Equal(columns, tilemapCel.Columns);
        Assert.Equal(rows, tilemapCel.Rows);
        Assert.Equal(tileCount, tilemapCel.Tiles.Length);
        Assert.Equal(tileCount, tilemapCel.TileCount);
        Assert.Equal(tilesetName, tilemapCel.Tileset.Name);
    }

    private void AssertTile(AsepriteTile tile, int tilesetTileID, int xFlip, int yFlip, int rotation)
    {
        Assert.Equal(tilesetTileID, tile.TilesetTileID);
        Assert.Equal(xFlip, tile.XFlip);
        Assert.Equal(yFlip, tile.YFlip);
        Assert.Equal(rotation, tile.Rotation);
    }

    private void AssertSlice(AsepriteSlice slice, string name, int keyCount, bool isNine, bool hasPivot, string? userDataText, Color? userDataColor)
    {
        Assert.Equal(name, slice.Name);
        Assert.Equal(isNine, slice.IsNinePatch);
        Assert.Equal(hasPivot, slice.HasPivot);
        Assert.Equal(keyCount, slice.Keys.Length);
        Assert.Equal(keyCount, slice.KeyCount);
        AssertUserData(slice.UserData, userDataText, userDataColor);
    }

    private void AssertSliceKey(AsepriteSliceKey key, int frame, Rectangle bounds, Rectangle? center, Point? pivot)
    {
        Assert.Equal(frame, key.FrameIndex);
        Assert.Equal(bounds, key.Bounds);
        Assert.Equal(center, key.CenterBounds);
        Assert.Equal(center is not null, key.IsNinePatch);
        Assert.Equal(pivot, key.Pivot);
        Assert.Equal(pivot is not null, key.HasPivot);
    }

    private void AssertTileset(AsepriteTileset tileset, int id, string name, int tileCount, int tileWidth, int tileHeight, int width, int height, Color[] pixels)
    {
        Assert.Equal(id, tileset.ID);
        Assert.Equal(name, tileset.Name);
        Assert.Equal(tileCount, tileset.TileCount);
        Assert.Equal(tileWidth, tileset.TileWidth);
        Assert.Equal(tileHeight, tileset.TileHeight);
        Assert.Equal(width, tileset.Width);
        Assert.Equal(height, tileset.Height);
        Assert.Equal(pixels, tileset.Pixels.ToArray());
    }

    private void AssertUserData(AsepriteUserData userData, string? text, Color? color)
    {
        Assert.Equal(text, userData.Text);
        Assert.Equal(color, userData.Color);
    }
}
