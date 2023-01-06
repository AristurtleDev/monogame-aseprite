/* ----------------------------------------------------------------------------
MIT License

Copyright (c) 2023 Christopher Whitley

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
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MonoGame.Aseprite.AsepriteTypes;

/// <summary>
///     Represents a frame in an Aseprite image.
/// </summary>
/// <param name="Size">
///     The width and height extents, in pixels, of this
///     <see cref="AsepriteFrame"/>.
/// </param>
/// <param name="Cels">
///     A <see cref="ReadOnlyCollection{T}"/> of the <see cref="AsepriteCel"/>
///     elements that are on this <see cref="AsepriteFrame"/>.
/// </param>
/// <param name="Duration">
///     The total duration, in milliseconds, of this <see cref="AsepriteFrame"/>
///     when used in an animation.
/// </param>
public sealed record AsepriteFrame(Size Size, ImmutableArray<AsepriteCel> Cels, int Duration)
{

    /// <summary>
    ///     Flattens this <see cref="AsepriteFrame"/> by combining all of the
    ///     <see cref="AsepriteCel"/> elements in this frame into a single
    ///     image representation.  <see cref="AsepriteCel"/> elements are
    ///     combined by blending each <see cref="AsepriteCel"/> from
    ///     bottom-to-top using the <see cref="BlendMode"/> of the
    ///     <see cref="AsepriteLayer"/> element that each
    ///     <see cref="AsepriteCel"/> is on.  The result is a new array of
    ///     <see cref="Microsoft.Xna.Framework.Color"/> values where each index
    ///     represents a pixel in the generated image.  Pixel order is from
    ///     top-to-bottom, read left-to-right.
    /// </summary>
    /// <param name="onlyVisibleLayers">
    ///     Indicates whether only <see cref="AsepriteCel"/> elements in this
    ///     <see cref="AsepriteFrame"/> that are on a
    ///     <see cref="AsepriteLayer"/> that is visible should be included.
    /// </param>
    /// <param name="includeBackgroundLayer">
    ///     Indicates whether <see cref="AsepriteCel"/> elements in this
    ///     <see cref="AsepriteFrame"/> that are on a
    ///     <see cref="AsepriteLayer"/> that is marked as the background in
    ///     the Aseprite UI should be included.
    /// </param>
    /// <returns>
    ///     A new array of <see cref="Microsoft.Xna.Framework.Color"/> values
    ///     where each index represents a pixel in the generated image.  Pixel
    ///     order is from top-to-bottom, read left-to-right.
    /// </returns>
    public ImmutableArray<Color> FlattenFrame(bool onlyVisibleLayers = true, bool includeBackgroundLayer = false)
    {
        Color[] result = new Color[Size.Width * Size.Height];

        for (int c = 0; c < Cels.Length; c++)
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
                celWidth = imageCel.Size.Width;
                celHeight = imageCel.Size.Height;
                celX = imageCel.Position.X;
                celY = imageCel.Position.Y;
            }
            else if (cel is AsepriteTilemapCel tilemapCel)
            {
                AsepriteTileset tileset = ((AsepriteTilemapLayer)cel.Layer).Tileset;
                celWidth = tilemapCel.Size.Width * tileset.TileSize.Width;
                celHeight = tilemapCel.Size.Height * tileset.TileSize.Height;
                celX = tilemapCel.Position.X;
                celY = tilemapCel.Position.Y;
                pixels = new Color[celWidth * celHeight];

                for (int t = 0; t < tilemapCel.Tiles.Length; t++)
                {
                    AsepriteTile tile = tilemapCel.Tiles[t];
                    int column = t % tilemapCel.Size.Width;
                    int row = t / tilemapCel.Size.Width;
                    ImmutableArray<Color> tilePixels = tileset[tile.TilesetTileId];

                    for (int p = 0; p < tilePixels.Length; p++)
                    {
                        // int px = (p % tileset.TileWidth) + (column * tileset.TileWidth);
                        int px = (p % tileset.TileSize.Width) + (column * tileset.TileSize.Width);
                        int py = (p / tileset.TileSize.Width) + (row * tileset.TileSize.Height);
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
                int index = py * Size.Width + px;

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

        return result.ToImmutableArray();
    }
}

// /// <summary>
// ///     Represents a frame in an Aseprite image.
// /// </summary>
// public sealed class AsepriteFrame
// {
//     /// <summary>
//     ///     Gets the width and height extents, in pixels of this
//     ///     <see cref="Frame"/>.
//     /// </summary>
//     public Size Size { get; }

//     /// <summary>
//     ///     Gets a <see cref="ReadOnlyCollection{T}"/> of the
//     ///     <see cref="AsepriteCel"/> elements that are in this
//     ///     <see cref="AsepriteFrame"/>.
//     /// </summary>
//     public ReadOnlyCollection<AsepriteCel> Cels { get; }

//     /// <summary>
//     ///     Gets the total duration, in milliseconds, of this
//     ///     <see cref="AsepriteFrame"/> when used in an animation.
//     /// </summary>
//     public int Duration { get; }

//     internal AsepriteFrame(int width, int height, int duration, List<AsepriteCel> cels)
//     {
//         Size = new(width, height);
//         Duration = duration;
//         Cels = cels.AsReadOnly();
//     }

//     /// <summary>
//     ///     Flattens this <see cref="AsepriteFrame"/> by combining all of the
//     ///     <see cref="AsepriteCel"/> elements in this frame into a single
//     ///     image representation.  <see cref="AsepriteCel"/> elements are
//     ///     combined by blending each <see cref="AsepriteCel"/> from
//     ///     bottom-to-top using the <see cref="BlendMode"/> of the
//     ///     <see cref="AsepriteLayer"/> element that each
//     ///     <see cref="AsepriteCel"/> is on.  The result is a new array of
//     ///     <see cref="Microsoft.Xna.Framework.Color"/> values where each index
//     ///     represents a pixel in the generated image.  Pixel order is from
//     ///     top-to-bottom, read left-to-right.
//     /// </summary>
//     /// <param name="onlyVisibleLayers">
//     ///     Indicates whether only <see cref="AsepriteCel"/> elements in this
//     ///     <see cref="AsepriteFrame"/> that are on a
//     ///     <see cref="AsepriteLayer"/> that is visible should be included.
//     /// </param>
//     /// <param name="includeBackgroundLayer">
//     ///     Indicates whether <see cref="AsepriteCel"/> elements in this
//     ///     <see cref="AsepriteFrame"/> that are on a
//     ///     <see cref="AsepriteLayer"/> that is marked as the background in
//     ///     the Aseprite UI should be included.
//     /// </param>
//     /// <returns>
//     ///     A new array of <see cref="Microsoft.Xna.Framework.Color"/> values
//     ///     where each index represents a pixel in the generated image.  Pixel
//     ///     order is from top-to-bottom, read left-to-right.
//     /// </returns>
//     public Color[] FlattenFrame(bool onlyVisibleLayers = true, bool includeBackgroundLayer = false)
//     {
//         Color[] result = new Color[Size.Width * Size.Height];

//         for (int c = 0; c < _cels.Count; c++)
//         {
//             AsepriteCel cel = _cels[c];

//             //  Are we only processing cels on visible layers?
//             if (onlyVisibleLayers && !cel.Layer.IsVisible) { continue; }

//             //  Are we processing cels on background layers?
//             if (cel.Layer.IsBackground && !includeBackgroundLayer) { continue; }

//             //  Premultiply the opacity of the cell and the opacity of the layer
//             byte opacity = BlendFunctions.MUL_UN8(cel.Opacity, cel.Layer.Opacity);

//             Color[] pixels;
//             int celWidth, celHeight, celX, celY;

//             if (cel is AsepriteImageCel imageCel)
//             {
//                 pixels = imageCel.Pixels.ToArray();
//                 celWidth = imageCel.Width;
//                 celHeight = imageCel.Height;
//                 celX = imageCel.Position.X;
//                 celY = imageCel.Position.Y;
//             }
//             else if (cel is AsepriteTilemapCel tilemapCel)
//             {
//                 AsepriteTileset tileset = ((AsepriteTilemapLayer)cel.Layer).Tileset;
//                 celWidth = tilemapCel.Width * tileset.TileWidth;
//                 celHeight = tilemapCel.Height * tileset.TileHeight;
//                 celX = tilemapCel.Position.X;
//                 celY = tilemapCel.Position.Y;
//                 pixels = new Color[celWidth * celHeight];

//                 for (int t = 0; t < tilemapCel.TileCount; t++)
//                 {
//                     AsepriteTile tile = tilemapCel[t];
//                     int column = t % tilemapCel.Width;
//                     int row = t / tilemapCel.Width;
//                     Color[] tilePixels = tileset[tile.TilesetTileId];

//                     for (int p = 0; p < tilePixels.Length; p++)
//                     {
//                         int px = (p % tileset.TileWidth) + (column * tileset.TileWidth);
//                         int py = (p / tileset.TileWidth) + (row * tileset.TileHeight);
//                         int index = py * celWidth + px;
//                         pixels[index] = tilePixels[p];
//                     }
//                 }
//             }
//             else
//             {
//                 //  Only image and tilemap cels have pixels to process
//                 continue;
//             }

//             for (int p = 0; p < pixels.Length; p++)
//             {
//                 int px = (p % celWidth) + celX;
//                 int py = (p / celWidth) + celY;
//                 int index = py * Size.Width + px;

//                 //  Sometimes a cel can have a negative x and/or y value.  This
//                 //  is caused by selecting an area in within Aseprite and then
//                 //  moving a portion of the selected pixels outside the canvas.
//                 //  We don't care about these pixels, so if the index is outside
//                 //  the range of the array to store them in, then we'll just
//                 //  ignore them
//                 if (index < 0 || index >= result.Length) { continue; }

//                 Color backdrop = result[index];
//                 Color source = pixels[p];
//                 result[index] = BlendFunctions.Blend(cel.Layer.BlendMode, backdrop, source, opacity);
//             }
//         }

//         return result;
//     }
// }
