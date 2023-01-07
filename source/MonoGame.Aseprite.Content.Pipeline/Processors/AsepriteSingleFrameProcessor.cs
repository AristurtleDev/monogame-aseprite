/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Collections.Immutable;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite Single Frame Processor - MonoGame.Aseprite")]
public sealed class AsepriteSingleFrameProcessor : ContentProcessor<AsepriteFile, AsepriteSingleFrameProcessorResult>
{
    /// <summary>
    ///     The index of the frame in the Aseprite image to process. Must be
    ///     greater than zero and less than the total number of frames in the
    ///     Aseprite file.
    /// </summary>
    [DisplayName("Frame Index")]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Indicates whether only cels that are on layers that are visible
    ///     should be used.
    /// </summary>
    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Indicates whether the layer marked as a background layer in
    ///     Aseprite should be included when generating image data.
    /// </summary>
    [DisplayName("Include Background Layer")]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Processes the image of a single frame of an Aseprite file.  The
    ///     result contains the data for the image generated from the specified
    ///     frame.
    /// </summary>
    /// <param name="file">
    ///     The Aseprite file to process
    /// </param>
    /// <param name="context">
    ///     The context of the processor. This is provided by the MonoGame
    ///     framework when called from the mgcb-editor.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="AsepriteImageProcessorResult"/>
    ///     class containing the data of the image that was generated.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified frame index is less than zero or is greater
    ///     than or equal to the total number of frame elements in the Aseprite
    ///     file.
    /// </exception>
    public override AsepriteSingleFrameProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        if (FrameIndex < 0 || FrameIndex >= file.Frames.Count)
        {
            throw new IndexOutOfRangeException("The 'Frame Index' cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file");
        }

        ImmutableArray<Color> pixels = file.Frames[FrameIndex].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
        Size size = new(file.FrameWidth, file.FrameHeight);

        return new AsepriteSingleFrameProcessorResult(size, pixels);
    }
}
