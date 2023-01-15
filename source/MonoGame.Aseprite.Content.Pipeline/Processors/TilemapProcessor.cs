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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

namespace MonoGame.Aseprite.Content.Pipeline.Processors;

[ContentProcessor(DisplayName = "Aseprite Tilemap Processor - MonoGame.Aseprite")]
public sealed class TilemapProcessor : ContentProcessor<AsepriteFile, TilemapContent>
{
    public override TilemapContent Process(AsepriteFile file, ContentProcessorContext context)
    {
        List<TilesetContent> tilesetContent = GetTilesets(file);
        List<TilemapLayerContent> layerContent = GetLayers(file);

        return new(tilesetContent, layerContent);
    }

    private List<TilesetContent> GetTilesets(AsepriteFile file)
    {
        List<TilesetContent> content = new();
        for (int i = 0; i < file.Tilesets.Count; i++)
        {
            Tileset tileset = file.Tilesets[i];
            TextureContent textureContent = new(tileset.Width, tileset.Height, tileset.Pixels);
            TilesetContent tilesetContent = new(tileset.Name, tileset.TileCount, tileset.TileWidth, tileset.TileHeight, textureContent);
            content.Add(tilesetContent);
        }
        return content;
    }

    private List<TilemapLayerContent> GetLayers(AsepriteFile file)
    {
        List<TilemapLayerContent> content = new();

        Frame frame = file.Frames[0];

        for (int i = 0; i < frame.Cels.Count; i++)
        {
            if (frame.Cels[i] is TilemapCel tilemapCel && tilemapCel.Layer.IsVisible)
            {
                string name = tilemapCel.Layer.Name;
                int tilesetID = tilemapCel.LayerAs<TilemapLayer>().TilesetID;
                int columns = tilemapCel.Width;
                int rows = tilemapCel.Height;
                Point offset = new(tilemapCel.Position.X, tilemapCel.Position.Y);
                int x = tilemapCel.Position.X;
                int y = tilemapCel.Position.Y;
                string tilesetName = tilemapCel.Tileset.Name;

                TileContent[] tiles = new TileContent[tilemapCel.Tiles.Count];

                for (int j = 0; j < tilemapCel.Tiles.Count; j++)
                {
                    Tile tile = tilemapCel.Tiles[j];
                    byte flipFlag = (byte)(0 |
                                    (tile.XFlip != 0 ? 1 : 0) |
                                    (tile.YFlip != 0 ? 2 : 0));
                    TileContent tileContent = new(flipFlag, tile.Rotation, tile.TilesetTileID);
                    tiles[j] = tileContent;
                }

                TilemapLayerContent layerContent = new(name, tilesetID, columns, rows, offset, tiles);
                content.Add(layerContent);
            }
        }

        return content;
    }

    // private TilesetCollectionContent GetTilesetCollectionContent(AsepriteFile file)
    // {
    //     TilesetCollectionContent collectionContent = new();

    //     for (int i = 0; i < file.Tilesets.Count; i++)
    //     {
    //         Tileset tileset = file.Tilesets[i];
    //         TextureContent textureContent = new(tileset.Size, tileset.Pixels);
    //         TilesetContent tilesetContent = new(tileset.Name, tileset.TileCount, tileset.TileSize, textureContent);
    //         collectionContent.Tilesets.Add(tilesetContent);
    //     }

    //     return collectionContent;
    // }

    // private List<TilemapFrameContent> GetFrameContent(AsepriteFile file)
    // {
    //     List<TilemapFrameContent> content = new();

    //     for (int i = 0; i < file.Frames.Count; i++)
    //     {
    //         Frame frame = file.Frames[i];
    //         string frameName = new($"frame_{i}");
    //         TilemapFrameContent frameContent = new(frameName, frame.Duration);

    //         for (int c = 0; c < frame.Cels.Count; c++)
    //         {
    //             if (frame.Cels[c] is TilemapCel cel)
    //             {
    //                 Tileset celTileset = cel.Tileset;
    //                 TilemapLayer celLayer = cel.LayerAs<TilemapLayer>();

    //                 int widthInTiles = frame.Size.X / celTileset.TileSize.X;
    //                 int heightInTiles = frame.Size.Y / celTileset.TileSize.Y;

    //                 //  Create the tile array and fill it with 0's initially
    //                 int[] tiles = new int[widthInTiles * heightInTiles];
    //                 Array.Fill<int>(tiles, 0);

    //                 //  Then copy over the cel tiles
    //                 Array.Copy(cel.Tiles.ToArray(), 0, tiles, tiles.Length - cel.Tiles.Count, cel.Tiles.Count);

    //                 TilemapLayerContent layerContent = new(celLayer.Tileset.ID, celLayer.Name, celLayer.Tileset.TileSize, tiles);
    //                 frameContent.Layers.Add(layerContent);
    //             }
    //         }

    //         content.Add(frameContent);
    //     }

    //     return content;
    // }

}
