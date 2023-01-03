/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2022 Christopher Whitley

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

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class AsepriteFrame
{
    internal Point Size { get; }
    internal List<AsepriteCel> Cels { get; } = new();
    internal int CelCount => Cels.Count;
    internal int Duration { get; }

    internal AsepriteCel this[int index]
    {
        get
        {
            if(index < 0 || index > CelCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return Cels[index];
        }
    }

    internal AsepriteFrame(Point size, int duration)
    {
        Size = size;
        Duration = duration;
    }

    internal Color[] FlattenFrame(bool onlyVisibleLayers, bool includeBackgroundLayer)
    {
        Color[] result = new Color[Size.X * Size.Y];

        for (int c = 0; c < CelCount; c++)
        {
            AsepriteCel cel = Cels[c];

            //  Are we only processing cels on visible layers?
            if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }

            //  Are we processing cels on background layers?
            if (cel.Layer.IsBackground && !includeBackgroundLayer) { continue; }

            //  Premultiply the opacity of the cell and the opacity of the layer
            byte opacity = BlendFunctions.MUL_UN8(cel.Opacity, cel.Layer.Opacity);

            Color[] pixels;
            int width, height, x, y;

            if(cel is AsepriteImageCel imageCel)
            {
                pixels = imageCel.Pixels;
                width = imageCel.Size.X;
                height = imageCel.Size.Y;
                x = imageCel.Position.X;
                y = imageCel.Position.Y;
            }
            else if(cel is AsepriteTilemapCel tilemapCel)
            {
                AsepriteTileset tileset = ((AsepriteTilemapLayer)cel.Layer).Tileset;
                width = tilemapCel.Size.X * tileset.Size.X;
                height = tilemapCel.Size.Y * tileset.Size.Y;
                x = tilemapCel.Position.X;
                y = tilemapCel.Position.Y;
                pixels = new Color[width * height];

                for (int t = 0; t < tilemapCel.TileCount; t++)
                {
                    AsepriteTile tile = tilemapCel[t];
                    int column = t % tilemapCel.Size.X;
                    int row = t / tilemapCel.Size.X;
                    Color[] tilePixels = tileset[tile.TilesetTileId];

                    for (int p = 0; p < tilePixels.Length; p++)
                    {
                        int px = (p % tileset.Size.X) + (column * tileset.Size.X);
                        int py = (p / tileset.Size.X) + (row * tileset.Size.Y);
                        int index = py * width + px;
                        pixels[index] = tilePixels[p];
                    }
                }
            }
            else
            {
                //  Only image and tilemap cels have pixels to process
                continue;
            }

            for (int p = 0; p < pixels.Length; p++)
            {
                int px = (p % width) + x;
                int py = (p / width) + y;
                int index = py * Size.X + px;

                //  Sometimes a cel can have a negative x and/or y value.  This
                //  is caused by selecting an area in within Aseprite and then
                //  moving a portion of the selected pixels outside the canvas.
                //  We don't care about these pixels, so if the index is outside
                //  the range of the array to store them in, then we'll just
                //  ignore them
                if (index < 0 || index >= result.Length) { continue; }

                Color backdrop = result[index];
                Color source = pixels[p];
                result[index] = BlendFunctions.Blend(cel.Layer.BlendMode, backdrop, source, opacity);
            }
        }

        return result;
    }
}
