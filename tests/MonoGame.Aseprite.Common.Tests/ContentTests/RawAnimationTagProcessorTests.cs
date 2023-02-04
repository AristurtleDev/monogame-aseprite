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
using MonoGame.Aseprite.RawProcessors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Tests;

public class RawAnimationTagProcessorTestFixture
{
    private readonly AsepriteFrame[] _frames;

    public ReadOnlySpan<AsepriteFrame> Frames => _frames;

    public RawAnimationTagProcessorTestFixture()
    {
        _frames = new AsepriteFrame[]
        {
            new("frame-1", 0, 0, 100, Array.Empty<AsepriteCel>()),
            new("frame-2", 0, 0, 200, Array.Empty<AsepriteCel>()),
            new("frame-3", 0, 0, 300, Array.Empty<AsepriteCel>()),
            new("frame-4", 0, 0, 400, Array.Empty<AsepriteCel>()),
        };
    }

}

public sealed class RawAnimationTagProcessorTests : IClassFixture<RawAnimationTagProcessorTestFixture>
{
    private readonly RawAnimationTagProcessorTestFixture _fixture;

    public RawAnimationTagProcessorTests(RawAnimationTagProcessorTestFixture fixture) => _fixture = fixture;

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void Produces_Correct_RawAnimationFrames(ushort from, ushort to)
    {
        AsepriteTag tag = new(from, to, AsepriteLoopDirection.Forward, Color.Red, nameof(tag));
        RawAnimationTag rawTag = RawAnimationTagProcessor.Process(tag, _fixture.Frames);

        int frameCount = to - from + 1;
        for (int i = 0; i < frameCount; i++)
        {
            int index = from + i;
            int duration = _fixture.Frames[index].Duration;

            Assert.Equal(index, rawTag.RawAnimationFrames[i].FrameIndex);
            Assert.Equal(duration, rawTag.RawAnimationFrames[i].DurationInMilliseconds);
        }
    }

    [Fact]
    public void RawAnimationTag_Has_Same_Name_As_AsepriteTag()
    {
        string name = "hello tag";
        AsepriteTag tag = new(0, 0, AsepriteLoopDirection.Forward, Color.Red, name);
        RawAnimationTag rawTag = RawAnimationTagProcessor.Process(tag, _fixture.Frames);
        Assert.Equal(tag.Name, rawTag.Name);
    }

    [Fact]
    public void RawAnimationTag_IsLooping()
    {
        //  In aseprite, tags are always looping, so they should be processed always as looping
        AsepriteTag tag = new(0, 0, AsepriteLoopDirection.Forward, Color.Red, nameof(tag));
        RawAnimationTag rawTag = RawAnimationTagProcessor.Process(tag, _fixture.Frames);
        Assert.True(rawTag.IsLooping);
    }

    [Fact]
    public void RawAnimationTag_Is_Not_Reversed_When_AsepriteTag_Is_Forward()
    {
        AsepriteTag tag = new(0, 0, AsepriteLoopDirection.Forward, Color.Red, nameof(tag));
        RawAnimationTag rawTag = RawAnimationTagProcessor.Process(tag, _fixture.Frames);
        Assert.False(rawTag.IsReversed);
    }

    [Fact]
    public void RawAnimationTag_Is_Reversed_When_AsepriteTag_Is_Reversed()
    {
        AsepriteTag tag = new(0, 0, AsepriteLoopDirection.Reverse, Color.Red, nameof(tag));
        RawAnimationTag rawTag = RawAnimationTagProcessor.Process(tag, _fixture.Frames);
        Assert.True(rawTag.IsReversed);
    }

    [Fact]
    public void RawAnimationTag_Is_PingPong_When_AsepriteTag_IsPingPong()
    {
        AsepriteTag tag = new(0, 0, AsepriteLoopDirection.PingPong, Color.Red, nameof(tag));
        RawAnimationTag rawTag = RawAnimationTagProcessor.Process(tag, _fixture.Frames);
        Assert.True(rawTag.IsPingPong);
    }

}
