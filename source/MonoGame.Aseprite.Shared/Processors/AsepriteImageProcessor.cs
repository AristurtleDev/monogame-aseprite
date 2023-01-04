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
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Processors;

public static class AsepriteImageProcessor
{
    /// <summary>
    ///     Processes the image of a single frame of an Aseprite file.  The
    ///     result contains the data for the image generated from the specified
    ///     frame.
    /// </summary>
    /// <param name="file">
    ///     The Aseprite file to process.
    /// </param>
    /// <param name="frame">
    ///     The index of hte frame in the Aseprite file to process.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates whether only the cels on layers that are visible should
    ///     be processed.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates whether cels on a layer that was marked as a background
    ///     layer in Aseprite should be processed.
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
    public static AsepriteImageProcessorResult Process(AsepriteFile file, int frame, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false)
    {
        if (frame < 0 || frame >= file.Frames.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(frame), $"{nameof(frame)} cannot be less than zero or greater than or equal to the total number of frames in the Aseprite file");
        }

        Color[] pixels = file.Frames[frame].FlattenFrame(onlyVisibleLayers, includeBackgroundLayer);
        int width = file.FrameWidth;
        int height = file.FrameHeight;

        return new AsepriteImageProcessorResult(width, height, pixels);
    }
}
