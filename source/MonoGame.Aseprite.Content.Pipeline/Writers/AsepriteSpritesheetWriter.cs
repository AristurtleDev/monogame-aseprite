/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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

[ContentTypeWriter]
public sealed class AsepriteSpritesheetWriter : ContentTypeWriter<AsepriteSpritesheetProcessorResult>
{
    protected override void Write(ContentWriter output, AsepriteSpritesheetProcessorResult value)
    {
        output.Write(value.Name);
        output.Write(value.Size.Width);
        output.Write(value.Size.Height);

        output.Write(value.Pixels.Length);
        for (int i = 0; i < value.Pixels.Length; i++)
        {
            output.Write(value.Pixels[i]);
        }

        output.Write(value.Frames.Length);
        for (int i = 0; i < value.Frames.Length; i++)
        {
            Frame frame = value.Frames[i];
            output.Write(frame.X);
            output.Write(frame.Y);
            output.Write(frame.Width);
            output.Write(frame.Height);
            output.Write(frame.Duration.TotalMilliseconds);
        }

        output.Write(value.Tags.Length);
        for (int i = 0; i < value.Tags.Length; i++)
        {
            Tag tag = value.Tags[i];
            output.Write(tag.Name);
            output.Write(tag.Color);
            output.Write(tag.From);
            output.Write(tag.To);
            output.Write((int)tag.Direction);
        }

        output.Write(value.Slices.Length);
        for (int i = 0; i < value.Slices.Length; i++)
        {
            Slice slice = value.Slices[i];
            output.Write(slice.Name);
            output.Write(slice.Color);
            output.Write(slice.FrameIndex);
            output.Write(slice.X);
            output.Write(slice.Y);
            output.Write(slice.Width);
            output.Write(slice.Height);

            output.Write(slice.IsNinePatch);
            if (slice.IsNinePatch)
            {
                output.Write(slice.CenterX.Value);
                output.Write(slice.CenterY.Value);
                output.Write(slice.CenterWidth.Value);
                output.Write(slice.CenterHeight.Value);
            }

            output.Write(slice.HasPivot);
            if (slice.HasPivot)
            {
                output.Write(slice.PivotX.Value);
                output.Write(slice.PivotY.Value);
            }
        }

    }

    public override string GetRuntimeReader(TargetPlatform targetPlatform)
    {
        return "MonoGame.Aseprite.Content.SpriteSheetReader, MonoGame.Aseprite";
    }
}
