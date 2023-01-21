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
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using MonoGame.Aseprite.Content.Pipeline.Processors;
using MonoGame.Aseprite.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Writers;

/// <summary>
///     Defines a content writer that writes the content of a <see cref="SpriteSheetContentProcessorResult"/> to an
///     xnb file.
/// </summary>
[ContentTypeWriter]
public sealed class SpriteSheetContentWriter : ContentTypeWriter<SpriteSheetContentProcessorResult>
{
    protected override void Write(ContentWriter writer, SpriteSheetContentProcessorResult content)
    {
        WriteTexture(writer, content.TextureContent);
        WriteSpriteSheet(writer, content.RawSpriteSheet);
    }

    private void WriteTexture(ContentWriter writer, TextureContent textureContent)
    {
        writer.Write(textureContent);
        writer.Write(textureContent.Name);
    }

    private void WriteSpriteSheet(ContentWriter writer, RawSpriteSheet spriteSheet)
    {
        writer.Write(spriteSheet.Name);
        WriteRegions(writer, spriteSheet.Regions);
        WriteCycles(writer, spriteSheet.Cycles);
    }

    private void WriteRegions(ContentWriter writer, ReadOnlySpan<Rectangle> regions)
    {
        writer.Write(regions.Length);
        for (int i = 0; i < regions.Length; i++)
        {
            writer.Write(regions[i]);
        }
    }

    private void WriteCycles(ContentWriter writer, Dictionary<string, RawAnimationCycle> cycles)
    {
        writer.Write(cycles.Count);
        foreach (KeyValuePair<string, RawAnimationCycle> kvp in cycles)
        {
            string name = kvp.Key;
            RawAnimationCycle cycle = kvp.Value;

            writer.Write(name);
            writer.Write(cycle.IsLooping);
            writer.Write(cycle.IsReversed);
            writer.Write(cycle.IsPingPong);
            WriteCycleFrameData(writer, cycle.FrameIndexes, cycle.FrameDurations);
        }
    }

    private void WriteCycleFrameData(ContentWriter writer, ReadOnlySpan<int> indexes, ReadOnlySpan<int> durations)
    {
        writer.Write(indexes.Length);
        for (int i = 0; i < indexes.Length; i++)
        {
            writer.Write(indexes[i]);
            writer.Write(durations[i]);
        }
    }

    public override string GetRuntimeType(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.SpriteSheet, MonoGame.Aseprite";
    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.Pipeline.Readers.SpriteSheetReader, MonoGame.Aseprite";
    }
}
