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

using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a processor that accepts an Aseprite file and generates a
///     texture based on a single frame in the Aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Texture Processor - MonoGame.Aseprite")]
public sealed class SingleFrameProcessor : CommonProcessor<AsepriteFile, SingleFrameProcessorResult>
{
    /// <summary>
    ///     Gets or Sets the index of the frame in the Aseprite file to process.
    /// </summary>
    [DisplayName("(Aseprite) Frame Index")]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Processes a single frame in an Aseprite file.
    /// </summary>
    /// <param name="file">
    ///     The Aseprite file to process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that contains contextual information
    ///     about content being processed.
    /// </param>
    /// <returns>
    ///     A new instance of the SingleFrameProcessorResult class that contains
    ///     the texture content of the frame processed.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     Thrown if the FrameIndex property that was set in the mgcb-editor
    ///     is less than zero or is greater than or equal to the total number
    ///     of frames in the Aseprite file being processed.
    /// </exception>
    public override SingleFrameProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        if (FrameIndex < 0 || FrameIndex >= file.Frames.Count)
        {
            throw new IndexOutOfRangeException("The 'Frame Index' cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file");
        }

        Color[] pixels = file.Frames[FrameIndex].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
        TextureContent textureContent = CreateTextureContent(file.Name, pixels, file.FrameWidth, file.FrameHeight, context);
        return new SingleFrameProcessorResult(textureContent);
    }
}
