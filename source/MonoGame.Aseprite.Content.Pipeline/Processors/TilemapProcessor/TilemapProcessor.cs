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

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a content processor that processes a single <see cref="AsepriteFrame"/>, that contains
///     <see cref="AsepriteTilemapCel"/> elements, and generates a tilemap.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tilemap Processor - MonoGame.Aseprite")]
public sealed class TilemapProcessor : CommonProcessor<ContentImporterResult<AsepriteFile>, TilemapProcessorResult>
{
    /// <summary>
    ///     Gets or Sets a value that indicates whether only <see cref="AsepriteCel"/> elements that are on a visible
    ///     <see cref="AsepriteLayer"/> should be processed.
    /// </summary>
    [DisplayName("(Aseprite) Only Visible Layers")]
    public bool OnlyVisibleLayers { get; set; } = true;


    /// <summary>
    ///     Processes a single <see cref="AsepriteFrame"/> in an <see cref="AsepriteFile"/> as a tilemap.
    /// </summary>
    /// <param name="content">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <param name="context">
    ///     The <see cref="ContentProcessorContext"/> that provides contextual information  about the content being
    ///     processed.
    /// </param>
    /// <returns>
    ///     A new instance of the <see cref="TilemapProcessorResult"/> class that contains the result of this method.
    /// </returns>
    public override TilemapProcessorResult Process(ContentImporterResult<AsepriteFile> content, ContentProcessorContext context)
    {

        // *********************************************************************
        //  Generate the tileset content for each tileset
        // *********************************************************************
        ReadOnlySpan<AsepriteTileset> tilesets = content.Data.Tilesets;
        TilesetContent[] tilesetContent = new TilesetContent[tilesets.Length];
        for (int i = 0; i < tilesets.Length; i++)
        {
            AsepriteTileset tileset = tilesets[i];
            tilesetContent[i] = CreateTilesetContent(tileset, content.FileNameWithoutExtension, context);
        }

        // *********************************************************************
        //  Generate the layer data for each layer in the tilemap
        // *********************************************************************
        ReadOnlySpan<AsepriteCel> cels = content.Data.Frames[0].Cels;
        List<TilemapLayerContent> layerContent = new();
        for (int i = 0; i < cels.Length; i++)
        {
            if (cels[i] is AsepriteTilemapCel cel && (cel.Layer.IsVisible || !OnlyVisibleLayers))
            {
                TilemapLayerContent layer = CreateTilemapLayerContent(cel);
                layerContent.Add(layer);
            }
        }

        return new(tilesetContent, layerContent.ToArray());
    }
}
