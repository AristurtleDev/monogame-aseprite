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

public sealed class RawSpriteProcessorTests
{

    [Fact]
    public void RawSpriteProcessor_ProcessTest()
    {
        string name = "single-frame-processor-test";
        string path = FileUtils.GetLocalPath($"{name}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        int frameIndex = 1;
        bool onlyVisibleLayers = true;
        bool includeBackgroundLayer = false;
        bool includeTilemapLayers = false;
        RawSprite rawSprite = RawSpriteProcessor.Process(aseFile, frameIndex, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);

        Color transparent = new Color(0, 0, 0, 0);
        Color[] pixels = new Color[]
        {
            transparent, transparent,
            transparent, transparent,
            aseFile.Palette[2], aseFile.Palette[2],
            aseFile.Palette[2], aseFile.Palette[2]
        };
        Assert.Equal($"{name} {frameIndex}", rawSprite.Name);
        Assert.Equal($"{name} {frameIndex}", rawSprite.RawTexture.Name);
        Assert.Equal(pixels, rawSprite.RawTexture.Pixels.ToArray());
        Assert.Equal(2, rawSprite.RawTexture.Width);
        Assert.Equal(4, rawSprite.RawTexture.Height);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void RawSpriteProcessor_Process_InvalidFrame_ThrowsExceptionTest(int index)
    {
        string path = FileUtils.GetLocalPath("single-frame-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);
        Exception ex = Record.Exception(() => RawSpriteProcessor.Process(aseFile, index));
        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }
}
