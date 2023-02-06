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
///     Defines a processor that processes a  <see cref="RawSprite"/> from a <see cref="AsepriteFrame"/> in an 
///     <see cref="AsepriteFile"/>.
/// </summary>
public static class RawSpriteProcessor
{
    /// <summary>
    ///     Processes a <see cref="RawSprite"/> from the <see cref="AsepriteFrame"/> at the specified index in the 
    ///     given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> that contains the <see cref="AsepriteFrame"/> to processes.
    /// </param>
    /// <param name="aseFrameIndex">
    ///     The index of the <see cref="AsepriteFrame"/> in the <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements should 
    ///     be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates if <see cref="AsepriteCel"/> elements on an <see cref="AsepriteLayer"/> marked as the background 
    ///     layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">
    ///     Indicates if <see cref="AsepriteCel"/> elements on a <see cref="AsepriteTilemapLayer"/> should be included.
    /// </param>
    /// <returns>
    ///     The <see cref="RawSprite"/> created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified  <see cref="AsepriteFrame"/> index is less than zero or is greater than or equal to 
    ///     the total number of  <see cref="AsepriteFrame"/> elements in the given  <see cref="AsepriteFile"/>.
    /// </exception>
    public static RawSprite Process(AsepriteFile aseFile, int aseFrameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true)
    {
        AsepriteFrame aseFrame = aseFile.GetFrame(aseFrameIndex);
        return Process(aseFrame, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
    }

    /// <summary>
    ///     Processes a <see cref="RawSprite"/> from the given <see cref="AsepriteFrame"/>.
    /// </summary>
    /// <param name="aseFrame">
    ///     The <see cref="AsepriteFrame"/> to process.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements should 
    ///     be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates if <see cref="AsepriteCel"/> elements on an <see cref="AsepriteLayer"/> marked as the background 
    ///     layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">
    ///     Indicates if <see cref="AsepriteCel"/> elements on a <see cref="AsepriteTilemapLayer"/> should be included.
    /// </param>
    /// <returns>
    ///     The <see cref="RawSprite"/> created by this method.
    /// </returns>
    internal static RawSprite Process(AsepriteFrame aseFrame, bool onlyVisibleLayers, bool includeBackgroundLayer, bool includeTilemapLayers)
    {
        Color[] pixels = aseFrame.FlattenFrame(onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
        RawTexture rawTexture = new(aseFrame.Name, pixels, aseFrame.Width, aseFrame.Height);
        RawSprite rawSprite = new(aseFrame.Name, rawTexture);
        return rawSprite;
    }
}
