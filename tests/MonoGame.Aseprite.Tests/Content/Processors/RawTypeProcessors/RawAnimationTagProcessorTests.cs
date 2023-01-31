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

using MonoGame.Aseprite.Content.Processors.RawProcessors;
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Tests;

public sealed class RawAnimationTagProcessorTests
{
    [Fact]
    public void RawAnimationTagProcessor_Process_ByIndexTest()
    {
        RawAnimationFrame frame0 = new(0, 100);
        RawAnimationFrame frame1 = new(1, 100);
        RawAnimationFrame frame2 = new(2, 100);

        RawAnimationTag tag1Expected = new("tag-1-forward-black", new RawAnimationFrame[] { frame0 }, true, false, false);
        RawAnimationTag tag2Expected = new("tag-2-reversed-white", new RawAnimationFrame[] { frame1 }, true, true, false);
        RawAnimationTag tag3Expected = new("tag-3-pingpong-red", new RawAnimationFrame[] { frame2 }, true, false, true);

        string path = FileUtils.GetLocalPath("raw-animation-tag-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawAnimationTag tag1Actual = RawAnimationTagProcessor.Process(aseFile, 0);
        RawAnimationTag tag2Actual = RawAnimationTagProcessor.Process(aseFile, 1);
        RawAnimationTag tag3Actual = RawAnimationTagProcessor.Process(aseFile, 2);

        Assert.Equal(tag1Expected, tag1Actual);
        Assert.Equal(tag2Expected, tag2Actual);
        Assert.Equal(tag3Expected, tag3Actual);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public void RawAnimationTagProcessor_Process_ByIndex_OutOfRangeTest(int index)
    {
        string path = FileUtils.GetLocalPath("raw-animation-tag-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => RawAnimationTagProcessor.Process(aseFile, index));
        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }

    [Fact]
    public void RawAnimationTagProcessor_Process_ByNameTest()
    {
        RawAnimationFrame frame0 = new(0, 100);
        RawAnimationFrame frame1 = new(1, 100);
        RawAnimationFrame frame2 = new(2, 100);

        RawAnimationTag tag1Expected = new("tag-1-forward-black", new RawAnimationFrame[] { frame0 }, true, false, false);
        RawAnimationTag tag2Expected = new("tag-2-reversed-white", new RawAnimationFrame[] { frame1 }, true, true, false);
        RawAnimationTag tag3Expected = new("tag-3-pingpong-red", new RawAnimationFrame[] { frame2 }, true, false, true);

        string path = FileUtils.GetLocalPath("raw-animation-tag-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        RawAnimationTag tag1Actual = RawAnimationTagProcessor.Process(aseFile, tag1Expected.Name);
        RawAnimationTag tag2Actual = RawAnimationTagProcessor.Process(aseFile, tag2Expected.Name);
        RawAnimationTag tag3Actual = RawAnimationTagProcessor.Process(aseFile, tag3Expected.Name);

        Assert.Equal(tag1Expected, tag1Actual);
        Assert.Equal(tag2Expected, tag2Actual);
        Assert.Equal(tag3Expected, tag3Actual);
    }

    [Fact]
    public void RawAnimationTagProcessor_Process_ByName_InvalidOperationTest()
    {
        string path = FileUtils.GetLocalPath("raw-animation-tag-processor-test.aseprite");
        AsepriteFile aseFile = AsepriteFile.Load(path);

        Exception ex = Record.Exception(() => RawAnimationTagProcessor.Process(aseFile, "error"));
        Assert.IsType<InvalidOperationException>(ex);
    }
}
