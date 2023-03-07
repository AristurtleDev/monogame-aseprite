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


public sealed class SpriteSheetProcessorTestFixture
{
    public string Name { get; } = "raw-sprite-processor-test";
    public AsepriteFile AsepriteFile { get; }

    public SpriteSheetProcessorTestFixture()
    {
        int width = 2;
        int height = 2;

        Color[] palette = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };
        AsepriteTileset[] tilesets = Array.Empty<AsepriteTileset>();

        AsepriteLayer[] layers = new AsepriteLayer[]
        {
            new(AsepriteLayerFlags.Visible, AsepriteBlendMode.Normal, 255, "layer")
        };

        AsepriteCel[] frame0Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(width, height, new Color[]{Color.Red, Color.Red, Color.Red, Color.Red}, layers[0], Point.Zero, 255)
        };

        AsepriteCel[] frame1Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(width, height, new Color[] {Color.Green, Color.Green, Color.Green, Color.Green}, layers[0], Point.Zero, 255)
        };

        AsepriteCel[] frame2Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(width, height, new Color[] {Color.Blue, Color.Blue, Color.Blue, Color.Blue}, layers[0], Point.Zero, 255)
        };

        AsepriteCel[] frame3Cels = new AsepriteCel[]
        {
            new AsepriteImageCel(width, height, new Color[] {Color.Red, Color.Red, Color.Red, Color.Red}, layers[0], Point.Zero, 255)
        };

        AsepriteFrame[] frames = new AsepriteFrame[]
        {
            new($"{Name} 0", width, height, 100, frame0Cels),
            new($"{Name} 1", width, height, 100, frame1Cels),
            new($"{Name} 2", width, height, 100, frame2Cels),
            new($"{Name} 3", width, height, 100, frame3Cels),
        };

        AsepriteTag[] tags = new AsepriteTag[]
        {
            new(0, 0, AsepriteLoopDirection.Forward, 0, Color.Red, "tag-0"),
            new(0, 1, AsepriteLoopDirection.Forward, 0, Color.Green, "tag-1"),
            new(1, 2, AsepriteLoopDirection.PingPong, 0, Color.Blue, "tag-2")
        };

        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();
        AsepriteUserData userData = new();
        AsepriteFile = new(Name, width, height, palette, frames, layers, tags, slices, tilesets, userData);
    }
}

public sealed class SpriteSheetProcessorTests : IClassFixture<SpriteSheetProcessorTestFixture>
{
    private readonly SpriteSheetProcessorTestFixture _fixture;

    public SpriteSheetProcessorTests(SpriteSheetProcessorTestFixture fixture) => _fixture = fixture;

    [Theory]
    [InlineData(true, true, true, true, 1, 1, 1)]
    [InlineData(true, false, true, true, 1, 0, 1)]
    [InlineData(true, true, false, true, 1, 1, 0)]
    [InlineData(true, true, true, false, 0, 1, 1)]
    [InlineData(false, true, true, true, 0, 1, 0)]
    [InlineData(false, false, true, true, 0, 0, 1)]
    [InlineData(false, true, false, true, 0, 0, 0)]
    [InlineData(false, true, true, false, 0, 0, 0)]
    [InlineData(false, false, false, true, 0, 0, 0)]
    [InlineData(false, false, true, false, 0, 0, 0)]
    [InlineData(false, false, false, false, 0, 0, 0)]
    public void ProcessRaw_asses_Parameters_To_TextureAtlasProcess_Correctly(bool onlyVisible, bool includeBackground, bool includeTilemap, bool mergeDuplicates, int borderPadding, int spacing, int innerPadding)
    {
        RawSpriteSheet sheet = SpriteSheetProcessor.ProcessRaw(_fixture.AsepriteFile, onlyVisible, includeBackground, includeTilemap, mergeDuplicates, borderPadding, spacing, innerPadding);
        RawTextureAtlas expected = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, onlyVisible, includeBackground, includeTilemap, mergeDuplicates, borderPadding, spacing, innerPadding);

        Assert.Equal(expected, sheet.RawTextureAtlas);
    }

    [Fact]
    public void ProcessRaw_SpriteSheet_Atlas_and_Texture_Names_Same_As_File_Name()
    {
        RawSpriteSheet sheet = SpriteSheetProcessor.ProcessRaw(_fixture.AsepriteFile);
        Assert.Equal(_fixture.Name, sheet.Name);
        Assert.Equal(_fixture.Name, sheet.RawTextureAtlas.Name);
        Assert.Equal(_fixture.Name, sheet.RawTextureAtlas.RawTexture.Name);
    }

    [Fact]
    public void ProcessRaw_Processes_All_Tags()
    {
        RawSpriteSheet sheet = SpriteSheetProcessor.ProcessRaw(_fixture.AsepriteFile);
        Assert.Equal(_fixture.AsepriteFile.Tags.Length, sheet.RawAnimationTags.Length);
    }

    [Fact]
    public void ProcessRaw_Duplicate_AsepriteTag_Names_Throws_Exception()
    {
        AsepriteTag[] tags = new AsepriteTag[]
        {
            new(0, 0, AsepriteLoopDirection.Forward, 0,  Color.Red, "tag-0"),
            new(0, 1, AsepriteLoopDirection.Forward, 0, Color.Green, "tag-1"),
            new(1, 2, AsepriteLoopDirection.PingPong, 0, Color.Blue, "tag-0")
        };

        //  Reuse the fixture, but use the tags array from above with duplicate tag names
        AsepriteFile aseFile = new(_fixture.Name,
                                   _fixture.AsepriteFile.CanvasWidth,
                                   _fixture.AsepriteFile.CanvasHeight,
                                   _fixture.AsepriteFile.Palette.ToArray(),
                                   _fixture.AsepriteFile.Frames.ToArray(),
                                   _fixture.AsepriteFile.Layers.ToArray(),
                                   tags,
                                   _fixture.AsepriteFile.Slices.ToArray(),
                                   _fixture.AsepriteFile.Tilesets.ToArray(),
                                   _fixture.AsepriteFile.UserData);

        Assert.Throws<InvalidOperationException>(() => SpriteSheetProcessor.ProcessRaw(aseFile));
    }
}
