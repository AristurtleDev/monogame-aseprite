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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Content.Processors;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes a <see cref="RawTilemap"/> from an aseprite file that was imported.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tilemap Processor - MonoGame.Aseprite")]
public sealed class TilemapContentProcessor : ContentProcessor<ContentImporterResult, ContentProcessorResult<RawTilemap>>
{
    /// <summary>
    ///     Gets or Sets the index of the <see cref="AsepriteFile"/> element in the <see cref="AsepriteFile"/> file that
    ///     contains the tilemap to process.
    /// </summary>
    [DisplayName("Frame Index")]
    public int FrameIndex { get; set; } = 0;

    /// <summary>
    ///     Gets or Sets a value that indicates whether only visible <see cref="AsepriteLayer"/> elements should be 
    ///     included.
    /// </summary>
    [DisplayName("Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;

    /// <summary>
    ///     Processes a <see cref="RawTilemap"/> from the contents of an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="ContentImporterResult"/> from the import process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that provides contextual information about the content being processed.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentProcessorResult{T}"/> containing the <see cref="RawTilemap"/> created by this
    ///     method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the <see cref="FrameIndex"/> property is less than zero or is greater than or  equal to the total 
    ///     number of  <see cref="AsepriteFrame"/> elements in the given <see cref="AsepriteFile"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if <see cref="AsepriteLayer"/> elements are found in the <see cref="AsepriteFile"/> with duplicate 
    ///     names.  Tilemaps must contain layers with unique names even though aseprite does not enforce unique names 
    ///     for <see cref="AsepriteLayer"/> elements.
    /// </exception>
    public override ContentProcessorResult<RawTilemap> Process(ContentImporterResult content, ContentProcessorContext context)
    {
        AsepriteFile aseFile = AsepriteFile.Load(content.Path);
        RawTilemap tilemap = RawTilemapProcessor.Process(aseFile, FrameIndex, OnlyVisibleLayers);
        return new(tilemap);
    }
}
