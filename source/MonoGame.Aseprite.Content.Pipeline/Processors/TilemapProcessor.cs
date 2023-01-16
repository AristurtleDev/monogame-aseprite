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
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

/// <summary>
///     Defines a processor that accepts an Aseprite file and generates a
///     tilemap based on a single frame in the Aseprite file.
/// </summary>
[ContentProcessor(DisplayName = "Aseprite Tilemap Processor - MonoGame.Aseprite")]
public sealed class TilemapProcessor : CommonProcessor<AsepriteFile, TilemapProcessorResult>
{
    /// <summary>
    ///     Processes a single frame in an Aseprite file as a tilemap.
    /// </summary>
    /// <param name="file">
    ///     The Aseprite file to process.
    /// </param>
    /// <param name="context">
    ///     The content processor context that contains contextual information
    ///     about content being processed.
    /// </param>
    /// <returns>
    ///     A new instance of the TilemapProcessResult class that contains the
    ///     tilemap content of the frame processed.
    /// </returns>
    public override TilemapProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        TilemapProcessorResult result = new();

        // *********************************************************************
        //  Generate the tileset content for each tileset
        // *********************************************************************
        for (int i = 0; i < file.Tilesets.Count; i++)
        {
            Tileset tileset = file.Tilesets[i];
            TilesetContent tilesetContent = CreateTilesetContent(tileset, file.Name, context);
            result.Tilesets.Add(tilesetContent);
        }

        // *********************************************************************
        //  Generate the layer data for each layer in the tilemap
        // *********************************************************************
        Frame frame = file.Frames[0];

        for (int i = 0; i < frame.Cels.Count; i++)
        {
            if (frame.Cels[i] is TilemapCel cel && (OnlyVisibleLayers && !cel.Layer.IsVisible))
            {
                TilemapLayerContent layer = CreateTilemapLayerContent(cel);
                result.Layers.Add(layer);
            }
        }

        return result;
    }



    // private List<TilemapLayerContent> GetLayers(AsepriteFile file)
    // {
    //     List<TilemapLayerContent> content = new();

    //     Frame frame = file.Frames[0];

    //     for (int i = 0; i < frame.Cels.Count; i++)
    //     {
    //         if (frame.Cels[i] is TilemapCel tilemapCel && tilemapCel.Layer.IsVisible)
    //         {
    //             string name = tilemapCel.Layer.Name;
    //             int tilesetID = tilemapCel.LayerAs<TilemapLayer>().TilesetID;
    //             int columns = tilemapCel.Width;
    //             int rows = tilemapCel.Height;
    //             Point offset = new(tilemapCel.Position.X, tilemapCel.Position.Y);
    //             int x = tilemapCel.Position.X;
    //             int y = tilemapCel.Position.Y;
    //             string tilesetName = tilemapCel.Tileset.Name;

    //             TileContent[] tiles = new TileContent[tilemapCel.Tiles.Count];

    //             for (int j = 0; j < tilemapCel.Tiles.Count; j++)
    //             {
    //                 Tile tile = tilemapCel.Tiles[j];
    //                 byte flipFlag = (byte)(0 |
    //                                 (tile.XFlip != 0 ? 1 : 0) |
    //                                 (tile.YFlip != 0 ? 2 : 0));
    //                 TileContent tileContent = new(flipFlag, tile.Rotation, tile.TilesetTileID);
    //                 tiles[j] = tileContent;
    //             }

    //             TilemapLayerContent layerContent = new(name, tilesetID, columns, rows, offset, tiles);
    //             content.Add(layerContent);
    //         }
    //     }

    //     return content;
    // }
}
