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

using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Processors;

/// <summary>
///     Defines the configuration for the <see cref="TilemapProcessor"/>.
/// </summary>
public class TilemapProcessorConfiguration
{
    /// <summary>
    ///     Gets or Sets the index of the <see cref="AsepriteFrame"/> in the <see cref="AsepriteFile"/> that contains
    ///     the <see cref="Tilemap"/> to process.
    /// </summary>
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Gets or Sts a value that indicates whether only <see cref="AsepriteTilemapCel"/> elements that are on
    ///     visible <see cref="AsepriteTilemapLayer"/> elements should be processed.
    /// </summary>
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TilemapProcessorConfiguration"/> class with the default values.
    /// </summary>
    /// <remarks>
    ///     The default configuration is to process the <see cref="Tilemap"/> from <see cref="AsepriteFrame"/> zero and
    ///     to only process visible <see cref="AsepriteTilemapLayer"/> elements.
    /// </remarks>
    public TilemapProcessorConfiguration() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TilemapProcessorConfiguration"/> class.
    /// </summary>
    /// <param name="frameIndex">
    ///     The index of the <see cref="AsepriteFrame"/> in the <see cref="AsepriteFile"/> that contains the
    ///     <see cref="Tilemap"/> to process.
    ///     The index of the frame in the Aseprite file that contains the tilemap to process.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates whether only <see cref="AsepriteTilemapCel"/> elements that are on visible
    ///     <see cref="AsepriteTilemapLayer"/> elements should be processed.
    /// </param>
    public TilemapProcessorConfiguration(int frameIndex, bool onlyVisibleLayers) =>
        (FrameIndex, OnlyVisibleLayers) = (frameIndex, onlyVisibleLayers);

}
