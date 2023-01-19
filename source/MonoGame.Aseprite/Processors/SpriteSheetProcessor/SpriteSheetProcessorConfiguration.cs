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

namespace MonoGame.Aseprite.Processors;

/// <summary>
///     Defines the configuration for the <see cref="SpriteSheetProcessor"/>.
/// </summary>
public sealed class SpriteSheetProcessorConfiguration
{
    /// <summary>
    ///     Gets or Sets a value that indicates whether only on cels on visible layers should be processed.
    /// </summary>
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Gets or Sets a value that indicates whether cels on the layer set as the background layer in Aseprite should
    ///     be processed.
    /// </summary>
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    ///     Gets or Sets a value that indicates whether duplicate frames should be merged.
    /// </summary>
    public bool MergeDuplicateFrames { get; set; } = true;

    /// <summary>
    ///     Gets or Sets the amount of transparent pixels to add between between the edge of the generated image for the
    ///     spritesheet and each of the regions within it.
    /// </summary>
    public int BorderPadding { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets the amount of transparent pixels to add between each of the regions in the generated image for
    ///     the spritesheet.
    /// </summary>
    public int Spacing { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets the amount of transparent pixels to add around the edge of each region in the generated image
    ///     for the spritesheet.
    /// </summary>
    public int InnerPadding { get; set; } = 0;

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheetProcessorConfiguration"/> class with the default
    ///     values.
    /// </summary>
    /// <remarks>
    ///     The default configuration is to only process visible layers, do not include background layer, merge
    ///     duplicate frames, zero border padding, zero spacing, and zero inner padding.
    /// </remarks>
    public SpriteSheetProcessorConfiguration() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SpriteSheetProcessorConfiguration"/> class.
    /// </summary>
    /// <param name="onlyVisibleLayers">
    ///     Indicates whether only on cels on visible layers should be processed.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates whether cels on the layer set as the background layer in Aseprite should be processed.
    /// </param>
    /// <param name="mergeDuplicates">
    ///     Indicates whether duplicate frames should be merged.
    /// </param>
    /// <param name="borderPadding">
    ///     The amount of transparent pixels to add between between the edge of the generated image for the spritesheet
    ///     and each of the regions within it.
    /// </param>
    /// <param name="spacing">
    ///     The amount of transparent pixels to add between each of the regions in the generated image for the
    ///     spritesheet.
    /// </param>
    /// <param name="innerPadding">
    ///     The amount of transparent pixels to add around the edge of each region in the generated image for the
    ///     spritesheet.
    /// </param>
    public SpriteSheetProcessorConfiguration(bool onlyVisibleLayers, bool includeBackgroundLayer, bool mergeDuplicates, int borderPadding, int spacing, int innerPadding)
    {
        OnlyVisibleLayers = onlyVisibleLayers;
        IncludeBackgroundLayer = includeBackgroundLayer;
        MergeDuplicateFrames = mergeDuplicates;
        BorderPadding = borderPadding;
        Spacing = spacing;
        InnerPadding = innerPadding;
    }
}
