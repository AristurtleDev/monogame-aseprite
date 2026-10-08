using System;
using Microsoft.Xna.Framework;
using Xunit;

namespace MonoGame.Aseprite.Tests;

public sealed class AnimatedSpriteTests
{
    [Fact]
    public void GetFrame_TagStartsAtGlobalFrameTwo_ReturnsTagLocalFrame()
    {
        TextureAtlas textureAtlas = new("atlas", null!);
        textureAtlas.CreateRegion("frame-0", Rectangle.Empty);
        textureAtlas.CreateRegion("frame-1", Rectangle.Empty);
        textureAtlas.CreateRegion("frame-2", Rectangle.Empty);

        SpriteSheet spriteSheet = new("sheet", textureAtlas);
        spriteSheet.CreateAnimationTag("tag", builder => builder.AddFrame(2, TimeSpan.FromMilliseconds(100)));
        AnimatedSprite sprite = spriteSheet.CreateAnimatedSprite("tag");

        AnimationFrame frame = sprite.GetFrame(0);

        Assert.Equal(2, frame.FrameIndex);
        Assert.Equal(0, sprite.CurrentFrameIndex);
        Assert.Same(frame, sprite.CurrentFrame);
    }

    [Fact]
    public void SetFrame_CurrentFrameIndex_RestoresTagLocalFrame()
    {
        TextureAtlas textureAtlas = new("atlas", null!);
        textureAtlas.CreateRegion("frame-0", Rectangle.Empty);
        textureAtlas.CreateRegion("frame-1", Rectangle.Empty);
        textureAtlas.CreateRegion("frame-2", Rectangle.Empty);
        textureAtlas.CreateRegion("frame-3", Rectangle.Empty);

        SpriteSheet spriteSheet = new("sheet", textureAtlas);
        spriteSheet.CreateAnimationTag("tag", builder =>
        {
            builder.AddFrame(2, TimeSpan.FromMilliseconds(100));
            builder.AddFrame(3, TimeSpan.FromMilliseconds(100));
        });
        AnimatedSprite sprite = spriteSheet.CreateAnimatedSprite("tag");
        sprite.SetFrame(1);
        int currentFrameIndex = sprite.CurrentFrameIndex;

        sprite.SetFrame(0);
        sprite.SetFrame(currentFrameIndex);

        Assert.Equal(3, sprite.CurrentFrame.FrameIndex);
    }
}
