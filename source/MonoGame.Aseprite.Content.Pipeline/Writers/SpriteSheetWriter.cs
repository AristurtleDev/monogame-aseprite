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

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

/// <summary>
///     Provides method for writing an instance of the
///     <see cref="SpriteSheetContent"/> class to an xnb file.
/// </summary>
[ContentTypeWriter]
public sealed class SpriteSheetWriter : ContentTypeWriter<SpriteSheetContent>
{
    protected override void Write(ContentWriter output, SpriteSheetContent value)
    {
        output.Write(value.Name);

        //  Texture content
        output.Write(value.TextureContent.Width);
        output.Write(value.TextureContent.Height);
        output.Write(value.TextureContent.Pixels.Length);
        for (int i = 0; i < value.TextureContent.Pixels.Length; i++)
        {
            output.Write(value.TextureContent.Pixels[i]);
        }

        //  Texture Region Content
        output.Write(value.Regions.Count);
        for (int i = 0; i < value.Regions.Count; i++)
        {
            TextureRegionContent region = value.Regions[i];
            output.Write(region.Name);
            output.Write(region.Bounds.X);
            output.Write(region.Bounds.Y);
            output.Write(region.Bounds.Width);
            output.Write(region.Bounds.Height);
        }

        //  Animation Content
        output.Write(value.Animations.Count);
        for (int i = 0; i < value.Animations.Count; i++)
        {
            AnimationContent animation = value.Animations[i];
            output.Write(animation.Name);
            output.Write(animation.LoopReversePingPongMask);

            //  Animation Frames
            output.Write(animation.Frames.Length);
            for (int j = 0; j < animation.Frames.Length; j++)
            {
                AnimationFrameContent frame = animation.Frames[j];
                output.Write(frame.FrameIndex);
                output.Write(frame.Duration.Ticks);
            }
        }
    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.SpriteSheetReader, MonoGame.Aseprite";
    }
}
