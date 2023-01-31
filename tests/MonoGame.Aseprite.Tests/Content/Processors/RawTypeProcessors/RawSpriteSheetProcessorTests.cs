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

public sealed class RawSpriteSheetProcessorTests
{
    private Color _ = new Color(0, 0, 0, 0);            //  Padding added from export
    private Color t = new Color(0, 0, 0, 0);            //  Transparent pixel
    private Color r = new Color(255, 0, 0, 255);        //  Red pixel
    private Color g = new Color(0, 255, 0, 255);        //  Green pixel
    private Color b = new Color(0, 0, 255, 255);        //  Blue pixel
    private Color w = new Color(255, 255, 255, 255);    //  White pixel
    private Color k = new Color(0, 0, 0, 255);          //  Black pixel


    [Fact]
    public void RawSpriteSheetProcessor_ProcessTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] pixels = new Color[]
        {
            t, r,    r, t,
            r, r,    r, r,

            r, r,    r, r,
            r, t,    t, r
        };

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new (2, 0, 2, 2)),
            new($"{fileName} 2", new (0, 2, 2, 2)),
            new($"{fileName} 3", new (2, 2, 2, 2)),
            new($"{fileName} 4", new (2, 2, 2, 2))
        };

        RawAnimationFrame[] tag0Frames = new RawAnimationFrame[] { new(0, 100), new(1, 100) };
        RawAnimationFrame[] tag1Frames = new RawAnimationFrame[] { new(2, 100), new(3, 100) };

        RawAnimationTag[] rawAnimationTags = new RawAnimationTag[]
        {
            new("tag_1_2_reverse_black", tag0Frames, true, true, false),
            new("tag_3_4_pingpong_red", tag1Frames, true, false, true)
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile);

        Assert.Equal(fileName, rawSpriteSheet.Name);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 4, 4);

        Assert.Equal(rawAnimationTags, rawSpriteSheet.RawAnimationTags.ToArray());
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_MergeDuplicate_FalseTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] pixels = new Color[]
        {
            t, r,    r, t,    r, r,
            r, r,    r, r,    r, t,

            r, r,    r, r,    t, t,
            t, r,    t, r,    t, t
        };

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new (0, 0, 2, 2)),
            new($"{fileName} 1", new (2, 0, 2, 2)),
            new($"{fileName} 2", new (4, 0, 2, 2)),
            new($"{fileName} 3", new (0, 2, 2, 2)),
            new($"{fileName} 4", new (2, 2, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, mergeDuplicates: false);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 6, 4);
    }

    [Fact]
    public void RawSpriteSheetProcessor_Process_OnlyVisibleLayers_FalseTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] pixels = new Color[]
        {
            w, k,    k, w,
            k, k,    k, k,

            k, k,    k, k,
            k, w,    w, k
        };

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new(2, 0, 2, 2)),
            new($"{fileName} 2", new(0, 2, 2, 2)),
            new($"{fileName} 3", new(2, 2, 2, 2)),
            new($"{fileName} 4", new(2, 2, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, onlyVisibleLayers: false);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 4, 4);
    }

    [Fact]
    public void RawSpriteSheetProcessor_Process_IncludeBackgroundLayer_TrueTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] pixels = new Color[]
        {
            b, r,    r, b,
            r, r,    r, r,

            r, r,    r, r,
            r, b,    b, r
        };

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new(2, 0, 2, 2)),
            new($"{fileName} 2", new(0, 2, 2, 2)),
            new($"{fileName} 3", new(2, 2, 2, 2)),
            new($"{fileName} 4", new(2, 2, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, includeBackgroundLayer: true);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 4, 4);
    }

    [Fact]
    public void RawSpriteSheetProcessor_Process_BorderPaddingTest()
    {
        string fileName = "spritesheet-processor-test";

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

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(2, 2, 2, 2)),
            new($"{fileName} 1", new(4, 2, 2, 2)),
            new($"{fileName} 2", new(2, 4, 2, 2)),
            new($"{fileName} 3", new(4, 4, 2, 2)),
            new($"{fileName} 4", new(4, 4, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, borderPadding: 2);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 8, 8);
    }

    [Fact]
    public void RawSpriteSheetProcessor_Process_SpacingTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] pixels = new Color[]
        {
            t, r,  _, _,  r, t,
            r, r,  _, _,  r, r,
            _, _,  _, _,  _, _,
            _, _,  _, _,  _, _,
            r, r,  _, _,  r, r,
            r, t,  _, _,  t, r
        };

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new(4, 0, 2, 2)),
            new($"{fileName} 2", new(0, 4, 2, 2)),
            new($"{fileName} 3", new(4, 4, 2, 2)),
            new($"{fileName} 4", new(4, 4, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, spacing: 2);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 6, 6);
    }

    [Fact]
    public void RawSpriteSheetProcessor_Process_InnerPaddingTest()
    {
        string fileName = "spritesheet-processor-test";

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

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(2, 2, 2, 2)),
            new($"{fileName} 1", new(8, 2, 2, 2)),
            new($"{fileName} 2", new(2, 8, 2, 2)),
            new($"{fileName} 3", new(8, 8, 2, 2)),
            new($"{fileName} 4", new(8, 8, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, innerPadding: 2);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 12, 12);
    }

    [Fact]
    public void RawSpriteSheetProcessor_Process_BorderPadding_Spacing_InnerPaddingTest()
    {
        string fileName = "spritesheet-processor-test";

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

        RawTextureRegion[] rawTextureRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(4, 4, 2, 2)),
            new($"{fileName} 1", new(12, 4, 2, 2)),
            new($"{fileName} 2", new(4, 12, 2, 2)),
            new($"{fileName} 3", new(12, 12, 2, 2)),
            new($"{fileName} 4", new(12, 12, 2, 2))
        };

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawSpriteSheet rawSpriteSheet = RawSpriteSheetProcessor.Process(aseFile, borderPadding: 2, spacing: 2, innerPadding: 2);
        ValidateRawTextureAtlas(rawSpriteSheet.RawTextureAtlas, fileName, rawTextureRegions, pixels, 18, 18);
    }

    [Fact]
    public void SpriteSheetProcessor_GetRawSpriteSheet_DuplicateNamedTags_ThrowsExceptionTest()
    {
        string path = FileUtils.GetLocalPath("spritesheet-processor-duplicate-tag-name-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => RawSpriteSheetProcessor.Process(aseFile));

        Assert.IsType<InvalidOperationException>(ex);
    }

    private void ValidateRawTextureAtlas(RawTextureAtlas rawTextureAtlas, string name, RawTextureRegion[] rawTextureRegions, Color[] pixels, int width, int height)
    {
        Assert.Equal(name, rawTextureAtlas.Name);
        Assert.Equal(rawTextureRegions, rawTextureAtlas.RawTextureRegions.ToArray());
        ValidateRawTexture(rawTextureAtlas.RawTexture, name, pixels, width, height);
    }

    private void ValidateRawTexture(RawTexture rawTexture, string name, Color[] pixels, int width, int height)
    {
        Assert.Equal(name, rawTexture.Name);
        Assert.Equal(pixels, rawTexture.Pixels.ToArray());
        Assert.Equal(width, rawTexture.Width);
        Assert.Equal(height, rawTexture.Height);
    }
}
