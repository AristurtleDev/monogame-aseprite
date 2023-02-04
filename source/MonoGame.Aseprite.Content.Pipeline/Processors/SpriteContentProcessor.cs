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
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.RawProcessors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
/// Defines a content processor that processes a raw sprite from an frame in an aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Sprite Processor - MonoGame.Aseprite")]
internal sealed class SpriteContentProcessor : ContentProcessor<AsepriteFile, RawSprite>
{
    /// <summary>
    /// Gets or Sets a the index of the frame in the aseprite file to process.
    /// </summary>
    [DisplayName("Frame Index")]
    [DefaultValue(0)]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    /// Gets or Sets a value that indicates whether only cels on visible layers should be included.
    /// </summary>
    [DisplayName("Only Visible Layers")]
    [DefaultValue(true)]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    /// Gets or Sets a value that indicates whether cels on an layer marked as the background layer
    /// should be included.
    /// </summary>
    [DisplayName("Include Background Layer")]
    [DefaultValue(false)]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    /// Gets or Sets a value that indicates whether cels on an tilemap layer should be included.
    /// </summary>
    [DisplayName("Include Tilemap Layers")]
    [DefaultValue(true)]
    public bool IncludeTilemapLayers { get; set; } = true;

    /// <summary>
    /// Processes a sprite from the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file that was created as a result of the content importer.</param>
    /// <param name="context">
    /// The content processor context that provides contextual information about the content being
    /// processed.
    /// </param>
    /// <returns>The raw sprite created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the FrameIndex property is less than zero or is greater than or equal to the total number of frames in
    /// the aseprite file being processed.
    /// </exception>
    public override RawSprite Process(AsepriteFile aseFile, ContentProcessorContext context) =>
        RawSpriteProcessor.Process(aseFile, FrameIndex, OnlyVisibleLayers, IncludeBackgroundLayer, IncludeTilemapLayers);
}
