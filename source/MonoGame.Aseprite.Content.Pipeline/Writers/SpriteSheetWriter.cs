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
using MonoGame.Aseprite.Content.Pipeline.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

/// <summary>
///     Provides method for writing an instance of the
///     <see cref="SpriteSheetProcessorResult"/> class to an xnb file.
/// </summary>
[ContentTypeWriter]
public sealed class SpriteSheetWriter : ContentTypeWriter<SpriteSheetProcessorResult>
{
    protected override void Write(ContentWriter writer, SpriteSheetProcessorResult content)
    {
        writer.Write(content.Name);

        //  Texture content
        writer.WriteTextureContent(content.TextureContent);
        // writer.Write(content.TextureContent.Width);
        // writer.Write(content.TextureContent.Height);
        // writer.Write(content.TextureContent.Pixels.Length);
        // for (int i = 0; i < content.TextureContent.Pixels.Length; i++)
        // {
        //     writer.Write(content.TextureContent.Pixels[i]);
        // }

        //  Texture Region Content
        writer.Write(content.Regions.Count);
        for (int i = 0; i < content.Regions.Count; i++)
        {
            TextureRegionContent region = content.Regions[i];
            writer.Write(region.Name);
            writer.Write(region.Bounds.X);
            writer.Write(region.Bounds.Y);
            writer.Write(region.Bounds.Width);
            writer.Write(region.Bounds.Height);
        }

        //  Animation Content
        writer.Write(content.Animations.Count);
        for (int i = 0; i < content.Animations.Count; i++)
        {
            AnimationContent animation = content.Animations[i];
            writer.Write(animation.Name);
            writer.Write(animation.Flags);

            //  Animation Frames
            writer.Write(animation.Frames.Length);
            for (int j = 0; j < animation.Frames.Length; j++)
            {
                AnimationFrameContent frame = animation.Frames[j];
                writer.Write(frame.FrameIndex);
                writer.Write(frame.Duration.Ticks);
            }
        }
    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.SpriteSheetReader, MonoGame.Aseprite";
    }
}
