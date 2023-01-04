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
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite Image Processor - MonoGame.Aseprite")]
public sealed class AsepriteImageProcessor : ContentProcessor<AsepriteFile, AsepriteImageProcessorResult>
{
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

    public override AsepriteImageProcessorResult Process(AsepriteFile input, ContentProcessorContext context)
    {
        Color[] pixels = input.Frames[0].FlattenFrame(OnlyVisibleLayers, IncludeBackgroundLayer);
        int width = input.FrameWidth;
        int height = input.FrameHeight;

        context.Logger.LogMessage("Pixel Count: " + pixels.Length);
        context.Logger.LogMessage("Width: " + width);
        context.Logger.LogMessage("Height: " + height);

        return new AsepriteImageProcessorResult(pixels, width, height);
    }
}
