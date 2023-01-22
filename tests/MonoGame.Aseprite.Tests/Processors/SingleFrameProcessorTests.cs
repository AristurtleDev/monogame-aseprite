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

public sealed class SingleFrameProcessorTests
{

    [Fact]
    public void SingleFrameProcessor_CreateRawTextureTest()
    {
        SingleFrameProcessorConfiguration config = new()
        {
            FrameIndex = 1,
            OnlyVisibleLayers = true,
            IncludeBackgroundLayer = false,
            IncludeTilemapLayers = false
        };

        string path = FileUtils.GetLocalPath("single-frame-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTexture texture = SingleFrameProcessor.CreateRawTexture(aseFile, config);

        Color transparent = new Color(0, 0, 0, 0);
        Color[] pixels = new Color[]
        {
            transparent, transparent,
            transparent, transparent,
            aseFile.Palette[2], aseFile.Palette[2],
            aseFile.Palette[2], aseFile.Palette[2]
        };

        Assert.Equal("single-frame-processor-test", texture.Name);
        Assert.Equal(pixels, texture.Pixels);
        Assert.Equal(2, texture.Width);
        Assert.Equal(4, texture.Height);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void SingleFrameProcessor_CreateRawTexture_InvalidFrame_ThrowsExceptionTest(int index)
    {
        SingleFrameProcessorConfiguration config = new()
        {
            FrameIndex = index,
            OnlyVisibleLayers = true,
            IncludeBackgroundLayer = false,
            IncludeTilemapLayers = false
        };

        string path = FileUtils.GetLocalPath("single-frame-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => SingleFrameProcessor.CreateRawTexture(aseFile, config));

        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }
}
