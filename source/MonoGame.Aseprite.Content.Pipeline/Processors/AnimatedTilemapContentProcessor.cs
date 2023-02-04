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
/// Defines a content processor that processes a raw animated tilemap from an aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Animated Tilemap Processor - MonoGame.Aseprite")]
public sealed class AnimatedTilemapContentProcessor : ContentProcessor<AsepriteFile, RawAnimatedTilemap>
{
    /// <summary>
    /// Gets or Sets a value that indicates whether only visible layers should be included.
    /// </summary>
    [DisplayName("Only Visible Layers")]
    [DefaultValue(true)]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    /// Processes a raw animated tilemap from an aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file that was created as a result of the content importer.</param>
    /// <param name="context">
    /// The content processor context that provides contextual information about the content being
    /// processed.
    /// </param>
    /// <returns>The raw animated tilemap created by this method.</returns>
    public override RawAnimatedTilemap Process(AsepriteFile aseFile, ContentProcessorContext context) =>
        RawAnimatedTilemapProcessor.Process(aseFile, OnlyVisibleLayers);
}
