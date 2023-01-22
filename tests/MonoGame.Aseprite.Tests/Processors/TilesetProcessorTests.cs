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

public sealed class TilesetProcessorTests
{

    [Fact]
    public void TilesetProcessorTest_GetRawTileset()
    {
        string path = FileUtils.GetLocalPath("tileset-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Color tr = new Color(0, 0, 0, 0);
        Color p0 = aseFile.Palette[0];
        Color p1 = aseFile.Palette[1];
        Color p2 = aseFile.Palette[2];
        Color p3 = aseFile.Palette[3];
        Color p4 = aseFile.Palette[4];
        Color p5 = aseFile.Palette[5];
        Color p6 = aseFile.Palette[6];
        Color p7 = aseFile.Palette[7];
        Color p8 = aseFile.Palette[8];
        Color p9 = aseFile.Palette[9];

        Color[] pixels = new Color[]
        {
            tr, tr, tr, tr,
            tr, tr, tr, tr,
            tr, tr, tr, tr,
            tr, tr, tr, tr,

            p0, p0, p0, p0,
            p0, p0, p0, p0,
            p0, p0, p0, p0,
            p0, p0, p0, p0,

            p1, p1, p1, p1,
            p1, p1, p1, p1,
            p1, p1, p1, p1,
            p1, p1, p1, p1,

            p2, p2, p2, p2,
            p2, p2, p2, p2,
            p2, p2, p2, p2,
            p2, p2, p2, p2,

            p3, p3, p3, p3,
            p3, p3, p3, p3,
            p3, p3, p3, p3,
            p3, p3, p3, p3,

            p4, p4, p4, p4,
            p4, p4, p4, p4,
            p4, p4, p4, p4,
            p4, p4, p4, p4,

            p5, p5, p5, p5,
            p5, p5, p5, p5,
            p5, p5, p5, p5,
            p5, p5, p5, p5,

            p6, p6, p6, p6,
            p6, p6, p6, p6,
            p6, p6, p6, p6,
            p6, p6, p6, p6,

            p7, p7, p7, p7,
            p7, p7, p7, p7,
            p7, p7, p7, p7,
            p7, p7, p7, p7,

            p8, p8, p8, p8,
            p8, p8, p8, p8,
            p8, p8, p8, p8,
            p8, p8, p8, p8,

            p9, p9, p9, p9,
            p9, p9, p9, p9,
            p9, p9, p9, p9,
            p9, p9, p9, p9,

        };

        RawTileset tileset = TilesetProcessor.GetRawTileset(aseFile, "tileset");

        Assert.Equal(0, tileset.ID);
        Assert.Equal("tileset", tileset.Name);
        Assert.Equal("tileset", tileset.Texture.Name);
        Assert.Equal(pixels, tileset.Texture.Pixels);
        Assert.Equal(4, tileset.Texture.Width);
        Assert.Equal(44, tileset.Texture.Height);
        Assert.Equal(4, tileset.TileWidth);
        Assert.Equal(4, tileset.TileHeight);
    }

    [Fact]
    public void TilesetProcessorTest_GetRawTileset_InvalidName_ThrowsException()
    {
        string path = FileUtils.GetLocalPath("tileset-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => TilesetProcessor.GetRawTileset(aseFile, "fake-name"));

        Assert.IsType<InvalidOperationException>(ex);
    }
}
