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
using MonoGame.Aseprite.Utilities;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Defines a  <see cref="AsepriteFrame"/> in an aseprite image.
/// </summary>
public sealed class AsepriteFrame
{
    private AsepriteCel[] _cels;

    /// <summary>
    ///     Gets a read-only span of the <see cref="AsepriteCel"/> elements in this  <see cref="AsepriteFrame"/>.
    /// </summary>
    public ReadOnlySpan<AsepriteCel> Cels => _cels;

    /// <summary>
    ///     Gets the name assigned this <see cref="AsepriteFrame"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Gets the width, in pixels, of this  <see cref="AsepriteFrame"/>.
    /// </summary>
    public int Width { get; }

    /// <summary>
    ///     Gets the height, in pixels, of this  <see cref="AsepriteFrame"/>.
    /// </summary>
    public int Height { get; }

    /// <summary>
    ///     Gets the duration, in milliseconds, of this  <see cref="AsepriteFrame"/>.
    /// </summary>
    public int DurationInMilliseconds { get; }

    internal AsepriteFrame(string name, int width, int height, int duration, AsepriteCel[] cels) =>
        (Name, Width, Height, DurationInMilliseconds, _cels) = (name, width, height, duration, cels);

    /// <summary>
    ///     Flattens this  <see cref="AsepriteFrame"/> by combining all <see cref="AsepriteCel"/> elements into a single 
    ///     image representation.
    /// </summary>
    /// <param name="onlyVisibleLayers">
    ///     Indicates whether only <see cref="AsepriteCel"/> elements on visible <see cref="AsepriteLayer"/> elements 
    ///     should be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates whether <see cref="AsepriteCel"/> elements are on a <see cref="AsepriteLayer"/> marked as a 
    ///     background layer should be included.
    /// </param>
    /// <returns>
    ///     A new <see cref="Array"/> of color values that represent the image of this  <see cref="AsepriteFrame"/>.
    /// </returns>
    public Color[] FlattenFrame(bool onlyVisibleLayers = true, bool includeBackgroundLayer = false, bool includeTilemapCel = true)
    {
        Color[] result = new Color[Width * Height];

        for (int c = 0; c < _cels.Length; c++)
        {
            AsepriteCel cel = _cels[c];

            //  Are we only processing cels on visible layers?
            if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }

            //  Are we processing cels on background layers?
            if (cel.Layer.IsBackground && !includeBackgroundLayer) { continue; }

            if (cel is AsepriteImageCel imageCel)
            {
                BlendCel(backdrop: result,
                         source: imageCel.Pixels,
                         blendMode: imageCel.Layer.BlendMode,
                         celX: imageCel.Position.X,
                         celY: imageCel.Position.Y,
                         celWidth: imageCel.Width,
                         celOpacity: imageCel.Opacity,
                         layerOpacity: imageCel.Layer.Opacity);
            }
            else if (includeTilemapCel && cel is AsepriteTilemapCel tilemapCel)
            {
                BlendTilemapCel(backdrop: result,
                                cel: tilemapCel);
            }
        }

        return result;
    }

    private void BlendTilemapCel(Span<Color> backdrop, AsepriteTilemapCel cel)
    {
        //  Premultiply the opacity of the cel and the opacity of the layer
        byte opacity = BlendFunctions.MUL_UN8(cel.Opacity, cel.Layer.Opacity);

        AsepriteTileset tileset = cel.Tileset;
        int width = cel.Columns * tileset.TileWidth;
        int height = cel.Rows * tileset.TileHeight;
        int x = cel.Position.X;
        int y = cel.Position.Y;

        Span<Color> pixels = new Color[width * height];
        ReadOnlySpan<AsepriteTile> tiles = cel.Tiles;
        for (int i = 0; i < tiles.Length; i++)
        {
            AsepriteTile tile = tiles[i];
            int column = i % cel.Columns;
            int row = i / cel.Columns;
            ReadOnlySpan<Color> tilePixels = tileset[tile.TilesetTileID];

            for (int j = 0; j < tilePixels.Length; j++)
            {
                int px = (j % tileset.TileWidth) + (column * tileset.TileHeight);
                int py = (j / tileset.TileWidth) + (row * tileset.TileHeight);
                int index = py * width + px;
                pixels[index] = tilePixels[j];
            }
        }

        BlendCel(backdrop, pixels, cel.Layer.BlendMode, x, y, width, cel.Opacity, cel.Layer.Opacity);
    }

    private void BlendCel(Span<Color> backdrop, ReadOnlySpan<Color> source, AsepriteBlendMode blendMode, int celX, int celY, int celWidth, int celOpacity, int layerOpacity)
    {
        for (int i = 0; i < source.Length; i++)
        {
            int x = (i % celWidth) + celX;
            int y = (i / celWidth) + celY;
            int index = y * Width + x;

            //  Sometimes a cel can have a negative x and/or y value.  This is caused by selecting an area in within
            //  Aseprite and then moving a portion of the selected pixels outside the canvas. We don't care about these
            //  pixels, so if the index is outside the range of the array to store them in, then we'll just ignore them.
            if (index < 0 || index >= backdrop.Length) { continue; }

            Color b = backdrop[index];
            Color s = source[i];
            byte opacity = BlendFunctions.MUL_UN8(celOpacity, layerOpacity);
            backdrop[index] = BlendFunctions.Blend(blendMode, b, s, opacity);
        }
    }
}
