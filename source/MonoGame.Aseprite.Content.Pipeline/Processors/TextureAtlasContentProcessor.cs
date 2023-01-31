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
using MonoGame.Aseprite.Content.Processors.RawProcessors;
using MonoGame.Aseprite.Content.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
/// Defines a content processor that processes a raw texture atlas from an aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Texture Atlas Processor - MonoGame.Aseprite")]
internal sealed class TextureAtlasContentProcessor : ContentProcessor<AsepriteFile, RawTextureAtlas>
{
    /// <summary>
    /// Gets or Sets a value that indicates whether only aseprite cels on visible aseprite layers should be included.
    /// </summary>
    [DisplayName("Only Visible Layers")]
    [DefaultValue(true)]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    /// Gets or Sets a value that indicates whether aseprite cels on an aseprite layer marked as the background layer
    /// should be included.
    /// </summary>
    [DisplayName("Include Background Layer")]
    [DefaultValue(false)]
    public bool IncludeBackgroundLayer { get; set; } = false;

    /// <summary>
    /// Gets or Sets a value that indicates whether aseprite cels on an aseprite tilemap layer should be included.
    /// </summary>
    [DisplayName("Include Tilemap Layers")]
    [DefaultValue(true)]
    public bool IncludeTilemapLayers { get; set; } = true;

    /// <summary>
    /// Gets or Sets a value that indicates if duplicate aseprite frames should be merged into one.
    /// </summary>
    [DisplayName("Merge Duplicate Frames")]
    public bool MergeDuplicateFrames { get; set; } = true;

    /// <summary>
    /// Gets or Sets the amount of transparent pixels to add between the edge of the generate source image and the
    /// regions within it.
    /// </summary>
    [DisplayName("Border Padding")]
    [DefaultValue(0)]
    public int BorderPadding { get; set; } = 0;

    /// <summary>
    /// Gets or Sets the amount of transparent pixels to add between each region in the generated source image.
    /// </summary>
    [DisplayName("Spacing")]
    [DefaultValue(0)]
    public int Spacing { get; set; } = 0;

    /// <summary>
    /// Gets or Sets teh amount of transparent pixels to add around the edge of each region in the generated source
    /// image.
    /// </summary>
    [DisplayName("Inner Padding")]
    [DefaultValue(0)]
    public int InnerPadding { get; set; } = 0;

    /// <summary>
    /// Processes a raw texture atlas from an aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file that was created as a result of the content importer.</param>
    /// <param name="context">
    /// The content processor context that provides contextual information about the content being
    /// processed.
    /// </param>
    /// <returns>The raw texture atlas that is created by this method.</returns>
    public override RawTextureAtlas Process(AsepriteFile aseFile, ContentProcessorContext context) =>
        RawTextureAtlasProcessor.Process(aseFile, OnlyVisibleLayers, IncludeBackgroundLayer, IncludeTilemapLayers, MergeDuplicateFrames, BorderPadding, Spacing, InnerPadding);
}
