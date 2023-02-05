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

/// <summary>
/// Defines a processor that processes a raw tilemap from an aseprite file.
/// </summary>
public static class RawTilemapProcessor
{
    /// <summary>
    /// Processes a raw tilemap from the frame as the specified index in the given aseprite file.
    /// </summary>
    /// <param name="aseFile">The aseprite file to processes the raw tilemap from.</param>
    /// <param name="frameIndex">The index of the frame in the aseprite file to processes.</param>
    /// <param name="onlyVisibleLayers">Indicates if only layers that are visible should be processed.</param>
    /// <returns>The raw tilemap created by this method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if the frame index specified is less than zero or is greater than or equal to the total number of frames
    /// in the given aseprite file.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if layers are found in the aseprite file with duplicate names.  Tilemaps must contain layers with unique
    /// names even though aseprite does not enforce unique names for layers.
    /// </exception>
    public static RawTilemap Process(AsepriteFile aseFile, int frameIndex, bool onlyVisibleLayers = true)
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
                RawTileset rawTileset = RawTilesetProcessor.Process(aseTilemapLayer.Tileset);
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

        return new(aseFile.Name, rawLayers.ToArray(), rawTilesets.ToArray());
    }
}
