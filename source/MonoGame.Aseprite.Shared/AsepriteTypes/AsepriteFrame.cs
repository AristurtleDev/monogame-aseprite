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
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a frame in an Aseprite image.
/// </summary>
public sealed class AsepriteFrame
{
    private List<AsepriteCel> _cels = new();

    /// <summary>
    ///     The width and height extents, in pixels of this frame.
    /// </summary>
    public Point Size { get; }

    /// <summary>
    ///     The width, in pixels, of this frame.
    /// </summary>
    public int Width => Size.X;

    /// <summary>
    ///     THe height, in pixels, of this frame.
    /// </summary>
    public int Height => Size.Y;

    /// <summary>
    ///     A read-only collection of the cels that are on this frame.
    /// </summary>
    public ReadOnlyCollection<AsepriteCel> Cels => _cels.AsReadOnly();

    /// <summary>
    ///     The total number of cels on this frame.
    /// </summary>
    public int CelCount => _cels.Count;

    /// <summary>
    ///     Returns the cel from this frame at the specified index.
    /// </summary>
    /// <param name="index">
    ///     The index of the cel in this frame to return.
    /// </param>
    /// <returns>
    ///     The cel from this frame at the specified index.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than
    ///     or equal to the total number of cels in this frame.
    /// </exception>
    public AsepriteCel this[int index]
    {
        get
        {
            if (index < 0 || index > CelCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return _cels[index];
        }
    }

    /// <summary>
    ///     The duration, in milliseconds, of this frame when used in an
    ///     animation.
    /// </summary>
    public int Duration { get; }

    internal AsepriteFrame(Point size, int duration)
    {
        Size = size;
        Duration = duration;
    }

    internal void AddCel(AsepriteCel cel) => _cels.Add(cel);

    /// <summary>
    ///     Flattens this frame by combining all cels in this frame into a
    ///     single image.  Cels are combined by blending each cel from
    ///     bottom-to-top using the blend mode of the layer each cel is on. The
    ///     result is a new array of color values that represent the image of
    ///     this frame.
    /// </summary>
    /// <param name="onlyVisibleLayers">
    ///     Indicates whether only cels that are on layers that are visible
    ///     should be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates whether cels that are on a layer marked as a background
    ///     layer in this Aseprite UI should be included.
    /// </param>
    /// <returns>
    ///     A new array of color values that represent the flattened image of
    ///     this frame.
    /// </returns>
    public Color[] FlattenFrame(bool onlyVisibleLayers = true, bool includeBackgroundLayer = false)
    {
        Color[] result = new Color[Width * Height];

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
            int celWidth, celHeight, celX, celY;

            if (cel is AsepriteImageCel imageCel)
            {
                pixels = imageCel.Pixels.ToArray();
                celWidth = imageCel.Width;
                celHeight = imageCel.Height;
                celX = imageCel.X;
                celY = imageCel.Y;
            }
            else if (cel is AsepriteTilemapCel tilemapCel)
            {
                AsepriteTileset tileset = ((AsepriteTilemapLayer)cel.Layer).Tileset;
                celWidth = tilemapCel.Width * tileset.TileWidth;
                celHeight = tilemapCel.Height * tileset.TileHeight;
                celX = tilemapCel.X;
                celY = tilemapCel.Y;
                pixels = new Color[celWidth * celHeight];

                for (int t = 0; t < tilemapCel.TileCount; t++)
                {
                    AsepriteTile tile = tilemapCel[t];
                    int column = t % tilemapCel.Width;
                    int row = t / tilemapCel.Width;
                    Color[] tilePixels = tileset[tile.TilesetTileId];

                    for (int p = 0; p < tilePixels.Length; p++)
                    {
                        int px = (p % tileset.TileWidth) + (column * tileset.TileWidth);
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
