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

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Sprites;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
///     Defines a processor that processes a <see cref="SpriteSheet"/> from an <see cref="AsepriteFile"/>.
/// </summary>
public static class SpriteSheetProcessor
{
    /// <summary>
    ///     Processes a <see cref="SpriteSheet"/> from the given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    ///  </param>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> to process the <see cref="SpriteSheet"/> from.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements should 
    ///     be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates if <see cref="AsepriteCel"/> elements on the <see cref="AsepriteLayer"/> elements marked as the 
    ///     background layer should be included.
    /// </param>
    /// <param name="includeTilemapLayers">
    ///     Indicates if <see cref="AsepriteCel"/> elements on a <see cref="AsepriteTilemapLayer"/> element should be 
    ///     included.</param>
    /// <param name="mergeDuplicates">
    ///     Indicates if duplicate <see cref="AsepriteFrame"/> elements should be merged into one.
    /// </param>
    /// <param name="borderPadding">
    ///     The amount of transparent pixels to add between the edge of the generated image
    /// </param>
    /// <param name="spacing">
    ///     The amount of transparent pixels to add between each texture region in the generated image.
    /// </param>
    /// <param name="innerPadding">
    ///     The amount of transparent pixels to add around the edge of each texture region in the generated image.
    /// </param>
    /// <returns>
    ///     The <see cref="SpriteSheet"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if <see cref="AsepriteTag"/> elements are found in the <see cref="AsepriteFile"/> with duplicate 
    ///     names.  A <see cref="SpriteSheet"/> must contain tags with unique names even though aseprite does not 
    ///     enforce unique names for <see cref="AsepriteTag"/> elements.
    /// </exception>
    public static SpriteSheet Process(GraphicsDevice device, AsepriteFile aseFile, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true, bool mergeDuplicates = true, int borderPadding = 0, int spacing = 0, int innerPadding = 0)
    {
        RawSpriteSheet rawSpritSheet = RawSpriteSheetProcessor.Process(aseFile, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers, mergeDuplicates, borderPadding, spacing, innerPadding);
        return SpriteSheet.FromRaw(device, rawSpritSheet);
    }
}

