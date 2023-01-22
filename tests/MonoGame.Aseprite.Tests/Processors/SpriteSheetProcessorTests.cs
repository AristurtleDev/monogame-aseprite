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

public sealed class SpriteSheetProcessorTests
{
    private Color _ = new Color(0, 0, 0, 0);            //  Padding added from export
    private Color t = new Color(0, 0, 0, 0);            //  Transparent pixel
    private Color r = new Color(255, 0, 0, 255);        //  Red pixel
    private Color g = new Color(0, 255, 0, 255);        //  Green pixel
    private Color b = new Color(0, 0, 255, 255);        //  Blue pixel
    private Color w = new Color(255, 255, 255, 255);    //  White pixel
    private Color k = new Color(0, 0, 0, 255);          //  Black pixel


    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheetTest()
    {
        Color[] pixels = new Color[]
        {
            t, r,    r, t,
            r, r,    r, r,

            r, r,    r, r,
            r, t,    t, r
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(0, 0, 2, 2),
            new Rectangle(2, 0, 2, 2),
            new Rectangle(0, 2, 2, 2),
            new Rectangle(2, 2, 2, 2),
            new Rectangle(2, 2, 2, 2)
        };

        RawAnimationCycle cycle0 = new(new int[] { 0, 1 }, new int[] { 100, 100 }, true, true, false);
        RawAnimationCycle cycle1 = new(new int[] { 2, 3 }, new int[] { 100, 100 }, true, false, true);
        Dictionary<string, RawAnimationCycle> cycles = new();
        cycles.Add("tag_1_2_reverse_black", cycle0);
        cycles.Add("tag_3_4_pingpong_red", cycle1);

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        //  ************************************************
        //  Default config
        //  ************************************************
        SpriteSheetProcessorConfiguration config = new();
        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);


