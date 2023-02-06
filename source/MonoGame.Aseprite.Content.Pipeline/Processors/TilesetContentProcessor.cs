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
using MonoGame.Aseprite.Content.Processors;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes a <see cref="RawTileset"/> from an aseprite file that was imported.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Processor - MonoGame.Aseprite")]
internal sealed class TilesetContentProcessor : ContentProcessor<ContentImporterResult, ContentProcessorResult<RawTileset>>
{
    /// <summary>
    ///     Gets or Sets the name of the <see cref="AsepriteTileset"/> to processes from the <see cref="AsepriteFile"/>.
    /// </summary>
    [DisplayName("Tileset Name")]
    public string TilesetName { get; set; } = string.Empty;

    /// <summary>
    ///     Processes a <see cref="RawTileset"/> from the contents of an <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="ContentImporterResult"/> from the import process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that provides contextual information about the content being processed.
    /// </param>
    /// <returns>
    ///     A new <see cref="ContentProcessorResult{T}"/> containing the <see cref="RawTileset"/> created by this 
    ///     method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the given <see cref="AsepriteFile"/> does not contain an <see cref="AsepriteTileset"/> element 
    ///     with the name specified in the <see cref="TilesetName"/> property.
    /// </exception>
    public override ContentProcessorResult<RawTileset> Process(ContentImporterResult content, ContentProcessorContext context)
    {
        AsepriteFile aseFile = AsepriteFile.Load(content.Path);
        RawTileset tileset = RawTilesetProcessor.Process(aseFile, TilesetName);
        return new(tileset);
    }
}
