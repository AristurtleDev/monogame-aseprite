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

public sealed class TextureAtlasProcessorTestFixture
{
    public string Name { get; } = "raw-sprite-processor-test";
    public AsepriteFile AsepriteFile { get; }

    public TextureAtlasProcessorTestFixture()
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

        AsepriteTag[] tags = Array.Empty<AsepriteTag>();
        AsepriteSlice[] slices = Array.Empty<AsepriteSlice>();
        AsepriteUserData userData = new();
        AsepriteFile = new(Name, width, height, palette, frames, layers, tags, slices, tilesets, userData);
    }
}

public sealed class TextureAtlasProcessorTests : IClassFixture<TextureAtlasProcessorTestFixture>
{
    private readonly TextureAtlasProcessorTestFixture _fixture;

    //  These are the colors that will be expected in the raw texture created during each process.  They are created
    //  here an named this way so that it's easier to visualize what the pixel array should be in the test below.
    private readonly Color _ = Color.Transparent;   //  Represents a transparent pixel from padding/spacing, not source
    private readonly Color r = Color.Red;           //  Represents a red pixel
    private readonly Color g = Color.Green;         //  Represents a green pixel
    private readonly Color b = Color.Blue;          //  Represents a blue pixel
    private readonly Color t = Color.Transparent;   //  Represents a transparent pixel from source, not padding/spacing.

    public TextureAtlasProcessorTests(TextureAtlasProcessorTestFixture fixture) => _fixture = fixture;

    [Fact]
    public void ProcessRaw_Atlas_And_Texture_Name_Same_As_File_Name()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile);
        Assert.Equal(_fixture.Name, atlas.Name);
        Assert.Equal(_fixture.Name, atlas.RawTexture.Name);
    }

    [Fact]
    public void ProcessRaw_One_Region_Per_Frame()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile);
        Assert.Equal(_fixture.AsepriteFile.Frames.Length, atlas.RawTextureRegions.Length);
    }

    [Fact]
    public void ProcessRawRegion_Names_Are_Frame_Names()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile);

        Assert.Equal(_fixture.AsepriteFile.Frames[0].Name, atlas.RawTextureRegions[0].Name);
        Assert.Equal(_fixture.AsepriteFile.Frames[1].Name, atlas.RawTextureRegions[1].Name);
        Assert.Equal(_fixture.AsepriteFile.Frames[2].Name, atlas.RawTextureRegions[2].Name);
        Assert.Equal(_fixture.AsepriteFile.Frames[3].Name, atlas.RawTextureRegions[3].Name);
    }

    [Fact]
    public void ProcessRaw_Duplicate_Frame_Is_Merged()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, mergeDuplicates: true);

        Color[] expected = new Color[]
        {
            r, r, g, g,
            r, r, g, g,
            b, b, t, t,
            b, b, t, t
        };

        Color[] actual = atlas.RawTexture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.RawTextureRegions[0].Bounds);
        Assert.Equal(new Rectangle(2, 0, 2, 2), atlas.RawTextureRegions[1].Bounds);
        Assert.Equal(new Rectangle(0, 2, 2, 2), atlas.RawTextureRegions[2].Bounds);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.RawTextureRegions[3].Bounds);
    }

    [Fact]
    public void ProcessRaw_Duplicate_Frame_Not_Merged()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, mergeDuplicates: false);

        Color[] expected = new Color[]
        {
            r, r, g, g,
            r, r, g, g,
            b, b, r, r,
            b, b, r, r,
        };

        Color[] actual = atlas.RawTexture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.RawTextureRegions[0].Bounds);
        Assert.Equal(new Rectangle(2, 0, 2, 2), atlas.RawTextureRegions[1].Bounds);
        Assert.Equal(new Rectangle(0, 2, 2, 2), atlas.RawTextureRegions[2].Bounds);
        Assert.Equal(new Rectangle(2, 2, 2, 2), atlas.RawTextureRegions[3].Bounds);
    }

    [Fact]
    public void ProcessRaw_Border_Padding_Added_Correctly()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, borderPadding: 1);

        Color[] expected = new Color[]
        {
            _, _, _, _, _, _,
            _, r, r, g, g, _,
            _, r, r, g, g, _,
            _, b, b, t, t, _,
            _, b, b, t, t, _,
            _, _, _, _, _, _
        };

        Color[] actual = atlas.RawTexture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.RawTextureRegions[0].Bounds);
        Assert.Equal(new Rectangle(3, 1, 2, 2), atlas.RawTextureRegions[1].Bounds);
        Assert.Equal(new Rectangle(1, 3, 2, 2), atlas.RawTextureRegions[2].Bounds);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.RawTextureRegions[3].Bounds);
    }

    [Fact]
    public void ProcessRaw_Spacing_Added_Correctly()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, spacing: 1);

        Color[] expected = new Color[]
        {
            r, r, _, g, g,
            r, r, _, g, g,
            _, _, _, _, _,
            b, b, _, t, t,
            b, b, _, t, t
        };

        Color[] actual = atlas.RawTexture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.RawTextureRegions[0].Bounds);
        Assert.Equal(new Rectangle(3, 0, 2, 2), atlas.RawTextureRegions[1].Bounds);
        Assert.Equal(new Rectangle(0, 3, 2, 2), atlas.RawTextureRegions[2].Bounds);
        Assert.Equal(new Rectangle(0, 0, 2, 2), atlas.RawTextureRegions[3].Bounds);
    }

    [Fact]
    public void ProcessRaw_InnerPadding_Added_Correctly()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, innerPadding: 1);

        Color[] expected = new Color[]
        {
            _, _, _, _, _, _, _, _,
            _, r, r, _, _, g, g, _,
            _, r, r, _, _, g, g, _,
            _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,
            _, b, b, _, _, t, t, _,
            _, b, b, _, _, t, t, _,
            _, _, _, _, _, _, _, _,
        };

        Color[] actual = atlas.RawTexture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.RawTextureRegions[0].Bounds);
        Assert.Equal(new Rectangle(5, 1, 2, 2), atlas.RawTextureRegions[1].Bounds);
        Assert.Equal(new Rectangle(1, 5, 2, 2), atlas.RawTextureRegions[2].Bounds);
        Assert.Equal(new Rectangle(1, 1, 2, 2), atlas.RawTextureRegions[3].Bounds);
    }

    [Fact]
    public void ProcessRaw_Combined_Border_Padding_Spacing_Inner_Padding_Added_Correctly()
    {
        RawTextureAtlas atlas = TextureAtlasProcessor.ProcessRaw(_fixture.AsepriteFile, borderPadding: 1, spacing: 1, innerPadding: 1);

        Color[] expected = new Color[]
        {
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, r, r, _, _, _, g, g, _, _,
            _, _, r, r, _, _, _, g, g, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, b, b, _, _, _, t, t, _, _,
            _, _, b, b, _, _, _, t, t, _, _,
            _, _, _, _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _, _, _, _
        };

        Color[] actual = atlas.RawTexture.Pixels.ToArray();

        Assert.Equal(expected, actual);
        Assert.Equal(new Rectangle(2, 2, 2, 2), atlas.RawTextureRegions[0].Bounds);
        Assert.Equal(new Rectangle(7, 2, 2, 2), atlas.RawTextureRegions[1].Bounds);
        Assert.Equal(new Rectangle(2, 7, 2, 2), atlas.RawTextureRegions[2].Bounds);
        Assert.Equal(new Rectangle(2, 2, 2, 2), atlas.RawTextureRegions[3].Bounds);
    }
}
