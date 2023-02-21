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
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Tests;

public sealed class SpriteProcessorTestsFixture
{
    public string Name { get; } = "raw-sprite-processor-test";
    public AsepriteFile AsepriteFile { get; }

    public SpriteProcessorTestsFixture()
    {
        Color[] palette = new Color[] { Color.Black, Color.White };

        AsepriteTileset[] tilesets = Array.Empty<AsepriteTileset>();

        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new(AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "layer")
        };

        AsepriteCel[] frame0Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(2, 2, new Color[] {Color.Black, Color.Black, Color.Black, Color.Black}, layers[0], Point.Zero, 255)
        };

        AsepriteCel[] frame1Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(2, 2, new Color[] {Color.White, Color.White, Color.White, Color.White}, layers[0], Point.Zero, 255)
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{Name} 0", 2, 2, 100, frame0Cels),
            new($"{Name} 1", 2, 2, 100, frame1Cels)
        };

        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();


        AsepriteUserData userData = new();
        AsepriteFile = new(Name, 2, 2, palette, frames, layers, tags, slices, tilesets, userData);
    }
}

public sealed class SpriteProcessorTests : IClassFixture<SpriteProcessorTestsFixture>
{
    private readonly SpriteProcessorTestsFixture _fixture;


    public SpriteProcessorTests(SpriteProcessorTestsFixture fixture) => _fixture = fixture;

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void ProcessRaw_Sprite_And_Texture_Name_Include_Correct_Index(int frame)
    {
        RawSprite sprite = SpriteProcessor.ProcessRaw(_fixture.AsepriteFile, frame);

        string name = $"{_fixture.Name} {frame}";

        Assert.Equal(name, sprite.Name);
        Assert.Equal(name, sprite.RawTexture.Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void ProcessRaw_Throws_Exception_When_Frame_Index_Out_Of_Range(int frame)
    {
        Exception ex = Record.Exception(() => SpriteProcessor.ProcessRaw(_fixture.AsepriteFile, frame));
        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }

    [Fact]
    public void ProcessRaw_Processes_Given_Frame()
    {
        RawSprite frame0Sprite = SpriteProcessor.ProcessRaw(_fixture.AsepriteFile, 0);
        RawSprite frame1Sprite = SpriteProcessor.ProcessRaw(_fixture.AsepriteFile, 1);

        Color[] frame0Expected=  new Color[] { Color.Black, Color.Black, Color.Black, Color.Black };
        Color[] frame1Expected = new Color[] { Color.White, Color.White, Color.White, Color.White };

        Color[] frame0Actual = frame0Sprite.RawTexture.Pixels.ToArray();
        Color[] frame1Actual = frame1Sprite.RawTexture.Pixels.ToArray();

        Assert.Equal(frame0Expected, frame0Actual);
        Assert.Equal(frame1Expected, frame1Actual);
    }
}
