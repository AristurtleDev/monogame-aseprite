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
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
/// Defines a content processor that processes a raw tileset from an aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Processor - MonoGame.Aseprite")]
internal sealed class TilesetContentProcessor : ContentProcessor<ContentImporterResult, ContentProcessorResult<RawTileset>>
{
    /// <summary>
    /// Gets or Sets the name of the tileset to processes from the aseprite file.
    /// </summary>
    [DisplayName("Tileset Name")]
    public string TilesetName { get; set; } = string.Empty;

    /// <summary>
    /// Processes a raw tileset from an aseprite file.
    /// </summary>
    /// <param name="content">The result of the content importer.</param>
    /// <param name="context">
    /// The content processor context that provides contextual information about the content being
    /// processed.
    /// </param>
    /// <returns>The content processor result created by this method..</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the aseprite file being processes does not contain an tileset with the name specified for the
    /// the TilesetName property.
    /// </exception>
    public override ContentProcessorResult<RawTileset> Process(ContentImporterResult content, ContentProcessorContext context)
    {
        AsepriteFile aseFile = AsepriteFile.Load(content.Path);
        RawTileset tileset = RawTilesetProcessor.Process(aseFile, TilesetName);
        return new(tileset);
    }
}
