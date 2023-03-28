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
///     Defines a processor that processes a <see cref="Sprite"/> from an frame in an <see cref="AsepriteFile"/>
/// </summary>
/// <seealso cref="MonoGame.Aseprite.Content.Processors"/>
public static partial class SpriteProcessor
{
    /// <summary>
    ///     Processes a <see cref="Sprite"/> from the <see cref="AsepriteFrame"/> at the specified index in the given
    ///     <see cref="AsepriteFile"/>
    /// </summary>
    /// <param name="device">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    ///  </param>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> that contains the <see cref="AsepriteFrame"/> to processes.
    /// </param>
    /// <param name="aseFrameIndex">
    ///     The index of the <see cref="AsepriteFrame"/> in the <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="onlyVisibleLayers" default="true">
    ///     Indicates if only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements should
    ///     be included.
    /// </param>
    /// <param name="includeBackgroundLayer" default="false">
    ///     Indicates if <see cref="AsepriteCel"/> on the <see cref="AsepriteLayer"/> marked as the background layer 
    ///     should be included.
    /// </param>
    /// <param name="includeTilemapLayers" default="true">
    ///     Indicates if <see cref="AsepriteCel"/> on a <see cref="AsepriteTilemapLayer"/> should be included.
    /// </param>
    /// <returns cref="Sprite">
    ///     The <see cref="Sprite"/> created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the specified frame index is less than zero or is greater than or equal to the total number 
    ///     <see cref="AsepriteFrame"/> elements in the given <see cref="AsepriteFile"/>.
    /// </exception>
    /// <seealso cref="AsepriteFile"/>
    /// <seealso cref="Sprite"/>
    public static Sprite Process(GraphicsDevice device, AsepriteFile aseFile, int aseFrameIndex, bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapLayers = true)
    {
        RawSprite rawSprite = ProcessRaw(aseFile, aseFrameIndex, onlyVisibleLayers, includeBackgroundLayer, includeTilemapLayers);
        return Sprite.FromRaw(device, rawSprite);
    }
}
