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
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a processor that accepts an Aseprite file and generates a
///     collection of the tilesets from the tilesets in the Aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tileset Collection Processor - MonoGame.Aseprite")]
public sealed class TilesetCollectionProcessor : CommonProcessor<AsepriteFile, TilesetCollectionProcessorResult>
{
    /// <summary>
    ///     Processes all tilesets in the Aseprite file.
    /// </summary>
    /// <param name="file">
    ///     The Aseprite file to process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that contains contextual information
    ///     about content being processed.
    /// </param>
    /// <returns>
    ///     A new instance of the TilesetCollectionProcessorResult class that
    ///     contains the collection of tileset content from the tilesets
    ///     processed.
    /// </returns>
    public override TilesetCollectionProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        TilesetCollectionProcessorResult content = new();

        for (int i = 0; i < file.Tilesets.Count; i++)
        {
            AsepriteTileset tileset = file.Tilesets[i];
            TilesetContent tilesetContent = CreateTilesetContent(tileset, file.Name, context);
            content.Tilesets.Add(tilesetContent);
        }

        return content;
    }
}
