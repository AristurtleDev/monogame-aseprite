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

namespace MonoGame.Aseprite.Content.Pipeline.AsepriteTypes;

internal sealed class Frame
{
    internal List<Cel> Cels { get; } = new();
    internal int Width { get; }
    internal int Height { get; }
    internal int Duration { get; }

    internal Frame(int width, int height, int duration) => (Width, Height, Duration) = (width, height, duration);

    internal Color[] FlattenFrame(bool onlyVisibleLayers = true, bool includeBackgroundLayer = false)
    {
        Color[] result = new Color[Width * Height];

        for (int c = 0; c < Cels.Count; c++)
        {
            Cel cel = Cels[c];

            //  Are we only processing cels on visible layers?
            if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }

            //  Are we processing cels on background layers?
            if (cel.Layer.IsBackground && !includeBackgroundLayer) { continue; }

            //  Premultiply the opacity of the cell and the opacity of the layer
            byte opacity = BlendFunctions.MUL_UN8(cel.Opacity, cel.Layer.Opacity);

            Color[] pixels;
            int celWidth, celHeight, celX, celY;

            if (cel is ImageCel imageCel)
            {
                pixels = imageCel.Pixels.ToArray();
                celWidth = imageCel.Width;
                celHeight = imageCel.Height;
                celX = imageCel.Position.X;
                celY = imageCel.Position.Y;
            }
            else if (cel is TilemapCel tilemapCel)
            {
                Tileset tileset = tilemapCel.Tileset;
                celWidth = tilemapCel.Width * tileset.TileWidth;
                celHeight = tilemapCel.Height* tileset.TileHeight;
                celX = tilemapCel.Position.X;
                celY = tilemapCel.Position.Y;
                pixels = new Color[celWidth * celHeight];

                for (int t = 0; t < tilemapCel.Tiles.Count; t++)
                {
                    Tile tile = tilemapCel.Tiles[t];
                    int column = t % tilemapCel.Width;
                    int row = t / tilemapCel.Width;
                    Color[] tilePixels = tileset[tile.TilesetTileID];

                    for (int p = 0; p < tilePixels.Length; p++)
                    {
                        int px = (p % tileset.TileWidth) + (column * tileset.TileHeight);
                        int py = (p / tileset.TileWidth) + (row * tileset.TileHeight);
                        int index = py * celWidth + px;
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
                int px = (p % celWidth) + celX;
                int py = (p / celWidth) + celY;
                int index = py * Width + px;

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
