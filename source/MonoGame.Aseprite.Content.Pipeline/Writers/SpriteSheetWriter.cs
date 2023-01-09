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
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.Content.Pipeline.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

[ContentTypeWriter]
public sealed class SpriteSheetWriter : ContentTypeWriter<AsepriteSpritesheetProcessorResult>
{
    protected override void Write(ContentWriter output, AsepriteSpritesheetProcessorResult value)
    {
        output.Write(value.Name);
        output.Write(value.Size.X);
        output.Write(value.Size.Y);
        WritePixels(output, value.Pixels);
        WriteFrames(output, value.Frames);
        WriteAnimationDefinition(output, value.AnimationDefinitions);
    }

    private void WritePixels(ContentWriter output, Color[] pixels)
    {
        output.Write(pixels.Length);

        for (int i = 0; i < pixels.Length; i++)
        {
            output.Write(pixels[i]);
        }
    }

    private void WriteFrames(ContentWriter output, List<SpriteSheetFrameContent> frames)
    {
        output.Write(frames.Count);

        for (int i = 0; i < frames.Count; i++)
        {
            WriteSingleFrame(output, frames[i]);
        }
    }

    private void WriteSingleFrame(ContentWriter output, SpriteSheetFrameContent frame)
    {
        output.Write(frame.Name);
        output.Write(frame.Bounds.X);
        output.Write(frame.Bounds.Y);
        output.Write(frame.Bounds.Width);
        output.Write(frame.Bounds.Height);
        output.Write(frame.Duration.Ticks);
        output.Write(frame.Regions.Count);

        foreach (KeyValuePair<string, SpriteSheetFrameRegion> kvp in frame.Regions)
        {
            WriteSingleRegion(output, kvp.Value);
        }
    }

    private void WriteSingleRegion(ContentWriter output, SpriteSheetFrameRegion region)
    {
        output.Write(region.Name);
        output.Write(region.Color);
        output.Write(region.Bounds.X);
        output.Write(region.Bounds.Y);
        output.Write(region.Bounds.Width);
        output.Write(region.Bounds.Height);

        output.Write(region.IsNinePatch);

        if (region.IsNinePatch)
        {
            output.Write(region.CenterBounds.Value.X);
            output.Write(region.CenterBounds.Value.Y);
            output.Write(region.CenterBounds.Value.Width);
            output.Write(region.CenterBounds.Value.Height);
        }

        output.Write(region.HasPivot);

        if (region.HasPivot)
        {
            output.Write(region.Pivot.Value.X);
            output.Write(region.Pivot.Value.Y);
        }
    }

    private void WriteAnimationDefinition(ContentWriter output, List<SpriteSheetAnimationDefinition> definitions)
    {
        output.Write(definitions.Count);

        for (int i = 0; i < definitions.Count; i++)
        {
            WriteSingleAnimationDefinition(output, definitions[i]);
        }
    }

    private void WriteSingleAnimationDefinition(ContentWriter output, SpriteSheetAnimationDefinition definition)
    {
        output.Write(definition.Name);

        output.Write(definition.FrameIndexes.Length);

        for (int i = 0; i < definition.FrameIndexes.Length; i++)
        {
            output.Write(definition.FrameIndexes[i]);
        }

        output.Write(definition.IsLooping);
        output.Write(definition.IsReversed);
        output.Write(definition.IsPingPong);
    }


    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.SpriteSheetReader, MonoGame.Aseprite";
    }
}
