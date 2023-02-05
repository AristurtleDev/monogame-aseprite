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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
/// Defines a processor that processes a raw sprite from a frame in an aseprite file.
/// </summary>
public static class RawSpriteProcessor
{
    /// <summary>
    /// Processes a raw sprite from the frame at the specified index in the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file that contains the frame to processes.</param>
    /// <param name="aseFrameIndex">The index of the frame in the aseprite file to process.</param>
    /// <param name="onlyVisibleLayers">Indicates if only cels on visible layers should be included.</param>
    /// <param name="includeBackgroundLayer">
    /// Indicates if cels on the layer marked as the background layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">Indicates if cels on a tilemap layer should be included.</param>
    /// <returns>The raw sprite created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the specified frame index is less than zero or is greater than or equal to the total number of frames
    /// in the given aseprite file.
    /// </exception>
    public static SpriteContent Process(AsepriteFile aseFile, int aseFrameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true)
    {
        AsepriteFrame aseFrame = aseFile.GetFrame(aseFrameIndex);
        return Process(aseFrame, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
    }

    /// <summary>
    /// Processes a raw sprite from the given frame.
    /// </summary>
    /// <param name="aseFrame">The frame to process.</param>
    /// <param name="onlyVisibleLayers">Indicates if only cels on visible layers should be included.</param>
    /// <param name="includeBackgroundLayer">
    /// Indicates if cels on the layer marked as the background layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">Indicates if cels on a tilemap layer should be included.</param>
    /// <returns>The raw sprite created by this method.</returns>
    internal static SpriteContent Process(AsepriteFrame aseFrame, bool onlyVisibleLayers, bool includeBackgroundLayer, bool includeTilemapLayers)
    {
        Color[] pixels = aseFrame.FlattenFrame(onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
        TextureContent rawTexture = new(aseFrame.Name, pixels, aseFrame.Width, aseFrame.Height);
        SpriteContent rawSprite = new(aseFrame.Name, rawTexture);
        return rawSprite;
    }
}
