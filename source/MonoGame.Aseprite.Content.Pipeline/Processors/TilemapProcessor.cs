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

[ContentProcessor(DisplayName = "Aseprite Tilemap Processor - MonoGame.Aseprite")]
public sealed class TilemapProcessor : ContentProcessor<AsepriteFile, TilemapProcessorResult>
{
    public override TilemapProcessorResult Process(AsepriteFile file, ContentProcessorContext context)
    {
        List<TilemapFrameContent> frames = GetFrameContent(file);

        return new(file.Tilesets, frames);
    }

    private List<TilesetContent> GetTilesetContent(AsepriteFile file)
    {

        List<TilesetContent> tilesets = new();

        for (int i = 0; i < file.Tilesets.Count; i++)
        {
            Tileset tileset = file.Tilesets[i];
            TilesetContent content = new(tileset.Name, tileset.Pixels, tileset.Size, tileset.TileSize, tileset.TileCount);
            tilesets.Add(content);
        }

        return tilesets;
    }

    private List<TilemapFrameContent> GetFrameContent(AsepriteFile file)
    {
        List<TilemapFrameContent> content = new();

        for (int i = 0; i < file.Frames.Count; i++)
        {
            Frame frame = file.Frames[i];
            string frameName = new($"frame_{i}");
            TimeSpan frameDuration = TimeSpan.FromMilliseconds(frame.Duration);
            TilemapFrameContent frameContent = new(frameName, frameDuration);

            for (int c = 0; c < frame.Cels.Count; c++)
            {
                if(frame.Cels[c] is TilemapCel cel)
                {
                    Tileset celTileset = cel.Tileset;
                    TilemapLayer celLayer = cel.LayerAs<TilemapLayer>();

                    int widthInTiles = frame.Size.X / celTileset.TileSize.X;
                    int heightInTiles = frame.Size.Y / celTileset.TileSize.Y;

                    //  Create the tile array and fill it with 0's initially
                    int[] tiles = new int[widthInTiles * heightInTiles];
                    Array.Fill<int>(tiles, 0);

                    //  Then copy over the cel tiles
                    Array.Copy(cel.Tiles.ToArray(), 0, tiles, tiles.Length - cel.Tiles.Count, cel.Tiles.Count);

                    TilemapLayerContent layerContent = new(celLayer.Tileset.ID, celLayer.Name, celLayer.Tileset.TileSize, tiles);
                    frameContent.Layers.Add(layerContent);
                }
            }

            content.Add(frameContent);
        }

        return content;
    }
}
