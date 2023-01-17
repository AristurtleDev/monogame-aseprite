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
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content process that processes a single <see cref="AsepriteFrame"/> in a <see cref="AsepriteFile"/>
///     and generates a texture based on the frame.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Texture Processor - MonoGame.Aseprite")]
public sealed class SingleFrameProcessor : CommonProcessor<AsepriteFile, SingleFrameProcessorResult>
{
    /// <summary>
    ///     Gets or Sets the index of the <see cref="AsepriteFrame"/> in the <see cref="AsepriteFile"/> to process.
    /// </summary>
    [DisplayName("(Aseprite) Frame Index")]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets a value that indicates whether only <see cref="AsepriteCel"/> elements that are on a visible
    ///     <see cref="AsepriteLayer"/> should be processed.
    /// </summary>
    [DisplayName("(Aseprite) Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Gets or Sets whether <see cref="AsepriteCel"/> elements that are on a <see cref="AsepriteLayer"/> that was
    ///     marked as the background layer in Aseprite should be processed.
    /// </summary>
    [DisplayName("(Aseprite) Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Processes a single <see cref="AsepriteFrame"/> in the given <see cref="AsepriteFile"/>.
    ///     Processes a single frame in an Aseprite file.
    /// </summary>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="SingleFrameProcessorResult"/> class that contains the result of this
    ///     method.
    /// </returns>
    /// <exception cref="IndexOutOfRangeException">
    ///     Thrown if the <see cref="FrameIndex"/> property is less than zero or is greater than or equal to the total
    ///     number of <see cref="AsepriteFrame"/> elements in the <see cref="AsepriteFile"/> being processed.
    /// </exception>
    public override SingleFrameProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        if (FrameIndex < 0 || FrameIndex >= file.FrameCount)
        {
            throw new IndexOutOfRangeException("The 'Frame Index' cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file");
        }

        Color[] pixels = file.Frames[FrameIndex].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
        TextureContent textureContent = CreateTextureContent(file.Name, pixels, file.CanvasWidth, file.CanvasHeight, context);
        return new SingleFrameProcessorResult(textureContent);
    }
}
