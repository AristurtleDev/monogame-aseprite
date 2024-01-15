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
using MonoGame.Aseprite.AsepriteTypes;
using MonoGame.Aseprite.RawTypes;

namespace MonoGame.Aseprite.Content.Processors;

/// <inheritdoc/>
public static partial class TilemapProcessor
{
    /// <summary>
    ///     Processes a <see cref="RawTilemap"/> from the <see cref="AsepriteFrame"/> elements as the specified index 
    ///     in the given <see cref="AsepriteFile"/>.
    /// </summary>
    /// <param name="aseFile">
    ///     The <see cref="AsepriteFile"/> to processes the <see cref="RawTilemap"/> from.
    /// </param>
    /// <param name="frameIndex">
    ///     The index of the <see cref="AsepriteFrame"/> element in the <see cref="AsepriteFile"/> to processes.
    /// </param>
    /// <param name="onlyVisibleLayers">
    ///     Indicates if only <see cref="AsepriteLayer"/> elements that are visible should be processed.
    /// </param>
    /// <returns>
    ///     The <see cref="RawTilemap"/> created by this method.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown if the index specified is less than zero or is greater than or  equal to the total number of 
    ///     <see cref="AsepriteFrame"/> elements in the given <see cref="AsepriteFile"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if <see cref="AsepriteLayer"/> elements are found in the <see cref="AsepriteFile"/> with duplicate 
    ///     names.  Tilemaps must contain layers with unique names even though aseprite does not enforce unique names 
    ///     for <see cref="AsepriteLayer"/> elements.
    /// </exception>
    public static RawTilemap ProcessRaw(AsepriteFile aseFile, int frameIndex, bool onlyVisibleLayers = true)
    {
        AsepriteFrame aseFrame = aseFile.GetFrame(frameIndex);

        List<RawTileset> rawTilesets = new();
        List<RawTilemapLayer> rawLayers = new();
        HashSet<string> layerNameCheck = new();
        HashSet<int> tilesetIDCheck = new();

        for (int c = 0; c < aseFrame.Cels.Length; c++)
        {
            //  Only care about tilemap cels.
            if (aseFrame.Cels[c] is not AsepriteTilemapCel tilemapCel)
            {
                continue;
            }

            //  Only continue if layer is visible or if explicitly told to include non-visible <see cref="AsepriteLayer"/> elements.
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

                tiles[t] = new(aseTile.TilesetTileID, flipHorizontally, flipVertically, aseTile.DFlip);
            }

            int columns = tilemapCel.Columns;
            int rows = tilemapCel.Rows;
            Point offset = tilemapCel.Position;

            RawTilemapLayer rawLayer = new(layerName, tilesetID, columns, rows, tiles, offset);
            rawLayers.Add(rawLayer);
        }

        return new(aseFile.Name, rawLayers.ToArray(), rawTilesets.ToArray());
    }
}
