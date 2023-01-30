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

using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Processors;
using MonoGame.Aseprite.Processors.RawTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
/// Defines a content processor that processes all aseprite tilesets from an aseprite file as a collection of raw
/// tilesets.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Collection Processor - MonoGame.Aseprite")]
public sealed class RawTilesetCollectionProcessor : ContentProcessor<ContentImporterResult<AsepriteFile>, RawTileset[]>
{
    /// <summary>
    /// Processes all aseprite tilesets from an aseprite file.
    /// </summary>
    /// <param name="content">The result of the content importer that contains the aseprite file content.</param>
    /// <param name="context">
    /// The content processor context that provides contextual information about the content being
    /// processed.
    /// </param>
    /// <returns>An array of the raw tilesets created by this method.</returns>
    public override RawTileset[] Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {
        AsepriteFile aseFile = content.Data;
        RawTileset[] rawTilesets = new RawTileset[aseFile.Tilesets.Length];

        for (int i = 0; i < aseFile.Tilesets.Length; i++)
        {
            rawTilesets[i] = TilesetProcessor.ProcessRaw(aseFile.Tilesets[i]);
        }

        return rawTilesets;
    }
}
