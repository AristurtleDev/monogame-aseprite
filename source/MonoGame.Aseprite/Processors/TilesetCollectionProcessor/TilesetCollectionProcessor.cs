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

using Microsoft.Xna.Framework.Graphics;
using MonoGame.Aseprite.AsepriteTypes;

namespace MonoGame.Aseprite.Processors;

public static class TilesetCollectionProcessor
{
    /// <summary>
    ///     Processes all tilesets in an <see cref="AsepriteFile"/> as a collection.
    /// </summary>
    /// <param name="device">
    ///     The <see cref="GraphicsDevice"/> used to create the resources.
    /// </param>
    /// <param name="file">
    ///     The <see cref="AsepriteFile"/> to process.
    /// </param>
    /// <returns>
    ///     The instance of the <see cref="TilesetCollection"/> class created by this method.
    /// </returns>
    public static TilesetCollection Process(GraphicsDevice device, AsepriteFile file)
    {
        RawTileset[] results = GetRawTilesets(file);
        TilesetCollection collection = new();

        for (int i = 0; i < results.Length; i++)
        {
            collection.AddTileset(TilesetProcessor.CreateTileset(device, results[i]));
        }

        return collection;
    }

    internal static RawTileset[] GetRawTilesets(AsepriteFile file)
    {
        ReadOnlySpan<AsepriteTileset> tilesets = file.Tilesets;
        RawTileset[] processedTilesets = new RawTileset[tilesets.Length];
        HashSet<string> names = new();

        for (int i = 0; i < tilesets.Length; i++)
        {
            AsepriteTileset tileset = tilesets[i];

            if (names.Contains(tileset.Name))
            {
                throw new InvalidOperationException($"Duplicate tileset name found: '{tileset.Name}'.  Tilesets must have unique names for a {nameof(TilesetCollection)}.");
            }

            processedTilesets[i] = TilesetProcessor.GetRawTileset(tileset);
            names.Add(tileset.Name);
        }

        return processedTilesets;
    }
}
