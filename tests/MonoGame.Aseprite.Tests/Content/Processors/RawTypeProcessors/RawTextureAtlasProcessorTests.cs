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

public sealed class RawTextureAtlasProcessorTests
{
    private Color _ = new Color(0, 0, 0, 0);            //  Padding added from export
    private Color t = new Color(0, 0, 0, 0);            //  Transparent pixel
    private Color r = new Color(255, 0, 0, 255);        //  Red pixel
    private Color g = new Color(0, 255, 0, 255);        //  Green pixel
    private Color b = new Color(0, 0, 255, 255);        //  Blue pixel
    private Color w = new Color(255, 255, 255, 255);    //  White pixel
    private Color k = new Color(0, 0, 0, 255);          //  Black pixel


    [Fact]
    public void RawTextureAtlasProcessor_ProcessTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
        {
            t, r,    r, t,
            r, r,    r, r,

            r, r,    r, r,
            r, t,    t, r
        };

        RawTexture expectedTexture = new(fileName, expectedPixels, 4, 4);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new (2, 0, 2, 2)),
            new($"{fileName} 2", new (0, 2, 2, 2)),
            new($"{fileName} 3", new (2, 2, 2, 2)),
            new($"{fileName} 4", new (2, 2, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlasProcessor_Process_MergeDuplicate_FalseTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
        {
            t, r,    r, t,    r, r,
            r, r,    r, r,    r, t,

            r, r,    r, r,    t, t,
            t, r,    t, r,    t, t
        };

        RawTexture expectedTexture = new(fileName, expectedPixels, 6, 4);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new (0, 0, 2, 2)),
            new($"{fileName} 1", new (2, 0, 2, 2)),
            new($"{fileName} 2", new (4, 0, 2, 2)),
            new($"{fileName} 3", new (0, 2, 2, 2)),
            new($"{fileName} 4", new (2, 2, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, mergeDuplicates: false);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlasProcess_OnlyVisibleLayers_FalseTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
        {
            w, k,    k, w,
            k, k,    k, k,

            k, k,    k, k,
            k, w,    w, k
        };

        RawTexture expectedTexture = new(fileName, expectedPixels, 4, 4);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new(2, 0, 2, 2)),
            new($"{fileName} 2", new(0, 2, 2, 2)),
            new($"{fileName} 3", new(2, 2, 2, 2)),
            new($"{fileName} 4", new(2, 2, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, onlyVisibleLayers: false);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlas_Process_IncludeBackgroundLayer_TrueTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
        {
            b, r,    r, b,
            r, r,    r, r,

            r, r,    r, r,
            r, b,    b, r
        };

        RawTexture expectedTexture = new(fileName, expectedPixels, 4, 4);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new(2, 0, 2, 2)),
            new($"{fileName} 2", new(0, 2, 2, 2)),
            new($"{fileName} 3", new(2, 2, 2, 2)),
            new($"{fileName} 4", new(2, 2, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, includeBackgroundLayer: true);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlasProcessor_Process_BorderPaddingTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
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

        RawTexture expectedTexture = new(fileName, expectedPixels, 8, 8);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(2, 2, 2, 2)),
            new($"{fileName} 1", new(4, 2, 2, 2)),
            new($"{fileName} 2", new(2, 4, 2, 2)),
            new($"{fileName} 3", new(4, 4, 2, 2)),
            new($"{fileName} 4", new(4, 4, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, borderPadding: 2);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlasProcessor_Process_SpacingTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
        {
            t, r,  _, _,  r, t,
            r, r,  _, _,  r, r,
            _, _,  _, _,  _, _,
            _, _,  _, _,  _, _,
            r, r,  _, _,  r, r,
            r, t,  _, _,  t, r
        };

        RawTexture expectedTexture = new(fileName, expectedPixels, 6, 6);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(0, 0, 2, 2)),
            new($"{fileName} 1", new(4, 0, 2, 2)),
            new($"{fileName} 2", new(0, 4, 2, 2)),
            new($"{fileName} 3", new(4, 4, 2, 2)),
            new($"{fileName} 4", new(4, 4, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, spacing: 2);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlasProcessor_Process_InnerPaddingTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
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

        RawTexture expectedTexture = new(fileName, expectedPixels, 12, 12);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(2, 2, 2, 2)),
            new($"{fileName} 1", new(8, 2, 2, 2)),
            new($"{fileName} 2", new(2, 8, 2, 2)),
            new($"{fileName} 3", new(8, 8, 2, 2)),
            new($"{fileName} 4", new(8, 8, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, innerPadding: 2);

        Assert.Equal(expectedAtlas, actualAtlas);
    }

    [Fact]
    public void RawTextureAtlasProcessor_Process_BorderPadding_Spacing_InnerPaddingTest()
    {
        string fileName = "spritesheet-processor-test";

        Color[] expectedPixels = new Color[]
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

        RawTexture expectedTexture = new(fileName, expectedPixels, 18, 18);

        RawTextureRegion[] expectedRegions = new RawTextureRegion[]
        {
            new($"{fileName} 0", new(4, 4, 2, 2)),
            new($"{fileName} 1", new(12, 4, 2, 2)),
            new($"{fileName} 2", new(4, 12, 2, 2)),
            new($"{fileName} 3", new(12, 12, 2, 2)),
            new($"{fileName} 4", new(12, 12, 2, 2))
        };

        RawTextureAtlas expectedAtlas = new(fileName, expectedTexture, expectedRegions);

        string path = FileUtils.GetLocalPath($"{fileName}.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawTextureAtlas actualAtlas = RawTextureAtlasProcessor.Process(aseFile, borderPadding: 2, spacing: 2, innerPadding: 2);

        Assert.Equal(expectedAtlas, actualAtlas);
    }
}
