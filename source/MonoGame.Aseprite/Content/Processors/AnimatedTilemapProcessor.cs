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
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;
using MonoGame.Aseprite.Tilemaps;

namespace MonoGame.Aseprite.Content.Processors;

/// <summary>
///     Defines a processor that processes an <see cref="AnimatedTilemap"/> from an <see cref="AsepriteFile"/>.
/// </summary>
/// <seealso cref="MonoGame.Aseprite.Content.Processors"/>
public static class AnimatedTilemapProcessor
{
    /// <summary>
    ///     Processes an <see cref="AnimatedTilemap"/> from the given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <remarks>
    ///     When using this processor, all <see cref="MonoGame.Aseprite.AsepriteTypes.AsepriteFrame"/> elements from the
    ///     <see cref="AsepriteFile"/> are processed. <see cref="MonoGame.Aseprite.AsepriteTypes.AsepriteTag"/> elements
    ///     are ignored at this time (though may be implemented to support tags in the future).  Only 
    ///     <see cref="MonoGame.Aseprite.AsepriteTypes.AsepriteTilemapCel"/> elements in each frame are processed to
    ///     generate the tile data.
    /// </remarks>
    /// <param name="device" cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice">
    ///     The <see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> used to create graphical resources.
    ///  </param>
    /// <param name="aseFile" cref="AsepriteFile">
    ///     The <see cref="AsepriteFile"/> to processed the <see cref="AnimatedTilemap"/> from.
    /// </param>
    /// <param name="onlyVisibleLayers" cref="bool">
    ///     Indicates if only layers that are visible should be processed.
    /// </param>
    /// <returns cref="AnimatedTilemap">
    ///     The <see cref="AnimatedTilemap"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if layers are found in the <see cref="AsepriteFile"/> with duplicate names.  Tilemaps must contain 
    ///     layers with unique names even though Aseprite does not enforce unique names for layers.
    /// </exception>
    /// <seealso cref="AnimatedTilemap"/>
    /// <seealso cref="AsepriteFile"/>
    public static AnimatedTilemap Process(GraphicsDevice device, AsepriteFile aseFile, bool onlyVisibleLayers = true)
    {
        RawAnimatedTilemap rawTilemap = ProcessRaw(aseFile, onlyVisibleLayers);
        return AnimatedTilemap.FromRaw(device, rawTilemap);
    }

    /// <summary>
    ///     Processes a <see cref="RawAnimatedTilemap"/> from the given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> to processes the <see cref="RawAnimatedTilemap"/> from.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteLayer"/> elements that are visible should be processed.
    /// </param>
    /// <returns>
    ///     The <see cref="RawAnimatedTilemap"/> created by this method.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if <see cref="AsepriteLayer"/> elements are found in the <see cref="AsepriteFile"/> with duplicate
    ///     names. Tilemaps must contain layers with unique names even though Aseprite does not enforce unique names
    ///     for layers.
    /// </exception>
    public static RawAnimatedTilemap ProcessRaw(AsepriteFile aseFile, bool onlyVisibleLayers = true)
    {
        List<RawTileset> rawTilesets = new();
        RawTilemapFrame[] rawFrames = new RawTilemapFrame[aseFile.Frames.Length];
        HashSet<int> tilesetIDCheck = new();

        for (int f = 0; f < aseFile.Frames.Length; f++)
        {
            AsepriteFrame aseFrame = aseFile.Frames[f];
            List<RawTilemapLayer> rawLayers = new();
            HashSet<string> layerNameCheck = new();

            for (int c = 0; c < aseFrame.Cels.Length; c++)
            {
                //  Only care about tilemap cels.
                if (aseFrame.Cels[c] is not AsepriteTilemapCel tilemapCel)
                {
                    continue;
                }

                //  Only continue if layer is visible or if explicitly told to include non-visible layers.
                if (!tilemapCel.Layer.IsVisible && onlyVisibleLayers)
                {
                    continue;
                }

                //  Need to perform a check that we don't have duplicate layer names.  This is because aseprite allows
                //  duplicate layer names, but we require unique names from this point on.
                AsepriteTilemapLayer aseTilemapLayer = tilemapCel.LayerAs<AsepriteTilemapLayer>();
                string layerName = aseTilemapLayer.Name;

                if (!layerNameCheck.Add(layerName))
                {
                    throw new InvalidOperationException($"Duplicate layer name '{tilemapCel.Layer.Name}' found.  Layer names must be unique for tilemaps");
                }

                int tilesetID = aseTilemapLayer.Tileset.ID;

                if (tilesetIDCheck.Add(tilesetID))
                {
                    RawTileset rawTileset = TilesetProcessor.ProcessRaw(aseTilemapLayer.Tileset);
                    rawTilesets.Add(rawTileset);
                }

                RawTilemapTile[] tiles = new RawTilemapTile[tilemapCel.Tiles.Length];

                for (int t = 0; t < tilemapCel.Tiles.Length; t++)
                {
                    AsepriteTile aseTile = tilemapCel.Tiles[t];
                    bool flipHorizontally = aseTile.XFlip != 0;
                    bool flipVertically = aseTile.YFlip != 0;

                    tiles[t] = new(aseTile.TilesetTileID, flipHorizontally, flipVertically, aseTile.Rotation);
                }

                int columns = tilemapCel.Columns;
                int rows = tilemapCel.Rows;
                Point offset = tilemapCel.Position;

                RawTilemapLayer rawLayer = new(layerName, tilesetID, columns, rows, tiles, offset);
                rawLayers.Add(rawLayer);

            }

            rawFrames[f] = new(aseFrame.DurationInMilliseconds, rawLayers.ToArray());
        }

        return new(aseFile.Name, rawTilesets.ToArray(), rawFrames);

    }
}