        Assert.Equal("spritesheet-processor-test", sheet.Name);
        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 4, 4);
        Assert.Equal(regions, sheet.Regions.ToArray());
        Assert.Equal(cycles, sheet.Cycles);
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_MergeDuplicate_FalseTest()
    {
        Color[] pixels = new Color[]
        {
            t, r,    r, t,    r, r,
            r, r,    r, r,    r, t,

            r, r,    r, r,    t, t,
            t, r,    t, r,    t, t
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(0, 0, 2, 2),
            new Rectangle(2, 0, 2, 2),
            new Rectangle(4, 0, 2, 2),
            new Rectangle(0, 2, 2, 2),
            new Rectangle(2, 2, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            MergeDuplicateFrames = false
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 6, 4);
        Assert.Equal(regions, sheet.Regions.ToArray());

    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_OnlyVisibleLayers_FalseTest()
    {
        Color[] pixels = new Color[]
        {
            w, k,    k, w,
            k, k,    k, k,

            k, k,    k, k,
            k, w,    w, k
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(0, 0, 2, 2),
            new Rectangle(2, 0, 2, 2),
            new Rectangle(0, 2, 2, 2),
            new Rectangle(2, 2, 2, 2),
            new Rectangle(2, 2, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            OnlyVisibleLayers = false
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 4, 4);
        Assert.Equal(regions, sheet.Regions.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_IncludeBackgroundLayer_TrueTest()
    {
        Color[] pixels = new Color[]
        {
            b, r,    r, b,
            r, r,    r, r,

            r, r,    r, r,
            r, b,    b, r
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(0, 0, 2, 2),
            new Rectangle(2, 0, 2, 2),
            new Rectangle(0, 2, 2, 2),
            new Rectangle(2, 2, 2, 2),
            new Rectangle(2, 2, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            IncludeBackgroundLayer = true
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 4, 4);
        Assert.Equal(regions, sheet.Regions.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_BorderPaddingTest()
    {
        Color[] pixels = new Color[]
        {
            _, _,    _, _,    _, _,    _, _,
            _, _,    _, _,    _, _,    _, _,

            _, _,    t, r,    r, t,    _, _,
            _, _,    r, r,    r, r,    _, _,

            _, _,    r, r,    r, r,    _, _,
            _, _,    r, t,    t, r,    _, _,

            _, _,    _, _,    _, _,    _, _,
            _, _,    _, _,    _, _,    _, _,
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(2, 2, 2, 2),
            new Rectangle(4, 2, 2, 2),
            new Rectangle(2, 4, 2, 2),
            new Rectangle(4, 4, 2, 2),
            new Rectangle(4, 4, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            BorderPadding = 2
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 8, 8);
        Assert.Equal(regions, sheet.Regions.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_SpacingTest()
    {
        Color[] pixels = new Color[]
        {
            t, r,  _, _,  r, t,
            r, r,  _, _,  r, r,
            _, _,  _, _,  _, _,
            _, _,  _, _,  _, _,
            r, r,  _, _,  r, r,
            r, t,  _, _,  t, r
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(0, 0, 2, 2),
            new Rectangle(4, 0, 2, 2),
            new Rectangle(0, 4, 2, 2),
            new Rectangle(4, 4, 2, 2),
            new Rectangle(4, 4, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            Spacing = 2
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 6, 6);
        Assert.Equal(regions, sheet.Regions.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_InnerPaddingTest()
    {
        Color[] pixels = new Color[]
        {
            _, _, _, _, _, _,    _, _, _, _, _, _,
            _, _, _, _, _, _,    _, _, _, _, _, _,
            _, _, t, r, _, _,    _, _, r, t, _, _,
            _, _, r, r, _, _,    _, _, r, r, _, _,
            _, _, _, _, _, _,    _, _, _, _, _, _,
            _, _, _, _, _, _,    _, _, _, _, _, _,

            _, _, _, _, _, _,    _, _, _, _, _, _,
            _, _, _, _, _, _,    _, _, _, _, _, _,
            _, _, r, r, _, _,    _, _, r, r, _, _,
            _, _, r, t, _, _,    _, _, t, r, _, _,
            _, _, _, _, _, _,    _, _, _, _, _, _,
            _, _, _, _, _, _,    _, _, _, _, _, _
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(2, 2, 2, 2),
            new Rectangle(8, 2, 2, 2),
            new Rectangle(2, 8, 2, 2),
            new Rectangle(8, 8, 2, 2),
            new Rectangle(8, 8, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            InnerPadding = 2
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 12, 12);
        Assert.Equal(regions, sheet.Regions.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_BorderPadding_Spacing_InnerPaddingTest()
    {
        Color[] pixels = new Color[]
        {
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, t, r, _, _,    _, _,    _, _, r, t, _, _, _, _,
            _, _, _, _, r, r, _, _,    _, _,    _, _, r, r, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,

            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,

            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, r, r, _, _,    _, _,    _, _, r, r, _, _, _, _,
            _, _, _, _, r, t, _, _,    _, _,    _, _, t, r, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
            _, _, _, _, _, _, _, _,    _, _,    _, _, _, _, _, _, _, _,
        };

        Rectangle[] regions = new Rectangle[]
        {
            new Rectangle(4, 4, 2, 2),
            new Rectangle(12, 4, 2, 2),
            new Rectangle(4, 12, 2, 2),
            new Rectangle(12, 12, 2, 2),
            new Rectangle(12, 12, 2, 2)
        };

        string path = FileUtils.GetLocalPath("spritesheet-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        SpriteSheetProcessorConfiguration config = new()
        {
            BorderPadding = 2,
            Spacing = 2,
            InnerPadding = 2
        };

        RawSpriteSheet sheet = SpriteSheetProcessor.GetRawSpriteSheet(aseFile, config);

        ValidateRawTexture(sheet.Texture, "spritesheet-processor-test", pixels, 18, 18);
        Assert.Equal(regions, sheet.Regions.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_DuplicateNamedTags_ThrowsExceptionTest()
    {
        string path = FileUtils.GetLocalPath("spritesheet-processor-duplicate-tag-name-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => SpriteSheetProcessor.GetRawSpriteSheet(aseFile, new()));

        Assert.IsType<InvalidOperationException>(ex);
    }

    private void ValidateRawTexture(RawTexture texture, string name, Color[] pixels, int width, int height)
    {
        Assert.Equal(name, texture.Name);
        Assert.Equal(pixels, texture.Pixels);
        Assert.Equal(width, texture.Width);
        Assert.Equal(height, texture.Height);
    }
}
